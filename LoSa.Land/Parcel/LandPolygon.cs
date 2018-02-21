
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

using LoSa.Land.Service;
using System.Threading;
using Teigha.Geometry;
using LoSa.CAD;

namespace LoSa.Land.Parcel
{
    public interface ILandPolygon
    {
        List<LandInfo> Info { get; set; }
        AcGe.Point2dCollection Points { get; set; }
        bool Closed { get; set; }
    }

    [Serializable()]
    public class LandPolygon : ILandPolygon
    {
        [XmlArray("Інформація_про_полігон")]
        [XmlArrayItem("Інфо")]
        public List<LandInfo> Info { get; set; }

        [XmlIgnore()]
        public AcGe.Point2dCollection Points { get; set; }

        [XmlArray("Замкнений_полігон")]
        [XmlArrayItem("Замкнений")]
        public bool Closed { get; set; }

        public LandPolygon()
        {
            this.Info = new List<LandInfo>();
            this.Points = new AcGe.Point2dCollection();
            this.Closed = true;
        }

        public LandPolygon(AcGe.Point2dCollection points)
        {
            this.Info = new List<LandInfo>();
            this.Points = points;
            this.Closed = true;
        }

        public LandPolygon(List<LandInfo> info)
        {
            this.Info = info;
            this.Points = new AcGe.Point2dCollection();
            this.Closed = true;
        }

        public LandPolygon(List<LandInfo> info, bool closed)
        {
            this.Info = info;
            this.Points = new AcGe.Point2dCollection();
            this.Closed = closed;
        }

        public LandPolygon(List<LandInfo> info, AcGe.Point2dCollection points)
        {
            this.Info = info;
            this.Points = points;
            this.Closed = true;
        }

        public LandPolygon(List<LandInfo> info, AcGe.Point2dCollection points, bool closed)
        {
            this.Info = info;
            this.Points = points;
            this.Closed = closed;
        }

        public LandInfo FindInfo(string key)
        {
            return Info.Find
                (
                    delegate(LandInfo landInfo)
                    {
                        return landInfo.Key == key;
                    }
                );
        }

        private Point2d FindPoint(Point2d point)
        {
            List<Point2d> points = new List<Point2d>();
            points.AddRange(Points.ToArray());

            return points.Find
                (
                    delegate (Point2d pnt)
                    {
                        return pnt == point;
                    }
                );
        }

        public bool IsCommonPoints(LandPolygon polygon)
        {
            AcGe.Point2d pnt;
            for ( int i = 0; i < this.Points.Count; i++)
            {
                pnt = new AcGe.Point2d();
                pnt = this.Points.ToArray()[i];
                if (polygon.Points.Contains(pnt)) return true;
            }
            return false;
        }


        public List<PolygonSegment> GetPolygonSegments()
        {
            if (this.Points.Count < 3) return null;

            List<PolygonSegment> segmentsBorder = new List<PolygonSegment>();
            AcGe.Point2d[] arrayPoints = this.Points.ToArray();

            for ( int i = 0; i < this.Points.Count; i++)
            {
                if (i == 0)
                {
                    segmentsBorder.Add
                        (
                            new PolygonSegment
                                (
                                    arrayPoints[i + 1], 
                                    arrayPoints[i], 
                                    arrayPoints[this.Points.Count - 1]
                                ) 
                        );
                }
                else if (i == this.Points.Count - 1)
                {
                    segmentsBorder.Add
                         (
                             new PolygonSegment
                                 (
                                     arrayPoints[0],
                                     arrayPoints[i],
                                     arrayPoints[i-1]
                                 )
                         );
                }
                else
                {
                    segmentsBorder.Add
                         (
                             new PolygonSegment
                                 (
                                     arrayPoints[i + 1],
                                     arrayPoints[i],
                                     arrayPoints[i - 1]
                                 )
                         );
                }
            }

            return segmentsBorder;
        }

