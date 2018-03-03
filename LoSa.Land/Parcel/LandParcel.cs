
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
using LoSa.Land.ObjectGeo;

namespace LoSa.Land.Parcel
{
    /// <summary>
    /// Земельна ділянка
    /// </summary>
    /// <seealso cref="LoSa.Land.Parcel.LandPolygon" />
    [Serializable()]
    public class LandParcel : LandPolygon
    {
        /// <summary>
        /// Поверта чи встановлює список обмежень на земельній ділянці.
        /// </summary>
        /// <value>
        /// Список обмежень на земельній ділянці.
        /// </value>
        [XmlArray("Обмеження_земельної_ділянки")]
        [XmlArrayItem("Обмеження")]
        public List<LandPolygon> Limiting { get; set; }

        /// <summary>
        /// Поверта чи встановлює список угідь на земельній ділянці.
        /// </summary>
        /// <value>
        /// Список угідь на земельній ділянці.
        /// </value>
        [XmlArray("Угіддя_земельної_ділянки")]
        [XmlArrayItem("Угіддя")]
        public List<LandPolygon> Lands { get; set; }

        /// <summary>
        /// Поверта чи встановлює список суміжників земельної ділянки.
        /// </summary>
        /// <value>
        /// Список суміжників земельної ділянки.
        /// </value>
        [XmlArray("Суміжні_земельні_ділянки_по_контурам")]
        [XmlArrayItem("Контур")]
        public List<NeighborsAlongContour> ContoursNeighbors { get; set; }

        /// <summary>
        /// Поверта чи встановлює список данних для виносу внатуру точок межі.
        /// </summary>
        /// <value>
        /// Список данних для виносу внатуру точок межі.
        /// </value>
        [XmlArray("Данні_для_виносу_внатуру")]
        [XmlArrayItem("Розбивочний_точки_межі")]
        public List<StakeOutParcelPoint> StakeOutParcelPoints { get; set; }

        /// <summary>
        /// Поверта чи встановлює список інших землекористувачів.
        /// </summary>
        /// <value>
        /// Список інших землекористувачів.
        /// </value>
        [XmlArray("Інші_землекористувачі")]
        [XmlArrayItem("Землекористувач")]
        public List<LandPolygon> OtherLand { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LandParcel"/> class.
        /// </summary>
        public LandParcel() : base()
        {
            this.Limiting = new List<LandPolygon>();
            this.Lands = new List<LandPolygon>();
            this.ContoursNeighbors = new List<NeighborsAlongContour>();
            this.OtherLand = new List<LandPolygon>();
            this.StakeOutParcelPoints = new List<StakeOutParcelPoint>();
        }
    }

    /// <summary>
    /// Суміжники вздовж контуру.
    /// </summary>
    [Serializable()]
    public class NeighborsAlongContour
    {
        /// <summary>
        /// Gets or sets the neighbors.
        /// </summary>
        /// <value>
        /// The neighbors.
        /// </value>
        [XmlArray("Суміжні_земельні_ділянки_по_контуру")]
        [XmlArrayItem("Суміжник")]
        public List<LandPolygon> Neighbors { get; set; }

        /// <summary>
        /// Gets or sets the type contour.
        /// </summary>
        /// <value>
        /// The type contour.
        /// </value>
        [XmlElementAttribute("Тип_контуру")]
        public string TypeContour { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NeighborsAlongContour"/> class.
        /// </summary>
        public NeighborsAlongContour()
        {
            this.Neighbors = new List<LandPolygon>();
        }
    }

    /// <summary>
    /// Type contour 
    /// </summary>
    [Serializable()]
    public enum TypeContour
    {
        /// <summary>
        /// The internal contour
        /// </summary>
        [XmlEnum("Внутрішній")]
        Internal,

        /// <summary>
        /// The external contour
        /// </summary>
        [XmlEnum("Зовнішній")]
        External
    }
}
