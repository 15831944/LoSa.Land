using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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


namespace LoSa.Utility
{
    public static class ServiceGeodesy
    {
        public static double GetDirAngle(AcGe.Point3d currentPoint, AcGe.Point3d frontPoint)
        {
            double dirAngle = -1;
            double dX = frontPoint.Y - currentPoint.Y;
            double dY = frontPoint.X - currentPoint.X;

            if (dY == 0)
            {
                if (dX >= 0) dirAngle = 0;
                if (dX < 0) dirAngle = Math.PI;
                return dirAngle;
            }
            else if (dX == 0)
            {
                if (dY >= 0) dirAngle = Math.PI / 2;
                if (dY < 0) dirAngle = 3 * Math.PI / 2;
                return dirAngle;
            }

            double rumb = Math.Atan(dY / dX);

            if (dX > 0 && dY > 0)   // I   chetvert//
            {
                dirAngle = rumb;
            }
            else if (dX < 0 && dY > 0)   // II  chetvert//
            {
                dirAngle = Math.PI + rumb;
            }
            else if (dX < 0 && dY < 0)   // III chetvert//
            {
                dirAngle = Math.PI + rumb;
            }
            else if (dX > 0 && dY < 0)   // IV  chetvert//
            {
                dirAngle = 2 * Math.PI + rumb;
            }
            return dirAngle;
        }

        public static double GetDirAngle(AcGe.Point2d currentPoint, AcGe.Point2d frontPoint)
        {
            return GetDirAngle(
                        new AcGe.Point3d(new AcGe.Plane(), currentPoint),
                        new AcGe.Point3d(new AcGe.Plane(), frontPoint)
                        );
        }

        public static double GetLeftAngle(AcGe.Point3d backPoint, AcGe.Point3d currentPoint, AcGe.Point3d frontPoint)
        {
            double dirPrv = GetDirAngle(currentPoint, backPoint);
            double dirNxt = GetDirAngle(currentPoint, frontPoint);
            double leftAngle = (dirNxt - dirPrv);

            if (leftAngle < 0) leftAngle += Math.PI * 2;

            return leftAngle;
        }

        public static double GetLeftAngle(AcGe.Point2d backPoint, AcGe.Point2d currentPoint, AcGe.Point2d frontPoint)
        {
            return GetLeftAngle(
                        new AcGe.Point3d(new AcGe.Plane(), backPoint),
                        new AcGe.Point3d(new AcGe.Plane(), currentPoint),
                        new AcGe.Point3d(new AcGe.Plane(), frontPoint)
                        );
        }

        public static double GetRightAngle(AcGe.Point3d backPoint, AcGe.Point3d currentPoint, AcGe.Point3d frontPoint)
        {
            double leftAngle = GetLeftAngle(backPoint, currentPoint, frontPoint);
            return Math.PI * 2 - leftAngle;
        }

        public static double GetRightAngle(AcGe.Point2d backPoint, AcGe.Point2d currentPoint, AcGe.Point2d frontPoint)
        {
            return GetRightAngle(
                        new AcGe.Point3d(new AcGe.Plane(), backPoint),
                        new AcGe.Point3d(new AcGe.Plane(), currentPoint),
                        new AcGe.Point3d(new AcGe.Plane(), frontPoint)
                        );
        }

        public static double GetOffsetFromLine(AcDb.Line line, AcGe.Point3d point)
        {
            return GetOffsetFromLine(line.StartPoint, line.EndPoint, point);
        }

        public static double GetOffsetFromLine(AcGe.Point3d startPointLine, AcGe.Point3d endPointLine, AcGe.Point3d point)
        {
            double dirAngleLine = GetDirAngle(startPointLine, endPointLine);
            double dirAngleToPoint = GetDirAngle(startPointLine, point);
            double a = dirAngleToPoint - dirAngleLine;

            return Math.Sin(a) * startPointLine.GetVectorTo(point).Convert2d(new AcGe.Plane()).Length;

        }

        public static double GetProjectionOnLine(AcDb.Line line, AcGe.Point3d point)
        {
            return GetProjectionOnLine(line.StartPoint, line.EndPoint, point);
        }

        public static double GetProjectionOnLine(AcGe.Point3d startPointLine, AcGe.Point3d endPointLine, AcGe.Point3d point)
        {
            double dirAngleLine = GetDirAngle(startPointLine, endPointLine);
            double dirAngleToPoint = GetDirAngle(startPointLine, point);
            double a = dirAngleToPoint - dirAngleLine;

            return Math.Cos(a) * startPointLine.GetVectorTo(point).Convert2d(new AcGe.Plane()).Length;
        }
    }
}
