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

namespace LoSa.CAD
{
    public static class ServiceCAD
    {
        private static AcAp.Document doc = CurrentCAD.Document;
        private static AcDb.Database db = CurrentCAD.Database;
        private static AcEd.Editor ed = CurrentCAD.Editor;

        #region Objects
            public static AcDb.DBObject GetObject(AcDb.ObjectId objectId)
            {
                AcDb.DBObject obj = null;
                using (AcDb.Transaction tr = db.TransactionManager.StartTransaction())
                {
                    obj = tr.GetObject(objectId, AcDb.OpenMode.ForWrite);
                    obj.Erase();
                    tr.Commit();
                }
                return obj;
            }

            public static AcDb.ObjectId InsertObject(AcDb.Entity entityObject)
            {
                AcDb.ObjectId objectId;
                using (AcDb.Transaction tr = db.TransactionManager.StartTransaction())
                {
                    AcDb.BlockTableRecord btr = (AcDb.BlockTableRecord)tr.GetObject(db.CurrentSpaceId, AcDb.OpenMode.ForWrite);
                    btr.AppendEntity(entityObject);
                    tr.AddNewlyCreatedDBObject(entityObject, true);
                    objectId = entityObject.ObjectId;
                    btr.Dispose();
                    tr.Commit();
                }
                return objectId;
            }

            public static List<AcDb.ObjectId> InsertObjects(List<AcDb.Entity> entityObjects)
            {
                List<AcDb.ObjectId> listObjectId = new List<AcDb.ObjectId>();
                foreach (AcDb.Entity entityObject in entityObjects)
                {
                    listObjectId.Add(InsertObject(entityObject));
                }
                return listObjectId;
            }

            public static void DeleteObject(AcDb.ObjectId objectId)
            {
                try
                {
                    using (AcDb.Transaction tr = db.TransactionManager.StartTransaction())
                    {
                        AcDb.DBObject dbObj = tr.GetObject(objectId, AcDb.OpenMode.ForRead) as AcDb.DBObject;
                        dbObj.UpgradeOpen();
                        dbObj.Erase();
                        tr.Commit();
                    }
                }
                catch //(System.Exception exc)
                {

                }
            }

            public static void DeleteObjects(AcDb.ObjectIdCollection objectIds)
            {
                foreach (AcDb.ObjectId objectId in objectIds)
                {
                    DeleteObject(objectId);
                }
            }
        

            public static void SelectObjects(AcDb.ObjectIdCollection objectIds)
            {
                using (AcDb.Transaction tr = db.TransactionManager.StartTransaction())
                {
                    AcEd.PromptSelectionResult psr = ed.GetSelection();
                    if (psr.Status == AcEd.PromptStatus.OK)
                    {
                        AcEd.SelectionSet SSet = psr.Value;
                        foreach (AcEd.SelectedObject SObj in SSet)
                        {
                            if (SObj != null)
                            {
                                AcDb.Entity entityObject = tr.GetObject(SObj.ObjectId, AcDb.OpenMode.ForWrite) as AcDb.Entity;

                                if (entityObject != null)
                                {
                                    foreach (AcDb.ObjectId idObj in objectIds)
                                    {
                                        if (idObj.Equals(entityObject.ObjectId))
                                        {
                                            entityObject.Highlight();
                                        }
                                    }
                                }
                            }
                        }
                        // Save the new object to the database                      
                        tr.Commit();

                    }
                }
            }

        internal static void Regen()
        {
            throw new NotImplementedException();
        }


        #endregion Objects

        #region SystemVariable

        public static String GetSystemVariable(String variableName)
        {
            return (string)AcApp.GetSystemVariable(variableName);
        }

        public static List<String> GetSystemVariableSrchPath()
        {
            List<String> list = new List<string>();

            list.AddRange(GetSystemVariable("SRCHPATH").Split(';'));

            return list;
        }

        public static string GetSystemVariableSrchPathByName(String pathName)
        {

            return GetSystemVariableSrchPath().Find
                (
                    delegate (String value)
                    {
                        return value.IndexOf(pathName) > -1;//value == pathName;
                    }
                );
            //foreach (string lineSRCHPATH in ServiceCAD.GetSystemVariableSrchPath())
            //{
            //    if (lineSRCHPATH.IndexOf(pathName) > -1) { return lineSRCHPATH; }
            //}
            //return null;
        }


