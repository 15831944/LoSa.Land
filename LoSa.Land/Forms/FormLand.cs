
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


using LoSa.Land.Tables;
using LoSa.Land.ObjectIn4;
using LoSa.Land.ObjectExport;
using LoSa.CAD;

using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using LoSa.Land.EnumAttributes;
using LoSa.Land.Parcel;

using LoSa.Land.ObjectGeo;
using System.Text;
using LoSa.Xml;
using LoSa.Utility;
using System.Data;

namespace LoSa.Land.Forms
{
    /// <summary>
    /// Forms for LoSa.Land
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    public partial class FormLand: Form
    {

        private List<BasePoint> basePoints = null;
        private LocalPath localPath = new LocalPath("LoSa_Land");
        private FormAboutBox formAboutBox = new FormAboutBox();

        #region Filde

        /*      
         *      Filde for Settings  
         */
         
        /// <summary>
        /// The User settings
        /// </summary>
        private SettingsLand userSettings;          // Налаштування користувача
        
        /// <summary>
        /// The FormLand settings
        /// </summary>
        private SettingsFormLand formSettings;      // Налаштування FormLand
        
        /// <summary>
        /// The Tables settings
        /// </summary>
        private SettingsTable tableSettings;        // Налаштування таблиць
        
        /// <summary>
        /// The Frames Drawing settings
        /// </summary>
        private SettingsLand frameDrawingSettings;    // Налаштування рамок та штампів креслень
        
        /// <summary>
        /// The Drawing settings
        /// </summary>
        public SettingsDrawing drawingSettings;     // Налаштування креслень

        /*
         *      Filde for Plan
         */      

        private AcGe.Vector2d offsetBlockLandView = new AcGe.Vector2d(500, 500);
        private ServiceParcel serviceParcel = null;

        //
        //      variables for In4
        //
        private BL currentBlockLand = null;

        //
        //      variables for Parcel
        //
        private LandParcel currentParcel = new LandParcel();// null;
        //private List<StakeOutParcelPoint> stakeOutParcelPoints = new List<StakeOutParcelPoint>();

        //
        //      variables for ObjectId
        //
        private AcDb.ObjectId idCurrentHatchParcel;
        private List<AcDb.ObjectId> allIdBlockLandParcel = new List<AcDb.ObjectId>() ;
        private AcDb.ObjectIdCollection idNeighborsCurrenParcel = new AcDb.ObjectIdCollection();

        #endregion Filde

        #region Costructor

        /// <summary>
        /// Initializes a new instance of the <see cref="FormLand"/> class.
        /// </summary>
        public FormLand()
        {
            InitializeComponent();
            LoadAllSettings();
        }

        #endregion Costructor

        #region Events

        /// <summary>
        /// Handles the Load event of the FormLand control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void FormLand_Load(object sender, EventArgs e)
        {
            this.comboBoxScaleDrawing.Items.Add("1 : 100");
            this.comboBoxScaleDrawing.Items.Add("1 : 200");
            this.comboBoxScaleDrawing.Items.Add("1 : 500");
            this.comboBoxScaleDrawing.Items.Add("1 : 1000");
            this.comboBoxScaleDrawing.Items.Add("1 : 2000");
            this.comboBoxScaleDrawing.Items.Add("1 : 5000");

            this.comboBoxScaleDrawing.SelectedIndex = 3;

            //LoadAllSettings();
            LoadTableSettings();
            LoadFrameDrawingSettings();
            this.UpdateFormLandByFormSettings();
        }

        private void FormLand_FormClosing(object sender, FormClosingEventArgs e)
        {
            PurgeFromElementsBlockLand();
            SaveAllSettings();
        }

        #region Events Buttons

        private void btnSelectBasePoints_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog openDialogBasePoints = new OpenFileDialog();

            openDialogBasePoints.Filter = "Файл обміну (*.nxyhc)|*.nxyhc|Усі файли (*.*)|*.*";

            if (openDialogBasePoints.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    if ((myStream = openDialogBasePoints.OpenFile()) != null)
                    {
                        this.basePoints = new List<BasePoint>();
                        this.basePoints.Clear();
                        this.basePoints.AddRange(LoadBasePoints(openDialogBasePoints.FileName));

                        SetColPointStationAndOrientationItems();
                        AutoSearchingStationAndOrientationForAllPoints();

                        this.labelFileBasePoint.Text = openDialogBasePoints.FileName; 
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Помилка: " + ex.Message);
                }
            }
        }

        private void SetColPointStationAndOrientationItems()
        {
             if (this.basePoints == null) { return; }

            List<string> namesBasePoint = new List<string>();

            foreach (BasePoint basePoint in this.basePoints)
            {
                namesBasePoint.Add(basePoint.Name);
            }
            this.colPointStation.Items.Clear();
            this.colPointStation.Items.AddRange(namesBasePoint.ToArray());

            this.colPointOrientation.Items.Clear();
            this.colPointOrientation.Items.AddRange(namesBasePoint.ToArray());
        }

