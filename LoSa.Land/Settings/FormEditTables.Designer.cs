namespace LoSa.Land.Tables
{
    partial class FormEditTables
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.bindingSourceSettingsTable = new System.Windows.Forms.BindingSource(this.components);
            this.btnSaveSettingsTable = new System.Windows.Forms.Button();
            this.dataGridViewSettingsTable = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceSettingsTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSettingsTable)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 0);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.dataGridViewSettingsTable);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.btnSaveSettingsTable);
            this.splitContainer.Size = new System.Drawing.Size(580, 412);
            this.splitContainer.SplitterDistance = 367;
            this.splitContainer.TabIndex = 1;
            // 
            // btnSaveSettingsTable
            // 
            this.btnSaveSettingsTable.Location = new System.Drawing.Point(457, 11);
            this.btnSaveSettingsTable.Name = "btnSaveSettingsTable";
            this.btnSaveSettingsTable.Size = new System.Drawing.Size(120, 27);
            this.btnSaveSettingsTable.TabIndex = 0;
            this.btnSaveSettingsTable.Text = "Зберегти зміни";
            this.btnSaveSettingsTable.UseVisualStyleBackColor = true;
            this.btnSaveSettingsTable.Click += new System.EventHandler(this.button1_Click);
            // 
            // dataGridViewSettingsTable
            // 
            this.dataGridViewSettingsTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewSettingsTable.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewSettingsTable.Location = new System.Drawing.Point(0, 0);
            this.dataGridViewSettingsTable.Name = "dataGridViewSettingsTable";
            this.dataGridViewSettingsTable.Size = new System.Drawing.Size(580, 367);
            this.dataGridViewSettingsTable.TabIndex = 0;
            // 
            // FormEditTables
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(580, 412);
            this.Controls.Add(this.splitContainer);
            this.Name = "FormEditTables";
            this.Text = "FormEditTables";
            this.Load += new System.EventHandler(this.FormEditTables_Load);
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceSettingsTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewSettingsTable)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.Button btnSaveSettingsTable;
        private System.Windows.Forms.BindingSource bindingSourceSettingsTable;
        //private System.Windows.Forms.DataGridViewImageColumn imageDataGridViewImageColumn;
        //private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        //private System.Windows.Forms.DataGridViewTextBoxColumn typeNameDataGridViewTextBoxColumn;
        //private System.Windows.Forms.DataGridViewCheckBoxColumn isDesignableDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridView dataGridViewSettingsTable;

    }
}