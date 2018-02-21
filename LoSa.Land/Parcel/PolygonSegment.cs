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

namespace LoSa.Land.Parcel
{
    public interface IPolygonSegment
    {
        AcGe.Point2d FrontPoint { get; set; }
        AcGe.Point2d MediumPoint { get; set; }
        AcGe.Point2d BackPoint { get; set; }
    }

    public class PolygonSegment : IPolygonSegment
    {
        public AcGe.Point2d FrontPoint { get; set; }
        public AcGe.Point2d MediumPoint { get; set; }
        public AcGe.Point2d BackPoint { get; set; }

        public PolygonSegment() : 
            this    (
                        AcGe.Point2d.Origin,    
                        AcGe.Point2d.Origin, 
                        AcGe.Point2d.Origin
                    )
        {

        }

        public PolygonSegment(AcGe.Point2d frontPoint, AcGe.Point2d mediumPoint, AcGe.Point2d backPoint)
        {
            this.FrontPoint = frontPoint;
            this.MediumPoint = mediumPoint;
            this.BackPoint = backPoint;
        }

        public ResultComparePolygonSegments CompareSegments( PolygonSegment polygonSegment )
        {
            if ( polygonSegment == null ) { return new ResultComparePolygonSegments(); }

            ResultComparePolygonSegments result = new ResultComparePolygonSegments();

            if ( this.FrontPoint.GetDistanceTo( polygonSegment.FrontPoint) == 0 )
            {
                result.FrontPoint = CoincidencePoints.Coincides_With_Front;
            }
            else if (this.FrontPoint.GetDistanceTo(polygonSegment.MediumPoint) == 0)
            {
                result.FrontPoint = CoincidencePoints.Coincides_With_Medium;
            }
            else if (this.FrontPoint.GetDistanceTo(polygonSegment.BackPoint) == 0)
            {
                result.FrontPoint = CoincidencePoints.Coincides_With_Back;
            }

            if (this.MediumPoint.GetDistanceTo( polygonSegment.FrontPoint) == 0)
            {
                result.MediumPoint = CoincidencePoints.Coincides_With_Front;
            }
            else if (this.MediumPoint.GetDistanceTo(polygonSegment.MediumPoint) == 0)
            {
                result.MediumPoint = CoincidencePoints.Coincides_With_Medium;
            }
            else if (this.MediumPoint.GetDistanceTo(polygonSegment.BackPoint) == 0)
            {
                result.MediumPoint = CoincidencePoints.Coincides_With_Back;
            }

            if (this.BackPoint.GetDistanceTo(polygonSegment.FrontPoint) == 0)
            {
                result.BackPoint = CoincidencePoints.Coincides_With_Front;
            }
            else if (this.BackPoint.GetDistanceTo(polygonSegment.MediumPoint) == 0)
            {
                result.BackPoint = CoincidencePoints.Coincides_With_Medium;
            }
            else if (this.BackPoint.GetDistanceTo(polygonSegment.BackPoint) == 0)
            {
                result.BackPoint = CoincidencePoints.Coincides_With_Back;
            }

            return result;
        }

        public AcGe.Point2d[] ToArray()
        {
            return new AcGe.Point2d[] 
                {
                    this.FrontPoint,
                    this.MediumPoint,
                    this.BackPoint
                };
        }

        public bool IsAdjoiningSegment(PolygonSegment segment)
        {
            if (
                    this.CompareSegments(segment).FrontPoint == CoincidencePoints.Coincides_With_Nothing &&
                    this.CompareSegments(segment).MediumPoint == CoincidencePoints.Coincides_With_Medium &&
                    this.CompareSegments(segment).BackPoint == CoincidencePoints.Coincides_With_Front
                )
            {
                return true;
            }
            else if (
                       this.CompareSegments(segment).FrontPoint == CoincidencePoints.Coincides_With_Back &&
                       this.CompareSegments(segment).MediumPoint == CoincidencePoints.Coincides_With_Medium &&
                       this.CompareSegments(segment).BackPoint == CoincidencePoints.Coincides_With_Nothing
                    )
            {
                return true;
            }
           
            return false;

        }

