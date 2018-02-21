
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

    public class StakeOutParcelPoint
    {
        private AcDb.ObjectId txtDistanceID = AcDb.ObjectId.Null;
        private AcDb.ObjectId txtDirAngleID = AcDb.ObjectId.Null;
        private AcDb.ObjectId lineID = AcDb.ObjectId.Null;
        //private bool visible;

        public string Name { get; set; }
        public AcGe.Point2d Coordinates { get; set; }
        public BasePoint PointStation { get; set; }
        public BasePoint PointOrientation { get; set; }

        public bool Visible { get; set; }
       
        public AcDb.ObjectId TextDistanceID { get { return this.txtDistanceID; } }
        public AcDb.ObjectId TextDirAngleID { get { return this.txtDirAngleID; } }
        public AcDb.ObjectId LineStakeOutID { get { return this.lineID; } }

        public void Regen()
        {
            this.DeleteDrawingStakeOut();

            if (this.Visible == true)
            {
                this.CreateDrawingStakeOut();
            }
        }

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

            AcDb.DBText txtDistance = new AcDb.DBText();
            txtDistance.TextString = this.DistanceToString(AcRx.DistanceUnitFormat.Decimal);
            txtDistance.Rotation = line.Angle;
            txtDistance.Justify = AcDb.AttachmentPoint.BottomCenter;
            txtDistance.Position = pntMiddle;
            txtDistance.AlignmentPoint = pntMiddle;

            AcDb.DBText txtDirAngle = new AcDb.DBText();
            txtDirAngle.TextString = this.DirAngleToString(AcRx.AngularUnitFormat.DegreesMinutesSeconds);
            txtDirAngle.Rotation  = line.Angle;
            txtDirAngle.Justify = AcDb.AttachmentPoint.TopCenter;
            txtDirAngle.Position = pntMiddle;
            txtDirAngle.AlignmentPoint = pntMiddle;

            this.lineID = ServiceCAD.InsertObject(line);
            this.txtDistanceID = ServiceCAD.InsertObject(txtDistance);
            this.txtDirAngleID = ServiceCAD.InsertObject(txtDirAngle);

            //CurrentCAD.Editor.Regen();
        }

        private void DeleteDrawingStakeOut()
        {
            ServiceCAD.DeleteObject(this.lineID);
            ServiceCAD.DeleteObject(this.txtDistanceID);
            ServiceCAD.DeleteObject(this.txtDirAngleID);

            this.txtDistanceID = AcDb.ObjectId.Null;
            this.txtDirAngleID = AcDb.ObjectId.Null;
            this.lineID = AcDb.ObjectId.Null;
        }

        public double Distance
        {
            get
            {
                AcGe.Point2d basePoint2d = new AcGe.Point2d( this.PointStation.E, this.PointStation.N );
                return this.Coordinates.GetDistanceTo (basePoint2d); 
            }
        }

        public double DirlAngle
        {
            get
            {
                AcGe.Point2d basePoint2d = new AcGe.Point2d(this.PointStation.E, this.PointStation.N);
                return ServiceGeodesy.GetDirAngle(basePoint2d, this.Coordinates);
            }
        }

        public double LeftlAngle
        {
            get
            {
                AcGe.Point2d pointStation = new AcGe.Point2d(this.PointStation.E, this.PointStation.N);
                AcGe.Point2d pointOrientation = new AcGe.Point2d(this.PointOrientation.E, this.PointOrientation.N);
                return ServiceGeodesy.GetLeftAngle(pointOrientation, pointStation, this.Coordinates);
            }
        }

        public string DirAngleToString(AcRx.AngularUnitFormat format)
        {
            return AcRx.Converter.AngleToString(this.DirlAngle, format, 3);
        }

        public string DistanceToString(AcRx.DistanceUnitFormat format)
        {
            return AcRx.Converter.DistanceToString(this.Distance, format, 3);
        }
        
        public int IndexNearestPointStation(List<BasePoint> basePoints)
        {
            return -1;
        }

        public BasePoint NearestPointStation(List<BasePoint> basePoints)
        {
            return null;
        }

        public List<BasePoint> SortingByDistanceFromPoint(List<BasePoint> basePoints)
        {
            //BasePoint curPoint = new BasePoint(this.Name, this.Coordinates.Y, this.Coordinates.X, 0,"ParcelPoint");

            //double minDist = Double.MaxValue;

            basePoints.Sort(    delegate (BasePoint x, BasePoint y)
            {
                double dist_X = this.Coordinates.GetDistanceTo(new AcGe.Point2d(x.E, x.N));
                double dist_Y = this.Coordinates.GetDistanceTo(new AcGe.Point2d(y.E, y.N));

                int result = dist_X.CompareTo(dist_Y);

                return result;
            });

            /*
            basePoints.Sort(
                                delegate (double minDist, double curDist)
                                {
                                    return minDist.CompareTo(curDist);
                                }
                            );
            */
            return basePoints;
        }
        
    }
}
