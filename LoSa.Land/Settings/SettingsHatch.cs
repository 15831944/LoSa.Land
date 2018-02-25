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

using LoSa.Land.Service;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using LoSa.Land.Tables;
using LoSa.Land.Parcel;

using LoSa.Xml;
using LoSa.Utility;

namespace LoSa.Land.Tables
{
    public class HatchPolygon
    {
        LocalPath localPath = new LocalPath("LoSa_Land");

        [XmlAttributeAttribute()]
        public string Type { get; set; }
        
        [XmlAttributeAttribute()]
        public string Name { get; set; }
        
        [XmlAttributeAttribute()]
        public int ColorIndex { get; set; }

        public PatternHatch Pattern { get; set; }

        public static HatchPolygon DEFAULT
        {
            get
            {
                return new HatchPolygon("000.00", "", 0, PatternHatch.DEFAULT);
            }
        }

        public HatchPolygon()
        {
           
        }

        public HatchPolygon(string type,
                                string name,
                                int colorIndex,
                                PatternHatch pattern)
        {
            this.Type = type;
            this.Name = name;
            this.ColorIndex = colorIndex;
            this.Pattern = pattern;
        }

        internal static HatchPolygon GetHatchLimiting(LandPolygon poligon)
        {
            List<HatchPolygon> list = 
                ServiceXml.ReadXml<List<HatchPolygon>>(new LocalPath("LoSa_Land").FindFullPathFromXml("PathHatchPolygon"));
            string type = poligon.FindInfo("OK").Value;
            string name = poligon.FindInfo("OX").Value;
            HatchPolygon hatchPolygon = list.Find
                (
                    delegate(HatchPolygon hatchLimiting)
                    {
                        return hatchLimiting.Type == type && hatchLimiting.Name == name;
                    }
                );
           
            return hatchPolygon;
        }

        internal static HatchPolygon GetHatchParcel()
        {
            List<HatchPolygon> list = 
                ServiceXml.ReadXml<List<HatchPolygon>>(new LocalPath("LoSa_Land").FindFullPathFromXml("PathHatchPolygon"));
            HatchPolygon hatchPolygon = list.Find
                (
                    delegate(HatchPolygon hatchParcel)
                    {
                        return hatchParcel.Type == "Parcel";
                    }
                );
            return hatchPolygon;
        }

        internal static HatchPolygon GetHatchLand(LandPolygon poligon)
        {
            List<HatchPolygon> list = 
                ServiceXml.ReadXml<List<HatchPolygon>>(new LocalPath("LoSa_Land").FindFullPathFromXml("PathHatchPolygon"));

            HatchPolygon hatchPolygon = null;

            LandInfo infoCC = poligon.FindInfo("CC");

            if (infoCC != null)
            {
                string type = infoCC.Value;
                hatchPolygon = list.Find
                (
                    delegate (HatchPolygon hatchLand)
                    {
                        return hatchLand.Type == type;
                    }
                );
            }  
            
            return hatchPolygon;
        }
    }

 

    public class SettingHatch
    {
        [XmlAttributeAttribute()]
        public HatchPolygon Key { get; set; }

        [XmlAttributeAttribute()]
        public int ColorIndex { get; set; }

        public PatternHatch Pattern { get; set; }

        public SettingHatch()
        { 
        
        }

        public SettingHatch(HatchPolygon key,
                            int colorIndex , 
                            PatternHatch pattern)
        {
            this.Key = key;
            this.ColorIndex = colorIndex;
            this.Pattern = pattern;
        }

    }

    public class PatternHatch
    {
        [XmlAttributeAttribute()]
        public AcDb.HatchPatternType Type { get; set; }

        [XmlAttributeAttribute()]
        public string Name { get; set; }

        [XmlAttributeAttribute()]
        public double Angle { get; set; }

        [XmlAttributeAttribute()]
        public double Space { get; set; }

        public PatternHatch()
        {

        }

        public PatternHatch(AcDb.HatchPatternType patternType , 
                            string patternName , 
                            double patternAngle , 
                            double patternSpace)
        {
            this.Type = patternType;
            this.Name = patternName;
            this.Angle = patternAngle;
            this.Space = patternSpace;
        }

        public static PatternHatch DEFAULT 
        {
            get 
            {
                return new PatternHatch(AcDb.HatchPatternType.UserDefined, "_User", 0, 1);
            }
        }
    }
    /*
    public enum HatchPolygon
    { 
        Parcel,
        Limiting,
        Land
    }
    */ 
}