        public static void AddSystemVariableSRCHPATH(String valueVariable)
        {
            valueVariable = valueVariable.Replace("/", "\\");

            foreach ( String value in GetSystemVariableSrchPath() )
            {
                if (value.IndexOf(valueVariable) > -1)
                {
                    return;
                }
            }

            String newValueVariable = (string)AcApp.GetSystemVariable("SRCHPATH") + ";" + valueVariable;
            AcApp.SetSystemVariable("SRCHPATH", newValueVariable);
        }

        #endregion SystemVariable

        #region Zoom

            public static void ZoomAll()
            {
    #if BCAD
                CurrentCAD.Application.ZoomAll();
    #endif
            }

            public static void ZoomExtents()
            {
    #if BCAD
                try
                {
                    CurrentCAD.Application.ZoomExtents();
                }
                catch  { }
    #endif
            }

            public static void ZoomCenter(AcGe.Point3d pointCenter, double scaleFactor)
            {
                Zoom(new AcGe.Point3d(), new AcGe.Point3d(), pointCenter, scaleFactor);
            }

            public static void Zoom(AcGe.Point3d pMin, AcGe.Point3d pMax, AcGe.Point3d pCenter, double dFactor)
            {
                int nCurVport = System.Convert.ToInt32(AcApp.GetSystemVariable("CVPORT"));

                if (db.TileMode == true)
                {
                    if (pMin.Equals(new AcGe.Point3d()) == true &&
                        pMax.Equals(new AcGe.Point3d()) == true)
                    {
                        pMin = db.Extmin;
                        pMax = db.Extmax;
                    }
                }
                else
                {
                    // Check to see if Paper space is current
                    if (nCurVport == 1)
                    {
                        // Get the extents of Paper space
                        if (pMin.Equals(new AcGe.Point3d()) == true &&
                            pMax.Equals(new AcGe.Point3d()) == true)
                        {
                            pMin = db.Pextmin;
                            pMax = db.Pextmax;
                        }
                    }
                    else
                    {
                        // Get the extents of Model space
                        if (pMin.Equals(new AcGe.Point3d()) == true &&
                            pMax.Equals(new AcGe.Point3d()) == true)
                        {
                            pMin = db.Extmin;
                            pMax = db.Extmax;
                        }
                    }
                }

                using (AcDb.Transaction acTrans = db.TransactionManager.StartTransaction())
                {
                    using (AcDb.ViewTableRecord acView = ed.GetCurrentView())
                    {
                        AcDb.Extents3d eExtents;

                        AcGe.Matrix3d matWCS2DCS;
                        matWCS2DCS = AcGe.Matrix3d.PlaneToWorld(acView.ViewDirection);
                        matWCS2DCS = AcGe.Matrix3d.Displacement(acView.Target - AcGe.Point3d.Origin) * matWCS2DCS;
                        matWCS2DCS = AcGe.Matrix3d.Rotation(-acView.ViewTwist,
                                                        acView.ViewDirection,
                                                        acView.Target) * matWCS2DCS;

                        if (pCenter.DistanceTo(AcGe.Point3d.Origin) != 0)
                        {
                            pMin = new AcGe.Point3d(pCenter.X - (acView.Width / 2),
                                                pCenter.Y - (acView.Height / 2), 0);

                            pMax = new AcGe.Point3d((acView.Width / 2) + pCenter.X,
                                                (acView.Height / 2) + pCenter.Y, 0);
                        }

                        using (AcDb.Line acLine = new AcDb.Line(pMin, pMax))
                        {
                            eExtents = new AcDb.Extents3d(acLine.Bounds.Value.MinPoint,
                                                        acLine.Bounds.Value.MaxPoint);
                        }

                        double dViewRatio;
                        dViewRatio = (acView.Width / acView.Height);

                        matWCS2DCS = matWCS2DCS.Inverse();
                        eExtents.TransformBy(matWCS2DCS);

                        double dWidth;
                        double dHeight;
                        AcGe.Point2d pNewCentPt;

                        if (pCenter.DistanceTo(AcGe.Point3d.Origin) != 0)
                        {
                            dWidth = acView.Width;
                            dHeight = acView.Height;

                            if (dFactor == 0) { pCenter = pCenter.TransformBy(matWCS2DCS); }

                            pNewCentPt = new AcGe.Point2d(pCenter.X, pCenter.Y);
                        }
                        else
                        {
                            dWidth = eExtents.MaxPoint.X - eExtents.MinPoint.X;
                            dHeight = eExtents.MaxPoint.Y - eExtents.MinPoint.Y;

                            pNewCentPt = new AcGe.Point2d(((eExtents.MaxPoint.X + eExtents.MinPoint.X) * 0.5),
                                                        ((eExtents.MaxPoint.Y + eExtents.MinPoint.Y) * 0.5));
                        }

                        if (dWidth > (dHeight * dViewRatio)) dHeight = dWidth / dViewRatio;

                        if (dFactor != 0)
                        {
                            acView.Height = dHeight * dFactor;
                            acView.Width = dWidth * dFactor;
                        }
                        acView.CenterPoint = pNewCentPt;
                        ed.SetCurrentView(acView);
                    }
                    acTrans.Commit();
                }
            }