        public List<PolygonSegment> GetCommonPolygonSegments(LandPolygon polygon)
        {
            List<PolygonSegment> allSegmets = polygon.GetPolygonSegments();
            List<PolygonSegment> commonPolygonSegments = null;

            foreach ( AcGe.Point2d point in this.Points)
            {
                commonPolygonSegments.AddRange
                    ( 
                        allSegmets.FindAll
                            (
                                delegate (PolygonSegment segment)
                                {
                                    return segment.MediumPoint.Equals(point);
                                }
                            ) 
                    );
            }

            return commonPolygonSegments;
        }

        public List<NeighborsSegment> GetCommonNeighborSegments(LandPolygon polygon)
        {
            List<NeighborsSegment> commonNeighborSegments = new List<NeighborsSegment>();

            List<PolygonSegment> parcelSegmets = this.GetPolygonSegments();
            List<PolygonSegment> foundSegments = null;
            ResultComparePolygonSegments resultCompare = null;

            foreach ( PolygonSegment parcelSegment in parcelSegmets )
            {
                foundSegments = polygon.GetPolygonSegmentsByMediumPoint(parcelSegment.MediumPoint);

                foreach ( PolygonSegment neighborSegmet in foundSegments)
                {
                    resultCompare = parcelSegment.CompareSegments(neighborSegmet);

                    if      (
                                resultCompare.FrontPoint == CoincidencePoints.Coincides_With_Nothing &&
                                resultCompare.MediumPoint == CoincidencePoints.Coincides_With_Medium &&
                                resultCompare.BackPoint == CoincidencePoints.Coincides_With_Nothing
                            )
                    {
                        commonNeighborSegments.Add 
                            (  
                                new NeighborsSegment( neighborSegmet, parcelSegment.GetTypeNeighbor_OnlyOnePoint(neighborSegmet) )
                            );

                    }
                    else if (
                                resultCompare.FrontPoint == CoincidencePoints.Coincides_With_Nothing &&
                                resultCompare.MediumPoint == CoincidencePoints.Coincides_With_Medium &&
                                resultCompare.BackPoint == CoincidencePoints.Coincides_With_Front
                            )
                    {
                        commonNeighborSegments.Add
                            (
                                new NeighborsSegment(neighborSegmet, TypeNeighbor.Ending)
                            );
                    }
                    else if (
                                resultCompare.FrontPoint == CoincidencePoints.Coincides_With_Back &&
                                resultCompare.MediumPoint == CoincidencePoints.Coincides_With_Medium &&
                                resultCompare.BackPoint == CoincidencePoints.Coincides_With_Front
                            )
                    {
                        commonNeighborSegments.Add
                            (
                                new NeighborsSegment(neighborSegmet, TypeNeighbor.Intermediate)
                            );
                    }
                    else if (
                                resultCompare.FrontPoint == CoincidencePoints.Coincides_With_Back &&
                                resultCompare.MediumPoint == CoincidencePoints.Coincides_With_Medium &&
                                resultCompare.BackPoint == CoincidencePoints.Coincides_With_Nothing
                            )
                    {
                        commonNeighborSegments.Add
                            (
                                new NeighborsSegment(neighborSegmet, TypeNeighbor.Starting)
                            );
                    }
                    else if (
                               resultCompare.MediumPoint == CoincidencePoints.Coincides_With_Medium
                            )
                    {
                        commonNeighborSegments.Add
                            (
                                new NeighborsSegment(neighborSegmet, TypeNeighbor.Undefined)
                            );
                    }
                }
            }

            return commonNeighborSegments;
        }

        public List<NeighborsSegment> GetCommonNeighborSegmentsByType(LandPolygon polygonNeighbor, TypeNeighbor typeNeighbor)
        {
            List<NeighborsSegment> neighborSegments = this.GetCommonNeighborSegments(polygonNeighbor);

            if (!this.IsAllPointsParcelCommonNeighbor(polygonNeighbor))
            {
                neighborSegments = ServiceNeighborsSegments.JoinAdjoiningSegments(neighborSegments,false);
            }

            return neighborSegments.FindAll
                (
                    delegate (NeighborsSegment segment)
                    {
                        return segment.TypeNeighbor == typeNeighbor;
                    }
                );
        }

