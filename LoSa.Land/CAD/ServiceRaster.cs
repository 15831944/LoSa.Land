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
using System.Text;
using System.Windows.Forms;
using LoSa.CAD;

namespace LoSa.Land.CAD
{
    /// <summary>
    /// Service Raster
    /// </summary>
    public static class ServiceRaster
    {

        public static string GetFileNameRaster()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Файли растру |*.jpg;*.jpeg;*.tif;*.tiff;*.bmp",
                Title = "Вибір файлу растру"
            };
            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                return openFileDialog.FileName;
            }

            return null;
        }

        public static string GetPathRasters()
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog()
            {
                Description = "Вибір шлях до теки растрів"
            };
            if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            { 
                return folderBrowserDialog.SelectedPath;
            }

            return null;
         }

        public static AcGe.Point3d GetLocationRaster()
        {

            AcEd.PromptPointResult pPtRes;
            AcEd.PromptPointOptions pPtOpts = new AcEd.PromptPointOptions("*-*-")
            {
                Message = "\nВкажіть місце вставки растру: "
            };
            pPtRes = CurrentCAD.Editor.GetPoint(pPtOpts);

            if (pPtRes.Status == AcEd.PromptStatus.OK)
            {
                return pPtRes.Value;
            }

            return AcGe.Point3d.Origin;
        }

        public static void AttachRasterImage()
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
                    acRaster.Orientation = new AcGe.CoordinateSystem3d(insPt, new AcGe.Vector3d(1000, 0, 0), new AcGe.Vector3d(0, 2000, 0));

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
    }
}
