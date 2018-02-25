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

namespace LoSa.Land
{
    public class LandCommands: AcTrx.IExtensionApplication
    {

        LocalPath LocalPath = new LocalPath("LoSa_Land");

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
            msgPorgam += "\n Land_CreatingPlanLand - ";
            msgPorgam += "\n Land_IntersectionGridLines - ";
            msgPorgam += "\n Land_InsertBlocksAlongLine - ";
            msgPorgam += "\n Land_OrthogonalPolylines - ";
            msgPorgam += "\n Land_BuildingRectangle - ";
            msgPorgam += "\n Land_AddRaster2000 - ";
            msgPorgam += "\n Land_DelRaster2000 - ";
            msgPorgam += "\n--------------------------------------";

            
            CurrentCAD.Editor.WriteMessage(msgPorgam);
           
        }

        public void Terminate()
        {

        }

        [STAThread]
        public static void Main(string[] args)
        {

        }

        public FormLand frmLand = new FormLand();

        [AcTrx.CommandMethod("Land_CreatingPlanLand")]
        public void CreatingPlanLand()
        {
            frmLand.Show();
        }

        [AcTrx.CommandMethod("Land_CreatingLocalPaths")]
        public void CreatingLocalPaths()
        {
            LocalPath localPath = new LocalPath();

            localPath.Paths.Add(new Setting("key1", "name1", "description1"));
            localPath.Paths.Add(new Setting("key2", "name2", "description2"));
            localPath.Paths.Add(new Setting("key3", "name3", "description3"));

            ServiceXml.WriteXml<LocalPath>(localPath, "p:\\LocalPaths.xml");
        }

        

        [AcTrx.CommandMethod("tst_IT")]
        public void InsertTextPlacementJig()
        {
            AcDb.DBText oText = new AcDb.DBText();
            oText.TextString = "Test String TExt";
            oText.Height = 5.33;

            //ServiceSimpleElements.ManualInsertText(oText);
        }

        [AcTrx.CommandMethod("tst_IMT")]
        public void InsertMTextPlacementJig()
        {
            AcDb.MText  oMText = new AcDb.MText();
                        oMText.TextHeight = 7.37;
                        oMText.Attachment = AcDb.AttachmentPoint.MiddleLeft;
                        oMText.Contents = "Test Value #";

            //ServiceSimpleElements.ManualInsertMText(oMText);
        }

        //[CommandMethod("GXD")]
        static public void GetXData()
        {
            AcAp.Document doc = AcApp.DocumentManager.MdiActiveDocument;
            AcEd.Editor ed = doc.Editor;

            AcEd.PromptEntityOptions opt = new AcEd.PromptEntityOptions( "\nSelect entity: " );
            AcEd.PromptEntityResult res = ed.GetEntity(opt);

          if (res.Status == AcEd.PromptStatus.OK)
          {
        
            using (AcDb.Transaction tr = doc.TransactionManager.StartTransaction())
            {
                    AcDb.DBObject obj = tr.GetObject( res.ObjectId, AcDb.OpenMode.ForRead );
                    AcDb.ResultBuffer rb = obj.XData;
              if (rb == null)
              {
                ed.WriteMessage( "\nEntity does not have XData attached." );
              }
              else
              {
                int n = 0;
                foreach (AcDb.TypedValue tv in rb)
                {
                  ed.WriteMessage( "\nTypedValue {0} - type: {1}, value: {2}", n++, tv.TypeCode, tv.Value );
                }
                rb.Dispose();
              }
            }
          }
        }

       //[CommandMethod("SXD")]
       static public void SetXData()
       {
            AcAp.Document doc = AcApp.DocumentManager.MdiActiveDocument;
            AcEd.Editor ed = doc.Editor;

            AcEd.PromptEntityOptions opt =new AcEd.PromptEntityOptions("\nSelect entity: ");
            AcEd.PromptEntityResult res =ed.GetEntity(opt);

           if (res.Status == AcEd.PromptStatus.OK)
           {
               
               using (AcDb.Transaction tr =doc.TransactionManager.StartTransaction())
               {
                    AcDb.DBObject obj = tr.GetObject( res.ObjectId, AcDb.OpenMode.ForWrite );
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

       static void AddRegAppTableRecord(string regAppName)
       {
            AcAp.Document doc = AcApp.DocumentManager.MdiActiveDocument;
            AcEd.Editor ed = doc.Editor;
            AcDb.Database db = doc.Database;

           using (AcDb.Transaction tr = doc.TransactionManager.StartTransaction())
           {
                AcDb.RegAppTable rat = (AcDb.RegAppTable)tr.GetObject( db.RegAppTableId, AcDb.OpenMode.ForRead, false );
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


    }

    public class GeoCommands : AcTrx.IExtensionApplication
    {

        private LocalPath localPath = new LocalPath();

        public void Initialize()
        {

        }

        public void Terminate()
        {

        }

        [STAThread]
        public static void Main(string[] args)
        {

        }


    }
}
