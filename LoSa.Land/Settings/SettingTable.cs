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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Schema;
using System.Xml.Serialization;


namespace LoSa.Land.Tables
{
    [XmlRootAttribute("Tables")]
    public class SettingsTable
    {
        [XmlElementAttribute("Table")]
        public List<SettingTable> Settings { get; set; }

        public SettingsTable()
        {
            this.Settings = new List<SettingTable>();
        }

        public SettingTable FindTitle(string title)
        {
            return Settings.Find
                (
                    delegate(SettingTable setTable)
                    {
                        return setTable.Title == title;
                    }
                );
        }

        public SettingTable FindKeyTable(string keyTable)
        {
            return Settings.Find
                (
                    delegate(SettingTable setTable)
                    {
                        return setTable.KeyTable == keyTable;
                    }
                );
        }

        public static SettingsTable Default 
        {
            get
            { 
                SettingsTable setsDefault = new SettingsTable();
                SettingTable setDefault = new SettingTable();
                setDefault.Columns.Add( new ColumnTable(10,"UnknownName", "UnknownFormat") );
                setsDefault.Settings.Add( setDefault );

                return setsDefault;
            }
        }
    }

    public class SettingTable : Settings.ASettings
    {
        [XmlAttributeAttribute()]
        public string KeyTable { get; set; }

        [XmlAttributeAttribute()]
        public TypeTable TypeTable { get; set; }
        
        [XmlAttributeAttribute()]
        public double TextHeight { get; set; }

        private string title;

        public string Title
        {
            get { return this.title.Replace("&#x13&#x10;", "\n"); }
            set { this.title = value.Replace("\n", "&#x13&#x10;" ); }
        }
        public List<ColumnTable> Columns { get; set; }
        public AcGe.Point3d BasePointDrawing { get; set; }

        public SettingTable()
        {
            this.KeyTable = "NoKey";
            this.TypeTable = Tables.TypeTable.TableBorderParcel;
            this.title = "None";
            this.TextHeight = 2;
            this.Columns = new List<ColumnTable>();
            this.BasePointDrawing = AcGe.Point3d.Origin;
        }


        public List<string> GetCodeAddTitle()
        {
            List<string> list = new List<string>();

            int startIndex = this.Title.IndexOf("/*");
            int endIndex = this.Title.IndexOf("*/");

            while (startIndex > -1)
            {
                list.Add(this.Title.Substring(startIndex + 2, (endIndex - startIndex - 2)));
                startIndex = this.Title.IndexOf("/*", startIndex+1);
                endIndex = this.Title.IndexOf("*/", endIndex+1);
            }
            

            return list;
        }

        public double GetWidthTable()
        {
            double widthTable = 0;
            foreach (ColumnTable value in this.Columns)
            {
                widthTable += value.Width;
            }
            return widthTable;
        }

        public double GetHeightCapTable()
        {
            double heightCapTable = 0;
            foreach (ColumnTable value in this.Columns)
            {
                double dL = value.Name.Length - value.Name.Replace("\n", "").Length;
                if (heightCapTable < (dL + 4) * this.TextHeight)
                {
                    heightCapTable = (dL + 4) * this.TextHeight;
                }
            }
            return heightCapTable;
        }

        public double GetHeightTable(int iRows, double stepRows)
        {
            return GetHeightCapTable() + iRows * stepRows;
        }
    }

    public class ColumnTable
    {
        [XmlAttributeAttribute()]
        public double Width { get; set; }

        private string name;

        [XmlAttributeAttribute()]
        public string Name
        {
            get { return this.name.Replace("&#x13&#x10;" ,"\n" ); }
            set { this.name = value.Replace( "\n", "&#x13&#x10;"); }
        }

        [XmlAttributeAttribute()]
        public string Format { get; set; }

        public ColumnTable()
        {
            this.Width = 10;
            this.Name = "Noname";
            this.Format = "None";
        }

        public ColumnTable(double width, string name, string format)
        {
            this.Width = width;
            this.Name = name;
            this.Format = format;
        }
    }
}