        public PolygonSegment JoinAdjoiningSegment( PolygonSegment segment )
        {
            if  (
                    this.CompareSegments(segment).FrontPoint == CoincidencePoints.Coincides_With_Nothing &&
                    this.CompareSegments(segment).MediumPoint == CoincidencePoints.Coincides_With_Medium &&
                    this.CompareSegments(segment).BackPoint == CoincidencePoints.Coincides_With_Front
                )
            {
                this.BackPoint = segment.BackPoint;
            }
            else if (
                       this.CompareSegments(segment).FrontPoint == CoincidencePoints.Coincides_With_Back &&
                       this.CompareSegments(segment).MediumPoint == CoincidencePoints.Coincides_With_Medium &&
                       this.CompareSegments(segment).BackPoint == CoincidencePoints.Coincides_With_Nothing
                    )
            {
                this.FrontPoint = segment.FrontPoint;
            }
            else
            {
                return null;
            }

            return this;

        }

        internal TypeNeighbor GetTypeNeighbor_OnlyOnePoint(PolygonSegment neighborSegmet)
        {
            double angle_base = ServiceGeodesy
                .GetRightAngle(this.FrontPoint, this.MediumPoint, this.BackPoint);// *180/Math.PI;
            double angle_front = ServiceGeodesy
                .GetRightAngle(this.FrontPoint, this.MediumPoint, neighborSegmet.FrontPoint);// * 180 / Math.PI;
            double angle_back = ServiceGeodesy
                .GetRightAngle(this.FrontPoint, this.MediumPoint, neighborSegmet.BackPoint);// * 180 / Math.PI;

            if (angle_base < angle_front && angle_base < angle_back)
            {
                return TypeNeighbor.OnlyOnePoint_Inside;
            }
            else
            {
                return TypeNeighbor.OnlyOnePoint_Outside;
            }
        }
    }

    public class ResultComparePolygonSegments
    {
        public CoincidencePoints FrontPoint { get; set; }
        public CoincidencePoints MediumPoint { get; set; }
        public CoincidencePoints BackPoint { get; set; }

        public ResultComparePolygonSegments() :
            this(
                        CoincidencePoints.Coincides_With_Nothing,
                        CoincidencePoints.Coincides_With_Nothing,
                        CoincidencePoints.Coincides_With_Nothing
                    )
        {

        }

        public ResultComparePolygonSegments(
                                                CoincidencePoints frontPoint,
                                                CoincidencePoints mediumPoint,
                                                CoincidencePoints backPoint
                                            )
        {
            this.FrontPoint = frontPoint;
            this.MediumPoint = mediumPoint;
            this.BackPoint = backPoint;
        }
    }

    public enum CoincidencePoints
    {
        Coincides_With_Nothing = 0,
        Coincides_With_Front = 1,
        Coincides_With_Medium = 2,
        Coincides_With_Back = 3
    }

    public class NeighborsSegment : PolygonSegment
    {
        public TypeNeighbor TypeNeighbor { get; set; }


        public NeighborsSegment() :
            this(
                        AcGe.Point2d.Origin,
                        AcGe.Point2d.Origin,
                        AcGe.Point2d.Origin,
                        TypeNeighbor.Undefined
                    )
        {

        }

        public NeighborsSegment (
                                    AcGe.Point2d frontPoint, 
                                    AcGe.Point2d mediumPoint, 
                                    AcGe.Point2d backPoint, 
                                    TypeNeighbor typeNeighbor
                                ) : 
            base( frontPoint, mediumPoint, backPoint)
        {
            this.TypeNeighbor = typeNeighbor;
        }

        public NeighborsSegment ( 
                                    PolygonSegment polygonSegment, 
                                    TypeNeighbor typeNeighbor 
                                ) : 
            base(polygonSegment.FrontPoint, polygonSegment.MediumPoint, polygonSegment.BackPoint)
        {
            this.TypeNeighbor = typeNeighbor;
        }

