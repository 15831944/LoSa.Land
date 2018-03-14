
using LoSa.Xml;
using LoSa.Land.Parcel;
using LoSa.Land.Tables;
using LoSa.CAD;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LoSa.Utility;
using LoSa.Land.ObjectGeo;


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


namespace LoSa.Land.Service
{
    public class ServiceTable
    {

        /// <summary>
        /// Замінює у заголовку таблиці кода на відповідні їм значення.
        /// </summary>
        /// <param name="polygon">Ділянка, що є вихідною для таблиці.</param>
        /// <param name="settingTable">Налаштування таблиці.</param>
        /// <returns>
        /// Повертає  <see cref="T:System.Sting"/> після заміни кодів на відповідні їм значення.
        /// </returns>

        internal static String ReplaceValueCodeInTitle(LandParcel polygon, SettingTable settingTable)
        {
            string titleTable = settingTable.Title;

            List<string> keys = settingTable.GetCodeAddTitle();

            foreach (string key in keys)
            {
                LandInfo info = polygon.FindInfo(key);

                if (info != null)
                {
                    titleTable = titleTable.Replace("/*" + info.Key + "*/", info.Value);
                }
                else
                {
                    titleTable = titleTable.Replace("/*" + key + "*/", "< Невірний код /*" + key + "*/>");
                }
            }
            return titleTable;
        }

        /// <summary>
        /// Створює коллекцію мультітекстових обектів заголовку таблиці та заголовків колонок таблиці.
        /// </summary>
        /// <param name="titleTable">Заголовок таблиці.</param>
        /// <param name="settingTable">Налаштування таблиці.</param>
        /// <returns>
        /// Повертає <see cref="T:AcDb.DBObjectCollection"/>, що містить мультитекстові обекти заголовку таблиці та заголовків колонок таблиці
        /// </returns>

        internal static AcDb.DBObjectCollection GetCapTables(string titleTable, SettingTable settingTable)
        {
            AcDb.DBObjectCollection objects = new AcDb.DBObjectCollection();

            AcDb.MText textValue;
            AcGe.Point3d insertPoint = AcGe.Point3d.Origin;

            /*Заголовок таблиці*/
            textValue = new AcDb.MText();
            textValue.TextHeight = settingTable.TextHeight;
            textValue.LineSpaceDistance = settingTable.TextHeight * 0.7;
            textValue.Attachment = AcDb.AttachmentPoint.BottomCenter;
            textValue.Contents = titleTable;
            textValue.Location = settingTable.BasePointDrawing
                .Add(new AcGe.Vector3d(settingTable.GetWidthTable() / 2, settingTable.TextHeight, 0));
            objects.Add(textValue);

            /*Заголовоки колонок таблиці*/
            double colWidth = 0;
            foreach (ColumnTable value in settingTable.Columns)
            {
                colWidth += value.Width;
                insertPoint = new AcGe.Point3d(colWidth - value.Width / 2, settingTable.TextHeight / 2 * -1, 0);

                textValue = new AcDb.MText();
                textValue.TextHeight = settingTable.TextHeight;
                textValue.LineSpaceDistance = settingTable.TextHeight * 2;
                textValue.Attachment = AcDb.AttachmentPoint.TopCenter;
                textValue.Contents = value.Name;
                textValue.Location = settingTable.BasePointDrawing.Add(insertPoint.GetAsVector());

                objects.Add(textValue);
            }
            return objects;
        }

        /// <summary>
        /// Створює коллекцію лінійних обектів таблиці.
        /// </summary>
        /// <param name="numberRows">Кількість рядків таблиці.</param>
        /// <param name="stepRows">Крок рядків таблиці.</param>
        /// <param name="settingTable">Налаштування таблиці.</param>
        /// <returns>
        /// Повертає <see cref="T:AcDb.DBObjectCollection"/>, що містить лінійні обекти  таблиці.
        /// </returns>
        
