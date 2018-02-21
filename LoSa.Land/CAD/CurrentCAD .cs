﻿#if BCAD 
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
using System.Runtime.InteropServices;

namespace LoSa.CAD
{
    
    

    public static class CurrentCAD
    {
        private static AcAp.Document doc = AcApp.DocumentManager.MdiActiveDocument;
        private static AcDb.Database db = doc.Database;
        private static AcEd.Editor ed = doc.Editor;
        private static AcPApp.AcadApplication app = (AcPApp.AcadApplication)AcApp.AcadApplication;


        public static AcPApp.AcadDocument AcadDocument { get { return (AcPApp.AcadDocument)doc.AcadDocument; } }
        public static AcPApp.AcadApplication Application { get { return app; } }

        public static AcAp.Document Document { get { return doc; } }
        public static AcEd.Editor Editor { get { return ed; } }
        public static AcDb.Database Database { get { return db; } }
        public static string Version { get { return app.version; } }
    }
    
}