        private void AutoSearchingStationAndOrientationForAllPoints()
        {
            foreach (StakeOutParcelPoint stakeOutPoint in this.currentParcel.StakeOutParcelPoints)
            {
                AutoSearchingStationAndOrientationForPoint(stakeOutPoint);
            }
        }

        private void AutoSearchingStationAndOrientationForPoint(StakeOutParcelPoint stakeOutPoint)
        {
            int indexRow = this.currentParcel.StakeOutParcelPoints.FindIndex( x => x.Equals(stakeOutPoint) );

            if (stakeOutPoint.AutoSetStationAndOrientation(this.basePoints))
            {
                this.dataGridView_StakeOut[2, indexRow].Value = stakeOutPoint.PointStation.Name;
                this.dataGridView_StakeOut[3, indexRow].Value = stakeOutPoint.PointOrientation.Name;
            }

        }

        private List<BasePoint> LoadBasePoints(string fileNameBasePoints)
        {
            List<BasePoint> basePoints = new List<BasePoint>();


            StreamReader streamReader = new StreamReader(fileNameBasePoints, Encoding.GetEncoding(866));

            string strBuff;
            BasePoint bp;

            while ((strBuff = streamReader.ReadLine()) != null)
            {
                string[] buffs = strBuff.Split(new string[] { "," } ,StringSplitOptions.RemoveEmptyEntries);
                if (buffs.Length >= 5)
                {
                    String name = buffs[0];
                    double n = Double.Parse(buffs[1].Replace(".", ","));
                    double e = Double.Parse(buffs[2].Replace(".", ","));
                    double h = Double.Parse(buffs[3].Replace(".", ","));
                    String code = buffs[4];

                    basePoints.Add(new BasePoint(name, n, e, h, code));
                }
                
            }
            return basePoints;
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            Stream myStream = null;
            OpenFileDialog openDialogIn4 = new OpenFileDialog();

            openDialogIn4.InitialDirectory = new LocalPath("LoSa_Land").FindFullPathFromXml("PathExpotrSDR");
            openDialogIn4.Filter = "Файл обміну (*.in4)|*.in4|Усі файли (*.*)|*.*";

            if (openDialogIn4.ShowDialog() == DialogResult.OK)
            {

                try
                {
                    if ((myStream = openDialogIn4.OpenFile()) != null)
                    {
                        PurgeFromElementsBlockLand();
                    }
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show("Помилка: " + ex.Message);
                }

                this.comboBoxNumberParcel.Items.Clear();

                this.currentBlockLand = ServiceIn4.GetDataFromFileIN4(openDialogIn4.FileName);
                //int indexSR = 0;
                foreach (SR currentSR in this.currentBlockLand.SR)
                {
                    if (currentSR.SC != null)
                    {
                        this.comboBoxNumberParcel.Items.Add(currentSR.SC);
                    }
                    else
                    {
                        MessageBox.Show(" !!!!!! currentSR.SC == null"); return;
                    }
                    //indexSR++;
                }

                this.comboBoxNumberParcel.SelectedIndex = 0;
                allIdBlockLandParcel = InsertAllParcel(currentBlockLand, this.offsetBlockLandView);
                ServiceCAD.ZoomExtents();

            }
        }


        static List<AcDb.ObjectId> InsertAllParcel(BL blockLand, AcGe.Vector2d offset)
            {
                AcDb.Polyline2d curPolyline;
                AcGe.Point2dCollection points;
                List<AcDb.ObjectId> listObjectId = new List<AcDb.ObjectId>();

                foreach (SR sr in blockLand.SR)
                {
                    points = ServiceSimpleElements.Offset(ServiceIn4.GetPoints2dFromIn4Polygon(sr), offset);
                    curPolyline = ServiceSimpleElements.CreatePolyline2d(points, true);
                    listObjectId.Add(ServiceCAD.InsertObject(curPolyline));
                }

                return listObjectId;
            }

        public static AcDb.ObjectId CreateLayer(String layerName)
        {
            AcDb.ObjectId layerId;
            AcDb.Database db = CurrentCAD.Database;

            using (AcDb.Transaction tr = db.TransactionManager.StartTransaction())
            {
                AcDb.LayerTable layerTable = (AcDb.LayerTable)tr.GetObject(db.LayerTableId, AcDb.OpenMode.ForWrite);

                if (layerTable.Has(layerName))
                {
                    layerId = layerTable[layerName];
                }
                else
                {
                    AcDb.LayerTableRecord layerTableRecord = new AcDb.LayerTableRecord();
                    layerTableRecord.Name = layerName;
                    layerId = layerTable.Add(layerTableRecord);
                    tr.AddNewlyCreatedDBObject(layerTableRecord, true);
                }
                tr.Commit();
            }
            return layerId;
        }

