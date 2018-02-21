
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoSa.Xml;
using LoSa.CAD;

namespace LoSa.Land.Tables
{
    /*
    public class LocalPath
    {
        public static string RootDirectory { get { return ServiceCAD.GetPathLoSaLand();  } }

        public List<SettingLand> Paths { get; set; }

        public LocalPath()
        {
            this.Paths = new List<SettingLand>();
        }

        public SettingLand FindKey(string key)
        {
            return Paths.Find
                (
                    delegate(SettingLand setLand)
                    {
                        return setLand.Key == key;
                    }
                );
        }

        public static string FindFullPathFromXml(string key)
        {

            SettingLand localPath = LocalPath.FindLocalPathFromXml(key);
            if (localPath != null)
            {
                return LocalPath.RootDirectory + localPath.Name;
            }
            else
            {
                CurrentCAD.Editor.WriteMessage("\nЗначення '{0}' не знайдено в файлі '{1}'.",key,LocalPath.RootDirectory + "\\settings\\LocalPaths.xml");
                return null;
            }

        }

        public static SettingLand FindLocalPathFromXml(string key)
        {
            LocalPath localPath = ServiceXml.ReadXml<LocalPath>(LocalPath.RootDirectory + "\\settings\\LocalPaths.xml");
            return localPath.Paths.Find
                (
                    delegate(SettingLand setLand) 
                    { 
                        return setLand.Key == key; 
                    }
                );
        }

        public static LocalPath Default
        {
            get
            {
                LocalPath setsDefault = new LocalPath();
                setsDefault.Paths.Add(SettingLand.Default);
                return setsDefault;
            }
        }
    }
    */
}

/*
            LocalPath lPath = new LocalPath();

            lPath.Paths.Add(new SettingLand("PathFormLand", "\\settings\\FormLand.xml", "*"));
            lPath.Paths.Add(new SettingLand("PathDrawing", "\\settings\\Drawing.xml", "*"));
            lPath.Paths.Add(new SettingLand("PathTables", "\\settings\\Tables.xml", "*"));
            lPath.Paths.Add(new SettingLand("PathBoxDrawing", "\\settings\\BoxDrawing.xml", "*"));
            lPath.Paths.Add(new SettingLand("PathHatchPolygon", "\\settings\\HatchPolygon.xml", "*"));

            lPath.Paths.Add(new SettingLand("PathBlockPoint", "\\blocks\\Точка_Межі.dwg", "*"));
            lPath.Paths.Add(new SettingLand("PathBlockNeighbor", "\\blocks\\Суміжник.dwg", "*"));

            lPath.Paths.Add(new SettingLand("PathPathLimitingOnUseLand", "\\data\\PathLimitingOnUseLand.xml", "*"));
            lPath.Paths.Add(new SettingLand("PathClassificationOfLand", "\\settings\\ClassificationOfLand.xml", "*"));


            ServiceXml.WriteXml<LocalPath>(lPath, ServiceBricsCAD.GetPathLoSaLand() + "\\settings\\LocalPaths.xml"); 
 */
