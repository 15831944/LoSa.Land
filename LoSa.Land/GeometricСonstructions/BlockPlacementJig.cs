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

using LoSa.CAD;
using System;
using System.Collections.Generic;

namespace LoSa.Land.GeometricСonstructions
{
    public class BlockPlacementJig : AcEd.EntityJig
    {
        protected AcDb.BlockReference blockReference;
        protected AcGe.Point3d position;
        protected double rotation;
        protected double ucsRotation;
        protected Dictionary<string, string> tags;
        //protected ObjectId layerId;

        public BlockPlacementJig(AcDb.BlockReference br, Dictionary<string, string> tags)
            : base(br)
        {
            blockReference = br;
            position = blockReference.Position;
            AcEd.Editor ed = CurrentCAD.Editor;
            AcGe.CoordinateSystem3d ucs = ed.CurrentUserCoordinateSystem.CoordinateSystem3d;
            AcGe.Matrix3d ocsMat = AcGe.Matrix3d.WorldToPlane(new AcGe.Plane(AcGe.Point3d.Origin, ucs.Zaxis));
            ucsRotation = AcGe.Vector3d.XAxis.GetAngleTo(ucs.Xaxis.TransformBy(ocsMat), ucs.Zaxis);
            rotation = blockReference.Rotation - ucsRotation;
            this.tags = tags;
            //layerId = blockReference.LayerId;
        }

        protected override AcEd.SamplerStatus Sampler(AcEd.JigPrompts prompts)
        {
            System.Windows.Forms.Keys mods = System.Windows.Forms.Control.ModifierKeys;
            if ((mods & System.Windows.Forms.Keys.Control) > 0)
            {
                AcEd.JigPromptAngleOptions jpao = new AcEd.JigPromptAngleOptions("\nВкажіть обертання: ");
                jpao.UseBasePoint = true;
                jpao.BasePoint = blockReference.Position;
                jpao.Cursor = AcEd.CursorType.RubberBand;
                jpao.UserInputControls = (AcEd.UserInputControls.Accept3dCoordinates /*|
                                            UserInputControls.UseBasePointElevation*/);
                AcEd.PromptDoubleResult pdr = prompts.AcquireAngle(jpao);

                if (rotation == pdr.Value)
                {
                    return AcEd.SamplerStatus.NoChange;
                }
                else
                {
                    rotation = pdr.Value;
                    return AcEd.SamplerStatus.OK;
                }
            }
            else
            {
                AcEd.JigPromptPointOptions jppo =
                    new AcEd.JigPromptPointOptions("\nВкажіть точку вставки (або натисніть 'Ctrl' для  обертання): ");
                jppo.UserInputControls =
                  (AcEd.UserInputControls.Accept3dCoordinates | AcEd.UserInputControls.NullResponseAccepted);
                AcEd.PromptPointResult ppr = prompts.AcquirePoint(jppo);
                if (position.DistanceTo(ppr.Value) < AcGe.Tolerance.Global.EqualPoint)
                {
                    return AcEd.SamplerStatus.NoChange;
                }
                else
                {
                    position = ppr.Value;
                }
                return AcEd.SamplerStatus.OK;
            }
        }

        protected override bool Update()
        {
            blockReference.Position = position;
            blockReference.Rotation = rotation + ucsRotation;
            return true;
        }
    }

    class BlockAttribJig : AcEd.EntityJig
    {
        struct TextInfo
        {
            public AcGe.Point3d Position;
            public AcGe.Point3d Alignment;
            private double rotation;
            public bool IsAligned;

            public double Rotation { get => rotation; set => rotation = value; }
        }

        private AcDb.BlockReference _br;
        private AcDb.AttributeDefinition _attDef;
        private AcGe.Point3d _position;
        private AcGe.Point3d _alignment;
        private double _rotation;
        private bool _isAligned;

        private Dictionary<string, TextInfo> _attInfos;

        public BlockAttribJig(AcDb.BlockReference br,
                                Dictionary<string, string> tags)
            : base(br)
        {
            _br = br;
            if (tags != null)
            {
                ServiceBlockElements.ReplaceAttributeBlock(br, tags, true);
            }
            /*
            _attInfos = new Dictionary<string, TextInfo>();
            BlockTableRecord btr = (BlockTableRecord)br.BlockTableRecord.GetObject(OpenMode.ForRead);
            
            foreach (ObjectId id in btr)
            {
                if (id.ObjectClass.Name == "AcDbAttributeDefinition")
                {
                    AttributeDefinition attDef = (AttributeDefinition)id.GetObject(OpenMode.ForRead);
                    TextInfo ti = new TextInfo()
                    {
                        _attDef = attDef;
                        _position = attDef.Position;
                        _alignment = attDef.AlignmentPoint;
                        _isAligned = attDef.Justify != AttachmentPoint.BaseLeft;
                        _rotation = attDef.Rotation;
                    };
                    _attInfos.Add(attDef.Tag.ToUpper(), ti);
                }
            }
            */
        }
    
