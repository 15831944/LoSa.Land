
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
using LoSa.Utility;
using LoSa.CAD;

namespace LoSa.Land.ObjectGeo
{
    
    /// <summary>
    /// Клас, що містить данні для виносу точоки внатуру.
    /// </summary>
    public class StakeOutParcelPoint
    {
        private AcDb.ObjectId txtID = AcDb.ObjectId.Null;
        private AcDb.ObjectId lineID = AcDb.ObjectId.Null;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="StakeOutParcelPoint"/> class.
        /// </summary>
        public StakeOutParcelPoint()
        {
        }

        /// <summary>
        /// Ім'я точки, яку треба винисти в натуру.
        /// </summary>
        /// <value>
        /// Ім'я точки, яку треба винисти в натуру.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Координати точки, яку треба винисти в натуру.
        /// </summary>
        /// <value>
        /// Координати точки, яку треба винисти в натуру.
        /// </value>
        public AcGe.Point2d Coordinates { get; set; }

        /// <summary>
        /// Станція (точка планової основи для виносу внатуру).
        /// </summary>
        /// <value>
        /// Станція (точка планової основи для виносу внатуру).
        /// </value>
        public BasePoint PointStation { get; set; }

        /// <summary>
        /// Орієнтир (точка планової основи для виносу внатуру).
        /// </summary>
        /// <value>
        /// Орієнтир (точка планової основи для виносу внатуру).
        /// </value>
        public BasePoint PointOrientation { get; set; }


        /// <summary>
        /// Повертае або встановлюе значення чи то видимий вектор з данними виносу у кресленi CAD.
        /// </summary>
        /// <value>
        ///   <c>true</c> якщо видимий вектор з данними виносу у кресленi CAD; інакше, <c>false</c>.
        /// </value>
        public bool Visible { get; set; }

        /// <summary>
        /// Повертае або встановлюе значення масштабу відображення у кресленi CAD.
        /// </summary>
        /// <value> 
        /// Значеня масштабу, може бути лише більше 0.
        /// </value>
        /// <example>
        /// <code>
        /// this.ScaleDrawing = 2; // Масштаб 1:2000
        /// this.ScaleDrawing = 1; // Масштаб 1:1000
        /// this.ScaleDrawing = 0.5; // Масштаб 1:500
        /// </code>
        /// </example>
        public double ScaleDrawing { get; set; }

        /// <summary>
        /// Повертає ID лінії вектору виносу точки в натуру.
        /// </summary>
        /// <value>
        /// <see cref="global::ThisAssembly:AcDb.ObjectId"/> ID лінії вектору виносу точки в натуру.
        /// </value>
        public AcDb.ObjectId LineStakeOutID { get { return this.lineID; } }

        /// <summary>
        /// Оновлює значення у відповідності з поточними властивостями.
        /// </summary>
        public void Regen()
        {
            this.DeleteDrawingStakeOut();

            if (this.Visible == true)
            {
                this.CreateDrawingStakeOut();
            }
        }

        /// <summary>
        /// Створює елементи вектору з данними виносу.
        /// </summary>
        private void CreateDrawingStakeOut()
        {
            if (this.PointStation == null)
            {
                return;
            }

            AcGe.Point3d pntStart = new AcGe.Point3d(this.PointStation.E, this.PointStation.N, this.PointStation.H);
            AcGe.Point3d pntEnd = new AcGe.Point3d(this.Coordinates.X, this.Coordinates.Y, 0);

            AcDb.Line   line = new AcDb.Line(pntStart, pntEnd);
                        line.Visible = this.Visible;

            AcGe.Point3d pntMiddle = new AcGe.LineSegment3d(pntStart, pntEnd).MidPoint;

            double angleTXT = line.Angle;

            if (angleTXT > Math.PI/2  && angleTXT < Math.PI * 3 / 2)
            {
                angleTXT += Math.PI;
            }

            AcDb.MText text = new AcDb.MText();
            text.Contents = this.DistanceToString(AcRx.DistanceUnitFormat.Decimal) + "\r\n" + this.DirAngleToString(AcRx.AngularUnitFormat.DegreesMinutesSeconds);
            text.Rotation = angleTXT;
            text.Location = pntMiddle;
            text.Attachment = AcDb.AttachmentPoint.MiddleCenter;
            text.Width = 25;
            text.TextHeight = 1.8 * this.ScaleDrawing;


            this.lineID = ServiceCAD.InsertObject(line);
            this.txtID = ServiceCAD.InsertObject(text);

        }

        /// <summary>
        /// Видаляє елементи вектору з данними виносу.
        /// </summary>
        private void DeleteDrawingStakeOut()
        {
            ServiceCAD.DeleteObject(this.lineID);
            ServiceCAD.DeleteObject(this.txtID);

            this.txtID = AcDb.ObjectId.Null;
            this.lineID = AcDb.ObjectId.Null;
        }

