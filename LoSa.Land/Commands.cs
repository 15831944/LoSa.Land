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
using AcPApp =  Autodesk.AutoCAD.Interop; 


#endif

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Reflection;
using System.Collections.Specialized;

using LoSa.Land.Doc;
using LoSa.Land.EnumAttributes;
using LoSa.CAD;
using LoSa.Land.Tables;
using LoSa.Land.Forms;
using LoSa.Land.Parcel;
using LoSa.Utility;
using LoSa.Xml;
using System.IO;
using Teigha.Runtime;
using LoSa.Land.ObjectGeo;
using System.Windows.Forms;
using LoSa.Land.CAD;

namespace LoSa.Land
{
    /// <summary>
    /// Land Commands
    /// </summary>
    /// <seealso cref="Teigha.Runtime.IExtensionApplication" />
    public class LandCommands: AcTrx.IExtensionApplication
    {

        LocalPath LocalPath = new LocalPath("LoSa_Land");

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            ReStartImportBlock:

            try
            {
                ServiceCAD.ImportBlockFromPath(LocalPath.RootDirectory + "\\blocks\\");
            }
            catch (DirectoryNotFoundException dirExc)
            {
                string msgDirExc = "<!> \n<!>" + dirExc.Message + "\n<!>";
                
                CurrentCAD.Editor.WriteMessage(msgDirExc);

                Directory.CreateDirectory(LocalPath.RootDirectory + "\\blocks\\");
                goto ReStartImportBlock;
            }

            string msgPorgam = "\n--------------------------------------";
            msgPorgam += "\n > Name: " + assembly.GetName().Name;
            msgPorgam += "\n > Версия: " + assembly.GetName().Version;
            msgPorgam += "\n---------------------------------------";
            msgPorgam += "\n > email: artem.loban@gmail.com ";
            msgPorgam += "\n--------------------------------------";
            msgPorgam += "\n *Land_CreatingPlanLand - ";
            msgPorgam += "\n Land_IntersectionGridLines - ";
            msgPorgam += "\n Land_InsertBlocksAlongLine - ";
            msgPorgam += "\n Land_OrthogonalPolylines - ";
            msgPorgam += "\n Land_BuildingRectangle - ";
            msgPorgam += "\n Land_AddRaster2000 - ";
            msgPorgam += "\n Land_DelRaster2000 - ";
            msgPorgam += "\n--------------------------------------";

            
            CurrentCAD.Editor.WriteMessage(msgPorgam);
           
        }

        /// <summary>
        /// Terminates this instance.
        /// </summary>
        public void Terminate()
        {

        }

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        [STAThread]
        public static void Main(string[] args)
        {

        }

        private FormLand frmLand;

