
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

using LoSa.Land.Parcel;

namespace LoSa.Land.Service
{
    public static class ServiceNeighborsSegments
    {
        public static List<NeighborsSegment> GetFirstStartingNeighborsSegments
                                                ( 
                                                    LandPolygon parcel, 
                                                    LandPolygon neighbor
                                                )
        {
            List<NeighborsSegment> neighborsSegments = new List<NeighborsSegment>();

            neighborsSegments.AddRange
                (
                    parcel.GetCommonNeighborSegmentsByType(neighbor, TypeNeighbor.Starting)
                );

            neighborsSegments.AddRange
                (
                    parcel.GetCommonNeighborSegmentsByType(neighbor, TypeNeighbor.OnlyOnePoint_Outside)
                );

            List<NeighborsSegment> foundSegments = new List<NeighborsSegment>();

            foreach ( AcGe.Point2d point in parcel.Points)
            {
                foundSegments = neighborsSegments.FindAll
                                    (
                                        delegate (NeighborsSegment segment)
                                        {
                                            return segment.MediumPoint.Equals(point);
                                        }
                                    );
                if (foundSegments.Count > 0) break;
            }

            return foundSegments;
        }

        public static List<NeighborsSegment> FindNeighborSegments
                                                (
                                                    List<NeighborsSegment> segments, 
                                                    AcGe.Point2d point
                                                )
        {
            if (segments == null)
            {
                return new List<NeighborsSegment>();
            }

            try
            { 
               return segments.FindAll
               (
                   delegate (NeighborsSegment segment)
                   {
                       return segment.MediumPoint.Equals(point);
                   }
               );
            }
            catch (NullReferenceException)
            {
                return new List<NeighborsSegment>();
            }
           
        }

        public static List<NeighborsSegment> JoinAdjoiningSegments(List<NeighborsSegment> segments, bool isParcel)
        {

            if (segments == null) { return null; }

            List<NeighborsSegment> resultSegments = new List<NeighborsSegment>();

            if ( segments.Count > 1 )
            {
                NeighborsSegment newSegment;
                NeighborsSegment segmentBase;

/* ReStart */   markReStart:

                int index = -1;

                while (segments.Count > 0) 
                {
                    index++;

                    segmentBase = segments[0];

                    List<NeighborsSegment> segmentsFind = FindNeighborSegments(segments, segmentBase.MediumPoint);

                    if (segmentsFind.Count > 1)
                    {
                        foreach (NeighborsSegment segmentFind in segmentsFind)
                        {
                            ResultComparePolygonSegments result = segmentBase.CompareSegments(segmentFind);

                            if (
                                    result.MediumPoint == CoincidencePoints.Coincides_With_Medium &&
                                    !segmentBase.Equals(segmentFind)
                                )
                            {
                                if (
                                        result.BackPoint == CoincidencePoints.Coincides_With_Nothing &&
                                        result.FrontPoint == CoincidencePoints.Coincides_With_Nothing
                                    )
                                {
                                    newSegment = new NeighborsSegment(  segmentFind.FrontPoint, 
                                                                        segmentBase.MediumPoint, 
                                                                        segmentBase.BackPoint, 
                                                                        TypeNeighbor.Intermediate
                                                                        );

                                    resultSegments.Add(newSegment);

                                    newSegment = new NeighborsSegment(  segmentBase.FrontPoint, 
                                                                        segmentBase.MediumPoint, 
                                                                        segmentFind.BackPoint, 
                                                                        TypeNeighbor.Intermediate
                                                                        );
                                }
                                else
                                {
                                    if (isParcel)
                                    {
                                        newSegment = segmentBase.JoinAdjoiningSegmentParcel(segmentFind);
                                    }
                                    else
                                    {
                                        newSegment = segmentBase.JoinAdjoiningSegmentNeighbor(segmentFind);
                                    }
                                }
 
                                bool rSb = segments.Remove(segments.Find(f => f.Equals(segmentBase)));
                                bool rFs = segments.Remove(segments.Find(f => f.Equals(segmentFind)));

                                resultSegments.Add(newSegment);
                                /*
                                if (
                                        result.FrontPoint == CoincidencePoints.Coincides_With_Nothing && 
                                        result.BackPoint == CoincidencePoints.Coincides_With_Nothing
                                    )
                                {
                                    if (isParcel)
                                    {
                                        newSegment = segmentBase.JoinAdjoiningSegmentParcel(segmentFind);
                                    }
                                    else
                                    {
                                        newSegment = segmentBase.JoinAdjoiningSegmentNeighbor(segmentFind);
                                    }
                                    resultSegments.Add((NeighborsSegment)newSegment);
                                    
                                }
                                */
                                goto markReStart;
                            }
                        }  
                    }
                    else if (segmentsFind.Count == 1)
                    {
                        bool rSb = segments.Remove(segments.Find(f => f.Equals(segmentBase)));
                        resultSegments.Add(segmentBase);
                    }
                }  
            }
            else
            {
                resultSegments = segments;
            }

            return resultSegments;

        }

