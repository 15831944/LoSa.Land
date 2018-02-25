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
using LoSa.Land.Tables;

using LoSa.Land.Parcel;

using LoSa.CAD;
using LoSa.Xml;
using LoSa.Utility;
using System.Text;

namespace LoSa.Land.Parcel
{
    public class ServiceParcel
    {
        LocalPath localPath = new LocalPath("LoSa_Land");

        public LandParcel Parcel { get; set; }
        public SettingsFormLand SettingsForm { get; set; }
        private SettingsDrawing settingsDrawing;

        //private int versionCAD;

        public ServiceParcel(LandParcel parcel, SettingsFormLand formSettings)
        {
            settingsDrawing = ServiceXml.ReadXml<SettingsDrawing>(localPath.FindFullPathFromXml("PathDrawing"));
            if (settingsDrawing == null) settingsDrawing = SettingsDrawing.Default;
            this.Parcel = parcel;
            this.SettingsForm = formSettings;
        }

        private List<TextNeighbors> allTextNeighbors = new List<TextNeighbors>();

        public void BuildingPlan()
        {
            AddFills();
            AddNeighbors();
            AcDb.ObjectId idBorderParcel = AddBorders();
            AddPoints(idBorderParcel);
            if (SettingsForm.DisplayPointNumbers)  AddNumdersPoints();
            if (SettingsForm.DisplayLengthLine)  AddLengthLine();
            AddAreaAndPerimetr();
            if (SettingsForm.DisplayFillNeighbors) AddTextNeighbors();
        }

        private int currentLetterNeighbor = 0;
        private static string[] letters = new string[]
        {
            "А ", "Б ", "В ", "Г ", "Д ", "Е ", "Є ", "Ж ", "З ", "И ",
            "І ", "Ї ", "Й ", "К ", "Л ", "М ", "Н ", "О ", "П ", "Р ",
            "С ", "Т ", "У ", "Ф ", "Х ", "Ц ", "Ч ", "Ш ", "Щ ", "Ю ", "Я "
        };
        
        private string GetLetterNeighbor()
        {
            List<string> letterNeighbor = new List<string>();
            for (int iLetter = 0; iLetter < 10; iLetter++)
            {
                foreach( string letter in letters)
                {
                    if (iLetter == 0)
                    {
                        letterNeighbor.Add(letter);
                    }
                    else
                    {
                        letterNeighbor.Add(letter + (iLetter).ToString());
                    }
                }
            }

            currentLetterNeighbor ++;
            if (currentLetterNeighbor >= letterNeighbor.Count)
            {
                currentLetterNeighbor = 0;
            }
            return letterNeighbor[currentLetterNeighbor-1];
        }

