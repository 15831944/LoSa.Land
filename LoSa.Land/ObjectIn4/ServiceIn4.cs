
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
using System.IO;
using System.Text;

using LoSa.Land.ObjectIn4;
using System.Reflection;
using System.Collections;
using LoSa.Land.Parcel;
using LoSa.Land.Service;

namespace LoSa.Land.ObjectIn4
{
    public static class ServiceIn4
    {

        public static BL GetDataFromFileIN4(String filePath)
        {
            List<String> listLines = ReadingLinesFromFileIN4(filePath);
            return ConvertingToObjectBL(listLines);
        }

        // ReadingLinesFromFileIN4

        private static List<String> ReadingLinesFromFileIN4(string filePath)
        {
            List<string> listFile = new List<String>();
            StreamReader streamReader = new StreamReader(filePath, Encoding.GetEncoding(866)); 

            string strBuff;

            while ((strBuff = streamReader.ReadLine()) != null)
            {
                string[] buffs = strBuff.Split( new string[] { ","}, StringSplitOptions.RemoveEmptyEntries);
                listFile.AddRange(buffs);
            }

            return listFile;
        }

        private static String EncodingValue(Encoding in_code,  Encoding out_code, String value)
        {
            byte[] in_bytes     = in_code.GetBytes(value);
            byte[] out_bytes    = Encoding.Convert(in_code, out_code, in_bytes, 0, value.Length);

            return out_code.GetString(out_bytes);
        }

        // ConvertingToObjectBL

