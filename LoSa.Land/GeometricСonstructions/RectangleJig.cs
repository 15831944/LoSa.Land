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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoSa.Land.EnumAttributes;
using LoSa.Utility;

namespace LoSa.Land.GeometricСonstructions
{
    public class RectangleJig : AcEd.DrawJig
    {
        private AcGe.Point3d basePoint;
        private AcGe.Point3d diagonalPoint;
        private AcGe.Point3d diractionlPoint;
        private MethodConstructingRectangle methodConstructing;

        RectangleJig(MethodConstructingRectangle methodConstructing, AcGe.Point3d basePoint, AcGe.Point3d diractionlPoint)
        {
            this.methodConstructing = methodConstructing;
            this.basePoint = basePoint;
            this.diractionlPoint = diractionlPoint;
        }

        public AcGe.Matrix3d UCS
        {
            get { return CurrentCAD.Editor.CurrentUserCoordinateSystem; }
        }

        public AcGe.Point3dCollection Corners
        {
            get
            {
                AcGe.Point3dCollection listCorners = new AcGe.Point3dCollection();
                listCorners.Add(this.basePoint);

                AcDb.Line line = new AcDb.Line(this.basePoint, this.diractionlPoint);

                AcDb.Xline xLine = new AcDb.Xline();
                xLine.BasePoint = this.basePoint;
                xLine.UnitDir = this.basePoint.GetVectorTo(this.diractionlPoint);

                double widthRectangle = ServiceGeodesy.GetProjectionOnLine(line, diagonalPoint);
                double heightRectangle = ServiceGeodesy.GetOffsetFromLine(line, diagonalPoint);

                if ( this.methodConstructing == MethodConstructingRectangle.Diagonal)
                {
                    listCorners.Add(new AcGe.Point3d(this.diagonalPoint.X, this.basePoint.Y, this.basePoint.Z));
                    listCorners.Add(this.diagonalPoint);
                    listCorners.Add(new AcGe.Point3d(this.basePoint.X, this.diagonalPoint.Y, this.basePoint.Z));
                }

                else  if ( this.methodConstructing == MethodConstructingRectangle.DirectionAndDiagonal)
                {
                    listCorners.Add(xLine.GetPointAtDist(widthRectangle));
                    listCorners.Add(this.diagonalPoint);
                    listCorners.Add(this.basePoint.Add(xLine.GetPointAtDist(widthRectangle).GetVectorTo(this.diagonalPoint)));
                }

                else  if ( this.methodConstructing == MethodConstructingRectangle.HeightAndWidth)
                {
                    AcDb.DBObjectCollection list = line.GetOffsetCurves(heightRectangle*-1);
                    AcDb.Line lineOffset = (AcDb.Line)list[0];
                    listCorners.Add(this.diractionlPoint);
                    listCorners.Add(lineOffset.EndPoint);
                    listCorners.Add(lineOffset.StartPoint);
                }

                return listCorners;
            }
        }

        protected override bool WorldDraw(AcGi.WorldDraw draw)
        {
            AcGi.WorldGeometry geo = draw.Geometry;
            if (geo != null)
            {
                geo.PushModelTransform(UCS);
                geo.Polygon(Corners);
                geo.PopModelTransform();
            }

            return true;
        }

        protected override AcEd.SamplerStatus Sampler(AcEd.JigPrompts prompts)
        {
            AcEd.JigPromptPointOptions jppo = new AcEd.JigPromptPointOptions();
            AcEd.PromptPointResult ppr;
            
            jppo.Message = this.methodConstructing.GetMessage();
            jppo.UseBasePoint = false;
           
            ppr = prompts.AcquirePoint(jppo);
                
            if (ppr.Status == AcEd.PromptStatus.Cancel || ppr.Status == AcEd.PromptStatus.Error)  
            {   
                return AcEd.SamplerStatus.Cancel; 
            }
            AcGe.Point3d tmpPt = ppr.Value.TransformBy(UCS.Inverse());
            if (!this.diagonalPoint.IsEqualTo(tmpPt, new AcGe.Tolerance(10e-10, 10e-10)))
            {
                this.diagonalPoint = tmpPt;
                return AcEd.SamplerStatus.OK;
            }
            else
            {
                return AcEd.SamplerStatus.NoChange;
            }
            
        }