        protected override AcEd.SamplerStatus Sampler(AcEd.JigPrompts prompts)
        {
            System.Windows.Forms.Keys mods = System.Windows.Forms.Control.ModifierKeys;
            if ((mods & System.Windows.Forms.Keys.Control) > 0)
            {
                AcEd.JigPromptAngleOptions jpao = new AcEd.JigPromptAngleOptions("\nSpecify the rotation: ");
                jpao.UseBasePoint = true;
                jpao.BasePoint = _position;
                jpao.Cursor = AcEd.CursorType.RubberBand;
                jpao.UserInputControls = (
                    AcEd.UserInputControls.Accept3dCoordinates /*|
                    AcEd.UserInputControls.UseBasePointElevation*/);
                AcEd.PromptDoubleResult pdr = prompts.AcquireAngle(jpao);

                if (_rotation == pdr.Value)
                {
                    return AcEd.SamplerStatus.NoChange;
                }
                else
                {
                    _rotation = pdr.Value;
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
                if (_position.DistanceTo(ppr.Value) < AcGe.Tolerance.Global.EqualPoint)
                {
                    return AcEd.SamplerStatus.NoChange;
                }
                else
                {
                    _position = ppr.Value;
                }
                return AcEd.SamplerStatus.OK;
            }
        }

        protected override bool Update()
        {
            AcDb.AttributeDefinition attDef = (AcDb.AttributeDefinition)Entity;
            _attDef.Position = _position;
            _attDef.Rotation = _rotation;

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

    public class BlockAttributesMovingRotatingScaling : AcEd.EntityJig
    {

        private const double DblTol = 0.0001;

        public int mCurJigFactorNumber = 2;

        private AcGe.Point3d mPosition = new AcGe.Point3d(0,0,0);     // Factor #1
        //private double mRotation = 0.0;                   // Factor #2
        //private double mScaleFactor = 1.0;                // Factor #3

        private double mAngleOffset;

        private Dictionary<AcDb.AttributeReference, AcDb.AttributeDefinition> mRef2DefMap;

        public BlockAttributesMovingRotatingScaling(AcDb.BlockReference ent, Dictionary<AcDb.AttributeReference, AcDb.AttributeDefinition> dict)
            : base(ent)
        {
            ent.TransformBy(CurrentCAD.Editor.CurrentUserCoordinateSystem);
            mAngleOffset = (ent as AcDb.BlockReference).Rotation;
            mRef2DefMap = dict;
        }

        protected override bool Update()
        {

            (Entity as AcDb.BlockReference).Position = mPosition;

            foreach (KeyValuePair<AcDb.AttributeReference, AcDb.AttributeDefinition> ar2ad in mRef2DefMap)
                ar2ad.Key.SetAttributeFromBlock(ar2ad.Value, (Entity as AcDb.BlockReference).BlockTransform);

            return true;

            #region 0
            /*
            switch (mCurJigFactorNumber)
            {
                case 1:
                    (Entity as BlockReference).Position = mPosition;
                    break;
                case 2:
                    (Entity as BlockReference).Rotation = mAngleOffset + mRotation;
                    break;
                case 3:
                    (Entity as BlockReference).ScaleFactors = new Scale3d(mScaleFactor, mScaleFactor, mScaleFactor);
                    break;
                default:
                    break;
            }

            foreach (KeyValuePair<AttributeReference, AttributeDefinition> ar2ad in mRef2DefMap)
                ar2ad.Key.SetAttributeFromBlock(ar2ad.Value, (Entity as BlockReference).BlockTransform);

            return true;
            */
            #endregion 0
        }

        protected override AcEd.SamplerStatus Sampler(AcEd.JigPrompts prompts)
        {
            #region 0
            /*
            switch (mCurJigFactorNumber)
            {
                    
                case 1:
                    JigPromptPointOptions prOptions1 = new JigPromptPointOptions("\nBlock insertion point:");

                    /*
                    prOptions1.UserInputControls = UserInputControls.GovernedByUCSDetect | 
                        /*UserInputControls.UseBasePointElevation | * /
                        UserInputControls.Accept3dCoordinates;
                    * /

                     prOptions1.UserInputControls = 
                        UserInputControls.Accept3dCoordinates |
                        UserInputControls.NullResponseAccepted |
                        UserInputControls.NoNegativeResponseAccepted |
                        UserInputControls.GovernedByOrthoMode;

                    PromptPointResult prResult1 = prompts.AcquirePoint(prOptions1);
                    if (prResult1.Status == PromptStatus.Cancel)
                    {
                        if (mPosition.DistanceTo(prResult1.Value) < Tolerance.Global.EqualPoint)
                        {
                            return SamplerStatus.NoChange;
                        }
                        else
                        {
                            mPosition = prResult1.Value;
                            return SamplerStatus.OK;
                        }
                    }
                    return SamplerStatus.Cancel;
                case 2:
                    JigPromptAngleOptions prOptions2 = new JigPromptAngleOptions("\nBlock rotation angle:");
                    prOptions2.BasePoint = mPosition; 
                    prOptions2.UseBasePoint = true;
                    PromptDoubleResult prResult2 = prompts.AcquireAngle(prOptions2);
                    if (prResult2.Status == PromptStatus.Cancel) return SamplerStatus.Cancel;
                    
                    if (Math.Abs(prResult2.Value - mRotation) < DblTol)
                    {
                        return SamplerStatus.NoChange;
                    }
                    else
                    {
                        mRotation = prResult2.Value;
                        return SamplerStatus.OK;
                    }
                case 3:
                    JigPromptDistanceOptions prOptions3 = new JigPromptDistanceOptions("\nBlock scale factor:");
                    prOptions3.BasePoint = mPosition;
                    prOptions3.UseBasePoint = true;
                    PromptDoubleResult prResult3 = prompts.AcquireDistance(prOptions3);
                    if (prResult3.Status == PromptStatus.Cancel) return SamplerStatus.Cancel;
                    /*
                    if (Math.Abs(prResult3.Value - mScaleFactor) < DblTol)
                    {
                        return SamplerStatus.NoChange;
                    }
                    else
                    {* /
                        mScaleFactor = prResult3.Value;
                        return SamplerStatus.OK;
                    //}
                default:
                    break;
            }

            return SamplerStatus.OK;
             */
            #endregion 0

            AcEd.JigPromptPointOptions po = new AcEd.JigPromptPointOptions("\nВкажіть точку вставки:");

            po.UserInputControls =
              (AcEd.UserInputControls.Accept3dCoordinates |
                AcEd.UserInputControls.NullResponseAccepted |
                AcEd.UserInputControls.NoNegativeResponseAccepted |
                AcEd.UserInputControls.GovernedByOrthoMode);

            AcEd.PromptPointResult ppr = prompts.AcquirePoint(po);

            if (ppr.Status == AcEd.PromptStatus.Keyword)
            {
                return AcEd.SamplerStatus.NoChange;
            }
            else if (ppr.Status == AcEd.PromptStatus.OK)
            {
                if (mPosition.DistanceTo(ppr.Value) < AcGe.Tolerance.Global.EqualPoint)
                {
                    return AcEd.SamplerStatus.NoChange;
                }
                mPosition = ppr.Value;
                return AcEd.SamplerStatus.OK;
            }
            return AcEd.SamplerStatus.Cancel;
        }

        public static bool Jig(AcDb.BlockReference ent, Dictionary<AcDb.AttributeReference, AcDb.AttributeDefinition> dict)
        {
            try
            {
                AcEd.Editor ed = CurrentCAD.Editor;
                BlockAttributesMovingRotatingScaling jigger = new BlockAttributesMovingRotatingScaling(ent, dict);
                //PromptResult pr;


                AcEd.PromptStatus stat = AcEd.PromptStatus.Keyword;

                while (stat == AcEd.PromptStatus.Keyword)
                {
                    AcEd.PromptResult res = ed.Drag(jigger);
                    stat = res.Status;
                    if (stat != AcEd.PromptStatus.OK && stat != AcEd.PromptStatus.Keyword) { return false; }
                }

                /*
                while (stat == PromptStatus.Keyword)
                {
                    pr = ed.Drag(jigger);
                    stat = pr.Status;
                    if (stat != PromptStatus.Cancel && 
                        stat != PromptStatus.Error /*&&
                        jigger.mCurJigFactorNumber++ <= 0* /) //{ break; }
                }
                

                do
                {
                    pr = ed.Drag(jigger);
                } while (pr.Status != PromptStatus.Cancel &&
                            pr.Status != PromptStatus.Error /*&&
                            jigger.mCurJigFactorNumber++ <= 0* /);*/


                return true;
            }
            catch
            {
                return false;
            }
        }  
    }
}