        public bool Equals(NeighborsSegment segment)
        {
            if  ( 
                    this.FrontPoint.Equals(segment.FrontPoint) &&
                    this.MediumPoint.Equals(segment.MediumPoint) &&
                    this.BackPoint.Equals(segment.BackPoint) &&
                    this.TypeNeighbor.Equals(segment.TypeNeighbor) 
                )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public NeighborsSegment JoinAdjoiningSegmentParcel(NeighborsSegment segment)
        {

            NeighborsSegment    newSegment = new NeighborsSegment( (PolygonSegment)this, TypeNeighbor.Undefined);
                                //newSegment.TypeNeighbor = TypeNeighbor.Undefined;

            if (
                    this.CompareSegments(segment).FrontPoint == CoincidencePoints.Coincides_With_Nothing &&
                    this.CompareSegments(segment).MediumPoint == CoincidencePoints.Coincides_With_Medium &&
                    this.CompareSegments(segment).BackPoint == CoincidencePoints.Coincides_With_Front
                )
            {
                newSegment.BackPoint = segment.BackPoint;

            }
            else if (
                       this.CompareSegments(segment).FrontPoint == CoincidencePoints.Coincides_With_Back &&
                       this.CompareSegments(segment).MediumPoint == CoincidencePoints.Coincides_With_Medium &&
                       this.CompareSegments(segment).BackPoint == CoincidencePoints.Coincides_With_Nothing
                    )
            {
                newSegment.FrontPoint = segment.FrontPoint;
            }
            else if (
                       this.CompareSegments(segment).FrontPoint == CoincidencePoints.Coincides_With_Nothing &&
                       this.CompareSegments(segment).MediumPoint == CoincidencePoints.Coincides_With_Medium &&
                       this.CompareSegments(segment).BackPoint == CoincidencePoints.Coincides_With_Nothing
                    )
            {
                newSegment.FrontPoint = segment.FrontPoint;
            }
            else
            {
                return null;
            }

            return newSegment;

        }


        public NeighborsSegment JoinAdjoiningSegmentNeighbor(NeighborsSegment segment)
        {

            NeighborsSegment newSegment = this;
            TypeNeighbor type = TypeNeighbor.Undefined;

            if  (   
                    (this.TypeNeighbor == TypeNeighbor.Starting && segment.TypeNeighbor == TypeNeighbor.Ending) ||
                    (this.TypeNeighbor == TypeNeighbor.Ending && segment.TypeNeighbor == TypeNeighbor.Starting) 
                )
            {
                type = TypeNeighbor.Intermediate;
            }
            else if (this.TypeNeighbor == TypeNeighbor.OnlyOnePoint_Outside && segment.TypeNeighbor == TypeNeighbor.OnlyOnePoint_Outside)
            {
                type = TypeNeighbor.OnlyOnePoint_Outside;
            }
            else if (this.TypeNeighbor == TypeNeighbor.OnlyOnePoint_Inside && segment.TypeNeighbor == TypeNeighbor.OnlyOnePoint_Inside)
            {
                type = TypeNeighbor.OnlyOnePoint_Inside;
            }
            else if (
                        (this.TypeNeighbor == TypeNeighbor.Starting && segment.TypeNeighbor == TypeNeighbor.OnlyOnePoint_Outside) ||
                        (this.TypeNeighbor == TypeNeighbor.OnlyOnePoint_Outside && segment.TypeNeighbor == TypeNeighbor.Starting) ||
                        (this.TypeNeighbor == TypeNeighbor.Starting && segment.TypeNeighbor == TypeNeighbor.OnlyOnePoint_Inside) ||
                        (this.TypeNeighbor == TypeNeighbor.OnlyOnePoint_Inside && segment.TypeNeighbor == TypeNeighbor.Starting)
                    )
            {
                type = TypeNeighbor.Starting;
            }
            else if (
                        (this.TypeNeighbor == TypeNeighbor.Ending && segment.TypeNeighbor == TypeNeighbor.OnlyOnePoint_Outside) ||
                        (this.TypeNeighbor == TypeNeighbor.OnlyOnePoint_Outside && segment.TypeNeighbor == TypeNeighbor.Ending) ||
                        (this.TypeNeighbor == TypeNeighbor.Ending && segment.TypeNeighbor == TypeNeighbor.OnlyOnePoint_Inside) ||
                        (this.TypeNeighbor == TypeNeighbor.OnlyOnePoint_Inside && segment.TypeNeighbor == TypeNeighbor.Ending)
                    )
            {
                type = TypeNeighbor.Ending;
            }


            if (
                    this.CompareSegments(segment).FrontPoint == CoincidencePoints.Coincides_With_Nothing &&
                    this.CompareSegments(segment).MediumPoint == CoincidencePoints.Coincides_With_Medium &&
                    this.CompareSegments(segment).BackPoint == CoincidencePoints.Coincides_With_Front
                )
            {
                newSegment.BackPoint = segment.BackPoint;

            }
            else if (
                       this.CompareSegments(segment).FrontPoint == CoincidencePoints.Coincides_With_Back &&
                       this.CompareSegments(segment).MediumPoint == CoincidencePoints.Coincides_With_Medium &&
                       this.CompareSegments(segment).BackPoint == CoincidencePoints.Coincides_With_Nothing
                    )
            {
                newSegment.FrontPoint = segment.FrontPoint;
            }
            else
            {
                return null;
            }

            newSegment.TypeNeighbor = type;

            return newSegment;

        }

    }

    public enum TypeNeighbor
    {
        Undefined,
        Starting,
        Intermediate,
        Ending,
        OnlyOnePoint_Outside,
        OnlyOnePoint_Inside,
    }

}
