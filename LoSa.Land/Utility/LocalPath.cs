using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LoSa.CAD;
using LoSa.Xml;

namespace LoSa.Utility
{
    public class LocalPath
    {
        //public static string RootDirectory { get { return ServiceCAD.GetPathLoSaLand();  } }
        public string RootDirectory { get; set; }

        public List<Setting> Paths { get; set; }

        public LocalPath()
        {
            this.Paths = new List<Setting>();
        }

        public LocalPath( string pathName ):base()
        {
            try
            {
                this.RootDirectory = ServiceCAD.GetSystemVariableSrchPathByName(pathName);
            }
            catch (Exception exc)
            {

            }
        }

        public Setting FindKey(string key)
        {
            return Paths.Find
                (
                    delegate(Setting setLand)
                    {
                        return setLand.Key == key;
                    }
                );
        }

        public string FindFullPathFromXml(string key)
        {

            Setting localPath = this.FindLocalPathFromXml(key);
            if (localPath != null)
            {
                return this.RootDirectory + localPath.Name;
            }
            else
            {
                MessageBox.Show("Значення "+ key + " не знайдено в файлі " + this.RootDirectory + "\\settings\\LocalPaths.xml.",
                                "FindFullPathFromXml",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                return null;
            }

        }

        public Setting FindLocalPathFromXml(string key)
        {
            LocalPath localPath = ServiceXml.ReadXml<LocalPath>(this.RootDirectory + "\\settings\\LocalPaths.xml");

            if (localPath == null) return null;
            return localPath.Paths.Find
                (
                    delegate(Setting setLand) 
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
                setsDefault.Paths.Add(Setting.Default);
                return setsDefault;
            }
        }
    }
}