        private void AddFills()
        {
            Dictionary<string,HatchPolygon> listMissingHatch = new Dictionary<string,HatchPolygon>();
            //
            //  Parcel
            //

            if (SettingsForm.DisplayFillParcel)
            {
                AcDb.Polyline2d borderParcel = ServiceSimpleElements.CreatePolyline2d(this.Parcel.Points, true);
                AcDb.ObjectId idBorderParcel = ServiceCAD.InsertObject(borderParcel);
                AcDb.Hatch borderParcelHatch = ServiceSimpleElements.CreateHatch(idBorderParcel, true);

                HatchPolygon hatchParcel = HatchPolygon.GetHatchParcel();

                if (hatchParcel != null)
                {
                    borderParcelHatch.LayerId = ServiceCAD.CreateLayer(settingsDrawing.Plan.FillParcel.Layer);
                    borderParcelHatch.ColorIndex = hatchParcel.ColorIndex;
                    borderParcelHatch.SetHatchPattern(hatchParcel.Pattern.Type, hatchParcel.Pattern.Name);
                    borderParcelHatch.PatternAngle = hatchParcel.Pattern.Angle;
                    borderParcelHatch.PatternSpace = hatchParcel.Pattern.Space;

                    AcDb.ObjectId idborderParcelHatch = ServiceCAD.InsertObject(borderParcelHatch);
                }
                else
                {
                    try
                    {
                        listMissingHatch.Add("Parcel", new HatchPolygon("Parcel", "", 0, PatternHatch.DEFAULT));
                        CurrentCAD.Editor.WriteMessage("\n<!> Незнайдено штриховка Parcel;");
                    }
                    catch { }
                }
            }

            //
            //  Lands
            //

            AcDb.Polyline2d borderLand;
            AcDb.ObjectId idLand;
            AcDb.Hatch hatchLand;
            AcDb.ObjectId idHatchLand;

            if (SettingsForm.DisplayFillLand)
            {
                foreach (LandPolygon poligon in this.Parcel.Lands)
                {
                    borderLand = ServiceSimpleElements.CreatePolyline2d(poligon.Points, true);
                    borderLand.LayerId = ServiceCAD.CreateLayer(settingsDrawing.Plan.FillLand.Layer);
                    idLand = ServiceCAD.InsertObject(borderLand);
                    hatchLand = ServiceSimpleElements.CreateHatch(idLand, false);

                    HatchPolygon hatch = HatchPolygon.GetHatchLand(poligon);

                    if (hatch != null)
                    {
                        hatchLand.LayerId = ServiceCAD.CreateLayer(settingsDrawing.Plan.FillLand.Layer);
                        hatchLand.ColorIndex = hatch.ColorIndex;
                        hatchLand.SetHatchPattern(hatch.Pattern.Type, hatch.Pattern.Name);
                        hatchLand.PatternAngle = hatch.Pattern.Angle;
                        hatchLand.PatternSpace = hatch.Pattern.Space;

                        idHatchLand = ServiceCAD.InsertObject(hatchLand);
                    }
                    else 
                    {
                        string type = "000.00";
                        try
                        {
                            type = poligon.FindInfo("CC").Value;
                        }
                        catch
                        {
                            string ci = "*";
                            string cn = "*";
                            try
                            {
                                LandInfo infoCI = poligon.FindInfo("CI");
                                ci = infoCI.Value;
                            }
                            catch { }

                            try
                            {
                                LandInfo infoCN = poligon.FindInfo("CN");
                                cn = infoCN.Value;
                            }
                            catch { }

                            CurrentCAD.Editor.WriteMessage
                                ("\n<!> Незнайдено поле CC угіддя CI = {0}, CN = {1};",
                                ci,
                                cn);
                        }

                        try
                        {
                            listMissingHatch.Add(type, new HatchPolygon(type, "", 0, PatternHatch.DEFAULT));
                            CurrentCAD.Editor.WriteMessage
                                ("\n<!> Незнайдено CC = {0};",
                                    poligon.FindInfo("CC").Value
                                );
                        }
                        catch { }
                    }
                }
            }

            //
            //  Limiting
            //

            if (SettingsForm.DisplayFillLimiting)
            {
                foreach (LandPolygon poligon in this.Parcel.Limiting)
                {

                    borderLand = ServiceSimpleElements.CreatePolyline2d(poligon.Points, true);
                    borderLand.LayerId = ServiceCAD.CreateLayer(settingsDrawing.Plan.FillLimiting.Layer);
                    idLand = ServiceCAD.InsertObject(borderLand);
                    hatchLand = ServiceSimpleElements.CreateHatch(idLand, false);

                    HatchPolygon hatchLimiting = HatchPolygon.GetHatchLimiting(poligon);

                    if (hatchLimiting != null)
                    {
                        hatchLand.LayerId = ServiceCAD.CreateLayer(settingsDrawing.Plan.FillLimiting.Layer);

                        hatchLand.ColorIndex = hatchLimiting.ColorIndex;
                        hatchLand.SetHatchPattern(hatchLimiting.Pattern.Type, hatchLimiting.Pattern.Name);
                        hatchLand.PatternAngle = hatchLimiting.Pattern.Angle;
                        hatchLand.PatternSpace = hatchLimiting.Pattern.Space;

                        idHatchLand = ServiceCAD.InsertObject(hatchLand);
                    }
                    else 
                    {
                        string type = poligon.FindInfo("OK").Value;
                        string name = poligon.FindInfo("OX").Value;
                        try
                        {
                            listMissingHatch.Add((type + name), new HatchPolygon(type, name, 0, PatternHatch.DEFAULT));
                            CurrentCAD.Editor.WriteMessage
                                ("\n<!> Незнайдено OK = {0}, OX = {1};",
                                    poligon.FindInfo("OK").Value,
                                    poligon.FindInfo("OX").Value
                                );
                        }
                        catch { }
                    }
                }
            }

            if (listMissingHatch.Count > 0)
            {
                List<HatchPolygon> listForXml = new List<HatchPolygon>();
                foreach (KeyValuePair<string,HatchPolygon> hp in listMissingHatch)
                { 
                    listForXml.Add(hp.Value);
                }
                listMissingHatch.Clear();
                CurrentCAD.Editor.WriteMessage("\n\n<!> Побудова плана ділянки\n<!> Не визначено штриховку: \n");
                CurrentCAD.Editor.WriteMessage(ServiceXml.GetStringXml<List<HatchPolygon>>(listForXml));
                CurrentCAD.Editor.WriteMessage("\n<!>\n");
            }
        }