        /// <summary>
        /// Повертає відстань між станцією та точкою виносу.
        /// </summary>
        /// <value>
        /// Значення відстані між точкою виносу та станцією.
        /// </value>
        public double Distance
        {
            get
            {
                AcGe.Point2d basePoint2d = new AcGe.Point2d( this.PointStation.E, this.PointStation.N );
                return this.Coordinates.GetDistanceTo (basePoint2d); 
            }
        }

        /// <summary>
        /// Повертає текстовому вигляті відстань між станцією та точкою виносу.
        /// </summary>
        /// <param name="format">Формат значення відстані.</param>
        /// <returns>
        /// Значення  <see cref="T:System.string"/> відстані між станцією та точкою виносу.
        /// </returns>
        public string DistanceToString(AcRx.DistanceUnitFormat format)
        {
            return AcRx.Converter.DistanceToString(this.Distance, format, 3);
        }

        /// <summary>
        /// Повертає дирекційний кут напрямку зі станції на точку виносу в радіанах.
        /// </summary>
        /// <value>
        /// Значення дирекційного кута напрямку зі станції на точку виносу в радіанах.
        /// </value>
        public double DirlAngle
        {
            get
            {
                AcGe.Point2d basePoint2d = new AcGe.Point2d(this.PointStation.E, this.PointStation.N);
                return ServiceGeodesy.GetDirAngle(basePoint2d, this.Coordinates);
            }
        }

        /// <summary>
        /// Повертає текстове значення дирекційного кута напрямку зі станції 
        /// на точку виносу в форматі градуси минути та секунди.
        /// </summary>
        /// <param name="format">Формат вихідного значення кута.</param>
        /// <returns>
        /// Значення <see cref="T:System.string"/> дирекційного кута напрямку зі станції 
        /// на точку виносу в форматі градуси минути та секунди.
        /// </returns>
        public string DirAngleToString(AcRx.AngularUnitFormat format)
        {
            return AcRx.Converter.AngleToString(this.DirlAngle, format, 3);
        }

        /// <summary>
        /// Повертає лівий кут між напрямками зі станції на ориєнтир та точку виносу в радіанах.
        /// </summary>
        /// <value>
        /// Значення лівого кута між напрямками зі станції на ориєнтир та точкою виносу в радіанах.
        /// </value>
        public double LeftlAngle
        {
            get
            {
                AcGe.Point2d pointStation = new AcGe.Point2d(this.PointStation.E, this.PointStation.N);
                AcGe.Point2d pointOrientation = new AcGe.Point2d(this.PointOrientation.E, this.PointOrientation.N);
                return ServiceGeodesy.GetLeftAngle(pointOrientation, pointStation, this.Coordinates);
            }
        }

        /// <summary>
        /// Повертає текстове значення лівого кута між напрямками зі станції 
        /// на ориєнтир та точку виносу в форматі градуси минути та секунди.
        /// </summary>
        /// <param name="format">Формат вихідного значення кута.</param>
        /// <returns>
        /// Значення <see cref="T:System.string"/> лівого кута між напрямками зі станції 
        /// на ориєнтир та точку виносу в форматі градуси минути та секунди.
        /// </returns>
        public string LeftlAngleToString(AcRx.AngularUnitFormat format)
        {
            return AcRx.Converter.AngleToString(this.LeftlAngle, format, 3);
        }

        /// <summary>
        /// Повертає найблищу станцію планової основи до точки виносу.
        /// </summary>
        /// <param name="basePoints">Список точок планової основи.</param>
        /// <returns>
        /// Значення <see cref="T:LoSa.Land.ObjectGeo.BasePoint"/> найблищої станції планової основи до точки виносу.
        /// </returns>
        public BasePoint NearestPointStation(List<BasePoint> basePoints)
        {
            if (basePoints.Count > 0)
            {
                return this.SortingByDistanceFromPoint(basePoints)[0];
            }

            return null;
        }

        /// <summary>
        /// Повертає список <see cref="T:LoSa.Land.ObjectGeo.BasePoint"/> 
        /// сортований по віддаленості точки планової основи від точки виносу.
        /// </summary>
        /// <param name="basePoints">Список точок планової основи.</param>
        /// <returns>
        /// Список сортований по віддаленості точки планової основи від точки виносу
        /// </returns>
        public List<BasePoint> SortingByDistanceFromPoint(List<BasePoint> basePoints)
        {
            basePoints.Sort(    delegate (BasePoint x, BasePoint y)
            {
                double dist_X = this.Coordinates.GetDistanceTo(new AcGe.Point2d(x.E, x.N));
                double dist_Y = this.Coordinates.GetDistanceTo(new AcGe.Point2d(y.E, y.N));

                int result = dist_X.CompareTo(dist_Y);

                return result;
            });

            return basePoints;
        }

    }
}