        #endregion Zoom

        #region DrawOrder

            public static void DrawOrder_MoveAbove(AcDb.ObjectIdCollection collection, AcDb.ObjectId target)
            {
                using (AcDb.Transaction tr = db.TransactionManager.StartTransaction())
                {
                    AcDb.BlockTable bt = (AcDb.BlockTable)tr.GetObject(db.BlockTableId, AcDb.OpenMode.ForRead);
                    AcDb.BlockTableRecord btr = (AcDb.BlockTableRecord)tr.GetObject(bt[AcDb.BlockTableRecord.ModelSpace], AcDb.OpenMode.ForWrite);

                    AcDb.DrawOrderTable dot = (AcDb.DrawOrderTable)tr.GetObject(btr.DrawOrderTableId, AcDb.OpenMode.ForWrite);
                    dot.MoveAbove(collection, target);
                }
            }

            public static void DrawOrder_MoveBelow(AcDb.ObjectIdCollection collection, AcDb.ObjectId target)
            {
                using (AcDb.Transaction tr = db.TransactionManager.StartTransaction())
                {
                    AcDb.BlockTable bt = (AcDb.BlockTable)tr.GetObject(db.BlockTableId, AcDb.OpenMode.ForRead);
                    AcDb.BlockTableRecord btr = (AcDb.BlockTableRecord)tr.GetObject(bt[AcDb.BlockTableRecord.ModelSpace], AcDb.OpenMode.ForWrite);

                    AcDb.DrawOrderTable dot = (AcDb.DrawOrderTable)tr.GetObject(btr.DrawOrderTableId, AcDb.OpenMode.ForWrite);
                    dot.MoveBelow(collection, target);
                }
            }


            public static void DrawOrder_MoveToBottom(AcDb.ObjectIdCollection collection)
            {
                using (AcDb.Transaction tr = db.TransactionManager.StartTransaction())
                {
                    AcDb.BlockTable bt = (AcDb.BlockTable)tr.GetObject(db.BlockTableId, AcDb.OpenMode.ForRead);
                    AcDb.BlockTableRecord btr = (AcDb.BlockTableRecord)tr.GetObject(bt[AcDb.BlockTableRecord.ModelSpace], AcDb.OpenMode.ForWrite);

                    AcDb.DrawOrderTable dot = (AcDb.DrawOrderTable)tr.GetObject(btr.DrawOrderTableId, AcDb.OpenMode.ForWrite);
                    dot.MoveToBottom(collection);
                }
            }


            public static void DrawOrder_MoveToTop(AcDb.ObjectIdCollection collection)
            {
                using (AcDb.Transaction tr = db.TransactionManager.StartTransaction())
                {
                    AcDb.BlockTable bt = (AcDb.BlockTable)tr.GetObject(db.BlockTableId, AcDb.OpenMode.ForRead);
                    AcDb.BlockTableRecord btr = (AcDb.BlockTableRecord)tr.GetObject(bt[AcDb.BlockTableRecord.ModelSpace], AcDb.OpenMode.ForWrite);

                    AcDb.DrawOrderTable dot = (AcDb.DrawOrderTable)tr.GetObject(btr.DrawOrderTableId, AcDb.OpenMode.ForWrite);
                    dot.MoveToTop(collection);
                }
            }

        #endregion DrawOrder

        #region  Import

