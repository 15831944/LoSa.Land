
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

using LoSa.Land.EnumAttributes;
using LoSa.Land.ObjectExport;
using LoSa.Land.Parcel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LoSa.Land.ObjectExport
{
    public static class ServiceExport
    {
        public static void ToFile(LandParcel parcel, string pathExport, ExportFileFormat formatExport)
        {
            try
            {
                if (formatExport == ExportFileFormat.SDR20)
                {
                    ServiceExport.ToSDR(parcel, pathExport, ExportFileFormat.SDR20);
                }
                else if (formatExport == ExportFileFormat.SDR33)
                {
                    ServiceExport.ToSDR(parcel, pathExport, ExportFileFormat.SDR33);
                }
                else if (formatExport == ExportFileFormat.NXYZC)
                {
                    ServiceExport.ToText(parcel, pathExport, ExportFileFormat.NXYZC);
                }
                else if (formatExport == ExportFileFormat.NYXZC)
                {
                    ServiceExport.ToText(parcel, pathExport, ExportFileFormat.NYXZC);
                }
                else if (formatExport == ExportFileFormat.CREDO_DAT_TOP)
                {
                    ServiceExport.ToText(parcel, pathExport, ExportFileFormat.CREDO_DAT_TOP);
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Помилка експорту: " + ex.Message);
            }
        }

        public static void ToSDR(LandParcel parcel, string pathExport , ExportFileFormat formatExport )
        {
            DataSDR sdr = new DataSDR();

            HeaderRecordSDR hrSDR = new HeaderRecordSDR();
            sdr.Recording.Add(hrSDR);

            JobRecordSDR jrSDR = new JobRecordSDR();
            jrSDR.JobName = "Parcel_" + parcel.FindInfo("SC").Value;
            sdr.Recording.Add(jrSDR);

            int index = 0;
            string sdrData = "";

            foreach (AcGe.Point2d point in parcel.Points)
            {
                index++;
                CoordinatesRecordSDR crSDR =
                    new CoordinatesRecordSDR(
                        index.ToString(),
                        new AcGe.Point3d(new AcGe.Plane(), point),
                        parcel.FindInfo("SC").Value);
                sdr.Recording.Add(crSDR);

                if (formatExport == ExportFileFormat.SDR20)
                {
                    sdrData = sdr.ToStringSDR<FormatSDR20>();
                }
                else if (formatExport == ExportFileFormat.SDR33)
                {
                    sdrData = sdr.ToStringSDR<FormatSDR33>();
                }
            }

            SaveFile(sdrData, pathExport);
        }

        public static void ToText(LandParcel parcel, string pathExport, ExportFileFormat formatExport)
        {
            string txtData = "#Parcel_" + parcel.FindInfo("SC").Value;
            if (formatExport == ExportFileFormat.NXYZC)
            {
                txtData += "\r\n# NumberPoint; Northing; Easting ; Elevation; Code";
            }
            else if (formatExport == ExportFileFormat.NYXZC)
            {
                txtData += "\r\n# NumberPoint; Easting;  Northing; Elevation; Code";
            }
            else if (formatExport == ExportFileFormat.CREDO_DAT_TOP)
            {
                //txtData += "\r\n# NumberPoint; Code;  Northing; Easting; Elevation; ???";
            }

            int index = 0;

            foreach (AcGe.Point2d point in parcel.Points)
            {
                index++;

                if (formatExport == ExportFileFormat.NXYZC)
                {
                    txtData += "\r\n" + index.ToString("0000") + "; " +
                                        point.Y.ToString("0.000").Replace(",", ".") + "; " +
                                        point.X.ToString("0.000").Replace(",", ".") + "; 0.000; точка_межі";
                }
                else if (formatExport == ExportFileFormat.NYXZC)
                {
                    txtData += "\r\n" + index.ToString("0000") + "; " +
                                        point.X.ToString("0.000").Replace(",", ".") + "; " +
                                        point.Y.ToString("0.000").Replace(",", ".") + "; 0.000; точка_межі";
                }
                else if (formatExport == ExportFileFormat.CREDO_DAT_TOP)
                {
                    txtData += "\r\n" + DataSDR.Format(index.ToString(), 8, AlignmentText.ToLeft) + " 001 " +
                                      DataSDR.Format(point.Y.ToString("0.000").Replace(",", "."), 12, AlignmentText.ToRight) + " " +
                                      DataSDR.Format(point.X.ToString("0.000").Replace(",", "."), 12, AlignmentText.ToRight) + " " +
                                      DataSDR.Format(("0.000"), 12, AlignmentText.ToRight) + " " + "   0";
                }
            }

            SaveFile(txtData, pathExport);
        }

        private static void SaveFile(string dataFile, string nameFile)
        {
            StreamWriter sdrStreamWriter = new StreamWriter(nameFile);
            sdrStreamWriter.Write(dataFile);
            sdrStreamWriter.Close();
        }
    }
}