        public static RectangleJig jigger;
        [AcTrx.CommandMethod("Land_BuildingRectangle")]
        public static void BuildingRectangle()
        {
            try
            {
                AcDb.Database db = CurrentCAD.Database;
                AcEd.Editor ed = CurrentCAD.Editor;

                AcEd.PromptKeywordOptions pko;
                AcEd.PromptPointOptions ppt;
                AcEd.PromptPointResult ppr;
                AcGe.Point3d basePoint;
                AcGe.Point3d diractionPoint;

                pko = new AcEd.PromptKeywordOptions("\nПобудова прямокутника");

                pko.Keywords.Add("по Діагоналі");
                pko.Keywords.Add("по Напрямку та діагоналі");
                pko.Keywords.Add("по Ширині та висота");
                pko.Keywords.Default = "по Ширині та висота";
                pko.AllowNone = false;

                AcEd.PromptResult pkr =  ed.GetKeywords(pko);

                if (pkr.Status != AcEd.PromptStatus.OK) return;

                MethodConstructingRectangle methodConstructing;

                if (pkr.StringResult == "Діагоналі")
                {
                    methodConstructing = MethodConstructingRectangle.Diagonal;
                }
                else if (pkr.StringResult == "Напрямку")
                {
                    methodConstructing = MethodConstructingRectangle.DirectionAndDiagonal;
                }
                else
                {
                    methodConstructing = MethodConstructingRectangle.HeightAndWidth;
                }

                ppt = new AcEd.PromptPointOptions("\nВкажіть першу точку прямокутника:");
                ppr = ed.GetPoint(ppt);
                if (ppr.Status != AcEd.PromptStatus.OK) return;

                basePoint = ppr.Value;

                if (methodConstructing == MethodConstructingRectangle.Diagonal)
                {
                    diractionPoint = basePoint.Add(AcGe.Vector3d.XAxis );
                }
                else
                {
                    ppt = new AcEd.PromptPointOptions("\n");
                    if (methodConstructing == MethodConstructingRectangle.DirectionAndDiagonal)
                    {
                        ppt.Message = "\nВкажіть точку напрямку прямокутника:";
                    }
                    else if (methodConstructing == MethodConstructingRectangle.DirectionAndDiagonal)
                    {
                        ppt.Message = "\nВкажіть ширину прямокутника:";
                    }
                    ppt.UseBasePoint = true;
                    ppt.BasePoint = basePoint;
                    ppr = ed.GetPoint(ppt);
                    if (ppr.Status != AcEd.PromptStatus.OK) return;

                    diractionPoint = ppr.Value;
                }
                jigger = new RectangleJig(methodConstructing, basePoint, diractionPoint);
                ed.Drag(jigger);

                using (AcDb.Transaction tr = db.TransactionManager.StartTransaction())
                {
                    AcDb.BlockTableRecord btr = (AcDb.BlockTableRecord)tr.GetObject(db.CurrentSpaceId, AcDb.OpenMode.ForWrite);

                    Teigha.DatabaseServices.Polyline ent = new Teigha.DatabaseServices.Polyline();
                    ent.SetDatabaseDefaults();
                    for (int i = 0; i < jigger.Corners.Count; i++)
                    {
                        AcGe.Point3d pt3d = jigger.Corners[i];
                        AcGe.Point2d pt2d = new AcGe.Point2d(pt3d.X, pt3d.Y);
                        ent.AddVertexAt(i, pt2d, 0, db.Plinewid, db.Plinewid);
                    }
                    ent.Closed = true;
                    ent.TransformBy(jigger.UCS);
                    btr.AppendEntity(ent);
                    tr.AddNewlyCreatedDBObject(ent, true);

                    tr.Commit();
                }
            }
            catch (System.Exception ex)
            {
                CurrentCAD.Editor.WriteMessage(ex.ToString());
            }
        }
    }

    public enum MethodConstructingRectangle
    {
        [MessageConstructingRectangle("\nВкажіть точку діагоналі прямокутника:")]
        Diagonal,

        [MessageConstructingRectangle("\nВкажіть точку висоти прямокутника:")]
        HeightAndWidth,

        [MessageConstructingRectangle("\nВкажіть точку діагоналі прямокутника:")]
        DirectionAndDiagonal
    }
}

