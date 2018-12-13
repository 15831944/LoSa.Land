#if BCAD 
using AcAp = Bricscad.ApplicationServices;
using AcDb = Teigha.DatabaseServices;
using AcGe = Teigha.Geometry;
using AcEd = Bricscad.EditorInput;
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
using System.IO;
using LoSa.Land.GeometricСonstructions;

namespace LoSa.CAD
{
    public static class ServiceBlockElements
    {
        private static AcAp.Document doc = CurrentCAD.Document;
        private static AcDb.Database db = CurrentCAD.Database;
        private static AcEd.Editor ed = CurrentCAD.Editor;

        public static string CreateBlock(
            AcDb.DBObjectCollection blockElements, 
            string nameBlock)
        {
            using (AcDb.Transaction tr = db.TransactionManager.StartTransaction())
            {
                AcDb.BlockTable blockTable = tr.GetObject(db.BlockTableId, AcDb.OpenMode.ForRead) as AcDb.BlockTable;
                string oldNameBlock = nameBlock;
                int iNameBlock = 0;
            mark_reNameBlock:
                if (blockTable.Has(nameBlock))
                {
                    iNameBlock++;
                    nameBlock = oldNameBlock + iNameBlock.ToString("_000");
                    goto mark_reNameBlock;
                }


                AcDb.BlockTableRecord blockTableRecord = new AcDb.BlockTableRecord
                {
                    Name = nameBlock
                };
                blockTable.UpgradeOpen();
                AcDb.ObjectId btrId = blockTable.Add(blockTableRecord);
                tr.AddNewlyCreatedDBObject(blockTableRecord, true);

                foreach (AcDb.Entity ent in blockElements)
                {
                    blockTableRecord.AppendEntity(ent);
                    tr.AddNewlyCreatedDBObject(ent, true);
                }

                tr.Commit();
            }

            return nameBlock;
        }

        public static AcDb.ObjectId InsertBlock(
            String nameBlock,
            AcGe.Point3d insertionPoint,
            double scale,
            double rotation)
        {
            return InsertBlock(nameBlock, insertionPoint, scale, rotation, AcDb.ObjectId.Null, null);
        }

        public static AcDb.ObjectId InsertBlock(
            String nameBlock,
            AcGe.Point3d insertionPoint,
            double scale,
            AcDb.ObjectId layerId,
            double rotation)
        {
            return InsertBlock(nameBlock, insertionPoint, scale, rotation, layerId, null);
        }

        public static AcDb.ObjectId InsertBlock(
            string nameBlock,
            AcGe.Point3d insertionPoint,
            double scale,
            double rotation,
            Dictionary<string, string> tags)
        {
            return InsertBlock(nameBlock, insertionPoint, scale, rotation, AcDb.ObjectId.Null, tags);
        }