        private AcDb.ObjectId AddBorders()
        {
            AcDb.Polyline2d borderParcel = ServiceSimpleElements.CreatePolyline2d(this.Parcel.Points, true);
            borderParcel.LayerId = ServiceCAD.CreateLayer(settingsDrawing.Plan.FillParcel.Layer);
            AcDb.ObjectId idBorderParcel = ServiceCAD.InsertObject(borderParcel);

            AcDb.ResultBuffer xData = new AcDb.ResultBuffer();
            int dxfCode;
            AcDb.TypedValue typedValue;

            dxfCode = (int)AcDb.DxfCode.ExtendedDataRegAppName;
            typedValue = new AcDb.TypedValue(dxfCode, "Земельна ділянка");
            xData.Add(typedValue);

            foreach (LandInfo infoValue in this.Parcel.Info)
            {
                dxfCode = (int)AcDb.DxfCode.ExtendedDataAsciiString;
                typedValue = new AcDb.TypedValue(dxfCode, "<" + infoValue.Key + "> " + infoValue.Value);
                xData.Add(typedValue);
            }

            ServiceCAD.SetXData(idBorderParcel, xData);

            return idBorderParcel;
            
        }

        private void AddPoints()
        {
            AddPoints(AcDb.ObjectId.Null);
        }

        private void AddPoints(AcDb.ObjectId idBorderParcel)
        {
            AcDb.ResultBuffer xData = new AcDb.ResultBuffer();
            int dxfCode;
            AcDb.TypedValue typedValue;

            dxfCode = (int)AcDb.DxfCode.ExtendedDataRegAppName;
            typedValue = new AcDb.TypedValue(dxfCode, "Точки межі");
            xData.Add(typedValue);

            int iCurNumberPoint = 0;

            Dictionary<string, string> tags = new Dictionary<string, string>();

            foreach (AcGe.Point2d point in this.Parcel.Points)
            {
                iCurNumberPoint += 1;
                tags.Clear();
                tags.Add("NUMBER", "");
                AcDb.ObjectId idPoint = ServiceBlockElements.InsertBlock
                    (
                        settingsDrawing.Plan.Point.NameBlock,
                        new AcGe.Point3d(point.X, point.Y, 0.0),
                        this.SettingsForm.ScaleDrawing,
                        0,
                        ServiceCAD.CreateLayer(settingsDrawing.Plan.Point.Layer),
                        tags
                    );

                //dxfCode = (int)DxfCode.ExtendedDataHandle;
                dxfCode = (int)AcDb.DxfCode.ExtendedDataAsciiString;
                typedValue = new AcDb.TypedValue(dxfCode, point.ToString());
                xData.Add(typedValue);
            }
            if (!idBorderParcel.Equals(AcDb.ObjectId.Null))
            {
                ServiceCAD.SetXData(idBorderParcel, xData);
            }
        }