        internal static AcDb.DBObjectCollection GetBoundTable(int numberRows, double stepRows, SettingTable settingTable)
        {
            AcDb.DBObjectCollection objects = new AcDb.DBObjectCollection();

            double hTable = settingTable.GetHeightTable(numberRows, stepRows) * -1;
            double wTable = settingTable.GetWidthTable();

            AcGe.Point2dCollection pointsBoundTable =
                new AcGe.Point2dCollection(new AcGe.Point2d[] 
                { 
                    settingTable.BasePointDrawing.Convert2d( new AcGe.Plane() )
                    .Add( new AcGe.Vector2d(0,0)),

                    settingTable.BasePointDrawing.Convert2d( new AcGe.Plane() )
                    .Add( new AcGe.Vector2d(wTable,0)),

                    settingTable.BasePointDrawing.Convert2d( new AcGe.Plane() )
                    .Add( new AcGe.Vector2d(wTable,hTable)),

                    settingTable.BasePointDrawing.Convert2d( new AcGe.Plane() )
                    .Add( new AcGe.Vector2d(0,hTable))
                });

            objects.Add(ServiceSimpleElements.CreatePolyline2d(pointsBoundTable, true));

            AcGe.Point2dCollection pointsLine =
                new AcGe.Point2dCollection(new AcGe.Point2d[] 
                { 
                    settingTable.BasePointDrawing.Convert2d( new AcGe.Plane() )
                    .Add( new AcGe.Vector2d(wTable,settingTable.GetHeightCapTable() * -1)),

                    settingTable.BasePointDrawing.Convert2d( new AcGe.Plane() )
                    .Add( new AcGe.Vector2d(0,settingTable.GetHeightCapTable() * -1))
                });

            objects.Add(ServiceSimpleElements.CreatePolyline2d(pointsLine, false));

            double widthCur = 0;

            for (int i = 0; i < settingTable.Columns.Count - 1; i++)
            {
                ColumnTable value = settingTable.Columns.ToArray()[i];
                widthCur += value.Width;
                pointsLine =
                    new AcGe.Point2dCollection(new AcGe.Point2d[] 
                    { 
                        settingTable.BasePointDrawing.Convert2d( new AcGe.Plane() )
                        .Add( new AcGe.Vector2d(widthCur,0)),

                        settingTable.BasePointDrawing.Convert2d( new AcGe.Plane() )
                        .Add( new AcGe.Vector2d(widthCur,hTable))
                    });
                objects.Add(ServiceSimpleElements.CreatePolyline2d(pointsLine, false));
            }

            return objects;
        }

        /// <summary>
        /// Створює коллекцію текстових обектів значень данних таблиці межі земельної ділянки.
        /// </summary>
        /// <param name="polygon">Ділянка, що є вихідною для таблиці.</param>
        /// <param name="settingTable">Налаштування таблиці.</param>
        /// <returns>
        ///  Повертає <see cref="T:AcDb.DBObjectCollection"/>, що містить текстові значення данний таблиці межі земельної ділянки.
        /// </returns>

