using LoSa.Land.Service;
using LoSa.Utility;
using LoSa.Xml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace LoSa.Land.Tables
{
    public partial class FormEditTables : Form
    {
        LocalPath localPath = new LocalPath("LoSa_Land");

        private SettingsTable tableSettings;

        public FormEditTables()
        {
            tableSettings = ServiceXml.ReadXml<SettingsTable>(localPath.FindLocalPathFromXml("PathTables").Name);
            InitializeComponent(); 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SaveSettingsTable();
            LoadSettingsTable();
        }

        private void FormEditTables_Load(object sender, EventArgs e)
        {
            foreach (SettingTable sTable in tableSettings.Settings)
            {
                bindingSourceSettingsTable.Add(sTable);
            }

            dataGridViewSettingsTable.DataSource = bindingSourceSettingsTable;

            dataGridViewSettingsTable.AutoGenerateColumns = false;
            dataGridViewSettingsTable.AutoSize = true;

            DataGridViewColumn column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "KeyTable";
            column.Name = "Key";
            dataGridViewSettingsTable.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "TypeTable";
            column.Name = "Type";
            dataGridViewSettingsTable.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "TextHeight";
            column.Name = "Height";
            dataGridViewSettingsTable.Columns.Add(column);

            column = new DataGridViewTextBoxColumn();
            column.DataPropertyName = "Title";
            column.Name = "Title";
            dataGridViewSettingsTable.Columns.Add(column);

            this.Controls.Add(dataGridViewSettingsTable);
            this.AutoSize = true;
            LoadSettingsTable();
        }

        private void LoadSettingsTable()
        {


           
            

            /*
            try
            {
                XmlReader xmlFile = XmlReader.Create(SettingLocalPath.PathTableSettings, new XmlReaderSettings());
                DataSet ds = new DataSet();
                ds.ReadXml(xmlFile);
                dataGridViewSettingsTable.DataSource = ds.Tables[0];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            } 
             */ 
        }

        private void SaveSettingsTable()
        {
            /*
            try
            {
                DataSet ds = new DataSet();
                ds = (DataSet)(dataGridViewSettingsTable.DataSource);
                ds.WriteXml(SettingLocalPath.PathTableSettings);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            } 
            */
        }
    }
}