            public static AcDb.ObjectIdCollection ImportSymbolTableRecords<T>(
                                                           /*this Database db,*/
                                                           string sourceFile,
                                                           params string[] recordNames)
                                                           where T : AcDb.SymbolTable
            {
                using (AcDb.Database sourceDb = new AcDb.Database())
                {
                    sourceDb.ReadDwgFile(sourceFile, System.IO.FileShare.Read, false, "");

                    AcDb.ObjectId sourceTableId;
                    AcDb.ObjectId targetTableId;

                    switch (typeof(T).Name)
                    {
                        case "BlockTable":
                            sourceTableId = sourceDb.BlockTableId;
                            targetTableId = db.BlockTableId;
                            break;
                        case "DimStyleTable":
                            sourceTableId = sourceDb.DimStyleTableId;
                            targetTableId = db.DimStyleTableId;
                            break;
                        case "LayerTable":
                            sourceTableId = sourceDb.LayerTableId;
                            targetTableId = db.LayerTableId;
                            break;
                        case "LinetypeTable":
                            sourceTableId = sourceDb.LinetypeTableId;
                            targetTableId = db.LinetypeTableId;
                            break;
                        case "RegAppTable":
                            sourceTableId = sourceDb.RegAppTableId;
                            targetTableId = db.RegAppTableId;
                            break;
                        case "TextStyleTable":
                            sourceTableId = sourceDb.TextStyleTableId;
                            targetTableId = db.TextStyleTableId;
                            break;
                        case "UcsTable":
                            sourceTableId = sourceDb.UcsTableId;
                            targetTableId = db.UcsTableId;
                            break;
                        case "ViewTable":
                            sourceTableId = sourceDb.ViewportTableId;
                            targetTableId = db.ViewportTableId;
                            break;
                        case "ViewportTable":
                            sourceTableId = sourceDb.ViewportTableId;
                            targetTableId = db.ViewportTableId;
                            break;
                        default:
                            throw new ArgumentException("\nImportSymbolTableRecords > Потрібен конкретний тип, похідний від SymbolTable");
                    }

                    using (AcDb.Transaction tr = sourceDb.TransactionManager.StartTransaction())
                    {
                        T sourceTable = (T)tr.GetObject(sourceTableId, AcDb.OpenMode.ForRead);
                        AcDb.ObjectIdCollection idObjects = new AcDb.ObjectIdCollection();
                        foreach (string name in recordNames)
                        {
                            if (sourceTable.Has(name))
                            {
                                idObjects.Add(sourceTable[name]);
                            }
                        }
                        if (idObjects.Count == 0) return null;
                        AcDb.IdMapping idMap = new AcDb.IdMapping();
                        sourceDb.WblockCloneObjects(idObjects, targetTableId, idMap, AcDb.DuplicateRecordCloning.Replace, false);
                        tr.Commit();
                        AcDb.ObjectIdCollection retVal = new AcDb.ObjectIdCollection();
                        foreach (AcDb.ObjectId id in idObjects)
                        {
                            if (idMap[id].IsCloned)
                            {
                                retVal.Add(idMap[id].Value);
                            }
                        }
                        return retVal.Count == 0 ? null : retVal;
                    }
                }
            }

            public static bool ImportLineTypes(string fileLineTypes)
            {
                try
                {
                    string path = AcDb.HostApplicationServices.Current.FindFile( fileLineTypes, db, AcDb.FindFileHint.Default);
                    db.LoadLineTypeFile("*", path);
                    return true;
                }
                catch (Teigha.Runtime.Exception ex)
                {
                    if (ex.ErrorStatus == AcTrx.ErrorStatus.FilerError)
                        ed.WriteMessage("\nІмпору типів ліній > \n Не вдалося знайти файл '{0}'.", fileLineTypes);
                    else if (ex.ErrorStatus == AcTrx.ErrorStatus.DuplicateRecordName)
                        ed.WriteMessage("\nІмпору типів ліній > \n Неможливо завантажити деякі типи ліній.");
                    else
                        ed.WriteMessage( "\nІмпору типів ліній > {0}", ex.Message );
                    return false;
                }
            }