        internal static AcDb.DBObjectCollection GetDataTableBorderPolygon(LandParcel polygon, SettingTable settingTable)
        {
            AcDb.DBObjectCollection objects = new AcDb.DBObjectCollection();

            AcDb.DBText textValue;
            AcGe.Point3d insertPoint;

            AcGe.Point2d backPoint, currentPoint, frontPoint;

            double heightTable = (settingTable.GetHeightCapTable() + settingTable.TextHeight) * -1;

            for (int index = 0; index <= polygon.Points.Count; index++)//(Point2d point in polygon.Points)
            {
                double colWidth = 0;
                heightTable += settingTable.TextHeight * 2 * index;

                if (index == 0)
                {
                    backPoint = polygon.Points.ToArray()[polygon.Points.Count - 1];
                    currentPoint = polygon.Points.ToArray()[index];
                    frontPoint = polygon.Points.ToArray()[index + 1];
                }
                else if (index == polygon.Points.Count - 1)
                {
                    backPoint = polygon.Points.ToArray()[index - 1];
                    currentPoint = polygon.Points.ToArray()[index];
                    frontPoint = polygon.Points.ToArray()[0];
                }
                else if (index == polygon.Points.Count)
                {
                    backPoint = polygon.Points.ToArray()[index - 1];
                    currentPoint = polygon.Points.ToArray()[0];
                    frontPoint = polygon.Points.ToArray()[1];
                }
                else
                {
                    backPoint = polygon.Points.ToArray()[index - 1];
                    currentPoint = polygon.Points.ToArray()[index];
                    frontPoint = polygon.Points.ToArray()[index + 1];
                }

                foreach (ColumnTable col in settingTable.Columns)
                {
                    colWidth += col.Width;

                    insertPoint = new AcGe.Point3d();
                    insertPoint = new AcGe.Point3d(colWidth - col.Width / 2, (settingTable.GetHeightCapTable() + (index + 1) * settingTable.TextHeight * 2) * -1, 0);

                    textValue = new AcDb.DBText();

                    textValue.Height = settingTable.TextHeight;
                    textValue.Justify = AcDb.AttachmentPoint.BottomCenter;
                    textValue.Position = settingTable.BasePointDrawing.Add(insertPoint.GetAsVector());
                    textValue.AlignmentPoint = textValue.Position;

                    if (col.Format.IndexOf("Number") > -1)
                    {
                        if (index == polygon.Points.Count)
                        {
                            textValue.TextString = "1";
                        }
                        else
                        {
                            textValue.TextString = Math.Abs(index + 1).ToString();
                        }
                    }
                    else if (col.Format.IndexOf("X") > -1)
                    {
                        textValue.TextString = currentPoint.X.ToString("0.00");
                    }
                    else if (col.Format.IndexOf("Y") > -1)
                    {
                        textValue.TextString = currentPoint.Y.ToString("0.00");
                    }
                    else if (col.Format.IndexOf("LengthLine") > -1)
                    {
                        if (index < polygon.Points.Count)
                        {
                            textValue.Position = textValue.Position.Add(new AcGe.Vector3d(0, settingTable.TextHeight * -1, 0));
                            textValue.AlignmentPoint = textValue.Position;
                            textValue.TextString = currentPoint.GetDistanceTo(frontPoint).ToString("0.00");
                        }
                    }
                    else if (col.Format.IndexOf("DirAngle") > -1)
                    {
                        if (index < polygon.Points.Count)
                        {
                            textValue.Position = textValue.Position.Add(new AcGe.Vector3d(0, settingTable.TextHeight * -1, 0));
                            textValue.AlignmentPoint = textValue.Position;
                            double angle = ServiceGeodesy.GetDirAngle(currentPoint, frontPoint);
                            textValue.TextString = AcRx.Converter.AngleToString(angle, AcRx.AngularUnitFormat.DegreesMinutesSeconds, 3);
                        }
                    }
                    else if (col.Format.IndexOf("InnerAngle") > -1)
                    {
                        double angle = ServiceGeodesy.GetRightAngle(backPoint, currentPoint, frontPoint);
                        textValue.TextString = AcRx.Converter.AngleToString(angle, AcRx.AngularUnitFormat.DegreesMinutesSeconds, 3);
                    }
                    else
                    {
                        textValue.TextString = "None";
                    }

                    textValue.TextString = textValue.TextString.Replace(",", ".");

                    for (int i = 0; i < 10; i++)
                    {
                        textValue.TextString = textValue.TextString
                            .Replace("°" + i.ToString() + "'", "°" + i.ToString("00") + "'");
                    }
                    for (int i = 0; i < 10; i++)
                    {
                        textValue.TextString = textValue.TextString
                            .Replace("'" + i.ToString() + "\"", "'" + i.ToString("00") + "\"");
                    }

                    objects.Add(textValue);
                }
            }

            return objects;
        }