        private void AddNumdersPoints()
        {
            int iCurNumberPoint = 0;

            //AcDb.DBText oText;
            AcDb.MText oMText;

            AcDb.Circle circleCurPoint = null;
            AcDb.ObjectId idCircleCurPoint;

            foreach (AcGe.Point2d point in this.Parcel.Points)
            {
                iCurNumberPoint += 1;

                circleCurPoint = new AcDb.Circle(
                            new AcGe.Point3d(point.X, point.Y, 0), 
                            new AcGe.Vector3d(0, 0, 1), 
                            1.75 * this.SettingsForm.ScaleDrawing);

                circleCurPoint.ColorIndex = 222;
                circleCurPoint.LineWeight = AcDb.LineWeight.LineWeight030;
                idCircleCurPoint = ServiceCAD.InsertObject(circleCurPoint);

                /*
                oText = new AcDb.DBText();
                oText.TextString = Convert.ToString(iCurNumberPoint);
                oText.Height = settingsDrawing.Plan.NumberPoint.TextHeight * this.SettingsForm.ScaleDrawing;
                //oText.Layer = settingsDrawing.Plan.NumberPoint.Layer;
                */

                oMText = new AcDb.MText();
                oMText.TextHeight = 2 * this.SettingsForm.ScaleDrawing;
                oMText.Attachment = AcDb.AttachmentPoint.MiddleCenter;
                //oMText.Layer = settingsDrawing.Plan.LengthLine.Layer;

                oMText.Contents = Convert.ToString(iCurNumberPoint);

                ServiceCAD.ZoomCenter(new AcGe.Point3d(point.X, point.Y, 0), 1);
                ServiceSimpleElements.ManualInsertMText(oMText);
                ServiceCAD.DeleteObject(idCircleCurPoint);
            }
        }

        private void AddLengthLine()
        {
            //AcDb.DBText oText;
            AcDb.MText oMText;

            AcDb.Line lineCur = null;
            AcDb.ObjectId idLineCur;

            AcGe.Point2d startPoint = this.Parcel.Points.ToArray()[this.Parcel.Points.Count-1];
            AcGe.Point3d midPoint;

            foreach (AcGe.Point2d endPoint in this.Parcel.Points)
            {
                midPoint = new AcGe.Point3d( (endPoint.X + startPoint.X)/2 , (endPoint.Y + startPoint.Y)/2, 0 );



                lineCur = new AcDb.Line( new AcGe.Point3d( startPoint.X, startPoint.Y, 0), 
                                    new AcGe.Point3d( endPoint.X, endPoint.Y, 0));

                lineCur.ColorIndex = 222;
                lineCur.LineWeight = AcDb.LineWeight.LineWeight030;
                idLineCur = ServiceCAD.InsertObject(lineCur);

                ServiceCAD.ZoomCenter(midPoint, 1);

                /*
                oText = new AcDb.DBText();
                oText.Height = 2 * this.SettingsForm.ScaleDrawing;
                oText.TextString = startPoint.GetDistanceTo(endPoint).ToString("0.00").Replace(",",".");
                //oText.Layer = settingsDrawing.Plan.LengthLine.Layer;

                ServiceCAD.ManualInsertText(oText);
                */

                oMText = new AcDb.MText();
                oMText.TextHeight = 2 * this.SettingsForm.ScaleDrawing;
                oMText.Attachment = AcDb.AttachmentPoint.MiddleCenter;
                //oMText.Layer = settingsDrawing.Plan.LengthLine.Layer;

                oMText.Contents = startPoint.GetDistanceTo(endPoint).ToString("0.00").Replace(",", ".");

                ServiceCAD.ZoomCenter(midPoint, 1);
                ServiceSimpleElements.ManualInsertMText(oMText);
                ServiceCAD.DeleteObject(idLineCur);

                startPoint = endPoint;
            }
        }

        private void AddAreaAndPerimetr()
        {
            AcDb.Polyline2d borderParcel = ServiceSimpleElements.CreatePolyline2d(this.Parcel.Points, true);

            AcDb.MText oMText = new AcDb.MText();
            oMText.TextHeight = 2.5 * this.SettingsForm.ScaleDrawing;
            oMText.Attachment = AcDb.AttachmentPoint.MiddleCenter;
 
            if (SettingsForm.DisplayArea && SettingsForm.UnitArea)
            {
                oMText.Contents = borderParcel.Area.ToString("Площа ділянки: S=0.00") + " кв.м";
            }
            else if (SettingsForm.DisplayArea && !SettingsForm.UnitArea)
            {
                oMText.Contents = (borderParcel.Area / 10000).ToString("Всього: 0.0000") + " га";
            }

            if (SettingsForm.DisplayPerimeter)
            {
                oMText.Contents = oMText.Contents + "\n" +
                                    borderParcel.Length.ToString("Периметр: 0.00") + " м";
            }

            if ( SettingsForm.DisplayArea || SettingsForm.UnitArea)
            {
                oMText.Contents = oMText.Contents.Replace(',', '.');
            }

            ServiceSimpleElements.ManualInsertMText(oMText);
        }