        private static BL ConvertingToObjectBL(List<string> listLines)
        {
            string line;

            BL bl = new BL();

            for (Int32 indexLine = 0; indexLine < listLines.Count; indexLine++)
            {
                line = listLines[indexLine];

                if (line.IndexOf("##") > -1) 
                {
                    bl.Comments.Add(line.Replace("##", ""));
                }

                int isComment = line.IndexOf("##");

                if (line.IndexOf( "BL" ) > -1) 
                {
                    while (line.IndexOf("SR") < 0 ) 
                    {
                        indexLine++;
                        line = listLines[indexLine];

                        if (line.IndexOf("SR") > -1) { break; }
                        if (line.IndexOf("BS") > -1) { bl.BS = GetValueLine(line); }
                        else if (line.IndexOf("BC") > -1) { bl.BC = GetValueLine(line); }
                        else if (line.IndexOf("SZ") > -1) { bl.SZ = GetValueLine(line); }
                        else if (line.IndexOf("AB") > -1) { bl.AB = GetValueLine(line); }
                        else if (line.IndexOf("PB") > -1) { bl.PB = GetValueLine(line); }
                        else if (line.IndexOf("N=") > -1)
                        {
                            for ( int i = 0; i < 5; i++)
                            {
                                indexLine++;
                                line += "," + listLines[indexLine];
                            }
                            bl.Border.Add( GetPointLine(line) );
                        }
                    }
                }

                if (line.IndexOf("SR") > -1)
                {
                    marker_SR:
                        bl.SR.Add( new SR() );
                        while (line.IndexOf("NB") < 0 && line.IndexOf("BR") < 0) 
                        {
                            indexLine++;
                            if (indexLine > listLines.Count - 1) { break; }

                            line = listLines[indexLine];

                            if (line.IndexOf("SR") > -1) { goto marker_SR; }

                            if (line.IndexOf("##") > -1)      { bl.SR[bl.SR.Count-1].Comments.Add(line.Replace("##", "")); }
                            else if (line.IndexOf("SC") > -1) { bl.SR[bl.SR.Count-1].SC = GetValueLine(line); }
                            else if (line.IndexOf("AT") > -1) { bl.SR[bl.SR.Count-1].AT = GetValueLine(line); }
                            else if (line.IndexOf("NM") > -1) { bl.SR[bl.SR.Count-1].NM = GetValueLine(line); }
                            else if (line.IndexOf("AD") > -1) { bl.SR[bl.SR.Count-1].AD = GetValueLine(line); }
                            else if (line.IndexOf("TX") > -1) { bl.SR[bl.SR.Count-1].TX = GetValueLine(line); }
                            else if (line.IndexOf("ZS") > -1) { bl.SR[bl.SR.Count-1].ZS = GetValueLine(line); }
                            else if (line.IndexOf("PF") > -1) { bl.SR[bl.SR.Count-1].PF = GetValueLine(line); }
                            else if (line.IndexOf("CH") > -1) { bl.SR[bl.SR.Count-1].CH = GetValueLine(line); }
                            else if (line.IndexOf("KU") > -1) { bl.SR[bl.SR.Count-1].KU = GetValueLine(line); }
                            else if (line.IndexOf("KF") > -1) { bl.SR[bl.SR.Count-1].KF = GetValueLine(line); }
                            else if (line.IndexOf("AU") > -1) { bl.SR[bl.SR.Count-1].AU = GetValueLine(line); }
                            else if (line.IndexOf("SU") > -1) { bl.SR[bl.SR.Count-1].SU = GetValueLine(line); }
                            else if (line.IndexOf("PP") > -1) { bl.SR[bl.SR.Count-1].PP = GetValueLine(line); }
                            else if (line.IndexOf("PV") > -1) { bl.SR[bl.SR.Count-1].PV = GetValueLine(line); }
                            else if (line.IndexOf("CV") > -1) { bl.SR[bl.SR.Count-1].CV = GetValueLine(line); }
                            else if (line.IndexOf("CZ") > -1) { bl.SR[bl.SR.Count-1].CZ = GetValueLine(line); }
                            else if (line.IndexOf("PZ") > -1) { bl.SR[bl.SR.Count-1].PZ = GetValueLine(line); }
                            else if (line.IndexOf("TD") > -1) { bl.SR[bl.SR.Count-1].TD = GetValueLine(line); }
                            else if (line.IndexOf("GA") > -1) { bl.SR[bl.SR.Count-1].GA = GetValueLine(line); }
                            else if (line.IndexOf("FL") > -1) { bl.SR[bl.SR.Count-1].FL = GetValueLine(line); }
                            else if (line.IndexOf("KZ") > -1) { bl.SR[bl.SR.Count-1].KZ = GetValueLine(line); }

                            else if (line.IndexOf("RNM") > -1) { bl.SR[bl.SR.Count-1].RNM = GetValueLine(line); }
                            else if (line.IndexOf("RAU") > -1) { bl.SR[bl.SR.Count-1].RAU = GetValueLine(line); }
                            else if (line.IndexOf("RKU") > -1) { bl.SR[bl.SR.Count-1].RKU = GetValueLine(line); }
                            else if (line.IndexOf("RPV") > -1) { bl.SR[bl.SR.Count-1].RPV = GetValueLine(line); }
                            else if (line.IndexOf("RKZ") > -1) { bl.SR[bl.SR.Count-1].RKZ = GetValueLine(line); }
                            else if (line.IndexOf("RPF") > -1) { bl.SR[bl.SR.Count-1].RPF = GetValueLine(line); }
                            else if (line.IndexOf("AS") > -1) { bl.SR[bl.SR.Count - 1].AS = GetNumericLine(line); }
                            else if (line.IndexOf("PS") > -1) { bl.SR[bl.SR.Count - 1].PS = GetNumericLine(line); }
                            else if ( line.IndexOf("N=") > -1 &&  
                                      line.IndexOf("CN") < 0 )
                            {   
                                for ( int i = 0; i < 5; i++)
                                {
                                    indexLine++;
                                    line += "," + listLines[indexLine];
                                }
                                bl.SR[bl.SR.Count - 1].Border.Add(GetPointLine(line));
                            }

                            else if (line.IndexOf("OB") > -1) 
                            {
                            marker_OB:
                                bl.SR[bl.SR.Count - 1].OB.Add(new OB());

                                while ( line.IndexOf("NB") < 0 )
                                {
                                    indexLine++;
                                    if (indexLine > listLines.Count - 1) { break; }

                                    line = listLines[indexLine];

                                    if (line.IndexOf("SR") > -1) { goto marker_SR; }
                                    if (line.IndexOf("OB") > -1) { goto marker_OB; }
                                    if (line.IndexOf("CL") > -1) { indexLine--; break; }

                                    if (line.IndexOf("##") > -1) 
                                    { 
                                        bl.SR[bl.SR.Count - 1].
                                            OB[bl.SR[bl.SR.Count - 1].OB.Count - 1].
                                            Comments.Add(line.Replace("##", "")); 
                                    }
                                    else if (line.IndexOf("OK") > -1)
                                    {
                                        bl.SR[bl.SR.Count - 1].
                                            OB[bl.SR[bl.SR.Count - 1].OB.Count - 1].
                                            OK = GetValueLine(line); ;
                                    }
                                    else if (line.IndexOf("OX") > -1)
                                    {
                                        bl.SR[bl.SR.Count - 1].
                                            OB[bl.SR[bl.SR.Count - 1].OB.Count - 1].
                                            OX = GetValueLine(line); ;
                                    }
                                    else if (line.IndexOf("OD") > -1)
                                    {
                                        bl.SR[bl.SR.Count - 1].
                                            OB[bl.SR[bl.SR.Count - 1].OB.Count - 1].
                                            OD = GetValueLine(line); ;
                                    }
                                    else if (line.IndexOf("OT") > -1)
                                    {
                                        bl.SR[bl.SR.Count - 1].
                                            OB[bl.SR[bl.SR.Count - 1].OB.Count - 1].
                                            OT = GetValueLine(line); ;
                                    }
                                    else if (line.IndexOf("AO") > -1)
                                    {
                                        bl.SR[bl.SR.Count - 1].
                                            OB[bl.SR[bl.SR.Count - 1].OB.Count - 1].
                                            AO = GetNumericLine(line); ;
                                    }
                                    else if (line.IndexOf("PO") > -1)
                                    {
                                        bl.SR[bl.SR.Count - 1].
                                            OB[bl.SR[bl.SR.Count - 1].OB.Count - 1].
                                            PO = GetNumericLine(line); ;
                                    }
                                    else if (line.IndexOf("N=") > -1 &&
                                              line.IndexOf("CN") < 0) 
                                    {
                                        for (int i = 0; i < 5; i++)
                                        {
                                            indexLine++;
                                            line += "," + listLines[indexLine];
                                        }
                                        bl.SR[bl.SR.Count - 1].
                                                OB[bl.SR[bl.SR.Count - 1].OB.Count - 1].
                                                Border.Add(GetPointLine(line)); 
                                    }
                                }
                            }
                            else if (line.IndexOf("CL") > -1) 
                            {
                            marker_CL:
                                bl.SR[bl.SR.Count - 1].CL.Add(new CL());

                                while (line.IndexOf("NB") < 0)
                                {
                                    indexLine++;
                                    if (indexLine > listLines.Count - 1) { break; }

                                    line = listLines[indexLine];

                                    if (line.IndexOf("SR") > -1) { goto marker_SR; }
                                    if (line.IndexOf("CL") > -1) { goto marker_CL; }
                                    if (line.IndexOf("OB") > -1) { indexLine--;  break; }

                                    if (line.IndexOf("##") > -1)
                                    {
                                        bl.SR[bl.SR.Count - 1].
                                            CL[bl.SR[bl.SR.Count - 1].CL.Count - 1].
                                            Comments.Add(line.Replace("##", ""));
                                    }
                                    else if (line.IndexOf("CI") > -1)
                                    {
                                        bl.SR[bl.SR.Count - 1].
                                            CL[bl.SR[bl.SR.Count - 1].CL.Count - 1].
                                            //CI = Convert.ToInt32(GetNumericLine(line));
                                            CI = GetValueLine(line);
                                }
                                    else if (line.IndexOf("CN") > -1)
                                    {
                                        bl.SR[bl.SR.Count - 1].
                                            CL[bl.SR[bl.SR.Count - 1].CL.Count - 1].
                                            CN = GetValueLine(line); ;
                                    }
                                    else if (line.IndexOf("CC") > -1)
                                    {
                                        bl.SR[bl.SR.Count - 1].
                                            CL[bl.SR[bl.SR.Count - 1].CL.Count - 1].
                                            CC = GetValueLine(line); ;
                                    }
                                    else if (line.IndexOf("CZR") > -1)
                                    {
                                        bl.SR[bl.SR.Count - 1].
                                            CL[bl.SR[bl.SR.Count - 1].CL.Count - 1].
                                            CZR = GetValueLine(line); ;
                                    }
                                    else if (line.IndexOf("CZG") > -1)
                                    {
                                        bl.SR[bl.SR.Count - 1].
                                            CL[bl.SR[bl.SR.Count - 1].CL.Count - 1].
                                            CZG = GetValueLine(line); ;
                                    }
                                    else if (line.IndexOf("CPR") > -1)
                                    {
                                        bl.SR[bl.SR.Count - 1].
                                            CL[bl.SR[bl.SR.Count - 1].CL.Count - 1].
                                            CPR = GetValueLine(line); ;
                                    }
                                    else if (line.IndexOf("CPD") > -1)
                                    {
                                        bl.SR[bl.SR.Count - 1].
                                            CL[bl.SR[bl.SR.Count - 1].CL.Count - 1].
                                            CPD = GetValueLine(line); ;
                                    }
                                    else if (line.IndexOf("AL") > -1)
                                    {
                                        bl.SR[bl.SR.Count - 1].
                                            CL[bl.SR[bl.SR.Count - 1].CL.Count - 1].
                                            AL = GetNumericLine(line); ;
                                    }
                                    else if (line.IndexOf("PL") > -1)
                                    {
                                        bl.SR[bl.SR.Count - 1].
                                            CL[bl.SR[bl.SR.Count - 1].CL.Count - 1].
                                            PL = GetNumericLine(line); ;
                                    }
                                    else if (line.IndexOf("N=") > -1 &&
                                              line.IndexOf("CN") < 0)
                                    {
                                        for (int i = 0; i < 5; i++)
                                        {
                                            indexLine++;
                                            line += "," + listLines[indexLine];
                                        }
                                        bl.SR[bl.SR.Count - 1].
                                                CL[bl.SR[bl.SR.Count - 1].CL.Count - 1].
                                                Border.Add(GetPointLine(line));
                                    }
                                }
                            }
                        } // while while (line.IndexOf("NB") < 0 )
                    }

                if (line.IndexOf("BR") > -1)
                {
                    bl.BR = new BR();

                    while ( line.IndexOf("NB") < 0 )
                    {
                        indexLine++;

                        if (indexLine > listLines.Count - 1) { break; }

                        line = listLines[indexLine];
                        
                        if (line.IndexOf("##") > -1)
                        {
                            bl.NB[bl.NB.Count - 1].Comments.Add(line.Replace("##", ""));
                        }
                        else if (line.IndexOf("AB") > -1)
                        {
                            bl.AB = GetValueLine(line);
                        }
                        else if (line.IndexOf("PB") > -1)
                        {
                            bl.PB = GetValueLine(line);
                        }
                        else if (line.IndexOf("N=") > -1 && line.IndexOf("CN") < 0)
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                indexLine++;
                                line += "," + listLines[indexLine];
                            }
                            bl.BR.Border.Add(GetPointLine(line));
                        }
                    }
                }

