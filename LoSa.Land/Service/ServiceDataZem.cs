
using LoSa.Land.Tables;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoSa.Xml;
using LoSa.Utility;

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

namespace LoSa.Land.Service
{
    public static class ServiceDataZem
    {
        private static LocalPath localPath = new LocalPath("LoSa_Land");

        [AcTrx.CommandMethod("Land_CreatingCategoryOfLand")]
        public static void CreatingCategoryOfLand()
        {
            SettingsLand categoryLand = new SettingsLand();

            categoryLand.Setting
                .Add(new SettingLand("100", "Землі сільськогосподарського призначення", "*"));
            categoryLand.Setting
               .Add(new SettingLand("200", "Землі житлової та громадської забудови", "*"));
            categoryLand.Setting
               .Add(new SettingLand("300", "Землі природно-заповідного та іншого природоохоронного призначення", "*"));
            categoryLand.Setting
               .Add(new SettingLand("400", "Землі оздоровчого призначення", "*"));
            categoryLand.Setting
               .Add(new SettingLand("500", "Землі рекреаційного призначення", "*"));
            categoryLand.Setting
               .Add(new SettingLand("600", "Землі історико-культурного призначення", "*"));
            categoryLand.Setting
               .Add(new SettingLand("700", "Землі лісогосподарського призначення", "*"));
            categoryLand.Setting
               .Add(new SettingLand("800", "Землі водного фонду", "*"));
            categoryLand.Setting
               .Add(new SettingLand("900", "Землі промисловості, транспорту, зв'язку, енергетики, оборони та іншого призначення", "*"));

            ServiceXml.WriteXml<SettingsLand>(categoryLand, localPath.FindFullPathFromXml("CategoryOfLand"));
        }

        [AcTrx.CommandMethod("Land_CreatingKVCPZ")]
        public static void CreatingClassificationPurposLand()
        {
            SettingsLand purposLand = new SettingsLand();

            #region Section_A
            purposLand.Setting
                .Add(new SettingLand(
                    "01.01",
                    "Для ведення товарного сільськогосподарського виробництва", 
                    ""));

            purposLand.Setting
                .Add(new SettingLand(
                    "01.02",
                    "Для ведення фермерського господарства",
                    ""));

            purposLand.Setting
                .Add(new SettingLand(
                    "01.03",
                    "Для ведення особистого селянського господарства",
                    ""));

            purposLand.Setting
                .Add(new SettingLand(
                    "01.04",
                    "Для ведення підсобного сільського господарства",
                    ""));

            purposLand.Setting
                .Add(new SettingLand(
                    "01.05",
                    "Для індивідуального садівництва",
                    ""));

            purposLand.Setting
                .Add(new SettingLand(
                    "01.06",
                    "Для колективного садівництва",
                    ""));


            purposLand.Setting
                .Add(new SettingLand(
                    "01.07",
                    "Для городництва",
                    ""));

            purposLand.Setting
                .Add(new SettingLand(
                    "01.08",
                    "Для сінокосіння і випасання худоби",
                    ""));

            purposLand.Setting
                .Add(new SettingLand(
                    "01.09",
                    "Для дослідних і навчальних цілей",
                    ""));

            purposLand.Setting
                .Add(new SettingLand(
                    "01.10",
                    "Для пропаганди передового досвіду ведення сільського господарства",
                    ""));

            purposLand.Setting
                .Add(new SettingLand(
                    "01.11",
                    "Для надання послуг у сільському господарстві",
                    ""));

            purposLand.Setting
                .Add(new SettingLand(
                    "01.12",
                    "Для розміщення інфраструктури оптових ринків сільськогосподарської продукції",
                    ""));

            purposLand.Setting
                .Add(new SettingLand(
                    "01.13",
                    "Для іншого сільськогосподарського призначення",
                    ""));

            purposLand.Setting
                .Add(new SettingLand(
                    "01.014",
                    "Для цілей підрозділів 01.01-01.13 та для збереження та використання земель природно-заповідного фонду",
                    ""));
            #endregion Section_A

            ServiceXml.WriteXml<SettingsLand>(purposLand, localPath.FindFullPathFromXml("ClassificationPurposLand"));
        }
    }
}