        private void AddTextNeighbors()
        {
            AcDb.MText oMText = new AcDb.MText();
            oMText.TextHeight = 2.5 * this.SettingsForm.ScaleDrawing;
            oMText.Width = 300;
            oMText.Height = 100;
            oMText.Attachment = AcDb.AttachmentPoint.MiddleLeft;
            //oMText.Layer = settingsDrawing.Plan.Neighbors.Layer;
            oMText.Contents = "     Опис меж:"   ;
            foreach ( TextNeighbors textNeighbors in this.allTextNeighbors)
            {
                oMText.Contents += "\r\n";
                oMText.Contents += " - - - - - - - - - - - - - - ";
                oMText.Contents += "\r\n";
                foreach (string value in textNeighbors.ToListValue())
                {
                    oMText.Contents += "\r\n" + value;
                }

            }

            ServiceSimpleElements.ManualInsertMText(oMText);
        }

        private void AddNeighbors()
        {

            this.allTextNeighbors.Clear();

            Dictionary<string, string> tags = new Dictionary<string, string>();

                this.currentLetterNeighbor = -1;
                TextNeighbors textNeighbors;

            foreach (NeighborsAlongContour contour in this.Parcel.ContoursNeighbors)
            {
                textNeighbors = new TextNeighbors(this.currentLetterNeighbor);

                foreach (LandPolygon neighbor in contour.Neighbors)
                {

                    textNeighbors.addText(neighbor.FindInfo("NM").Value.Replace("|", " | "));

                    if (SettingsForm.DisplayFillNeighbors)
                    {

                        AcGe.Point2d[] points = neighbor.Points.ToArray();

                        AcGe.Point2d bPnt;
                        AcGe.Point2d mPnt;
                        AcGe.Point2d fPnt;

                        AcGe.Point3d directionPoint;

                        if (contour.Neighbors.Count > 1)
                        {
                            bPnt = points[0];
                            mPnt = points[1];
                            fPnt = points[2];

                            directionPoint = new AcGe.Point3d(bPnt.X, bPnt.Y, 0.0);
                        }
                        else
                        {
                            bPnt = points[points.Length-1];
                            mPnt = points[0];
                            fPnt = points[1];

                            double angLeft = ServiceGeodesy.GetLeftAngle(fPnt, mPnt, bPnt);
                            double dirFront = ServiceGeodesy.GetDirAngle(mPnt, fPnt);

                            double dirBisector = dirFront + angLeft/2;
                            if (dirBisector >= Math.PI * 2)
                            {
                                dirBisector -= Math.PI * 2;
                            }

                            directionPoint =new AcGe.Point3d
                                                    (
                                                        mPnt.X + Math.Sin(dirBisector),
                                                        mPnt.Y + Math.Cos(dirBisector),
                                                        0
                                                    );
                        }

                        AcGe.Point3d insertPoint = new AcGe.Point3d(mPnt.X, mPnt.Y, 0.0);

                        AcDb.Line lineNeighbor = new AcDb.Line(insertPoint, directionPoint);

                        tags.Clear();

                        tags.Add("NEIGHBOR", textNeighbors.GetLastText().Key);
                        ServiceBlockElements.InsertBlock
                            (
                                settingsDrawing.Plan.Neighbors.NameBlock,
                                insertPoint,
                                this.SettingsForm.ScaleDrawing,
                                lineNeighbor.Angle,
                                tags
                            );

                    }
                    
                }
                this.currentLetterNeighbor = textNeighbors.Count - 1;
                this.allTextNeighbors.Add(textNeighbors);
                
            }
        }
    }
}