        private bool IsAllPointsParcelCommonNeighbor(LandPolygon polygonNeighbor)
        {
            int common = 0;
            AcGe.Point2dCollection commonPoints = new AcGe.Point2dCollection();

            foreach ( AcGe.Point2d point in this.Points)
            {
                foreach (AcGe.Point2d pointNeighbor in polygonNeighbor.Points)
                {
                    if ( point.Equals(pointNeighbor) )
                    { 
                        if ( !commonPoints.Contains(point) )
                        {
                            common++;
                            commonPoints.Add(point);
                        }
                        
                    }
                }
            }

            if (common == this.Points.Count)  return true; 
            else return false;
        }

        public List<PolygonSegment> GetPolygonSegmentsByMediumPoint(AcGe.Point2d mediumPoint)
        {
            List<PolygonSegment> polygonSegments = this.GetPolygonSegments();

            return polygonSegments.FindAll
                (
                    delegate (PolygonSegment segment)
                    {
                        return segment.MediumPoint.Equals(mediumPoint);
                    }
                );
        }

        public List<LandPolygon> GetNeighborLines(LandPolygon polygon)
        {
            if (this.IsCommonPoints(polygon))
            {
                List<LandPolygon> neighborLines = new List<LandPolygon>();
                LandPolygon neighborLine = new LandPolygon(polygon.Info);

                List<NeighborsSegment> startingNeighborSegments =
                    this.GetCommonNeighborSegmentsByType(polygon, TypeNeighbor.Starting);

                List<NeighborsSegment> endingNeighborSegments =
                    this.GetCommonNeighborSegmentsByType(polygon, TypeNeighbor.Ending);

                List<NeighborsSegment> onlyOnePointOutsideNeighborSegments =
                    this.GetCommonNeighborSegmentsByType(polygon, TypeNeighbor.OnlyOnePoint_Outside);

                List<NeighborsSegment> onlyOnePointInsideNeighborSegments =
                    this.GetCommonNeighborSegmentsByType(polygon, TypeNeighbor.OnlyOnePoint_Inside);

                List<NeighborsSegment> intermediateNeighborSegments =
                    this.GetCommonNeighborSegmentsByType(polygon, TypeNeighbor.Intermediate);

                if ( this.Points.Count - intermediateNeighborSegments.Count == 0 )
                {
                    neighborLines.Add(polygon);
                }
                else
                {
                    AcGe.Point2d backPoint = AcGe.Point2d.Origin;

                    foreach ( NeighborsSegment startingNeighborSegment in startingNeighborSegments)
                    {
                        neighborLines.Add
                            ( 
                                ServiceNeighborsSegments
                                    .ExtractNeighborByStartingSegment
                                        ( this, polygon, startingNeighborSegment )
                            );
                    }

                    AcGe.Point2d[] pointsOnlyOnePointOutside;

                    foreach (NeighborsSegment onlyOnePointOutsideNeighborSegment in onlyOnePointOutsideNeighborSegments)
                    {
                        pointsOnlyOnePointOutside = new AcGe.Point2d[]
                                                        {
                                                            onlyOnePointOutsideNeighborSegment.FrontPoint,
                                                            onlyOnePointOutsideNeighborSegment.MediumPoint,
                                                            onlyOnePointOutsideNeighborSegment.BackPoint
                                                        };
                        neighborLines.Add
                            (
                                new LandPolygon 
                                        (
                                            polygon.Info, 
                                            new AcGe.Point2dCollection(pointsOnlyOnePointOutside)
                                        )
                            );
                    }

                    
                }

                return neighborLines;

            }

            return null;
        }
        