        public static AcDb.ObjectId InsertBlock(
            string nameBlock,
            AcGe.Point3d insertionPoint,
            double scale,
            double rotation,
            AcDb.ObjectId layerId,
            Dictionary<string, string> tags)
        {
            AcDb.ObjectId idBlock = AcDb.ObjectId.Null;
            using (AcDb.Transaction tr = db.TransactionManager.StartTransaction())
            {
                AcDb.BlockTable blockTable = (AcDb.BlockTable)tr.GetObject(db.BlockTableId, AcDb.OpenMode.ForRead);
                if (!blockTable.Has(nameBlock))
                {
                    ed.WriteMessage("\nНезнайдено блок '{0}' у таблиці блоків креслення.", nameBlock);
                    return idBlock;
                }
                AcDb.BlockTableRecord curSpace = (AcDb.BlockTableRecord)tr.GetObject(db.CurrentSpaceId, AcDb.OpenMode.ForWrite);
                AcDb.BlockReference blockReference = new AcDb.BlockReference(insertionPoint, blockTable[nameBlock])
                {
                    LayerId = layerId,
                    ScaleFactors = new AcGe.Scale3d(scale, scale, scale),
                    Rotation = rotation
                };
                blockReference.TransformBy(ed.CurrentUserCoordinateSystem);
                curSpace.AppendEntity(blockReference);
                tr.AddNewlyCreatedDBObject(blockReference, true);
                if (tags != null)
                {
                    ReplaceAttributeBlock(blockReference, tags, true);
                }
                idBlock = blockReference.ObjectId;

                tr.Commit();
            }
            return idBlock;
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

        public static void ReplaceAttributeBlock(   AcDb.ObjectId idBlock, 
                                                    string tag, 
                                                    string newValue)
        {
            ReplaceAttributeBlock(idBlock, tag, newValue, true);
        }

        public static void ReplaceAttributeBlock(AcDb.ObjectId idBlock, 
                                                    string tag, 
                                                    string newValue, 
                                                    Boolean visible)
        {
            try
            {
                //using (AcDb.Transaction tr = db.TransactionManager.StartTransaction())
                //{
                //    DBObject dbObj = tr.GetObject(idBlock, AcDb.OpenMode.ForRead) as DBObject;
                //    AcDb.BlockReference blockReference = dbObj as BlockReference;
                //    ReplaceAttributeBlock(blockReference, tag, newValue, visible);
                //}


                using (AcDb.Transaction tr = db.TransactionManager.StartTransaction())
                {
                    AcDb.BlockTable acBlockTable = (AcDb.BlockTable)tr.GetObject(db.BlockTableId, AcDb.OpenMode.ForRead);
                    if (acBlockTable == null) return;

                    AcDb.BlockTableRecord acBlockTableRecord = (AcDb.BlockTableRecord)
                        tr.GetObject(acBlockTable[AcDb.BlockTableRecord.ModelSpace], AcDb.OpenMode.ForRead);
                    if (acBlockTableRecord == null) return;

                    //foreach (var blkId in acBlockTableRecord)
                    //{
                        AcDb.BlockReference acBlock = (AcDb.BlockReference)tr.GetObject(idBlock, AcDb.OpenMode.ForRead);
                        //if (acBlock == null) continue;
                        //if (!acBlock.Name.Equals(blockName, StringComparison.CurrentCultureIgnoreCase)) continue;
                        foreach (AcDb.ObjectId attId in acBlock.AttributeCollection)
                        {
                            AcDb.AttributeReference acAtt = (AcDb.AttributeReference)tr.GetObject(attId, AcDb.OpenMode.ForRead);
                            if (acAtt == null) continue;

                            if (!acAtt.Tag.Equals(tag, StringComparison.CurrentCultureIgnoreCase)) continue;

                            acAtt.UpgradeOpen();
                            acAtt.TextString = newValue;
                        }
                    //}
                    tr.Commit();
                }
            }
            catch //(System.Exception exc)
            {

            }
        }

        public static void ReplaceAttributeBlock(   AcDb.ObjectId idBlock,
                                                    Dictionary<string, string> tags,
                                                    Boolean visible)
        {

        }

        public static void ReplaceAttributeBlock(   AcDb.BlockReference blockReference, 
                                                    string tag, 
                                                    string newValue)
        {
            ReplaceAttributeBlock(blockReference, tag, newValue, true);
        }

        public static void ReplaceAttributeBlock(   AcDb.BlockReference blockReference, 
                                                    string tag, 
                                                    string newValue, 
                                                    Boolean visible)
        {

            using (AcDb.Transaction trAdding = db.TransactionManager.StartTransaction())
            {

                AcDb.BlockTable btTable = (AcDb.BlockTable)trAdding.GetObject(db.BlockTableId, AcDb.OpenMode.ForRead);
                AcDb.BlockTableRecord btrTable = (AcDb.BlockTableRecord)btTable[blockReference.Name].GetObject(AcDb.OpenMode.ForRead);

                if (btrTable.HasAttributeDefinitions)
                {
                    var attDefs = btrTable.Cast<AcDb.ObjectId>()
                        .Where(n => n.ObjectClass.Name == "AcDbAttributeDefinition")
                        .Select(n => (AcDb.AttributeDefinition)n.GetObject(AcDb.OpenMode.ForRead));

                    foreach (AcDb.AttributeDefinition attDef in attDefs)
                    {

                        AcDb.AttributeReference attRef = new AcDb.AttributeReference();

                        attRef.SetAttributeFromBlock(attDef, blockReference.BlockTransform);

                        if (attRef.Tag == tag.ToUpper())
                        {
                            attRef.UpgradeOpen();
                            attRef.TextString = newValue;
                            attRef.Visible = visible;
                            attRef.Rotation = 0;
                        }

                        //blockReference.AttributeCollection.AppendAttribute(attRef);
                        //trAdding.AddNewlyCreatedDBObject(attRef, false);

                    }
                    
                }
                else { ed.WriteMessage("\n < ERROR > "); }

                trAdding.Commit();

            }

        }

        public static void ReplaceAttributeBlock(   AcDb.BlockReference blockReference, 
                                                    Dictionary<string, string> tags, 
                                                    Boolean visible)
        {

            using (AcDb.Transaction trAdding = db.TransactionManager.StartTransaction())
            {

                AcDb.BlockTable btTable = (AcDb.BlockTable)trAdding.GetObject(db.BlockTableId, AcDb.OpenMode.ForRead);
                AcDb.BlockTableRecord btrTable = 
                    (AcDb.BlockTableRecord)btTable[blockReference.Name].GetObject(AcDb.OpenMode.ForRead);

                if (btrTable.HasAttributeDefinitions)
                {
                    var attDefs = btrTable.Cast<AcDb.ObjectId>()
                        .Where(n => n.ObjectClass.Name == "AcDbAttributeDefinition")
                        .Select(n => (AcDb.AttributeDefinition)n.GetObject(AcDb.OpenMode.ForRead));

                    foreach (AcDb.AttributeDefinition attDef in attDefs)
                    {
                        AcDb.AttributeReference attRef = new AcDb.AttributeReference();
                        attRef.SetAttributeFromBlock(attDef, blockReference.BlockTransform);

                        foreach ( var tag in tags)
                        {
                            attRef.UpgradeOpen();
                            if (tag.Key.ToUpper() == attRef.Tag)
                            {
                                attRef.TextString = tag.Value;
                            }
                            else
                            {
                                attRef.TextString = "NoneValue";
                            }
                            attRef.Visible = visible;
                            attRef.Rotation = 0;
                        }

                        blockReference.AttributeCollection.AppendAttribute(attRef);
                        trAdding.AddNewlyCreatedDBObject(attRef, false);
                    }

                }
                else { ed.WriteMessage("\n < ERROR > "); }

                trAdding.Commit();
            }

        }


        public static void EditeAttributeBlock(string blockName, string tag, string newValue)
        {
            using (AcDb.Transaction tr = db.TransactionManager.StartTransaction())
            {
                AcDb.BlockTable acBlockTable = (AcDb.BlockTable)tr.GetObject(db.BlockTableId, AcDb.OpenMode.ForRead);
                if (acBlockTable == null) return;

                AcDb.BlockTableRecord acBlockTableRecord = (AcDb.BlockTableRecord)tr.GetObject(acBlockTable[AcDb.BlockTableRecord.ModelSpace], AcDb.OpenMode.ForRead);
                if (acBlockTableRecord == null) return;

                foreach (var blkId in acBlockTableRecord)
                {
                    try
                    {
                        AcDb.BlockReference acBlock = (AcDb.BlockReference)tr.GetObject(blkId, AcDb.OpenMode.ForRead);
                        if (acBlock == null) continue;
                        if (!acBlock.Name.Equals(blockName, StringComparison.CurrentCultureIgnoreCase)) continue;
                        foreach (AcDb.ObjectId attId in acBlock.AttributeCollection)
                        {
                            var acAtt = tr.GetObject(attId, AcDb.OpenMode.ForRead) as AcDb.AttributeReference;
                            if (acAtt == null) continue;

                            if (!acAtt.Tag.Equals(tag, StringComparison.CurrentCultureIgnoreCase)) continue;

                            acAtt.UpgradeOpen();
                            acAtt.TextString = newValue;
                        }
                    }
                    catch
                    {

                    }
                }

                tr.Commit();
            }
        }

        #region ManualInsert

            public static AcDb.ObjectId ManualInsertBlock(string nameBlock, double scaleBlock)
            {
                return ManualInsertBlock(nameBlock, scaleBlock, null);
            }

            public static AcDb.ObjectId ManualInsertBlock(string nameBlock,
                                                        double scaleBlock,
                                                        Dictionary<string, string> tags)
            {

                AcDb.ObjectId idBlock = AcDb.ObjectId.Null;
                using (AcDb.Transaction tr = db.TransactionManager.StartTransaction())
                {
                    AcDb.BlockTable blockTable = (AcDb.BlockTable)tr.GetObject(db.BlockTableId, AcDb.OpenMode.ForRead);
                    if (!blockTable.Has(nameBlock))
                    {
                        ed.WriteMessage("\nНезнайдено блок '{0}' у таблиці блоків креслення.", nameBlock);
                        return idBlock;
                    }
                    AcDb.BlockTableRecord curSpace = (AcDb.BlockTableRecord)tr.GetObject(db.CurrentSpaceId, AcDb.OpenMode.ForWrite);
                AcDb.BlockReference blockReference = new AcDb.BlockReference(AcGe.Point3d.Origin, blockTable[nameBlock])
                {
                    ScaleFactors = new AcGe.Scale3d(scaleBlock, scaleBlock, scaleBlock)
                };
                blockReference.TransformBy(ed.CurrentUserCoordinateSystem);
                    curSpace.AppendEntity(blockReference);

                    tr.AddNewlyCreatedDBObject(blockReference, true);

                    AcDb.BlockTableRecord btr = (AcDb.BlockTableRecord)tr.GetObject(blockTable[nameBlock], AcDb.OpenMode.ForRead);
                    BlockPlacementJig jig = new BlockPlacementJig(blockReference, tags);
                    AcEd.PromptResult pr = ed.Drag(jig);

                    if (pr.Status != AcEd.PromptStatus.OK) blockReference.Erase();

                    tr.Commit();
                }

                return idBlock;
            }

            public static void ManualInsertbAttribute(string nameBlock)
            {
                using (AcDb.Transaction tr = db.TransactionManager.StartTransaction())
                {
                    AcDb.BlockTable blockTable = (AcDb.BlockTable)tr.GetObject(db.BlockTableId, AcDb.OpenMode.ForRead);
                    if (!blockTable.Has(nameBlock))
                    {
                        ed.WriteMessage("\nНезнайдено блок '{0}' у таблиці блоків креслення.", nameBlock);
                        return;
                    }
                    AcDb.BlockTableRecord curSpace = (AcDb.BlockTableRecord)tr.GetObject(db.CurrentSpaceId, AcDb.OpenMode.ForWrite);
                    AcDb.BlockReference blockReference = new AcDb.BlockReference(AcGe.Point3d.Origin, blockTable[nameBlock]);
                    blockReference.TransformBy(ed.CurrentUserCoordinateSystem);
                    curSpace.AppendEntity(blockReference);
                    tr.AddNewlyCreatedDBObject(blockReference, true);

                    AcDb.BlockTableRecord btr = (AcDb.BlockTableRecord)tr.GetObject(blockTable[nameBlock], AcDb.OpenMode.ForRead);
                    AcDb.DBText text;
                    foreach (AcDb.ObjectId id in btr)
                    {
                        if (id.ObjectClass.Name == "AcDbAttributeDefinition")
                        {
                            AcDb.AttributeDefinition attDef =
                                    (AcDb.AttributeDefinition)tr.GetObject(id, AcDb.OpenMode.ForRead);

                        text = new AcDb.DBText
                        {
                            TextString = "jig_test"
                        };

                        TextPlacementJig jig = new TextPlacementJig(text);

                            //PromptResult pr = ed.Drag(jig);

                            AcEd.PromptStatus stat = AcEd.PromptStatus.Keyword;
                            while (stat == AcEd.PromptStatus.Keyword)
                            {
                                AcEd.PromptResult pr = ed.Drag(jig);
                                stat = pr.Status;
                                if (stat != AcEd.PromptStatus.OK && stat != AcEd.PromptStatus.Keyword) { return; }
                            }

                            AcDb.AttributeReference attRef = new AcDb.AttributeReference();
                            attRef.SetAttributeFromBlock(attDef, blockReference.BlockTransform);
                            AcDb.ObjectId attId = blockReference.AttributeCollection.AppendAttribute(attRef);
                            tr.AddNewlyCreatedDBObject(attRef, true);


                            tr.Commit();
                            //if (pr.Status != PromptStatus.OK) blockReference.Erase();
                        }
                    }

                    //tr.Commit();
                }
            }

        #endregion ManualInsert

    }

}