        /// <summary>
        /// Створює коллекцію текстових обектів значень данних таблиці виносу внатуру меж земельної ділянки.
        /// </summary>
        /// <param name="polygon">Земельна ділянка.</param>
        /// <param name="settingTable">Налаштування таблиці.</param>
        /// <returns>
        ///  Повертає <see cref="T:AcDb.DBObjectCollection"/>, що містить текстові значення данний таблиці виносу внатуру меж земельної ділянки.
        /// </returns>
        internal static AcDb.DBObjectCollection GetDataTableStakeOutParcelPoints(LandParcel polygon, SettingTable settingTable)
        {
            List<StakeOutParcelPoint> stakeoutPoints = polygon.StakeOutParcelPoints;
            //stakeoutPoints.Sort((x, y) => y.PointStation.Name.CompareTo(x.PointStation.Name));

            AcDb.DBObjectCollection objects = new AcDb.DBObjectCollection();

            AcDb.DBText textValue;
            AcGe.Point3d insertPoint;

            //AcGe.Point2d backPoint, stationPoint, parcelPoint;

            double heightTable = (settingTable.GetHeightCapTable() + settingTable.TextHeight) * -1;

            int index = -1;
            foreach (StakeOutParcelPoint stakeoutPoint in stakeoutPoints)
            {
                index++;
                double colWidth = 0;
                heightTable += settingTable.TextHeight * 2 * index;

                foreach (ColumnTable col in settingTable.Columns)
                {
                    colWidth += col.Width;

                    insertPoint = new AcGe.Point3d();
                    insertPoint = new AcGe.Point3d
                                        ( 
                                            colWidth - col.Width / 2, 
                                            (settingTable.GetHeightCapTable() + (index + 1) * settingTable.TextHeight * 2) * -1, 
                                            0
                                        );

                    textValue = new AcDb.DBText();

                    textValue.Height = settingTable.TextHeight;
                    textValue.Justify = AcDb.AttachmentPoint.BottomCenter;
                    textValue.Position = settingTable.BasePointDrawing.Add(insertPoint.GetAsVector());
                    textValue.AlignmentPoint = textValue.Position;

                    if (col.Format.IndexOf("Number") > -1)
                    {
                        textValue.TextString = stakeoutPoint.Name;
                    }
                    else if (col.Format.IndexOf("X") > -1)
                    {
                        textValue.TextString = stakeoutPoint.Coordinates.X.ToString("0.00");
                    }
                    else if (col.Format.IndexOf("Y") > -1)
                    {
                        textValue.TextString = stakeoutPoint.Coordinates.Y.ToString("0.00");
                    }
                    else if (col.Format.IndexOf("LengthLine") > -1)
                    {
                        textValue.TextString = stakeoutPoint.Distance.ToString("0.00");
                    }
                    else if (col.Format.IndexOf("DirAngle") > -1)
                    {
                        textValue.TextString = stakeoutPoint.DirAngleToString(AcRx.AngularUnitFormat.DegreesMinutesSeconds);
                    }
                    else if (col.Format.IndexOf("InnerAngle") > -1)
                    {
                        textValue.TextString = stakeoutPoint.LeftlAngleToString(AcRx.AngularUnitFormat.DegreesMinutesSeconds);
                    }
                    else if (col.Format.IndexOf("BasePoints_dirAngle") > -1)
                    {
                        textValue.TextString = stakeoutPoint.PointStation.Name;
                    }
                    else if (col.Format.IndexOf("BasePoints_innerAngle") > -1)
                    {
                        textValue.TextString = stakeoutPoint.PointStation.Name 
                                                + " -> " 
                                                + stakeoutPoint.PointOrientation.Name;
                    }
                    else
                    {
                        textValue.TextString = "None";
                    }

                    textValue.TextString = textValue.TextString.Replace(",", ".");
                    textValue.TextString = FormatAngleValue(textValue.TextString);

                    objects.Add(textValue);
                }
            }

            return objects;
        }

        public static String FormatAngleValue(String textAngleValue)
        {
            for (int i = 0; i < 10; i++)
            {
                textAngleValue = textAngleValue
                    .Replace("°" + i.ToString() + "'", "°" + i.ToString("00") + "'");
            }
            for (int i = 0; i < 10; i++)
            {
                textAngleValue = textAngleValue
                    .Replace("'" + i.ToString() + "\"", "'" + i.ToString("00") + "\"");
            }
            return textAngleValue;
        }

        /// <summary>
        /// Створює коллекцію текстових обектів значень данних таблиці обмежень земельної ділянки.
        /// </summary>
        /// <param name="parcel">Ділянка, що є вихідною для таблиці.</param>
        /// <param name="settingTable">Налаштування таблиці.</param>
        /// <returns>
        ///  Повертає <see cref="T:AcDb.DBObjectCollection"/>, що містить текстові значення данний таблиці обмежень земельної ділянки.
        /// </returns>