        /// <summary>
        /// Creatings the plan land.
        /// </summary>
        [AcTrx.CommandMethod("Land_CreatingPlanLand")]
        public void CreatingPlanLand()
        {
            frmLand = new FormLand();
            frmLand.Show();
        }

    }

    /// <summary>
    /// Geo Commands
    /// </summary>
    /// <seealso cref="Teigha.Runtime.IExtensionApplication" />
    public class GeoCommands : AcTrx.IExtensionApplication
    {

        private LocalPath localPath = new LocalPath();

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {

        }

        /// <summary>
        /// Terminates this instance.
        /// </summary>
        public void Terminate()
        {

        }

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        [STAThread]
        public static void Main(string[] args)
        {

        }


    }

    /// <summary>
    /// Test Commands
    /// </summary>
    /// <seealso cref="Teigha.Runtime.IExtensionApplication" />
    public class TestCommands : AcTrx.IExtensionApplication
    {

        private LocalPath localPath = new LocalPath();

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        public void Initialize()
        {
            string msgPorgam = "\n---------------------------------------";
            msgPorgam += "\n            TestCommands ";
            msgPorgam += "\n--------------------------------------";
            msgPorgam += "\n Test_CreatingLocalPaths - ";
            msgPorgam += "\n Test_IT - ";
            msgPorgam += "\n Test_IMT - ";
            msgPorgam += "\n Test_GXD - ";
            msgPorgam += "\n Test_SXD - ";
            msgPorgam += "\n Test_AddRaster2000 - ";
            msgPorgam += "\n Test_DelRaster2000 - ";
            msgPorgam += "\n--------------------------------------";

            CurrentCAD.Editor.WriteMessage(msgPorgam);
        }

        /// <summary>
        /// Terminates this instance.
        /// </summary>
        public void Terminate()
        {

        }

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        /// <param name="args">The arguments.</param>
        [STAThread]
        public static void Main(string[] args)
        {

        }


        /// <summary>
        /// Creatings the local paths.
        /// </summary>
        [AcTrx.CommandMethod("Test_CreatingLocalPaths")]
        public void CreatingLocalPaths()
        {
            LocalPath localPath = new LocalPath();

            localPath.Paths.Add(new Setting("key1", "name1", "description1"));
            localPath.Paths.Add(new Setting("key2", "name2", "description2"));
            localPath.Paths.Add(new Setting("key3", "name3", "description3"));

            ServiceXml.WriteXml<LocalPath>(localPath, "p:\\LocalPaths.xml");
        }

        /// <summary>
        /// Inserts the text placement jig.
        /// </summary>
        [AcTrx.CommandMethod("Test_IT")]
        public void InsertTextPlacementJig()
        {
            AcDb.DBText oText = new AcDb.DBText
            {
                TextString = "Test String TExt",
                Height = 5.33
            };

            //ServiceSimpleElements.ManualInsertText(oText);
        }

        /// <summary>
        /// Inserts the MText placement jig.
        /// </summary>
        [AcTrx.CommandMethod("Test_IMT")]
        public void InsertMTextPlacementJig()
        {
            AcDb.MText oMText = new AcDb.MText
            {
                TextHeight = 7.37,
                Attachment = AcDb.AttachmentPoint.MiddleLeft,
                Contents = "Test Value #"
            };

            //ServiceSimpleElements.ManualInsertMText(oMText);
        }

        /// <summary>
        /// Gets the XData.
        /// </summary>
        [CommandMethod("Test_GXD")]
        static public void GetXData()
        {
            AcAp.Document doc = AcApp.DocumentManager.MdiActiveDocument;
            AcEd.Editor ed = doc.Editor;

            AcEd.PromptEntityOptions opt = new AcEd.PromptEntityOptions("\nSelect entity: ");
            AcEd.PromptEntityResult res = ed.GetEntity(opt);

            if (res.Status == AcEd.PromptStatus.OK)
            {

                using (AcDb.Transaction tr = doc.TransactionManager.StartTransaction())
                {
                    AcDb.DBObject obj = tr.GetObject(res.ObjectId, AcDb.OpenMode.ForRead);
                    AcDb.ResultBuffer rb = obj.XData;
                    if (rb == null)
                    {
                        ed.WriteMessage("\nEntity does not have XData attached.");
                    }
                    else
                    {
                        int n = 0;
                        foreach (AcDb.TypedValue tv in rb)
                        {
                            ed.WriteMessage("\nTypedValue {0} - type: {1}, value: {2}", n++, tv.TypeCode, tv.Value);
                        }
                        rb.Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// Sets the XData.
        /// </summary>
        [CommandMethod("Test_SXD")]
        static public void SetXData()
        {
            AcAp.Document doc = AcApp.DocumentManager.MdiActiveDocument;
            AcEd.Editor ed = doc.Editor;

            AcEd.PromptEntityOptions opt = new AcEd.PromptEntityOptions("\nSelect entity: ");
            AcEd.PromptEntityResult res = ed.GetEntity(opt);

            if (res.Status == AcEd.PromptStatus.OK)
            {

                using (AcDb.Transaction tr = doc.TransactionManager.StartTransaction())
                {
                    AcDb.DBObject obj = tr.GetObject(res.ObjectId, AcDb.OpenMode.ForWrite);
                    AddRegAppTableRecord("KEAN");
                    AcDb.ResultBuffer rb = new AcDb.ResultBuffer(
                       new AcDb.TypedValue(1001, "KEAN"),
                       new AcDb.TypedValue(1000, "This is a test string")
                     );
                    obj.XData = rb;
                    rb.Dispose();
                    tr.Commit();
                }
            }
        }

        /// <summary>
        /// Adds the RegAppTableRecord.
        /// </summary>
        /// <param name="regAppName">Name of the reg application.</param>
        static void AddRegAppTableRecord(string regAppName)
        {
            AcAp.Document doc = AcApp.DocumentManager.MdiActiveDocument;
            AcEd.Editor ed = doc.Editor;
            AcDb.Database db = doc.Database;

            using (AcDb.Transaction tr = doc.TransactionManager.StartTransaction())
            {
                AcDb.RegAppTable rat = (AcDb.RegAppTable)tr.GetObject(db.RegAppTableId, AcDb.OpenMode.ForRead, false);
                if (!rat.Has(regAppName))
                {
                    rat.UpgradeOpen();
                    AcDb.RegAppTableRecord ratr = new AcDb.RegAppTableRecord
                    {
                        Name = regAppName
                    };
                    rat.Add(ratr);
                    tr.AddNewlyCreatedDBObject(ratr, true);
                }
                tr.Commit();
            }
        }

        /// <summary>
        /// Adds the rastr.
        /// </summary>
        [CommandMethod("Test_AddRastr")]
        static public void AddRastr()
        {
            //CurrentCAD.Editor.WriteMessage(ServiceRaster.GetLocationRaster());
            CurrentCAD.Editor.WriteMessage(ServiceRaster.GetPathRasters());
        }

        [CommandMethod("AttachRasterImage")]
        public void AttachRasterImage()
        {
            // Get the current database and start a transaction
            AcDb.Database acCurDb;
            acCurDb = AcAp.Application.DocumentManager.MdiActiveDocument.Database;

            using (AcDb.Transaction acTrans = acCurDb.TransactionManager.StartTransaction())
            {
                // Define the name and image to use
                string strImgName = "5525";
                string strFileName = "C:\\_Temp_\\5525.TIF";

                AcDb.RasterImageDef acRasterDef;
                bool bRasterDefCreated = false;
                AcDb.ObjectId acImgDefId;

                // Get the image dictionary
                AcDb.ObjectId acImgDctID = AcDb.RasterImageDef.GetImageDictionary(acCurDb);

                // Check to see if the dictionary does not exist, it not then create it
                if (acImgDctID.IsNull)
                {
                    acImgDctID = AcDb.RasterImageDef.CreateImageDictionary(acCurDb);
                }

                // Open the image dictionary
                AcDb.DBDictionary acImgDict = acTrans.GetObject(acImgDctID, AcDb.OpenMode.ForRead) as AcDb.DBDictionary;

                // Check to see if the image definition already exists
                if (acImgDict.Contains(strImgName))
                {
                    acImgDefId = acImgDict.GetAt(strImgName);

                    acRasterDef = acTrans.GetObject(acImgDefId, AcDb.OpenMode.ForWrite) as AcDb.RasterImageDef;
                }
                else
                {
                    // Create a raster image definition
                    AcDb.RasterImageDef acRasterDefNew = new AcDb.RasterImageDef
                    {
                        // Set the source for the image file
                        SourceFileName = strFileName
                    };

                    acImgDict.UpgradeOpen();
                    acImgDefId = acImgDict.SetAt(strImgName, acRasterDefNew);
                    
                        // Load the image into memory
                     acRasterDefNew.Load();
                    

                    // Add the image definition to the dictionary
                    ////acImgDict.UpgradeOpen();
                    ////acImgDefId = acImgDict.SetAt(strImgName, acRasterDefNew);

                    acTrans.AddNewlyCreatedDBObject(acRasterDefNew, true);

                    /***/

                    acRasterDef = acRasterDefNew;

                    bRasterDefCreated = true;
                }

                // Open the Block table for read
                AcDb.BlockTable acBlkTbl;
                acBlkTbl = acTrans.GetObject(acCurDb.BlockTableId, AcDb.OpenMode.ForRead) as AcDb.BlockTable;

                // Open the Block table record Model space for write
                AcDb.BlockTableRecord acBlkTblRec;
                acBlkTblRec = acTrans.GetObject(acBlkTbl[AcDb.BlockTableRecord.ModelSpace],
                                                AcDb.OpenMode.ForWrite) as AcDb.BlockTableRecord;


                // Create the new image and assign it the image definition
                using (AcDb.RasterImage acRaster = new AcDb.RasterImage())
                {
                    acRaster.ImageDefId = acImgDefId;

                    // Use ImageWidth and ImageHeight to get the size of the image in pixels (1024 x 768).
                    // Use ResolutionMMPerPixel to determine the number of millimeters in a pixel so you 
                    // can convert the size of the drawing into other units or millimeters based on the 
                    // drawing units used in the current drawing.

                    // Define the width and height of the image
                    AcGe.Vector3d width;
                    AcGe.Vector3d height;

                    // Check to see if the measurement is set to English (Imperial) or Metric units
                    if (acCurDb.Measurement == AcDb.MeasurementValue.English)
                    {
                        width = new AcGe.Vector3d((acRasterDef.ResolutionMMPerPixel.X * acRaster.ImageWidth) / 25.4, 0, 0);
                        height = new AcGe.Vector3d(0, (acRasterDef.ResolutionMMPerPixel.Y * acRaster.ImageHeight) / 25.4, 0);
                    }
                    else
                    {
                        width = new AcGe.Vector3d(acRasterDef.ResolutionMMPerPixel.X * acRaster.ImageWidth, 0, 0);
                        height = new AcGe.Vector3d(0, acRasterDef.ResolutionMMPerPixel.Y * acRaster.ImageHeight, 0);
                    }

                    // Define the position for the image 
                    AcGe.Point3d insPt = new AcGe.Point3d(52000.0, 56000.0, 100.0);

                    // Define and assign a coordinate system for the image's orientation
                    AcGe.CoordinateSystem3d coordinateSystem = new AcGe.CoordinateSystem3d(insPt, width * 2, height * 2);
                    acRaster.Orientation = coordinateSystem;

                    // Set the rotation angle for the image
                    acRaster.Rotation = 0;

                    // Add the new object to the block table record and the transaction
                    acBlkTblRec.AppendEntity(acRaster);
                    acTrans.AddNewlyCreatedDBObject(acRaster, true);

                    // Connect the raster definition and image together so the definition
                    // does not appear as "unreferenced" in the External References palette.
                    /*AcDb.RasterImage.EnableReactors(true);*/
                    acRaster.AssociateRasterDef(acRasterDef);
                    acRaster.ShowImage = true;
                    acRaster.ColorIndex = 200;
                    acRaster.ImageTransparency = true;
                    acRaster.Orientation = new AcGe.CoordinateSystem3d(insPt, new AcGe.Vector3d(1000,0,0), new AcGe.Vector3d(0,2000,0));

                    if (bRasterDefCreated)
                    {
                        acRasterDef.Dispose();
                    }
                }

                // Save the new object to the database
                acTrans.Commit();

                // Dispose of the transaction
            }
        }

        private void SubVarianLoadRaster()
        {
            String PIC_NAME = "C:\\_Temp_\\5525.TIF";
            AcDb.Database CurDb = AcAp.Application.DocumentManager.MdiActiveDocument.Database;
            

            using (AcDb.Transaction acTrans = CurDb.TransactionManager.StartTransaction())
            {
                try
                {
                    AcDb.ObjectId dictId = AcDb.RasterImageDef.GetImageDictionary(CurDb);
                    if (dictId == null) { dictId = AcDb.RasterImageDef.CreateImageDictionary(CurDb); }

                    AcDb.DBDictionary dict = acTrans.GetObject(dictId, AcDb.OpenMode.ForRead) as AcDb.DBDictionary;
                    String recName = "5525";
                    AcDb.RasterImageDef rid = new AcDb.RasterImageDef
                    {
                        SourceFileName = PIC_NAME
                    };
                    dict.UpgradeOpen();
                    AcDb.ObjectId defId = dict.SetAt(recName, rid);

                    rid.Load();

                    acTrans.AddNewlyCreatedDBObject(rid, true);
                    AcDb.RasterImage ri = new AcDb.RasterImage
                    {
                        ImageDefId = defId,
                        ShowImage = true,
                        Orientation = new AcGe.CoordinateSystem3d(new AcGe.Point3d(200, 300, 0), new AcGe.Vector3d(1, 0, 0), new AcGe.Vector3d(0, 1, 0))
                    };
                    AcDb.BlockTable bt = acTrans.GetObject(CurDb.BlockTableId, AcDb.OpenMode.ForRead, false) as AcDb.BlockTable;
                    AcDb.BlockTableRecord btr = acTrans.GetObject(bt[AcDb.BlockTableRecord.ModelSpace], AcDb.OpenMode.ForWrite, false) as AcDb.BlockTableRecord;
                    btr.AppendEntity(ri);
                    acTrans.AddNewlyCreatedDBObject(ri, true);
                    ri.AssociateRasterDef(rid);
                    ri.TransformBy(AcGe.Matrix3d.Scaling(1000, new AcGe.Point3d(200, 300, 0)));
                }
            catch
            {
                return;
            }
        }
            /*
                   Try
                            Dim PIC_NAME As String = original_picture_name
                            Dim dictId As ObjectId = RasterImageDef.GetImageDictionary(Db)
                            If dictId = Nothing Then
                                dictId = RasterImageDef.CreateImageDictionary(Db)
                            End If
                            Dim dict As DBDictionary = CType(acTrans.GetObject(dictId, OpenMode.ForRead), DBDictionary)
                            Dim recName As String = Picture_name_in_drawing
                            Dim rid As RasterImageDef = New RasterImageDef
                            rid.SourceFileName = PIC_NAME
                            dict.UpgradeOpen()
                            Dim defId As ObjectId = dict.SetAt(recName, rid)
                            rid.Load()
                            acTrans.AddNewlyCreatedDBObject(rid, True)
                            Dim ri As RasterImage = New RasterImage
                            ri.ImageDefId = defId
                            ri.ShowImage = True
                            ri.Orientation = New CoordinateSystem3d(New Point3d(x1, y1, 0), New Vector3d(1, 0, 0), New Vector3d(0, 1, 0))
                            Dim bt As BlockTable
                            Dim btr As BlockTableRecord
                            bt = acTrans.GetObject(Db.BlockTableId, OpenMode.ForRead, False)
                            btr = acTrans.GetObject(bt(BlockTableRecord.ModelSpace), OpenMode.ForWrite, False)
                            btr.AppendEntity(ri)
                            acTrans.AddNewlyCreatedDBObject(ri, True)
                            ri.AssociateRasterDef(rid)
                            ri.TransformBy(Matrix3d.Scaling(picture_scale, New Point3d(x1, y1, 0)))
                     Catch

                            Exit Sub
                     End Try

    */
        }
    }
}
