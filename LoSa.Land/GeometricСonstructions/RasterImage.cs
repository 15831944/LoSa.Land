
using LoSa.CAD;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace LoSa.Land.GeometricСonstructions
{
    public class LandRasterImage
    {
        public string ImageName { get; set; }

        public string FileName { get; set; }

        public AcGe.Point3d InsertPoint { get; set; }

        public AcGe.Vector3d Scale { get; set; }

        public string Layer { get; set; }

        public AcGe.Matrix3d UCS
        {
            get { return AcApp.DocumentManager
                                .MdiActiveDocument
                                .Editor.CurrentUserCoordinateSystem; }
        }

        public LandRasterImage()
        {

        }
#if BCAD
        [AcTrx.CommandMethod("Land_AddRaster2000")]
        public static void AddRaster2000()
        {
            /*
            LandRasterImage landRastr =
                SelectingRasterOnClick("Растр М 1:2000", LocalPath.FindFullPathFromXml("RasterImage2000"));
            if (landRastr != null)
            {
                AttachRasterImage(landRastr);
            }
            */
        }
#endif
        
        [AcTrx.CommandMethod("Land_DelRaster2000")]
        public static void DelRaster2000()
        {
            /*
            LandRasterImage landRastr =
               SelectingRasterOnClick("Растр М 1:2000", LocalPath.FindFullPathFromXml("RasterImage2000"));
            landRastr.Layer = "Растр М 1:2000";
            if (landRastr != null)
            {
               DetachRasterImage(landRastr);
            }
            */
        }
        

        public static LandRasterImage SelectingRasterOnClick(string key, string searchPath)
        {
            AcEd.Editor ed = AcApp.DocumentManager.MdiActiveDocument.Editor;

            LandRasterImage landRastr = null;
            AcEd.PromptPointOptions ppo = new AcEd.PromptPointOptions("\n" + key + " > Вкажіть точку вставки растру: ");
            ppo.UseBasePoint = false;
            var ppr = ed.GetPoint(ppo);
            if (ppr.Status == AcEd.PromptStatus.OK)
            {
                landRastr = new LandRasterImage();
                AcGe.Point3d tmpPt = ppr.Value.TransformBy(landRastr.UCS.Inverse());
                int valueKmX = (int)(tmpPt.X/1000);
                int valueKmY = (int)(tmpPt.Y/1000);
                string imgName = (valueKmY + 1).ToString("00") + valueKmX.ToString("00");
                landRastr.ImageName = key + "-" + imgName;
                string[] filesName = Directory.GetFiles(searchPath, imgName + ".*");

                if (filesName.Length < 1)
                {
                    ed.WriteMessage("\n" + key + "В указаной точці знайдено растр '{0}.*'.", landRastr.ImageName );
                    return null;
                }
                else if (filesName.Length == 1)
                {
                    landRastr.FileName = filesName[0];
                }
                else if (filesName.Length > 1)
                {
                    AcEd.PromptKeywordOptions pko = new AcEd.PromptKeywordOptions("\nВиберіть растр ");

                    for (int i = 0; i < filesName.Length; i++)
                    {
                        string nameRastr = Path.GetFileName(filesName[i]);
                        if (nameRastr.IndexOf(".bmp") > -1 ||
                            nameRastr.IndexOf(".tif") > -1 || 
                            nameRastr.IndexOf(".jpg") > -1)
                        {
                            pko.Keywords.Add("<" + nameRastr  
                                                .Replace(".", "> ")
                                                .Replace('b', 'B')
                                                .Replace('j', 'J')
                                                .Replace('t', 'T'));
                        }
                    }

                    pko.Keywords.Default = Path.GetFileName("<" + filesName[0])
                                                                    .Replace(".", "> ")
                                                                    .Replace('b', 'B')
                                                                    .Replace('j', 'J').Replace('t', 'T');
                    pko.AllowNone = false;


                    AcEd.PromptResult pr = ed.GetKeywords(pko);

                    landRastr.FileName = searchPath + pr.StringResult.Replace(' ', '.');
                }

                
                landRastr.InsertPoint = new AcGe.Point3d(Convert.ToDouble(valueKmX)*1000, Convert.ToDouble(valueKmY)*1000, 0);
            
            }

            return landRastr;
        }


#if BCAD
        public static void AttachRasterImage(LandRasterImage landRastr)
        {

            AcEd.Editor ed = AcApp.DocumentManager.MdiActiveDocument.Editor;

            //bool bRasterDefCreated = false;
            
            AcDb.Database curDb = AcApp.DocumentManager.MdiActiveDocument.Database;

            using (AcDb.Transaction tr = curDb.TransactionManager.StartTransaction())
            {
                AcDb.RasterImageDef rasterDef;
                AcDb.ObjectId imgDefId;
                AcDb.ObjectId imgDctID = AcDb.RasterImageDef.GetImageDictionary(curDb);
                if (imgDctID.IsNull)
                {
                    imgDctID = AcDb.RasterImageDef.CreateImageDictionary(curDb);
                }

                AcDb.DBDictionary imgDict = (AcDb.DBDictionary)tr.GetObject(imgDctID, AcDb.OpenMode.ForRead);
                
                if (imgDict.Contains(landRastr.ImageName))
                {
                    imgDefId = imgDict.GetAt(landRastr.ImageName);
                    rasterDef = (AcDb.RasterImageDef)tr.GetObject(imgDefId, AcDb.OpenMode.ForWrite);
                    if (rasterDef.IsLoaded)
                    {
                        //return;
                    }
                }
                else
                {
                    AcPApp.AcadApplication app = AcApp.AcadApplication as AcPApp.AcadApplication;
                    AcPApp.AcadDocument doc = app.ActiveDocument;
                    AcPDb.AcadModelSpace mSpace = doc.ModelSpace;
                    AcPDb.AcadRasterImage ri = mSpace.AddRaster(landRastr.FileName, landRastr.InsertPoint.ToArray(), 1000, 0);
                    ri.Name = landRastr.ImageName;
                    ri.Transparency = true;
                    ServiceCAD.CreateLayer(landRastr.Layer);
                    ri.Layer = landRastr.Layer;
                    ri.ImageHeight = 1000;
                    ri.ImageWidth = 1000;
                    
                    tr.Commit();
                    return;
                }

                AcDb.BlockTable blkTbl = 
                    (AcDb.BlockTable)tr.GetObject(curDb.BlockTableId, AcDb.OpenMode.ForRead);

                AcDb.BlockTableRecord acBlkTblRec =
                    (AcDb.BlockTableRecord)tr.GetObject(blkTbl[AcDb.BlockTableRecord.ModelSpace], AcDb.OpenMode.ForWrite);

                using (AcDb.RasterImage rasterImage = new AcDb.RasterImage())
                {
                    rasterImage.ImageDefId = imgDefId;

                    AcGe.Vector3d width;
                    AcGe.Vector3d height;

                    AcGe.Matrix3d ucs = ed.CurrentUserCoordinateSystem;
                    double size = 1000;

                    width = new AcGe.Vector3d(size, 0, 0);
                    height = new AcGe.Vector3d(0, size, 0);


                    AcGe.Point3d insPt = landRastr.InsertPoint;
                    AcGe.CoordinateSystem3d coordinateSystem = new AcGe.CoordinateSystem3d(insPt.TransformBy(ucs), width.TransformBy(ucs), height.TransformBy(ucs));
                    rasterImage.Orientation = coordinateSystem;
                    rasterImage.Rotation = 0;
                    rasterImage.ShowImage = true;

                    acBlkTblRec.AppendEntity(rasterImage);
                    tr.AddNewlyCreatedDBObject(rasterImage, true);

                    rasterImage.AssociateRasterDef(rasterDef);
                }
                tr.Commit();

            }
        }
#endif

        public static void DetachRasterImage(LandRasterImage landRastr)
        {
            AcDb.Database curDb = AcApp.DocumentManager.MdiActiveDocument.Database;
            using (AcDb.Transaction tr = curDb.TransactionManager.StartTransaction())
            {
                AcDb.RasterImageDef rasterDef;
                //bool bRasterDefCreated = false;
                AcDb.ObjectId imgDefId;

                AcDb.ObjectId imgDctID = AcDb.RasterImageDef.GetImageDictionary(curDb);
                if (imgDctID.IsNull)
                {
                    imgDctID = AcDb.RasterImageDef.CreateImageDictionary(curDb);
                }

                AcDb.DBDictionary imgDict = tr.GetObject(imgDctID, AcDb.OpenMode.ForWrite) as AcDb.DBDictionary;

                if (imgDict.Contains(landRastr.ImageName))
                {
                    imgDefId = imgDict.GetAt(landRastr.ImageName);
                    rasterDef = tr.GetObject(imgDefId, AcDb.OpenMode.ForWrite) as AcDb.RasterImageDef;
                    if (rasterDef.IsLoaded) 
                    {
                        rasterDef.Unload(true);
                        imgDict.Remove(landRastr.ImageName);
                         
                    }
                }
                tr.Commit();

            }
        }

    }
}