        internal static AcDb.DBObjectCollection GetDataTableLimiting(LandParcel parcel, SettingTable settingTable)
        {
            AcDb.DBObjectCollection objects = new AcDb.DBObjectCollection();

            AcDb.MText valueMText;
            AcGe.Point3d insertPoint;
            AcDb.Line lineRows;

            LandPolygon polygonLimiting;

            double steepRow = settingTable.TextHeight * 6;
            double heightTable = settingTable.GetHeightCapTable()  * -1;

            List<HatchPolygon> listMissingHatch = new List<HatchPolygon>();

            for (int index = 0; index < parcel.Limiting.Count; index++)
            {

                polygonLimiting = parcel.Limiting.ToArray()[index];

                double colWidth = 0;

                if (index > 0)
                {
                    lineRows = new AcDb.Line(
                      new AcGe.Point3d(0, heightTable, 0),
                      new AcGe.Point3d(settingTable.GetWidthTable(), heightTable, 0));

                    objects.Add(lineRows);
                }

                heightTable -= steepRow;

                

                foreach (ColumnTable col in settingTable.Columns)
                {
                    colWidth += col.Width;

                    insertPoint = new AcGe.Point3d();
                    insertPoint = new AcGe.Point3d(colWidth - col.Width / 2, (heightTable + steepRow / 2 ), 0);

                    valueMText = new AcDb.MText();
                    valueMText.Width = col.Width * 0.9;
                    valueMText.TextHeight = settingTable.TextHeight;
                    valueMText.LineSpaceDistance = settingTable.TextHeight * 1.5;
                    valueMText.Attachment = AcDb.AttachmentPoint.MiddleCenter;
                    valueMText.Location = insertPoint;

                    if (col.Format.IndexOf("LegendLimiting") > -1)
                    {
                        AcGe.Point2dCollection pointsHatch = new AcGe.Point2dCollection( new AcGe.Point2d[] 
                        { 
                            new AcGe.Point2d(insertPoint.X - col.Width / 2 + 2, heightTable + steepRow - 2),
                            new AcGe.Point2d(insertPoint.X - col.Width / 2 + 2, heightTable + 2),
                            new AcGe.Point2d(insertPoint.X - col.Width / 2 + col.Width - 2, heightTable + 2),
                            new AcGe.Point2d(insertPoint.X - col.Width / 2 + col.Width - 2, heightTable + steepRow - 2)
                        } );

                        AcDb.Polyline2d polylineLimiting = ServiceSimpleElements.CreatePolyline2d(pointsHatch, true);
                        AcDb.Hatch hatch =
                            ServiceSimpleElements.CreateHatch(ServiceCAD.InsertObject(polylineLimiting), true);

                        HatchPolygon hatchLimiting = HatchPolygon.GetHatchLimiting(polygonLimiting);

                        if (hatchLimiting != null)
                        {
                            hatch.ColorIndex = hatchLimiting.ColorIndex;
                            hatch.SetHatchPattern(AcDb.HatchPatternType.UserDefined, hatchLimiting.Pattern.Name);
                            hatch.PatternAngle = hatchLimiting.Pattern.Angle;
                            hatch.PatternSpace = hatchLimiting.Pattern.Space;
                        }
                        else 
                        {
                            string type = polygonLimiting.FindInfo("OK").Value;
                            string name = polygonLimiting.FindInfo("OX").Value;
                            listMissingHatch.Add(new HatchPolygon(type, name, 0, PatternHatch.DEFAULT));
                        }

                        objects.Add(hatch);
                        polylineLimiting = ServiceSimpleElements.CreatePolyline2d(pointsHatch, true);
                        objects.Add(polylineLimiting);
                    }
                    else if (col.Format.IndexOf("CodeLimiting") > -1)
                    {
                        valueMText.Contents = polygonLimiting.FindInfo("OK").Value;
                        objects.Add(valueMText);
                    }
                    else if (col.Format.IndexOf("NameLimiting") > -1)
                    {
                        valueMText.Contents = polygonLimiting.FindInfo("OX").Value;
                        objects.Add(valueMText);
                    }
                    else if (col.Format.IndexOf("LegalActsLimiting") > -1)
                    {
                        valueMText.Contents = polygonLimiting.FindInfo("OD").Value;
                        objects.Add(valueMText);
                    }
                    else if (col.Format.IndexOf("AreaLimiting") > -1)
                    {
                        double area = Convert.ToDouble(polygonLimiting.FindInfo("AO").Value.Replace(".",","));
                        valueMText.Contents = (area / 10000).ToString("0.0000").Replace(",","."); ;
                        objects.Add(valueMText);
                    }
                }
            }

            if (listMissingHatch.Count > 0)
            {
                CurrentCAD.Editor.WriteMessage("\n\nПобудова таблиці омеженнь\n Не визначено штриховку: \n");
                CurrentCAD.Editor.WriteMessage(ServiceXml.GetStringXml<List<HatchPolygon>>(listMissingHatch));
            }

            return objects;
        }

