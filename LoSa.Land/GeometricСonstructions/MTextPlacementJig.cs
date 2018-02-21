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
    
    public class MTextPlacementJig : AcEd.EntityJig
    {

        AcGe.Point3d _location;
        double _angle;
        double _txtSize;

        public MTextPlacementJig(AcDb.MText mText)
            : base((AcDb.Entity)mText)
        {
            _angle = mText.Rotation;
            _txtSize = mText.Height;
        }
        
        protected override AcEd.SamplerStatus Sampler(AcEd.JigPrompts jp)
        {

            AcEd.JigPromptPointOptions po = new AcEd.JigPromptPointOptions("\nВкажіть точку вставки:");

            po.UserInputControls =
              ( AcEd.UserInputControls.Accept3dCoordinates |
                AcEd.UserInputControls.NullResponseAccepted |
                AcEd.UserInputControls.NoNegativeResponseAccepted |
                AcEd.UserInputControls.GovernedByOrthoMode);

            AcEd.PromptPointResult ppr = jp.AcquirePoint(po);

            if (ppr.Status == AcEd.PromptStatus.Keyword)
            {
                return AcEd.SamplerStatus.NoChange;
            }
            else if (ppr.Status == AcEd.PromptStatus.OK)
            {
                if (_location.DistanceTo(ppr.Value) < AcGe.Tolerance.Global.EqualPoint)
                {
                    return AcEd.SamplerStatus.NoChange;
                }
                _location = ppr.Value;
                return AcEd.SamplerStatus.OK;
            }
            return AcEd.SamplerStatus.Cancel;
        }

        protected override bool Update()
        {
            AcDb.MText mText = (AcDb.MText)Entity;

            mText.Location = _location;
            mText.Height = _txtSize;
            mText.Rotation = _angle;

            return true;
        }
    }
}
