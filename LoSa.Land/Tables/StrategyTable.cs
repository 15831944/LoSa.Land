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

using LoSa.CAD;
using LoSa.Land.Service;
using LoSa.Land.Parcel;

namespace LoSa.Land.Tables
{

    public interface IStrategyTable
    {
        SettingTable Setting { get; set; }
        void сreate(LandParcel landPolygon, double scaleTable);
    }
     

    public class StrategyTableStakeOutPoints : IStrategyTable
    {
        private AcDb.DBObjectCollection objects = new AcDb.DBObjectCollection();

        public SettingTable Setting { get; set; }

        public StrategyTableStakeOutPoints()
        {
            this.Setting = new SettingTable();
        }

        public void сreate(LandParcel polygon, double scale)
        {
            string titleTable = ServiceTable.ReplaceValueCodeInTitle(polygon, this.Setting);

            foreach (AcDb.DBObject obj in ServiceTable.GetCapTables(titleTable, this.Setting))
            { objects.Add(obj); }

            foreach (AcDb.DBObject obj in ServiceTable.GetBoundTable(polygon.Points.Count + 1, this.Setting.TextHeight * 2, this.Setting))
            { objects.Add(obj); }

            foreach (AcDb.DBObject obj in ServiceTable.GetDataTableStakeOutParcelPoints(polygon, this.Setting))
            { objects.Add(obj); }

            string nameBlockTable = ServiceBlockElements.CreateBlock(this.objects, this.Setting.KeyTable);
            ServiceBlockElements.ManualInsertBlock(nameBlockTable, scale);
            objects = new AcDb.DBObjectCollection();
        }
    }

    public class StrategyTableBorder : IStrategyTable
    {
        private AcDb.DBObjectCollection objects = new AcDb.DBObjectCollection();

        public SettingTable Setting { get; set; }

        public StrategyTableBorder()
        {
            this.Setting = new SettingTable();
        }

        public void сreate(LandParcel polygon, double scale)
        {
            string titleTable = ServiceTable.ReplaceValueCodeInTitle(polygon, this.Setting);

            foreach (AcDb.DBObject obj in ServiceTable.GetCapTables(titleTable, this.Setting) )
                { objects.Add(obj); }

            foreach (AcDb.DBObject obj in ServiceTable.GetBoundTable(polygon.Points.Count + 1, this.Setting.TextHeight*2, this.Setting) )
                { objects.Add(obj); }

            foreach (AcDb.DBObject obj in ServiceTable.GetDataTableBorderPolygon(polygon, this.Setting) )
                { objects.Add(obj); }

            string nameBlockTable = ServiceBlockElements.CreateBlock(this.objects, this.Setting.KeyTable);
            ServiceBlockElements.ManualInsertBlock(nameBlockTable, scale);
            objects = new AcDb.DBObjectCollection();
        }
    }

    public class  StrategyTableBorderParcel: StrategyTableBorder
    {

    }

    public class StrategyTableBorderLimiting : StrategyTableBorder
    {

    }

    public class StrategyTableLimiting : IStrategyTable
    {
        private AcDb.DBObjectCollection objects = new AcDb.DBObjectCollection();

        public SettingTable Setting { get; set; }

        public void сreate(LandParcel parcel, double scaleTable)
        {
            LandParcel parcelСombinedLimiting = ServiceTable.СombinedLimiting(parcel);

            string titleTable = ServiceTable.ReplaceValueCodeInTitle(parcelСombinedLimiting, this.Setting);

            foreach (AcDb.DBObject obj in ServiceTable.GetCapTables(titleTable, this.Setting))
            { objects.Add(obj); }

            foreach (AcDb.DBObject obj in ServiceTable.GetBoundTable(parcelСombinedLimiting.Limiting.Count, this.Setting.TextHeight * 6, this.Setting))
            { objects.Add(obj); }

            foreach (AcDb.DBObject obj in ServiceTable.GetDataTableLimiting(parcelСombinedLimiting, this.Setting))
            { objects.Add(obj); }

            string nameBlockTable = ServiceBlockElements.CreateBlock(this.objects, this.Setting.KeyTable);
            ServiceBlockElements.ManualInsertBlock(nameBlockTable, scaleTable);
            objects = new AcDb.DBObjectCollection();
        }

        public void сreate(LandPolygon landPolygon, double scaleTable)
        {
            throw new NotImplementedException();
        }
    }

    public class StrategyTableExplicationLands : IStrategyTable
    {
        private AcDb.DBObjectCollection objects = new AcDb.DBObjectCollection();

        public SettingTable Setting { get; set; }

        public void сreate(LandParcel parcel, double scaleTable)
        {
            // ???????
            LandParcel parcelСombinedLand = ServiceTable.СombinedLand(parcel);
            throw new NotImplementedException();
        }
    }
    
    public class StrategyTableExplicationLandForm6Zem: IStrategyTable
    {
        private AcDb.DBObjectCollection objects = new AcDb.DBObjectCollection();

        public SettingTable Setting { get; set; }

        public void сreate(LandParcel parcel, double scaleTable)
        {
            // ???????
            LandParcel parcelСombinedLand = ServiceTable.СombinedLand(parcel);

            throw new NotImplementedException();
        }
    }
    
}
