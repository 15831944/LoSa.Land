
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

using System.Collections.Generic;

using LoSa.Land.Service;
using LoSa.Land.Parcel;

namespace LoSa.Land.Tables
{
    public interface IBlockTableLand
    {
        LandParcel Parcel { get; set; }
        SettingTable Setting { get; set; }
        IStrategyTable StrategyTable { get; }
        void сreate();
    }

    public class BlockTableLand : IBlockTableLand
    {
        public LandParcel Parcel { get; set; }

        public SettingTable Setting { get; set; }

        public double Scale { get; set; }

        private IStrategyTable strategyTable;

        public IStrategyTable StrategyTable 
        {
            get 
            {
                if (this.Setting == null) return null;

                if (this.Setting.TypeTable == TypeTable.TableBorderParcel) 
                    strategyTable = new StrategyTableBorderParcel();

                else if (this.Setting.TypeTable == TypeTable.TableBorderLimiting) 
                    strategyTable = new StrategyTableBorderLimiting();

                else if (this.Setting.TypeTable == TypeTable.TableLimiting) 
                    strategyTable = new StrategyTableLimiting();

                else if (this.Setting.TypeTable == TypeTable.TableLans)
                    strategyTable = new StrategyTableExplicationLands();

                else if (this.Setting.TypeTable == TypeTable.TableForm6Zem) 
                    strategyTable = new StrategyTableExplicationLandForm6Zem();

                strategyTable.Setting = this.Setting;

                return strategyTable;
            }
        }

        public BlockTableLand()
        {

        }

        public BlockTableLand(LandParcel parcel, SettingTable settingTable)
        { 
            this.Parcel = parcel;
            this.Setting = settingTable;
        }

        public BlockTableLand(SettingTable settingTable)
        {
            this.Setting = settingTable;
        }

        public void сreate()
        {
            this.StrategyTable.сreate(this.Parcel, this.Scale);
        }
    }

    public enum TypeTable
    { 
        TableBorderParcel,
        TableBorderLimiting,
        TableLimiting,
        TableLans,
        TableForm6Zem
    }
}
