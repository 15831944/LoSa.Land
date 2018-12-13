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
using AcPDb = BricscadDb;
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
using AcPDb = Teigha;
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

using System;
using System.Collections.Generic;
using System.Linq;

using System.Reflection;
using System.IO;
using LoSa.Land.GeometricСonstructions;

namespace LoSa.CAD
{
    public static class ServiceSimpleElements
    {
        private static AcAp.Document doc = CurrentCAD.Document;
        private static AcDb.Database db = CurrentCAD.Database;
        private static AcEd.Editor ed = CurrentCAD.Editor;

        #region CreateSimpleElements
        
            public static AcDb.DBText CreateText(AcGe.Point3d insertPoint, string textValue)
            {
            AcDb.DBText text = new AcDb.DBText
            {
                TextString = textValue,
                Position = insertPoint
            };
            return text;
            }

            public static AcDb.Polyline2d CreatePolyline2d(AcGe.Point2dCollection points, bool isClosed)
            {
                AcDb.Polyline2d polyLine2d = new AcDb.Polyline2d();
                AcDb.Vertex2d vertex;

                foreach (AcGe.Point2d point in points)
                {
                    vertex = new AcDb.Vertex2d(new AcGe.Point3d(point.X, point.Y, 0), 0, 0, 0, 0);
                    polyLine2d.AppendVertex(vertex);
                    polyLine2d.Closed = isClosed;
                }

                return polyLine2d;
            }

            public static AcDb.Polyline3d CreatePolyline3d(List<AcGe.Point2d> points, bool isClosed)
            {
                AcDb.Polyline3d polyLine3d = new AcDb.Polyline3d();
                AcDb.PolylineVertex3d vertex;

                foreach (AcGe.Point2d point in points)
                {
                    vertex = new AcDb.PolylineVertex3d(new AcGe.Point3d(point.ToArray()));
                    polyLine3d.AppendVertex(vertex);
                    polyLine3d.Closed = isClosed;
                }

                return polyLine3d;
            }

            public static AcDb.Hatch CreateHatch(AcDb.ObjectIdCollection objectIds, bool removeBoundaries)
            {
                AcDb.Hatch oHatch = new AcDb.Hatch();
                AcGe.Vector3d normal = new AcGe.Vector3d(0.0, 0.0, 1.0);
                oHatch.Normal = normal;
                oHatch.Elevation = 0;
                oHatch.PatternScale = 1;
                oHatch.SetHatchPattern(AcDb.HatchPatternType.UserDefined, "ANSI31");    
                oHatch.ColorIndex = 1;
                oHatch.Associative = true;
                oHatch.AppendLoop((int)AcDb.HatchLoopTypes.Default, objectIds);
                oHatch.EvaluateHatch(true);

                if (removeBoundaries) ServiceCAD.DeleteObjects(objectIds);

                return oHatch;
            }

            public static AcDb.Hatch CreateHatch(AcDb.ObjectIdCollection objectIds)
            {
                return CreateHatch(objectIds, true);
            }

            public static AcDb.Hatch CreateHatch(AcDb.ObjectId objectId, bool removeBoundaries)
            {
                AcDb.ObjectIdCollection objectIds = new AcDb.ObjectIdCollection(new AcDb.ObjectId[] { objectId });
                return CreateHatch(objectIds, removeBoundaries);
            }

            public static AcDb.Hatch CreateHatch(AcDb.ObjectId objectId)
            {
                AcDb.ObjectIdCollection objectIds = new AcDb.ObjectIdCollection(new AcDb.ObjectId[] { objectId });
                return CreateHatch(objectIds, true);
            }

            public static AcDb.Wipeout CreateWipeout(AcGe.Point2dCollection points)
            {
                AcDb.Wipeout oWipeout = new AcDb.Wipeout();
                return oWipeout;
            }

        #endregion CreateSimpleElements

        #region OffsetElements

            public static AcGe.Point2dCollection Offset(
                AcGe.Point2dCollection collection, 
                AcGe.Vector2d offset)
            {
                AcGe.Point2dCollection offsetCollection = new AcGe.Point2dCollection();
                foreach (AcGe.Point2d point in collection)
                {
                    offsetCollection.Add(point.Add(offset));
                }
                return offsetCollection;
            }

            public static AcGe.Point3dCollection Offset(
                AcGe.Point3dCollection collection,
                AcGe.Vector3d offset)
            {
                AcGe.Point3dCollection offsetCollection = new AcGe.Point3dCollection();
                foreach (AcGe.Point3d point in collection)
                {
                    offsetCollection.Add(point.Add(offset));
                }
                return offsetCollection;
            }

        #endregion OffsetElements

        #region ManualInsert
            
            public static void ManualInsertText(AcDb.DBText oText)
            {
                using (AcDb.Transaction tr = db.TransactionManager.StartTransaction())
                {
                    AcDb.BlockTableRecord btr = (AcDb.BlockTableRecord)tr.GetObject(db.CurrentSpaceId, AcDb.OpenMode.ForWrite);

                    oText.Normal = ed.CurrentUserCoordinateSystem.CoordinateSystem3d.Zaxis;

                    btr.AppendEntity(oText);
                    tr.AddNewlyCreatedDBObject(oText, true);

                    TextPlacementJig pj = new TextPlacementJig(oText);

                    AcEd.PromptStatus stat = AcEd.PromptStatus.Keyword;
                    while (stat == AcEd.PromptStatus.Keyword)
                    {
                        AcEd.PromptResult res = ed.Drag(pj);
                        stat = res.Status;
                        if (stat != AcEd.PromptStatus.OK && stat != AcEd.PromptStatus.Keyword) { return; }
                    }
                    tr.Commit();
                    //return (DBText)pj.Entity;
                }
            }

            public static void ManualInsertMText(AcDb.MText oMText)
            {
                using (AcDb.Transaction tr = db.TransactionManager.StartTransaction())
                {
                    AcDb.BlockTableRecord btr = (AcDb.BlockTableRecord)tr.GetObject(db.CurrentSpaceId, AcDb.OpenMode.ForWrite);

                    oMText.Normal = ed.CurrentUserCoordinateSystem.CoordinateSystem3d.Zaxis;

                    btr.AppendEntity(oMText);
                    tr.AddNewlyCreatedDBObject(oMText, true);

                    MTextPlacementJig pj = new MTextPlacementJig(oMText);

                    AcEd.PromptStatus stat = AcEd.PromptStatus.Keyword;
                    while (stat == AcEd.PromptStatus.Keyword)
                    {
                        AcEd.PromptResult res = ed.Drag(pj);
                        stat = res.Status;
                        if (stat != AcEd.PromptStatus.OK && stat != AcEd.PromptStatus.Keyword) { return; }
                    }
                    tr.Commit();
                    //return (MText)pj.Entity;
                }
            }

        #endregion ManualInsert

    }

}