                if (line.IndexOf("NB") > -1)
                {
                    marker_NB:
                    bl.NB.Add(new NB());

                    while (indexLine < listLines.Count ) 
                    {
                        indexLine++;

                        if (indexLine > listLines.Count - 1)  {  break;  }

                        line = listLines[indexLine];
                        if (line.IndexOf("NB") > -1) { goto marker_NB; }

                        if  ( line.IndexOf("##") > -1 )
                        {
                                bl.NB[bl.NB.Count - 1].Comments.Add(line.Replace("##", ""));
                        }
                        else if (line.IndexOf("NM") > -1)
                        {
                                bl.NB[bl.NB.Count - 1].NM = GetValueLine(line);
                        }
                        else if ( line.IndexOf("N=") > -1 && line.IndexOf("CN") < 0 )
                        {
                            for (int i = 0; i < 5; i++)
                            {
                                indexLine++;
                                line += "," + listLines[indexLine];
                            }
                            bl.NB[bl.NB.Count - 1].Border.Add(GetPointLine(line));
                        }
                    }

                    bl.NB[bl.NB.Count - 1].Border.Reverse();

                }

            }

            return bl; ;
        }

        private static Point GetPointLine(string line)
        {
            Point point = new Point();

            string[] arrayPoint = line.Split(',');

            foreach (string value in arrayPoint)
            {
                if (value.IndexOf("N") > -1 && value.IndexOf("NP") < 0)  
                { 
                    point.Number = Convert.ToInt32(GetValueLine(value)); 
                }
                else if (value.IndexOf("NP") > -1)  
                { 
                    point.Name = GetValueLine(value); 
                }
                else if (value.IndexOf("X") > -1 && value.IndexOf("MX") < 0 )   
                { 
                    point.Y = Convert.ToDouble(GetValueLine(value).Replace(".",",")); 
                }
                else if (value.IndexOf("Y") > -1 && value.IndexOf("MY") < 0 )   
                { 
                    point.X = Convert.ToDouble(GetValueLine(value).Replace(".", ",")); 
                }
                else if (value.IndexOf("MX") > -1)  
                { 
                    point.Mx = Convert.ToDouble(GetValueLine(value).Replace(".", ",")); 
                }
                else if (value.IndexOf("MY") > -1)  
                { 
                    point.My = Convert.ToDouble(GetValueLine(value).Replace(".", ",")); 
                }
            }

            return point;
        }

        private static string GetValueLine(string line)
        {
            string[] arrayValue = line.Split('=');
            arrayValue[1] = arrayValue[1]              
                .Replace("\"", "")
                .Replace("\\", "\"")
                .Replace(",", ", ")
                .Replace("  ", " ").Trim();
            return arrayValue[1];
        }

        private static string GetNumericLine(string line)
        {       
            string[] arrayValue = line.Split('=');
            arrayValue[1] = arrayValue[1].Replace(",", "").Replace("\"", "").Trim();
            return arrayValue[1];
        }

        // GetParcel

        public static LandParcel GetParcelForCadarstralNumber(BL blockLand, String cadastralNumber)
        {
            LandParcel parcel = null;

            if (blockLand != null)
            {
                parcel = new LandParcel();

                foreach (SR curSR in blockLand.SR)
                {
                    if (curSR.SC == cadastralNumber)
                    {
                        parcel.Info = GetInfoFromIn4Polygon<SR>(curSR);
                        parcel.Points = GetPoints2dFromIn4Polygon(curSR);
                        parcel.Lands = GetLandPoligonsFromIn4Polygon(curSR.CL);
                        parcel.Limiting = GetLandPoligonsFromIn4Polygon(curSR.OB);
                        parcel.ContoursNeighbors = GetNeighborsForParcel(blockLand, curSR);
                        parcel.OtherLand = GetOtherLandForParcel(blockLand, curSR);

                        break;
                    }
                }
            }

            //parcel.Neighbors = SortingNeighbors(parcel);

            return parcel;
        }

        public static LandPolygon GetPolygonForCadarstralNumber(BL blockLand, String cadastralNumber)
        {
            LandParcel parcel = null;

            if (blockLand != null)
            {
                parcel = new LandParcel();

                foreach (SR curSR in blockLand.SR)
                {
                    if (curSR.SC == cadastralNumber)
                    {
                        parcel.Info = GetInfoFromIn4Polygon<SR>(curSR);
                        parcel.Points = GetPoints2dFromIn4Polygon(curSR);

                        break;
                    }
                }
            }

            return parcel;
        }

        public static List<LandPolygon> GetAllPolygons(BL blockLand)
        {
            List<LandPolygon> parcels = new List<LandPolygon>();
            LandPolygon parcel = null;

            if (blockLand != null)
            {
                foreach (SR curSR in blockLand.SR)
                {
                    parcel = new LandPolygon();
                    parcel.Info = GetInfoFromIn4Polygon<SR>(curSR);
                    parcel.Points = GetPoints2dFromIn4Polygon(curSR);
                    parcels.Add(parcel);
                }
            }

            return parcels;
        }

        private static List<LandPolygon> SortingNeighbors(LandPolygon parcel, List<LandPolygon> neighbors)
        {
            List<LandPolygon> sortedNeighbors = new List<LandPolygon>();
            List<LandPolygon> temp = new List<LandPolygon>();

            AcGe.Point2d point0 = AcGe.Point2d.Origin;

            foreach (AcGe.Point2d pointParcel in parcel.Points)
            {

               temp = neighbors.FindAll
                            (
                                delegate (LandPolygon neighbor)
                                {
                                    return neighbor.Points.ToArray()[1].GetDistanceTo(pointParcel) == 0;
                                }
                            );
                /*
                if (temp.Count == 1)
                {
                    sortedNeighbors.Add(temp.ToArray()[0]);
                }
                else */
                if (temp.Count > 0)
                {

                    temp.Sort
                        (
                            delegate (LandPolygon polygon1, LandPolygon polygon2)
                            {
                                AcGe.Line2d line0 = new AcGe.Line2d(pointParcel, point0);

                                AcGe.Point2d point1 = polygon1.Points.ToArray()[0];
                                AcGe.Line2d line1 = new AcGe.Line2d(pointParcel, point1);

                                AcGe.Point2d point2 = polygon2.Points.ToArray()[0];
                                AcGe.Line2d line2 = new AcGe.Line2d(pointParcel, point2);

                                double angle1 = line0.Direction.GetAngleTo(line1.Direction);
                                double angle2 = line0.Direction.GetAngleTo(line2.Direction);

                                return angle1.CompareTo(angle2);
                            }
                        );

                    foreach ( LandPolygon polygon in temp)
                    {
                        sortedNeighbors.Add(polygon);
                    }
                }

                point0 = pointParcel;
            }



            return sortedNeighbors;
        }
      

        private static double LengthPolylineBetweenPoints(AcGe.Point2dCollection polylinePoints, 
                                                            AcGe.Point2d startPoint, 
                                                            AcGe.Point2d endPoint)
         {
            int indexStart = polylinePoints.IndexOf(startPoint);
            int indexEnd = polylinePoints.IndexOf(endPoint);

            if (indexStart < 0 || indexEnd < 0 ) return double.NaN;

            if (indexStart > indexEnd)
            {
                indexEnd  = polylinePoints.IndexOf(startPoint);
                indexStart = polylinePoints.IndexOf(endPoint);
            }

            double length = 0;

            for (int i = indexStart; i < indexEnd; i++)
            {
                length += polylinePoints.ToArray()[i].GetDistanceTo(polylinePoints.ToArray()[i+1]);
            }
            return length;
         }

        private static List<LandPolygon> GetLandPoligonsFromIn4Polygon<T>(List<T> list)  where T: In4Polygon
        {
            List<LandPolygon> listLand = new List<LandPolygon>();
            LandPolygon landPolygon;
            foreach (T land in list)
            {
                landPolygon = new LandPolygon();
                landPolygon.Info = GetInfoFromIn4Polygon<T>(land);
                landPolygon.Points = GetPoints2dFromIn4Polygon(land);
                listLand.Add(landPolygon);
            }
            return listLand; 
        }

        private static List<LandPolygon> GetOtherLandForParcel(BL blockLand, SR curSR)
        {
            List<LandPolygon> otherLand = new List<LandPolygon>();

            LandPolygon landPolygon;

            foreach (SR sr in blockLand.SR)
            {
                if (sr.SC.IndexOf(":-") > 0)
                {
                    landPolygon = new LandPolygon();
                    landPolygon.Info = GetInfoFromIn4Polygon<SR>(sr);
                    landPolygon.Points = GetPoints2dFromIn4Polygon(sr);
                    otherLand.Add(landPolygon);
                }
            }

            return otherLand; 
        }

        private static List<LandPolygon> GetBlockLandNeighbors(BL blockLand)
        {
            List<LandPolygon> neighbors = new List<LandPolygon>();

            LandPolygon landPolygon;

            foreach (NB nb in blockLand.NB)
            {
                landPolygon = new LandPolygon();
                landPolygon.Info = GetInfoFromIn4Polygon<NB>(nb);

                AcGe.Point2d[]  pointsNeighbor = GetPoints2dFromIn4Polygon(nb).ToArray();
                Array.Reverse(pointsNeighbor);
                landPolygon.Points = new AcGe.Point2dCollection(pointsNeighbor);


                neighbors.Add(landPolygon);
            }


            return neighbors; 
        }

        private static List<NeighborsAlongContour> GetNeighborsForParcel(BL blockLand, SR curSR)
        {

            List<NeighborsAlongContour> neighbors = new List<NeighborsAlongContour>();

            LandPolygon polygonParcel = GetPolygonForCadarstralNumber(blockLand, curSR.SC);

            Dictionary<LandPolygon,TypeContour> contoursParcel = GetContours(polygonParcel);

            List<LandPolygon>   allNeighbors = new List<LandPolygon>();
                                allNeighbors.AddRange(GetAllPolygons(blockLand));
                                allNeighbors.AddRange(GetBlockLandNeighbors(blockLand));

            // Neighboring Parcels & BlockLandNeighbors

            foreach ( KeyValuePair<LandPolygon, TypeContour> contourParcel in contoursParcel)
            {
                neighbors.Add(GetNeighborsAlongContour (allNeighbors, contourParcel.Key));
            }

            return neighbors;
        }

        private static NeighborsAlongContour GetNeighborsAlongContour(List<LandPolygon> allNeighbors, LandPolygon contourParcel)
        {
            NeighborsAlongContour contour = new NeighborsAlongContour();


            foreach (LandPolygon curNeighbor in allNeighbors)
            {

                LandInfo curNeighbor_SC = curNeighbor.FindInfo("SC");
                LandInfo curNeighbor_NM = curNeighbor.FindInfo("NM");

                if (contourParcel.IsCoincidesDirectionBorder(curNeighbor))
                {
                    curNeighbor.ReverseBorder();
                }

                if (contourParcel.IsCommonPoints(curNeighbor) )
                {

                    //if ( ! contourParcel.FindInfo("SC").Value.Equals( curNeighbor_SC.Value ) )
                   
                    if (curNeighbor_SC != null)
                    {
                        if (contourParcel.FindInfo("SC").Value != curNeighbor_SC.Value)
                        {

                            contour.Neighbors.AddRange(contourParcel.GetNeighborLines(curNeighbor));
                        }
                    }
                    else
                    {
                        contour.Neighbors.AddRange(contourParcel.GetNeighborLines(curNeighbor));
                    }
                   
                }
            }

            contour.Neighbors = ServiceIn4.SortingNeighbors(contourParcel, contour.Neighbors);

            return contour;
        }


        public static Dictionary<LandPolygon, TypeContour> GetContours(LandPolygon  polygon)
        {

            List<PolygonSegment> allSegments = polygon.GetPolygonSegments();
            List<NeighborsSegment> segments = new List<NeighborsSegment>();

            foreach ( PolygonSegment segment in allSegments)
            {
                segments.Add(new NeighborsSegment(segment, TypeNeighbor.Undefined));
            }

            segments = ServiceNeighborsSegments.JoinAdjoiningSegments(segments,true);
            
            Dictionary<LandPolygon, TypeContour> contours = new Dictionary<LandPolygon, TypeContour>();

            NeighborsSegment curSegment;

            AcGe.Point2d backPoint = segments[0].MediumPoint;

            LandPolygon contour = new LandPolygon(polygon.Info);

            while (segments.Count > 0) 
            {

                List<NeighborsSegment> findSegments = ServiceNeighborsSegments.FindNeighborSegments(segments, backPoint);

                if ( findSegments.Count > 0 )
                {
                    curSegment = findSegments[0];

                    contour.Points.Add(curSegment.MediumPoint);

                    backPoint = curSegment.FrontPoint;
                    segments.Remove(segments.Find(f => f.Equals(curSegment)));
                }
                else
                {
                    backPoint = segments[0].MediumPoint;

                    contours.Add(contour, TypeContour.Internal);
                    contour = new LandPolygon(polygon.Info);
                }

            }

            contours.Add(contour, TypeContour.Internal);

            return contours;
        }

        public static AcGe.Point2dCollection GetPoints2dFromIn4Polygon(In4Polygon curSR)
        {
            AcGe.Point2dCollection points = new AcGe.Point2dCollection();

            foreach (Point point in curSR.Border)
            {
                points.Add(new AcGe.Point2d(point.X, point.Y));
            }

            return points;
        }


        public static AcGe.Point3dCollection GetPoints3dFromIn4Polygon(In4Polygon curSR)
        {
            AcGe.Point3dCollection points = new AcGe.Point3dCollection();

            foreach (Point point in curSR.Border)
            {
                points.Add(new AcGe.Point3d(point.X, point.Y, 0));
            }

            return points;
        }

        private static List<LandInfo> GetInfoFromIn4Polygon<T>(T in4Polygon) where T : In4Polygon
        {
            List<LandInfo> info = new List<LandInfo>();
            LandInfo landInfo = null;
            foreach (PropertyInfo property in typeof(T).GetProperties())
            {
                if (property.GetValue(in4Polygon, null) != null && 
                    typeof(String) == property.GetValue(in4Polygon, null).GetType())
                {
                    landInfo = new LandInfo(property.Name, (String)property.GetValue(in4Polygon, null));
                    info.Add( landInfo );
                }
            }

            return info;
        }
    }
}
