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

using LoSa.Land.EnumAttributes;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LoSa.Land.ObjectExport
{
    public interface ISokkiaSDR
    {
        [FormatSDR20(2)]
        [FormatSDR33(2)]
        string ID { get; }

        [FormatSDR20(2)]
        [FormatSDR33(2)]
        string Code { get; }

        string ToStringSDR<T>() where T: IFormatSDR;
    }

    public class DataSDR
    {
        public List<ISokkiaSDR> Recording  { get; set; }

        public DataSDR()
        { 
            this.Recording = new List<ISokkiaSDR>();
        }

        public string ToStringSDR<T>() where T : IFormatSDR
        {
            string sdrString = string.Empty;

            foreach (ISokkiaSDR record in this.Recording)
            {
                if (sdrString != string.Empty) { sdrString += "\r\n"; }
                sdrString += record.ToStringSDR<T>();
            }

            return sdrString;
        }

        public static string Format(string value, int numberCharacters)
        {
            if (value == null) value="";
            int numberSpaces = numberCharacters - value.Length;
            if (numberSpaces < 0 || numberCharacters <= 0) return string.Empty;
            string spaces = "";
            for (int i = 0; i < numberSpaces; i++) spaces += " "; 
            return (spaces + value);
        }
        

        public static string Format(string value, int numberCharacters, AlignmentText alignment)
        {
            if (value == null) value = "";
            int numberSpaces = numberCharacters - value.Length;
            if (numberSpaces < 0 || numberCharacters <= 0) return string.Empty;
            string spaces = "";
            for (int i = 0; i < numberSpaces; i++) spaces += " ";
            if (alignment == AlignmentText.ToLeft) return (value + spaces);
            else return (spaces + value);
        }
    }

    public enum AlignmentText
    {
        ToRight,
        ToLeft
    }

    // 00*
    #region HeaderRecordSDR
    // 00 NM SDR33 V04-04.02  #### 25-FЕВ-02 18:20  abcdef 
    // 12 34 5678901234567890 1234 5678901234567890 123456
    // 00 00 0000011111111112 2222 2222233333333334 444444
    public class HeaderRecordSDR : ISokkiaSDR
    {
        [FormatSDR20(2)]
        [FormatSDR33(2)]
        public string ID { get { return "00"; } }

        [FormatSDR20(2)]
        [FormatSDR33(2)]
        public string Code { get { return "NM"; } }

        [FormatSDR20(16)]
        [FormatSDR33(16)]
        public string Version { get; set; }

        [FormatSDR20(4)]
        [FormatSDR33(4)]
        public string SerialNumber { get { return "0000"; } }

        [FormatSDR20(16)]
        [FormatSDR33(16)]
        public string DateAndTime 
        {
            get 
            { 
                return DateTime.Today.ToString(); 
            } 
        }

        [FormatSDR20(1)]
        [FormatSDR33(1)]
        public string UnitsAngles { get; set; }

        [FormatSDR20(1)]
        [FormatSDR33(1)]
        public string UnitsDistance { get; set; }

        [FormatSDR20(1)]
        [FormatSDR33(1)]
        public string UnitsPressure { get; set; }

        [FormatSDR20(1)]
        [FormatSDR33(1)]
        public string UnitsTemperature { get; set; }

        [FormatSDR20(1)]
        [FormatSDR33(1)]
        public string CoordinateSystem { get; set; }

        public HeaderRecordSDR()
        {
            this.UnitsAngles = "1";
            this.UnitsDistance = "1";
            this.UnitsPressure = "1";
            this.UnitsTemperature = "1";
            this.CoordinateSystem = "1";
        }

        public string ToStringSDR<T>() where T : IFormatSDR
        {
            string sdrString = string.Empty;
            
            var properties = this.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                var attributes = property.GetCustomAttributes(typeof(T), false);
                int numChar = (int)((T)attributes[0]).GetValue();
                string propertyValue = (string)property.GetValue(this, null);
                if (property.Name == "Version")
                {
                    if (typeof(FormatSDR33).Equals(typeof(T)))
                    {
                        propertyValue = "SDR33 V04-04.02 ";
                    }
                    else if (typeof(FormatSDR20).Equals(typeof(T)))
                    {
                        propertyValue = "SDR20V03-05     ";
                    }
                }
                sdrString += DataSDR.Format(propertyValue, numChar);
            }

            return sdrString += "1";
        }
    }

    public enum UnitAngle
    {
        Degrees = 1,
        Gon = 2,
        Mile = 4
    }

    public enum UnitDistance
    {
        Meters = 1,
        Feet = 2
    }

    public enum UnitPressure
    {
        mmHg = 1,
        InHg = 2,
        hPa = 3
    }

    public enum UnitTemperature
    {
        Celsius = 1,
        Fahrenheit = 2
    }

    public enum CoordSystem
    {
        NorthEastHeight = 1,
        EastNorthHeight = 2
    }

    #endregion HeaderRecordSDR
    
    // 01
    #region InstrumentRecordSDR

    // 01 NM :SET500 V31-08 013783SET500 V31-08 01378331     
    // 12 34 56789012345678901 234567 8901234567890123 4567890
    // 00 00 00000111111111122 222222 2233333333334444 4444445

    public class InstrumentRecordSDR : ISokkiaSDR
    {
        [FormatSDR20(2)]
        [FormatSDR33(2)]
        public string ID { get { return "01"; } }

        [FormatSDR20(2)]
        [FormatSDR33(2)]
        public string Code { get { return "KI"; } }

        [FormatSDR20(46)]
        [FormatSDR33(46)]
        public string Temp { get { return ""; } }

        [FormatSDR20(1)]
        [FormatSDR33(1)]
        public string VerticalAngles { get { return "1"; } }

        [FormatSDR20(16)]
        [FormatSDR33(16)]
        public string DateAndTime
        {
            get
            {
                return DateTime.Today.ToString();
            }
        }

        [FormatSDR20(1)]
        [FormatSDR33(1)]
        public string UnitsAngles { get; set; }

        [FormatSDR20(1)]
        [FormatSDR33(1)]
        public string UnitsDistance { get; set; }

        [FormatSDR20(1)]
        [FormatSDR33(1)]
        public string UnitsPressure { get; set; }

        [FormatSDR20(1)]
        [FormatSDR33(1)]
        public string UnitsTemperature { get; set; }

        [FormatSDR20(1)]
        [FormatSDR33(1)]
        public string CoordinateSystem { get; set; }

        public InstrumentRecordSDR()
        {
            this.UnitsAngles = UnitAngle.Degrees.ToString();
            this.UnitsDistance = UnitDistance.Meters.ToString();
            this.UnitsPressure = UnitPressure.mmHg.ToString();
            this.UnitsTemperature = UnitTemperature.Celsius.ToString();
            this.CoordinateSystem = CoordSystem.NorthEastHeight.ToString();
        }

        public string ToStringSDR<T>() where T : IFormatSDR
        {
            string sdrString = string.Empty;

            var properties = this.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                var attributes = property.GetCustomAttributes(typeof(T), false);
                int numChar = (int)((T)attributes[0]).GetValue();
                string propertyValue = (string)property.GetValue(this, null);
                sdrString += DataSDR.Format(propertyValue, numChar);
            }

            return sdrString += "1";
        }
    }

    public enum VerticalAngles
    {
        Zenith = 1,
        Horizon = 2
    }

    #endregion InstrumentRecordSDR
    
    // 08*
    #region CoordinatesRecordSDR

    public class CoordinatesRecordSDR : ISokkiaSDR
    {
        [FormatSDR20(2)]
        [FormatSDR33(2)]
        public string ID { get { return "08"; } }

        [FormatSDR20(2)]
        [FormatSDR33(2)]
        public string Code { get { return "KI"; } }

        [FormatSDR20(4)]
        [FormatSDR33(16)]
        public string NamePoint { get; set; }

        [FormatSDR20(10)]
        [FormatSDR33(16)]
        public string Northing { get; set; }

        [FormatSDR20(10)]
        [FormatSDR33(16)]
        public string Easting { get; set; }

        [FormatSDR20(10)]
        [FormatSDR33(16)]
        public string Elevation { get; set; }

        [FormatSDR20(16)]
        [FormatSDR33(16)]
        public string Description { get; set; }

        public CoordinatesRecordSDR()
        {
            this.NamePoint = "None";
            this.Northing = AcGe.Point3d.Origin.Y.ToString("0.000").Replace(",", ".");
            this.Easting = AcGe.Point3d.Origin.X.ToString("0.000").Replace(",", ".");
            this.Elevation = AcGe.Point3d.Origin.Z.ToString("0.000").Replace(",", ".");
            this.Description = "Description";
        }

        public CoordinatesRecordSDR(string namePoint, AcGe.Point3d point, string description)
        {
            this.NamePoint = namePoint;
            this.Northing = point.Y.ToString("0.000").Replace(",",".");
            this.Easting = point.X.ToString("0.000").Replace(",", ".");
            this.Elevation = point.Z.ToString("0.000").Replace(",", ".");
            this.Description = description;
        }

        public string ToStringSDR<T>() where T : IFormatSDR
        {
            string sdrString = string.Empty;

            var properties = this.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                var attributes = property.GetCustomAttributes(typeof(T), false);
                int numChar = (int)((T)attributes[0]).GetValue();
                string propertyValue = (string)property.GetValue(this, null);
                string propertyName = (string)property.Name;
                if (propertyName == "NamePoint")
                {
                    sdrString += DataSDR.Format(propertyValue, numChar, AlignmentText.ToRight);
                }
                else
                {
                    sdrString += DataSDR.Format(propertyValue, numChar, AlignmentText.ToLeft);
                }
            }

            return sdrString;
        }
    }
    #endregion CoordinatesRecordSDR

    // 10*
    #region JobRecordSDR
    public class JobRecordSDR : ISokkiaSDR
    {
        [FormatSDR20(2)]
        [FormatSDR33(2)]
        public string ID { get { return "10"; } }

        [FormatSDR20(2)]
        [FormatSDR33(2)]
        public string Code { get { return "NM"; } }

        [FormatSDR20(4)]
        [FormatSDR33(16)]
        public string JobName { get; set; }

        [FormatSDR20(0)]
        [FormatSDR33(1)]
        public string PointID {get { return "1"; } }

        [FormatSDR20(0)]
        [FormatSDR33(1)]
        public string IncZ { get; set; }

        [FormatSDR20(0)]
        [FormatSDR33(1)]
        public string AtmosphericCorrection { get; set; }

        [FormatSDR20(0)]
        [FormatSDR33(1)]
        public string CurvatureAndRefractionCorrection { get; set; }

        [FormatSDR20(0)]
        [FormatSDR33(1)]
        public string RefractionConstant { get; set; }

        [FormatSDR20(0)]
        [FormatSDR33(1)]
        public string SeaLevelCorrection { get; set; }

        public string ToStringSDR<T>() where T : IFormatSDR
        {
            string sdrString = string.Empty;

            var properties = this.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                var attributes = property.GetCustomAttributes(typeof(T), false);
                int numChar = (int)((T)attributes[0]).GetValue();
                string propertyValue = (string)property.GetValue(this, null);
                sdrString += DataSDR.Format(propertyValue, numChar);
            }

            return sdrString;
        }

    }
    #endregion JobRecordSDR

    // 13*
    #region NoteRecordSDR
    public class NoteRecordSDR : ISokkiaSDR
    {
        [FormatSDR20(2)]
        [FormatSDR33(2)]
        public string ID { get { return "13"; } }

        [FormatSDR20(2)]
        [FormatSDR33(2)]
        public string Code { get { return "NM"; } }

        [FormatSDR20(60)]
        [FormatSDR33(60)]
        public string Note { get; set; }

        public string ToStringSDR<T>() where T : IFormatSDR
        {
            string sdrString = string.Empty;

            var properties = this.GetType().GetProperties();
            foreach (PropertyInfo property in properties)
            {
                var attributes = property.GetCustomAttributes(typeof(T), false);
                int numChar = (int)((T)attributes[0]).GetValue();
                string propertyValue = (string)property.GetValue(this, null);
                sdrString += DataSDR.Format(propertyValue, numChar);
            }

            return sdrString;
        }

    }
    #endregion NoteRecordSDR
}