        /// <summary>
        /// Об'єднуе площі однотипних обмежень.
        /// </summary>
        /// <param name="parcel">Ділянка з обмеженнями.</param>
        /// <returns>
        /// Повертає <see cref="T:LoSa.Land.Parcel.LandParcel"/>, що містить обеднані площі однотипних обмежень.
        /// </returns>

        internal static LandParcel СombinedLimiting(LandParcel parcel)
        {
            LandParcel newParcel = new LandParcel();

            bool isNewLimiting = true;

            foreach (LandPolygon limiting in parcel.Limiting)
            {
                if (newParcel.Limiting.Count == 0)
                {
                    newParcel.Limiting.Add(limiting);
                }
                else
                {
                    isNewLimiting = true;
                    foreach (LandPolygon newLimiting in newParcel.Limiting)
                    {
                        if (limiting.FindInfo("OK").Value.Equals(newLimiting.FindInfo("OK").Value) &&
                             limiting.FindInfo("OX").Value.Equals(newLimiting.FindInfo("OX").Value))
                        {
                            double oldArea =
                                Convert.ToDouble(
                                   newLimiting.FindInfo("AO").Value.Replace(".", ",")
                                );
                            double newArea =
                                Convert.ToDouble(
                                    limiting.FindInfo("AO").Value.Replace(".", ",")
                                );

                            newLimiting.FindInfo("AO").Value = (oldArea + newArea).ToString("").Replace(",", ".");
                            isNewLimiting = false;
                        }  
                    }

                    if (isNewLimiting == true) 
                    { 
                        newParcel.Limiting.Add(limiting); 
                    }
                }
            }

            return newParcel;
        }

        /// <summary>
        /// Об'єднуе площі однотипних угідь.
        /// </summary>
        /// <param name="parcel">Ділянка з обмеженнями.</param>
        /// <returns>
        /// Повертає <see cref="T:LoSa.Land.Parcel.LandParcel"/>, що містить обеднані площі однотипних угідь.
        /// </returns>

        internal static LandParcel СombinedLand(LandParcel parcel)
        {
            LandParcel newParcel = new LandParcel();

            bool isNewLand = true;

            foreach (LandPolygon land in parcel.Lands)
            {
                if (newParcel.Lands.Count == 0)
                {
                    newParcel.Lands.Add(land);
                }
                else
                {
                    isNewLand = true;
                    foreach (LandPolygon newLand in newParcel.Lands)
                    {
                        if (land.FindInfo("СZG").Value.Equals(newLand.FindInfo("СZG").Value))
                        {
                            double oldArea =
                                Convert.ToDouble(
                                   newLand.FindInfo("AL").Value.Replace(".", ",")
                                );
                            double newArea =
                                Convert.ToDouble(
                                    land.FindInfo("AL").Value.Replace(".", ",")
                                );

                            newLand.FindInfo("AL").Value = (oldArea + newArea).ToString("").Replace(",", ".");
                            isNewLand = false;
                        }
                    }

                    if (isNewLand == true)
                    {
                        newParcel.Limiting.Add(land);
                    }
                }
            }
            return newParcel;
        }

    } // end class ServiceTable
}