        public static bool IsAdjoiningSegments(List<NeighborsSegment> segments)
        {
            foreach (NeighborsSegment segmentBase in segments)
            {
                foreach (NeighborsSegment segmentFind in segments)
                {
                    if (segmentBase != null)
                    {
                        if (
                           segmentBase.IsAdjoiningSegment(segmentFind) &&
                           !segmentBase.Equals(segmentFind)
                       )
                        {
                            return true;
                        }
                    }
                   
                }

            }

            return false;

        }

        public static LandPolygon ExtractNeighborByStartingSegment
                                                ( 
                                                    LandPolygon parcel, 
                                                    LandPolygon neighbor, 
                                                    NeighborsSegment startingSegment
                                                )
        {
            List<NeighborsSegment> intermediateNeighborSegments =
                        parcel.GetCommonNeighborSegmentsByType(neighbor, TypeNeighbor.Intermediate);

            List<NeighborsSegment> endingNeighborSegments =
                        parcel.GetCommonNeighborSegmentsByType(neighbor, TypeNeighbor.Ending);



            AcGe.Point2d backPoint = AcGe.Point2d.Origin;
            List<NeighborsSegment> nextSegments;
            NeighborsSegment nextSegment = startingSegment;
            LandPolygon neighborLine = new LandPolygon(neighbor.Info, false);
 
            int index = intermediateNeighborSegments.Count + endingNeighborSegments.Count;

            while ( index != -1 )
            {

                if (
                        nextSegment.TypeNeighbor == TypeNeighbor.Starting &&
                        nextSegment.TypeNeighbor != TypeNeighbor.OnlyOnePoint_Outside
                    )
                {
                    
                    neighborLine.Points.Add(nextSegment.FrontPoint);
                    neighborLine.Points.Add(nextSegment.MediumPoint);
                }

                neighborLine.Points.Add(nextSegment.BackPoint);

                backPoint = nextSegment.BackPoint;

                if (
                        nextSegment.TypeNeighbor == TypeNeighbor.Ending &&
                        nextSegment.TypeNeighbor != TypeNeighbor.OnlyOnePoint_Outside
                    )
                {
                    break;
                }

                nextSegments = FindNeighborSegments(intermediateNeighborSegments, backPoint);

                if (nextSegments.Count == 0)
                {
                    nextSegments = FindNeighborSegments(endingNeighborSegments, backPoint);
                    if (nextSegments.Count == 0 && startingSegment.MediumPoint.Equals(backPoint))
                    {
                        neighborLine.Points.Add(startingSegment.FrontPoint);
                    }
                }

                if (nextSegments.Count == 1)
                {
                    nextSegment = nextSegments[0];
                }

                index--;
            }

            return neighborLine;
        }
    }
  
}
