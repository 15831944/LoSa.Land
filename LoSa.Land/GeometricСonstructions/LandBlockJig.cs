
using LoSa.CAD;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if BCAD 
using AcRx = Bricscad.Runtime; 
using AcTrx = Teigha.Runtime; 
using AcAp = Bricscad.ApplicationServices; 
using AcDb = Teigha.DatabaseServices; 
using AcGe = Teigha.Geometry; 
using AcEd = Bricscad.EditorInput; 
using AcGi = Teigha.GraphicsInterface; 
using AcClr = Teigha.Colors; 
using AcWnd = Bricscad.Windows; 
using AcApp = Bricscad.ApplicationServices.Application;
using AcPApp = BricscadApp;
#endif

#if NCAD
using AcRx = HostMgd.Runtime;
using AcTrx = Teigha.Runtime;
using AcAp = HostMgd.ApplicationServices;
using AcDb = Teigha.DatabaseServices;
using AcGe = Teigha.Geometry;
using AcEd = HostMgd.EditorInput;
using AcGi = Teigha.GraphicsInterface;
using AcClr = Teigha.Colors;
using AcWnd = HostMgd.Windows;
using AcApp = HostMgd.ApplicationServices.Application;
using AcPApp = HostMgd;
#endif

#if ACAD
using AcRx = Autodesk.AutoCAD.Runtime; 
using AcTrx = Autodesk.AutoCAD.Runtime; 
using AcAp = Autodesk.AutoCAD.ApplicationServices; 
using AcDb = Autodesk.AutoCAD.DatabaseServices; 
using AcGe = Autodesk.AutoCAD.Geometry; 
using AcEd = Autodesk.AutoCAD.EditorInput; 
using AcGi = Autodesk.AutoCAD.GraphicsInterface; 
using AcClr = Autodesk.AutoCAD.Colors; 
using AcWnd = Autodesk.AutoCAD.Windows; 
using AcApp = Autodesk.AutoCAD.ApplicationServices.Application; 
using AcPApp = Autodesk.AutoCAD.Interop; 
#endif

namespace LoSa.Land.GeometricСonstructions
{
    class LandBlockJig : AcEd.EntityJig
    {
        protected AcDb.BlockReference _br;
        protected AcGe.Point3d _pos;
        protected double _rot, _ucsRot;

        public LandBlockJig(AcDb.BlockReference br)
            : base(br)
        {
            _br = br;
            _pos = _br.Position;
            AcEd.Editor ed = CurrentCAD.Editor;
            AcGe.CoordinateSystem3d ucs = ed.CurrentUserCoordinateSystem.CoordinateSystem3d;
            AcGe.Matrix3d ocsMat = AcGe.Matrix3d.WorldToPlane(new AcGe.Plane(AcGe.Point3d.Origin, ucs.Zaxis));
            _ucsRot = AcGe.Vector3d.XAxis.GetAngleTo(ucs.Xaxis.TransformBy(ocsMat), ucs.Zaxis);
            _rot = _br.Rotation - _ucsRot;
        }

        protected override AcEd.SamplerStatus Sampler(AcEd.JigPrompts prompts)
        {
            System.Windows.Forms.Keys mods = System.Windows.Forms.Control.ModifierKeys;
            if ((mods & System.Windows.Forms.Keys.Control) > 0)
            {
                AcEd.JigPromptAngleOptions jpao =
                    new AcEd.JigPromptAngleOptions("\nSpecify the rotation: ");
                jpao.UseBasePoint = true;
                jpao.BasePoint = _br.Position;
                jpao.Cursor = AcEd.CursorType.RubberBand;
                jpao.UserInputControls = (
                    AcEd.UserInputControls.Accept3dCoordinates /*|
                    AcEd.UserInputControls.UseBasePointElevation*/);
                AcEd.PromptDoubleResult pdr = prompts.AcquireAngle(jpao);

                if (_rot == pdr.Value)
                {
                    return AcEd.SamplerStatus.NoChange;
                }
                else
                {
                    _rot = pdr.Value;
                    return AcEd.SamplerStatus.OK;
                }
            }
            else
            {
                AcEd.JigPromptPointOptions jppo =
                    new AcEd.JigPromptPointOptions("\nSpecify insertion point (or press Ctrl for rotation): ");
                jppo.UserInputControls =
                  (AcEd.UserInputControls.Accept3dCoordinates | AcEd.UserInputControls.NullResponseAccepted);
                AcEd.PromptPointResult ppr = prompts.AcquirePoint(jppo);
                if (_pos.DistanceTo(ppr.Value) < AcGe.Tolerance.Global.EqualPoint)
                {
                    return AcEd.SamplerStatus.NoChange;
                }
                else
                {
                    _pos = ppr.Value;
                }
                return AcEd.SamplerStatus.OK;
            }
        }

        protected override bool Update()
        {
            _br.Position = _pos;
            _br.Rotation = _rot + _ucsRot;
            return true;
        }
    }

    class LandBlockAttribJig : LandBlockJig
    {
        struct TextInfo
        {
            public AcGe.Point3d Position;
            public AcGe.Point3d Alignment;
            public double Rotation;
            public bool IsAligned;
        }

        private Dictionary<string, TextInfo> _attInfos;

        public LandBlockAttribJig(AcDb.BlockReference br)
            : base(br)
        {
            _attInfos = new Dictionary<string, TextInfo>();
            AcDb.BlockTableRecord btr = (AcDb.BlockTableRecord)br.BlockTableRecord.GetObject(AcDb.OpenMode.ForRead);
            foreach (AcDb.ObjectId id in btr)
            {
                if (id.ObjectClass.Name == "AcDbAttributeDefinition")
                {
                    AcDb.AttributeDefinition attDef = (AcDb.AttributeDefinition)id.GetObject(AcDb.OpenMode.ForRead);
                    TextInfo ti = new TextInfo()
                    {
                        Position = attDef.Position,
                        Alignment = attDef.AlignmentPoint,
                        IsAligned = attDef.Justify != AcDb.AttachmentPoint.BaseLeft,
                        Rotation = attDef.Rotation
                    };
                    _attInfos.Add(attDef.Tag.ToUpper(), ti);
                }
            }
        }

        protected override bool Update()
        {
            base.Update();
            foreach (AcDb.ObjectId id in _br.AttributeCollection)
            {
                AcDb.AttributeReference att = (AcDb.AttributeReference)id.GetObject(AcDb.OpenMode.ForWrite);
                att.Rotation = _br.Rotation;
                string tag = att.Tag.ToUpper();
                if (_attInfos.ContainsKey(tag))
                {
                    TextInfo ti = _attInfos[tag];
                    att.Position = ti.Position.TransformBy(_br.BlockTransform);
                    if (ti.IsAligned)
                    {
                        att.AlignmentPoint =
                            ti.Alignment.TransformBy(_br.BlockTransform);
                        att.AdjustAlignment(_br.Database);
                    }
                    if (att.IsMTextAttribute)
                    {
                        att.UpdateMTextAttribute();
                    }
                    att.Rotation = ti.Rotation + _br.Rotation;
                }
            }
            return true;
        }
    }
}