            public static bool ImportBlock(String fileBlock)
            {
                try
                {
                    AcDb.Database refDb = new AcDb.Database(false, true);
                    refDb.ReadDwgFile(fileBlock, System.IO.FileShare.Read, true, "");
                    string nameBlock = Path.GetFileNameWithoutExtension(fileBlock);
                    AcDb.ObjectId idBTR = db.Insert(AcDb.BlockTableRecord.ModelSpace, nameBlock, refDb, false);

                    if (!idBTR.IsNull) return true;
                }
                catch { }

                return false;
            }

            public static void ImportBlockFromPath(String pathBlocks)
            {
                string[] filesBlock = Directory.GetFiles(pathBlocks,"*.dwg");

                foreach (string fileBlock in filesBlock)
                {
                    if (ImportBlock(fileBlock))
                    {
                        ed.WriteMessage("\nІмпортовано блок '{0}' з файлу '{1}'.", Path.GetFileNameWithoutExtension(fileBlock), fileBlock);
                    }
                    else 
                    {
                        ed.WriteMessage("\nПомилка при імпорті з файлу '{1}'.", Path.GetFileNameWithoutExtension(fileBlock), fileBlock);
                    }
                }
            }

        #endregion  Import

        #region  XData
            public static void SetXData(AcDb.Entity entity, AcDb.ResultBuffer valueXData)
            {
                SetXData (entity.ObjectId,  valueXData);
            }

            public static void SetXData(AcDb.ObjectId objectId, AcDb.ResultBuffer valueXData)
            {
                AcAp.Document doc = AcApp.DocumentManager.MdiActiveDocument;

                using (AcDb.Transaction tr = doc.TransactionManager.StartTransaction())
                {
                    AcDb.DBObject obj = tr.GetObject(objectId, AcDb.OpenMode.ForWrite);
                    AddRegAppTableRecord((string)valueXData.AsArray()[0].Value);
                    obj.XData = valueXData;
                    valueXData.Dispose();
                    tr.Commit();
                }
            }

            static void AddRegAppTableRecord(string regAppName)
            {
                AcAp.Document doc = AcApp.DocumentManager.MdiActiveDocument;
                AcEd.Editor ed = doc.Editor;
                AcDb.Database db = doc.Database;

                using (AcDb.Transaction tr = doc.TransactionManager.StartTransaction())
                {
                    AcDb.RegAppTable rat = 
                        (AcDb.RegAppTable)tr.GetObject(db.RegAppTableId, AcDb.OpenMode.ForRead, false);
                    if (!rat.Has(regAppName))
                    {
                        rat.UpgradeOpen();
                        AcDb.RegAppTableRecord ratr = new AcDb.RegAppTableRecord();
                        ratr.Name = regAppName;
                        rat.Add(ratr);
                        tr.AddNewlyCreatedDBObject(ratr, true);
                    }
                    tr.Commit();
                }
            }

            public static AcDb.ResultBuffer GetXData(AcDb.Entity entity)
            {
                return GetXData (entity.ObjectId);
            }

            public static AcDb.ResultBuffer GetXData(AcDb.ObjectId objectId)
            {
                AcAp.Document doc = AcApp.DocumentManager.MdiActiveDocument;

                using (AcDb.Transaction tr = doc.TransactionManager.StartTransaction())
                {
                    AcDb.DBObject obj = tr.GetObject(objectId, AcDb.OpenMode.ForRead);
                    AcDb.ResultBuffer resultBuffer = obj.XData;
                    if (resultBuffer == null) return null;
                    else return resultBuffer;
                }
            }

        #endregion  XData

        #region CAD

            public static AcDb.ObjectId CreateLayer(String layerName)
            {
                AcDb.ObjectId layerId;

                using (AcDb.Transaction tr = db.TransactionManager.StartTransaction())
                {
                    AcDb.LayerTable layerTable = (AcDb.LayerTable)tr.GetObject(db.LayerTableId, AcDb.OpenMode.ForWrite);

                    if (layerTable.Has(layerName))
                    {
                        layerId = layerTable[layerName];
                    }
                    else
                    {
                        AcDb.LayerTableRecord layerTableRecord = new AcDb.LayerTableRecord();
                        layerTableRecord.Name = layerName;
                        layerId = layerTable.Add(layerTableRecord);
                        tr.AddNewlyCreatedDBObject(layerTableRecord, true);
                    }
                    tr.Commit();
                }
                return layerId;
            }
        #endregion CAD
    }

}