        private void btnBuildingPlan_Click(object sender, EventArgs e)
                {
                    //UpdatSettingsOnForm();

                    if (this.serviceParcel != null)
                    {
                        this.drawingSettings.Scale.Value = this.formSettings.ScaleDrawing;
                        this.serviceParcel.BuildingPlan();
                    }
                    //ServiceCAD.ZoomAll();
                }

        private void btnExportToFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveDialogSDR = new SaveFileDialog();

            string directoryExport = localPath.FindFullPathFromXml("PathExpotr");

            Directory.CreateDirectory(directoryExport);

            saveDialogSDR.InitialDirectory = directoryExport;
            saveDialogSDR.FileName = this.currentParcel.FindInfo("SC").Value.Replace(":", "_");
            saveDialogSDR.Filter =  "Файл для Sokkia SDR2x (*.sdr)|*.sdr|"+             // 1   
                                    "Файл для Sokkia SDR33 (*.sdr)|*.sdr|" +            // 2
                                    "Файл текстовый формата NXYZC (*.nxyzc)|*.nxyzc|" + // 3
                                    "Файл текстовый формата NYXZC (*.nyxzc)|*.nyxzc|" +  // 4
                                    "Обменный формат Credo_Dat (*.top)|*.top";  // 5
            //saveDialogSDR.RestoreDirectory = true;

