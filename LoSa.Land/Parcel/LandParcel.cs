
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
using System.Xml.Serialization;

namespace LoSa.Land.Parcel
{
    [Serializable()]
    public class LandParcel : LandPolygon
    {
        [XmlArray("Обмеження_земельної_ділянки")]
        [XmlArrayItem("Обмеження")]
        public List<LandPolygon> Limiting { get; set; }

        [XmlArray("Угіддя_земельної_ділянки")]
        [XmlArrayItem("Угіддя")]
        public List<LandPolygon> Lands { get; set; }

        [XmlArray("Суміжні_земельні_ділянки_по_контурам")]
        [XmlArrayItem("Контур")]
        public List<NeighborsAlongContour> ContoursNeighbors { get; set; }

        [XmlArray("Інші_землекористувачі")]
        [XmlArrayItem("Землекористувач")]
        public List<LandPolygon> OtherLand { get; set; }

        public LandParcel() : base()
        {
            this.Limiting = new List<LandPolygon>();
            this.Lands = new List<LandPolygon>();
            this.ContoursNeighbors = new List<NeighborsAlongContour>();
            this.OtherLand = new List<LandPolygon>();
        }
    }

    [Serializable()]
    public class NeighborsAlongContour
    {
        [XmlArray("Суміжні_земельні_ділянки_по_контуру")]
        [XmlArrayItem("Суміжник")]
        public List<LandPolygon> Neighbors { get; set; }

        [XmlElementAttribute("Тип_контуру")]
        public string TypeContour { get; set; }

        public NeighborsAlongContour()
        {
            this.Neighbors = new List<LandPolygon>();
        }
    }

    [Serializable()]
    public enum TypeContour
    {
        [XmlEnum("Внутрішній")]
        Internal,

        [XmlEnum("Зовнішній")]
        External
    }
}