        public bool IsCoincidesDirectionBorder(LandPolygon polygon)
        {
            if (this.IsCommonPoints(polygon))
            {

                List<NeighborsSegment> polygonSegments = 
                    this.GetCommonNeighborSegmentsByType(polygon,TypeNeighbor.Undefined);
                

                //List<PolygonSegment> polygonSegments = this.GetCommonPolygonSegments(polygon);

                if (polygonSegments.Count > 0)
                {
                    List<PolygonSegment> thisSegments =
                        this.GetPolygonSegmentsByMediumPoint(polygonSegments.ToArray()[0].MediumPoint);

                    if (    polygonSegments.ToArray()[0]
                                .FrontPoint.Equals( thisSegments.ToArray()[0].FrontPoint) )
                    {
                        return true;
                    }

                }
                return false;
            }
            else
            {
                return false;
            } 
        }

        public void ReverseBorder()
        {
            Point2d[] borderPoints = this.Points.ToArray();
            Array.Reverse(borderPoints);
            this.Points.Clear();
            this.Points.AddRange(borderPoints);

            //Array.Reverse(this.Points.ToArray());

        }
        /*
        internal bool IsBelongs(LandPolygon curNeighbor)
        {
            foreach( PolygonSegment segmentNeighbor in curNeighbor.GetPolygonSegments())
            {
                Point2d point = segmentNeighbor.MediumPoint;
                if ( ! this.IsBelongs(point) )
                {
                    return false;
                }
            }

            return true;

        }

        internal bool IsBelongs(PolygonSegment segment)
        {
            throw new NotImplementedException();
        }

        internal bool IsBelongs(Point2d point)
        {
            Point2d  findPoints = this.FindPoint(point);
            if (findPoints != Point2d.Origin)
            {
                return true;
            }

            List<Point2dCollection> points = new List<Point2dCollection>();

            points.Add(
                            new Point2dCollection
                            (
                                new Point2d[]
                                    { point, point.Add( new Vector2d(0, 100000) ) }
                                    
                            )
                        );
            points.Add(
                            new Point2dCollection
                            (
                                new Point2d[]
                                    { point, point.Add( new Vector2d(0, -100000) ) }

                            )
                        );
            points.Add(
                            new Point2dCollection
                            (
                                new Point2d[]
                                    { point, point.Add( new Vector2d(100000, 0) ) }

                            )
                        );
            points.Add(
                            new Point2dCollection
                            (
                                new Point2d[]
                                    { point, point.Add( new Vector2d(-100000, 0) ) }

                            )
                        );


            AcDb.Polyline2d polygon = CAD.ServiceSimpleElements.CreatePolyline2d(this.Points,true);  

            List<Point3dCollection> intersects = new List<Point3dCollection>(4);
            Point3dCollection intersect;

            for (int i = 0; i < 4; i++)
            {
                intersect = new Point3dCollection();
                CAD.ServiceSimpleElements.CreatePolyline2d(points[i], false)
                    .IntersectWith(polygon, AcDb.Intersect.ExtendThis, intersect,IntPtr.Zero, IntPtr.Zero);
                intersects.Add( DeleteDuplicatePoints(intersect) );
            }

            if ( 
                    intersects[0].Count == intersects[1].Count 
                    && intersects[2].Count == intersects[3].Count 
                    && intersects[0].Count == intersects[3].Count
                )
            {
                if (intersects[0].Count % 2 == 0)  return false; 
                else return true;
            }
            else
            {
                return false;
            }
            
        }
        */


        private Point3dCollection DeleteDuplicatePoints(Point3dCollection points)
        {
            Dictionary<int, Point3d> dicPoints = new Dictionary<int, Point3d>();
            foreach( Point3d pnt in points)
            {
                if ( ! dicPoints.ContainsKey(pnt.GetHashCode()))
                {
                    dicPoints.Add(pnt.GetHashCode(), pnt);
                }
                    
            }

            Point3dCollection newPoints = new Point3dCollection();

            foreach (KeyValuePair<int, Point3d> item in dicPoints)
            {
                newPoints.Add( item.Value );
            }

            return newPoints;
        }
    }
}