            if (saveDialogSDR.ShowDialog() == DialogResult.OK)
            {
                ExportFileFormat formatExpotr = ExportFileFormat.NXYZC;

                if (saveDialogSDR.FilterIndex == 1)
                {
                    formatExpotr = ExportFileFormat.SDR20;
                }
                else if (saveDialogSDR.FilterIndex == 2)
                {
                    formatExpotr = ExportFileFormat.SDR33;
                }
                else if (saveDialogSDR.FilterIndex == 3)
                {
                    formatExpotr = ExportFileFormat.NXYZC;
                }
                else if (saveDialogSDR.FilterIndex == 4)
                {
                    formatExpotr = ExportFileFormat.NYXZC;
                }
                else if (saveDialogSDR.FilterIndex == 5)
                {
                    formatExpotr = ExportFileFormat.CREDO_DAT_TOP;
                }
                    
            
                ServiceExport.ToFile(this.currentParcel, saveDialogSDR.FileName, formatExpotr);
            }
        }

        private void btnBuildingTable_Click(object sender, EventArgs e)
        {
            if (this.currentParcel == null)
            {
                return;
            }

            BlockTableLand table = new BlockTableLand(); 

            foreach (string keyTable in this.checkedListBox_TypeTable.CheckedItems)
            {

                SettingTable setTable =  this.tableSettings.FindKeyTable(keyTable);
                table.Setting = setTable;
                table.Scale = this.formSettings.ScaleDrawing;

                if (table.Setting == null)
                {
                    return;
                }

                if (table.StrategyTable.GetType().Equals(typeof(StrategyTableBorderParcel)))
                {
                    table.Parcel = this.currentParcel;
                    table.сreate();
                }
                else if (table.StrategyTable.GetType().Equals(typeof(StrategyTableBorderLimiting)))
                {
                    foreach (LandPolygon polygon in this.currentParcel.Limiting)
                    {
                        LandParcel parcel = new LandParcel();
                        parcel.Info = polygon.Info;
                        parcel.Points = polygon.Points;
                        table.Parcel = parcel;
                        table.сreate();
                    }
                }
                else if (table.StrategyTable.GetType().Equals(typeof(StrategyTableLimiting)))
                {
                    table.Parcel = this.currentParcel;
                    table.сreate();
                }
                else if (table.StrategyTable.GetType().Equals(typeof(StrategyTableStakeOutPoints)))
                {
                    table.Parcel = this.currentParcel;
                    table.сreate();
                }
            }
        }

        private void btnEditTypeTable_Click(object sender, EventArgs e)
        {
            //new FormEditTables().Show();
            LoadTableSettings();
            //string pathXml = SettingLocalPath.PathTableSettings + ".xml";
            //ServiceXML.WriteXML<SettingsTable>(this.tableSettings, pathXml);
        }

                private void btnBuildingBoxDrawing_Click(object sender, EventArgs e)
                {
                    if ( this.checkedListBoxTypeBoxDrawing.CheckedItems.Count < 1 ) { return; }
                    foreach (string nameBoxDrawing in this.checkedListBoxTypeBoxDrawing.CheckedItems)
                    {
                        SettingLand setBoxDrawing = this.frameDrawingSettings.FindName(nameBoxDrawing);

                        Dictionary<string,string> tags = new Dictionary<string,string>();

                        foreach( SettingLand tagValue in this.userSettings.Setting)
                        {
                            tags.Add(tagValue.Key, tagValue.Name);
                        }

                        tags.Add( "SC", this.currentParcel.FindInfo("SC").Value );

                /*
                ObjectId idBoxDrawing =
                    ServiceBricsCAD.ManualInsertBlock(
                        setBoxDrawing.Key,
                        this.settingsDrawing.Scale.Value,
                        tags);
                */

        AcDb.ObjectId idBoxDrawing =
                           ServiceBlockElements.InsertBlock(
                               setBoxDrawing.Key,
                               CurrentCAD.Editor.GetPoint(">>>>>>").Value,
                               this.drawingSettings.Scale.Value,
                               0,
                               AcDb.ObjectId.Null,
                               tags);
                        /*
                        if (tags != null)
                        {
                            ServiceBricsCAD.ReplaceAttributeBlock(idBoxDrawing, tags, true);
                        }
                        */
                    }
                }

                private void btnEditTypeBoxDrawing_Click(object sender, EventArgs e)
                {
                    //LoadBoxDriwingSettings();
                }

        #endregion Events Buttons

        #region Events ComboBox

        private void comboBoxNumberParcel_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Text = this.comboBoxNumberParcel.SelectedItem.ToString();
            this.currentParcel = ServiceIn4.GetParcelForCadarstralNumber
                                    (this.currentBlockLand, this.comboBoxNumberParcel.SelectedItem.ToString());
            if (!this.idCurrentHatchParcel.IsNull)
            {
                ServiceCAD.DeleteObject(this.idCurrentHatchParcel);
                ServiceCAD.DeleteObjects(this.idNeighborsCurrenParcel);
            }

            serviceParcel = new ServiceParcel(this.currentParcel, this.formSettings);

            AcGe.Point2dCollection pointsCurrentHatchParcel =
                ServiceSimpleElements.Offset(this.currentParcel.Points, this.offsetBlockLandView);
            AcDb.ObjectId idPolyline2d =
                ServiceCAD.InsertObject(ServiceSimpleElements.CreatePolyline2d(pointsCurrentHatchParcel, true));
            AcDb.Hatch curHatch =
                ServiceSimpleElements.CreateHatch(new AcDb.ObjectIdCollection(new AcDb.ObjectId[] { idPolyline2d }));
            this.idCurrentHatchParcel =
                ServiceCAD.InsertObject(curHatch);
            ServiceCAD.DeleteObject(idPolyline2d);


            /// =======================================================
            /// 

            if (this.currentParcel.StakeOutParcelPoints != null)
            {
                this.currentParcel.StakeOutParcelPoints.Clear();
            }

            StakeOutParcelPoint stakeOutParcelPoint;

            this.dataGridView_StakeOut.ClearSelection();
            this.dataGridView_StakeOut.Rows.Clear();

            int indexPoint = 0;
            object[] row = new object[2];

            foreach (AcGe.Point2d point in this.currentParcel.Points)
            {
                indexPoint++;

                stakeOutParcelPoint = new StakeOutParcelPoint();

                stakeOutParcelPoint.ScaleDrawing = this.drawingSettings.Scale.Value;

                stakeOutParcelPoint.Name = indexPoint.ToString();
                stakeOutParcelPoint.Coordinates = point;

                this.currentParcel.StakeOutParcelPoints.Add(stakeOutParcelPoint);
                row[0] = false;
                row[1] = indexPoint.ToString();

                this.dataGridView_StakeOut.Rows.Add(row);
            }

            SetColPointStationAndOrientationItems();
            AutoSearchingStationAndOrientationForAllPoints();

            this.dataGridView_StakeOut.Update();

            ReLoad_treeViewParcel();
        }

        private void comboBoxScaleDrawing_SelectedIndexChanged(object sender, EventArgs e)
                {
                    String strScaleDrawing = this.comboBoxScaleDrawing.SelectedItem.ToString();

                    this.formSettings.ScaleDrawing = Convert.ToDouble(strScaleDrawing.Replace("1 : ", "")) / 1000;

                    foreach (StakeOutParcelPoint spp in this.currentParcel.StakeOutParcelPoints)
                    {
                        spp.ScaleDrawing = this.formSettings.ScaleDrawing;
                    }
                }

                private void checkBoxPointsDisplay_CheckedChanged(object sender, EventArgs e)
                {
                    this.formSettings.DisplayPointNumbers = this.checkBoxPointsDisplay.Checked;
                }

            #endregion Events ComboBox

            #region Events CheckBox

                private void checkBoxDistDisplay_CheckedChanged(object sender, EventArgs e)
                {
                    this.formSettings.DisplayLengthLine = this.checkBoxDistDisplay.Checked;
                }

                private void checkBoxFillParcel_CheckedChanged(object sender, EventArgs e)
                {
                    this.formSettings.DisplayFillParcel = this.checkBoxFillParcel.Checked;
                }

                private void checkBoxFillLands_CheckedChanged(object sender, EventArgs e)
                {
                    this.formSettings.DisplayFillLand = this.checkBoxFillLands.Checked;
                }

                private void checkBoxFillLimiting_CheckedChanged(object sender, EventArgs e)
                {
                    this.formSettings.DisplayFillLimiting = this.checkBoxFillLimiting.Checked;
                }

                private void checkBoxFillNeighbors_CheckedChanged(object sender, EventArgs e)
                {
                    this.formSettings.DisplayFillNeighbors = this.checkBoxFillNeighbors.Checked;
                }
                private void checkBoxArea_CheckedChanged(object sender, EventArgs e)
                {
                    this.formSettings.DisplayArea = this.checkBoxArea.Checked;
                }

                private void checkBoxPerimeter_CheckedChanged(object sender, EventArgs e)
                {
                    this.formSettings.DisplayPerimeter = this.checkBoxPerimeter.Checked;
                }

        #endregion Events CheckBox

            #region Events RadioButton

        private void radioButtonPointsAutomatic_CheckedChanged(object sender, EventArgs e)
                {
                    this.formSettings.AutomaticDisplayPointNumbers = this.radioButtonPointsAutomatic.Checked;
                    if (!this.radioButtonPointsAutomatic.Checked) this.radioButtonPointsManually.Checked = true;
                }

                private void radioButtonDistAutomatic_CheckedChanged(object sender, EventArgs e)
                {
                    this.formSettings.AutomaticDisplayLengthLine = this.radioButtonDistAutomatic.Checked;
                    if (!this.radioButtonDistAutomatic.Checked) this.radioButtonDistManually.Checked = true;
                }

                private void radioButtonNeighborsStateAct_CheckedChanged(object sender, EventArgs e)
                {
                    this.formSettings.DisplayFillNeighborsStateAct = this.radioButtonNeighborsStateAct.Checked;
                    if (this.radioButtonNeighborsStateAct.Checked) this.radioButtonNeighborsAll.Checked = false;
                }

                private void radioButtonUnitAreaSquareMeter_CheckedChanged(object sender, EventArgs e)
                {
                    this.formSettings.UnitArea = this.radioButtonUnitAreaSquareMeter.Checked;
                    if (!this.radioButtonUnitAreaSquareMeter.Checked) this.radioButtonUnitAreaHectare.Checked = true;
                }

        #endregion Events RadioButton

        #region Events TabControl

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.formSettings.IndexTabControl = this.tabControl.TabIndex;
        }

        private void tabControl_Selected(object sender, TabControlEventArgs e)
        {
            this.formSettings.IndexTabControl = this.tabControl.SelectedIndex;
        }

        #endregion Events TabControl

        #endregion Events


        private void PurgeFromElementsBlockLand()
        {
            if (this.currentBlockLand != null) 
            { 
                this.currentBlockLand = null; 
            }

            if (!this.idCurrentHatchParcel.IsNull)
            {
                ServiceCAD.DeleteObject(this.idCurrentHatchParcel);
                this.idCurrentHatchParcel = new AcDb.ObjectId();
            }

            marker_reSart_allIdBlockLandParcel:

            foreach (AcDb.ObjectId objectId in allIdBlockLandParcel)
            {
                ServiceCAD.DeleteObject(objectId);
                allIdBlockLandParcel.Remove(objectId);
                goto marker_reSart_allIdBlockLandParcel;
            }
        }

        //  for TreeView
        #region TreeView

        private void ReLoad_treeViewParcel()
        {
            this.treeView_Parcel.Nodes.Clear();
            this.treeView_Parcel.Nodes.Add( new TreeNode( "Земельна ділянка" ) );

            TreeNode rootNode = new TreeNode();
            rootNode = this.treeView_Parcel.Nodes[0];

            List<string> listFilter = new List<string>();

            listFilter.Add("NM");
            listFilter.Add("OX");
            listFilter.Add("CN");
            listFilter.Add("NB");

            //
            //      Інформація
            //
            TreeNode treeNodeParcelInfo = new TreeNode("Інформація");
            rootNode.Nodes.Add(treeNodeParcelInfo);
            AddNode(this.currentParcel, treeNodeParcelInfo );

            //
            //      Угіддя земельної ділянки
            //
            TreeNode treeNodeLans = new TreeNode("Угіддя земельної ділянки");
            rootNode.Nodes.Add(treeNodeLans);
            foreach (LandPolygon polygon in this.currentParcel.Lands)
            {
                AddNode(polygon, treeNodeLans, listFilter);
            }

            //
            //      Обмеження земельної ділянки
            //
            TreeNode treeNodeLimiting = new TreeNode("Обмеження земельної ділянки");
            rootNode.Nodes.Add(treeNodeLimiting);
            foreach (LandPolygon polygon in this.currentParcel.Limiting)
            {
                AddNode(polygon, treeNodeLimiting, listFilter);
            }

            //
            //      Суміжні ділянки
            //
            TreeNode treeNodeNeighbors = new TreeNode("Суміжні ділянки");
            rootNode.Nodes.Add(treeNodeNeighbors);
            foreach (NeighborsAlongContour contour in this.currentParcel.ContoursNeighbors)
            {
                foreach (LandPolygon polygon in contour.Neighbors)
                {
                    AddNode(polygon, treeNodeNeighbors, listFilter);
                }
            }
           

            /*
            foreach (LandPolygon polygon in this.currentParcel.Neighbors)
            {
                AddNode(polygon, treeNodeNeighbors, listFilter);
            }
            */

            //
            //      Інші ділянки
            //      
            TreeNode treeNodeOtherLand = new TreeNode("Інші ділянки");
            rootNode.Nodes.Add(treeNodeOtherLand);
            foreach (LandPolygon polygon in this.currentParcel.OtherLand)
            {
                AddNode(polygon, treeNodeOtherLand, listFilter);
            }

            this.treeView_Parcel.ExpandAll();
        }

        private void AddNode(ILandPolygon poligon, TreeNode inTreeNode)
        {
            foreach (LandInfo info in poligon.Info)
            {
                inTreeNode.Nodes.Add(new TreeNode(info.Key + " - " + info.Value));
            }
        }

        private void AddNode(ILandPolygon poligon, TreeNode inTreeNode, List<string> filter)
        {
            foreach (LandInfo info in poligon.Info)
            {
                if ( filter.IndexOf(info.Key) >-1)
                {
                    inTreeNode.Nodes.Add(new TreeNode(info.Value));
                }
            }
        }
        #endregion TreeView

        //  for TableSettings
        #region TableSettings

        private void LoadTableSettings()
        {
            tableSettings = null;
            tableSettings = LoadSettings<SettingsTable>(localPath.FindFullPathFromXml("PathTables"));
            if (tableSettings == null)
            {
                tableSettings = SettingsTable.Default;
            }
            this.checkedListBox_TypeTable.Items.Clear();
            foreach (SettingTable setting in this.tableSettings.Settings)
            {
                this.checkedListBox_TypeTable.Items.Add(setting.KeyTable);
            }
        }

        /*
        private void AddTableSettings(SettingTable setting)
        {
            this.tableSettings.Settings.Add(setting);
            LoadTableSettings();
        }
        */

        #endregion TableSettings

        //  for BoxDriwingSettings
        #region BoxDriwingSettings

        private void LoadFrameDrawingSettings()
        {
            this.frameDrawingSettings = null;
            this.frameDrawingSettings = LoadSettings<SettingsLand>(localPath.FindFullPathFromXml("PathFrameDrawing"));
            if (this.frameDrawingSettings == null)
            {
                this.frameDrawingSettings = SettingsLand.Default;
                ServiceXml.WriteXml<SettingsLand>(this.frameDrawingSettings, localPath.FindFullPathFromXml("PathFrameDrawing"));
            }
            this.checkedListBoxTypeBoxDrawing.Items.Clear();
            foreach (SettingLand setting in this.frameDrawingSettings.Setting)
            {
                this.checkedListBoxTypeBoxDrawing.Items.Add(setting.Name);
            }
        }

        /*
        private void _AddBoxDriwingSettings(SettingLand setting)
        {
            this.frameDrawingSettings.Setting.Add(setting);
            LoadFrameDrawingSettings();
        }
        */

        #endregion TableSettings

        //  for Settings
        #region Settings

        private static T LoadSettings<T>( string pathSettings )
        {
            return ServiceXml.ReadXml<T>(pathSettings);
        }
        
        private void LoadAllSettings()
        {
            this.drawingSettings = LoadSettings<SettingsDrawing>(localPath.FindFullPathFromXml("PathDrawing"));
            if (this.drawingSettings == null)
            {
                this.drawingSettings = SettingsDrawing.Default;
            }

            this.userSettings = LoadSettings<SettingsLand>(localPath.FindFullPathFromXml("PathUsers"));
            if (this.userSettings == null) 
            {
                this.userSettings = SettingsLand.Default;
            }

            this.formSettings = LoadSettings<SettingsFormLand>(localPath.FindFullPathFromXml("PathFormLand"));
            if (this.formSettings == null)
            {
                this.formSettings = new SettingsFormLand();
            }

            this.tableSettings = LoadSettings<SettingsTable>(localPath.FindFullPathFromXml("PathTables"));
            if (this.formSettings == null)
            {
                this.tableSettings = SettingsTable.Default;
            }

            this.frameDrawingSettings = LoadSettings<SettingsLand>(localPath.FindFullPathFromXml("PathFrameDrawing"));
            if (this.formSettings == null)
            {
                this.frameDrawingSettings = SettingsLand.Default;
            }
        }

        /// <summary>
        /// Saves the settings.
        /// </summary>
        /// <typeparam name="T">Type settings.</typeparam>
        /// <param name="settings">The settings.</param>
        /// <param name="pathSettings">The path settings.</param>
        private void SaveSettings<T>(T settings, string pathSettings)
        {
            try
            {
                ServiceXml.WriteXml<T>(settings, pathSettings);
            }
            catch(Exception exc)
            {
                CurrentCAD.Editor.WriteMessage("* Error * SaveSettings<" + settings.GetType().Name + ">");
                CurrentCAD.Editor.WriteMessage("* " + exc.ToString());
            }
            
        }

        /// <summary>
        /// Saves all settings.
        /// </summary>
        private void SaveAllSettings()
        {
            SaveSettings(this.userSettings, localPath.FindFullPathFromXml("PathUsers"));
            SaveSettings<SettingsFormLand>(this.formSettings, localPath.FindFullPathFromXml("PathFormLand"));
            SaveSettings<SettingsTable>(this.tableSettings, localPath.FindFullPathFromXml("PathTableSettings"));
            SaveSettings(this.frameDrawingSettings, localPath.FindFullPathFromXml("PathFrameDrawingSettings"));
        }

        /// <summary>
        /// Updates the form settings.
        /// </summary>
        private void UpdateFormSettings()
        {
            this.formSettings = new SettingsFormLand();

            this.formSettings.ScaleDrawing = 
                Convert.ToDouble(
                    this.comboBoxScaleDrawing.SelectedItem.ToString().Replace("1 : ", "")
                ) / 1000;

            this.formSettings.IndexTabControl = this.tabControl.SelectedIndex;

            this.formSettings.DisplayPointNumbers = this.checkBoxPointsDisplay.Checked;
            this.formSettings.AutomaticDisplayPointNumbers = this.radioButtonPointsAutomatic.Checked;

            this.formSettings.DisplayLengthLine = this.checkBoxDistDisplay.Checked;
            this.formSettings.AutomaticDisplayLengthLine = this.radioButtonDistAutomatic.Checked;

            this.formSettings.DisplayFillParcel = this.checkBoxFillParcel.Checked;
            this.formSettings.DisplayFillLand = this.checkBoxFillLands.Checked;
            this.formSettings.DisplayFillLimiting =this.checkBoxFillLimiting.Checked;
            this.formSettings.DisplayFillNeighbors =this.checkBoxFillNeighbors.Checked;
            this.formSettings.DisplayFillNeighborsStateAct =this.radioButtonNeighborsStateAct.Checked;

            this.formSettings.DisplayArea = this.checkBoxArea.Checked;
            this.formSettings.UnitArea = this.radioButtonUnitAreaSquareMeter.Checked;

            this.formSettings.DisplayPerimeter = this.checkBoxPerimeter.Checked;
        }

        /// <summary>
        /// Updates <see cref="T:Losa.Land.Forms.FormLand"/> the by form settings.
        /// </summary>
        private void UpdateFormLandByFormSettings()
        {
            string strScale = "1 : " + (this.formSettings.ScaleDrawing * 1000).ToString("0");
            int indexScale = this.comboBoxScaleDrawing.Items.IndexOf(strScale);

            if (indexScale > 0)
            {
                this.comboBoxScaleDrawing.SelectedIndex = indexScale;
            }
            else
            {
                MessageBox.Show (
                                    "Помилка налаштувань масштабу '" + strScale + "'",
                                    "Помилка UpdateByFormSettings()",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error
                                );
            }
            /*
            int i = -1;  
            foreach ( string obj in this.comboBoxScaleDrawing.Items)
            {
                i ++;
                double dblScale = Convert.ToDouble(obj.Replace("1 : ", "")) / 1000;
                //string strScale = dblScale.ToString("0");
                if ( dblScale == this.formSettings.ScaleDrawing ) 
                {
                    this.comboBoxScaleDrawing.SelectedIndex = i;
                    break;
                }
            }
            */
            this.tabControl.SelectedIndex = this.formSettings.IndexTabControl+1;

            this.checkBoxPointsDisplay.Checked = this.formSettings.DisplayPointNumbers;
            this.radioButtonPointsAutomatic.Checked = this.formSettings.AutomaticDisplayPointNumbers;

            this.checkBoxDistDisplay.Checked = this.formSettings.DisplayLengthLine ;
            this.radioButtonDistAutomatic.Checked = this.formSettings.AutomaticDisplayLengthLine;

            this.checkBoxFillParcel.Checked = this.formSettings.DisplayFillParcel;
            this.checkBoxFillLands.Checked = this.formSettings.DisplayFillLand ;
            this.checkBoxFillLimiting.Checked = this.formSettings.DisplayFillLimiting ;
            this.checkBoxFillNeighbors.Checked = this.formSettings.DisplayFillNeighbors ;
            this.radioButtonNeighborsStateAct.Checked  = this.formSettings.DisplayFillNeighborsStateAct;

            this.checkBoxArea.Checked = this.formSettings.DisplayArea;
            this.radioButtonUnitAreaSquareMeter.Checked = this.formSettings.UnitArea;

            this.checkBoxPerimeter.Checked = this.formSettings.DisplayPerimeter;
        }

        #endregion Settings

        #region dataGridView_StakeOut

        private void dataGridView_StakeOut_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            
        }

        private void dataGridView_StakeOut_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView senderGrid = (DataGridView)sender;

            int iCol = this.dataGridView_StakeOut.CurrentCell.ColumnIndex;
            int iRow = this.dataGridView_StakeOut.CurrentCell.RowIndex;

            if (senderGrid.Columns[iCol] is DataGridViewComboBoxColumn &&
                iRow >= 0 &&
                this.basePoints != null)
            {
                var curCell = senderGrid.CurrentCell;
                if (senderGrid.CurrentCell.Value.Equals("АвтоПошук"))
                {
                    bool isSetStationAndOrientation = this.currentParcel.StakeOutParcelPoints[iRow]
                                                                .AutoSetStationAndOrientation(this.basePoints);
                    senderGrid.CurrentCell.Value = this.currentParcel.StakeOutParcelPoints[iRow].PointStation.Name;
                }
            }
        }


        private void dataGridView_StakeOut_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (this.basePoints == null)
            {
                return;
            }

            DataGridView senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0 &&
                this.basePoints.Count > 0)
            {
                int indexRow = (int)this.dataGridView_StakeOut.CurrentRow.Index;
                /*
                if ( this.currentParcel.StakeOutParcelPoints[indexRow].AutoSetStationAndOrientation(this.basePoints) )
                {
                    this.dataGridView_StakeOut[2, indexRow].Value = 
                        this.currentParcel.StakeOutParcelPoints[indexRow].PointStation.Name;

                    this.dataGridView_StakeOut[3, indexRow].Value = 
                        this.currentParcel.StakeOutParcelPoints[indexRow].PointOrientation.Name;
                }
                */
                AutoSearchingStationAndOrientationForPoint(this.currentParcel.StakeOutParcelPoints[indexRow]);
            }   
        }

        private void dataGridView_StakeOut_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        { 
            //UpdateStakeOut();
        }

        private void dataGridView_StakeOut_SelectionChanged(object sender, EventArgs e)
        {
            reStart_SelectionChanged:
            if (this.dataGridView_StakeOut.SelectedRows.Count == 0)
            {
                int indexRow = (int)this.dataGridView_StakeOut.CurrentRow.Index;
                this.dataGridView_StakeOut.Rows[indexRow].Selected = true;
                goto reStart_SelectionChanged;
            }

            UpdateStakeOutByDataGridView();
        }

        private void UpdateStakeOutByDataGridView()
        {
            string nameBasePoint = "";
            BasePoint bp = null;

            for (int i = 0; i < this.dataGridView_StakeOut.RowCount; i++)
            {
                if (this.dataGridView_StakeOut[2, i].Value != null)
                {
                    nameBasePoint = this.dataGridView_StakeOut[2, i].Value.ToString();
                    bp = this.basePoints.Find(x => x.Name.Contains(nameBasePoint));
                    this.currentParcel.StakeOutParcelPoints[i].PointStation = bp;
                }

                this.currentParcel.StakeOutParcelPoints[i].Visible = (bool)this.dataGridView_StakeOut[0, i].Value;

                this.dataGridView_StakeOut.Rows[(int)this.dataGridView_StakeOut.CurrentRow.Index].Selected = true;

                this.currentParcel.StakeOutParcelPoints[i].Regen();
            }
        }

        private void UpdateDataGridViewByStakeOut()
        {
            int indexRow = -1;
            foreach ( StakeOutParcelPoint stakeOut in this.currentParcel.StakeOutParcelPoints)
            {
                indexRow++;
                this.dataGridView_StakeOut[0, indexRow].Value = stakeOut.Visible;
                this.dataGridView_StakeOut[1, indexRow].Value = stakeOut.Name;
                this.dataGridView_StakeOut[2, indexRow].Value = stakeOut.PointStation;
                this.dataGridView_StakeOut[3, indexRow].Value = stakeOut.PointOrientation;
            }
        }
 
        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.dataGridView_StakeOut.RowCount; i++)
            {
                this.currentParcel.StakeOutParcelPoints[i].Visible = true;
                this.dataGridView_StakeOut[0, i].Value = true;
                this.currentParcel.StakeOutParcelPoints[i].Regen();
            }
        }

        private void btnRemoveSelectionAll_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < this.dataGridView_StakeOut.RowCount; i++)
            {
                this.currentParcel.StakeOutParcelPoints[i].Visible = false;
                this.dataGridView_StakeOut[0, i].Value = false;
                this.currentParcel.StakeOutParcelPoints[i].Regen();
            }
        }

        private void mnu_About_Click(object sender, EventArgs e)
        {
            try
            {
                formAboutBox.Show();
            }
            catch (Exception exc)
            {
                formAboutBox.Close();
                this.formAboutBox = new FormAboutBox();
                formAboutBox.Show();
            }
        }

        private void btnAddTableStakeoutPoints_Click(object sender, EventArgs e)
        {
            this.tabControl.SelectedIndex=2;
            this.checkedListBox_TypeTable.Update();          
            var indexFind = this.checkedListBox_TypeTable.Items
                                    .IndexOf("Винос меж зем.ділянки (Дир. Кути)");
            this.checkedListBox_TypeTable.SelectedIndex = indexFind;
            this.checkedListBox_TypeTable.SetItemCheckState(indexFind ,CheckState.Checked);
        }
    }
    #endregion dataGridView_StakeOut
}
