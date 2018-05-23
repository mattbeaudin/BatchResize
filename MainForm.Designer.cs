using System.Windows.Forms;

namespace BatchResize
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.txtPhotoDirectory = new System.Windows.Forms.TextBox();
            this.lblPhotoDirectory = new System.Windows.Forms.Label();
            this.btnSelectDirectory = new System.Windows.Forms.Button();
            this.gbSaveOptions = new System.Windows.Forms.GroupBox();
            this.nudHeight = new System.Windows.Forms.NumericUpDown();
            this.lblNewHeight = new System.Windows.Forms.Label();
            this.nudWidth = new System.Windows.Forms.NumericUpDown();
            this.lblNewWidth = new System.Windows.Forms.Label();
            this.btnResize = new System.Windows.Forms.Button();
            this.pbResize = new System.Windows.Forms.ProgressBar();
            this.cmbFileExtension = new System.Windows.Forms.ComboBox();
            this.lblGuide = new System.Windows.Forms.Label();
            this.chkMaintainAspectRatio = new System.Windows.Forms.CheckBox();
            this.gbSaveOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWidth)).BeginInit();
            this.SuspendLayout();
            // 
            // txtPhotoDirectory
            // 
            this.txtPhotoDirectory.BackColor = System.Drawing.SystemColors.Window;
            this.txtPhotoDirectory.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPhotoDirectory.Location = new System.Drawing.Point(12, 30);
            this.txtPhotoDirectory.Name = "txtPhotoDirectory";
            this.txtPhotoDirectory.ReadOnly = true;
            this.txtPhotoDirectory.Size = new System.Drawing.Size(321, 19);
            this.txtPhotoDirectory.TabIndex = 0;
            // 
            // lblPhotoDirectory
            // 
            this.lblPhotoDirectory.AutoSize = true;
            this.lblPhotoDirectory.Location = new System.Drawing.Point(12, 9);
            this.lblPhotoDirectory.Name = "lblPhotoDirectory";
            this.lblPhotoDirectory.Size = new System.Drawing.Size(106, 17);
            this.lblPhotoDirectory.TabIndex = 1;
            this.lblPhotoDirectory.Text = "Photo Directory";
            // 
            // btnSelectDirectory
            // 
            this.btnSelectDirectory.Location = new System.Drawing.Point(424, 27);
            this.btnSelectDirectory.Name = "btnSelectDirectory";
            this.btnSelectDirectory.Size = new System.Drawing.Size(140, 26);
            this.btnSelectDirectory.TabIndex = 2;
            this.btnSelectDirectory.Text = "Select Photo Folder";
            this.btnSelectDirectory.UseVisualStyleBackColor = true;
            this.btnSelectDirectory.Click += new System.EventHandler(this.btnSelectDirectory_Click);
            // 
            // gbSaveOptions
            // 
            this.gbSaveOptions.Controls.Add(this.chkMaintainAspectRatio);
            this.gbSaveOptions.Controls.Add(this.lblGuide);
            this.gbSaveOptions.Controls.Add(this.nudHeight);
            this.gbSaveOptions.Controls.Add(this.lblNewHeight);
            this.gbSaveOptions.Controls.Add(this.nudWidth);
            this.gbSaveOptions.Controls.Add(this.lblNewWidth);
            this.gbSaveOptions.Location = new System.Drawing.Point(12, 59);
            this.gbSaveOptions.Name = "gbSaveOptions";
            this.gbSaveOptions.Size = new System.Drawing.Size(552, 106);
            this.gbSaveOptions.TabIndex = 6;
            this.gbSaveOptions.TabStop = false;
            this.gbSaveOptions.Text = "Save Options";
            // 
            // nudHeight
            // 
            this.nudHeight.Location = new System.Drawing.Point(95, 43);
            this.nudHeight.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.nudHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudHeight.Name = "nudHeight";
            this.nudHeight.Size = new System.Drawing.Size(72, 22);
            this.nudHeight.TabIndex = 9;
            this.nudHeight.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudHeight.ValueChanged += new System.EventHandler(this.nudHeight_ValueChanged);
            // 
            // lblNewHeight
            // 
            this.lblNewHeight.AutoSize = true;
            this.lblNewHeight.Location = new System.Drawing.Point(92, 23);
            this.lblNewHeight.Name = "lblNewHeight";
            this.lblNewHeight.Size = new System.Drawing.Size(80, 17);
            this.lblNewHeight.TabIndex = 8;
            this.lblNewHeight.Text = "New Height";
            // 
            // nudWidth
            // 
            this.nudWidth.Location = new System.Drawing.Point(14, 43);
            this.nudWidth.Maximum = new decimal(new int[] {
            4000,
            0,
            0,
            0});
            this.nudWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudWidth.Name = "nudWidth";
            this.nudWidth.Size = new System.Drawing.Size(72, 22);
            this.nudWidth.TabIndex = 7;
            this.nudWidth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudWidth.ValueChanged += new System.EventHandler(this.nudWidth_ValueChanged);
            // 
            // lblNewWidth
            // 
            this.lblNewWidth.AutoSize = true;
            this.lblNewWidth.Location = new System.Drawing.Point(11, 23);
            this.lblNewWidth.Name = "lblNewWidth";
            this.lblNewWidth.Size = new System.Drawing.Size(75, 17);
            this.lblNewWidth.TabIndex = 6;
            this.lblNewWidth.Text = "New Width";
            // 
            // btnResize
            // 
            this.btnResize.Enabled = false;
            this.btnResize.Location = new System.Drawing.Point(12, 171);
            this.btnResize.Name = "btnResize";
            this.btnResize.Size = new System.Drawing.Size(121, 33);
            this.btnResize.TabIndex = 7;
            this.btnResize.Text = "Resize Images";
            this.btnResize.UseVisualStyleBackColor = true;
            this.btnResize.Click += new System.EventHandler(this.btnResize_Click);
            // 
            // pbResize
            // 
            this.pbResize.Location = new System.Drawing.Point(139, 171);
            this.pbResize.Name = "pbResize";
            this.pbResize.Size = new System.Drawing.Size(425, 33);
            this.pbResize.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pbResize.TabIndex = 8;
            // 
            // cmbFileExtension
            // 
            this.cmbFileExtension.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFileExtension.FormattingEnabled = true;
            this.cmbFileExtension.Items.AddRange(new object[] {
            ".bmp",
            ".cr2",
            ".eps",
            ".gif",
            ".jpg",
            ".jpeg",
            ".nef",
            ".orf",
            ".png",
            ".raw",
            ".sr2",
            ".tif",
            ".tiff"});
            this.cmbFileExtension.Location = new System.Drawing.Point(339, 28);
            this.cmbFileExtension.Name = "cmbFileExtension";
            this.cmbFileExtension.Size = new System.Drawing.Size(79, 24);
            this.cmbFileExtension.TabIndex = 9;
            // 
            // lblGuide
            // 
            this.lblGuide.Location = new System.Drawing.Point(181, 12);
            this.lblGuide.Name = "lblGuide";
            this.lblGuide.Size = new System.Drawing.Size(365, 91);
            this.lblGuide.TabIndex = 10;
            this.lblGuide.Text = resources.GetString("lblGuide.Text");
            // 
            // chkMaintainAspectRatio
            // 
            this.chkMaintainAspectRatio.AutoSize = true;
            this.chkMaintainAspectRatio.Location = new System.Drawing.Point(8, 71);
            this.chkMaintainAspectRatio.Name = "chkMaintainAspectRatio";
            this.chkMaintainAspectRatio.Size = new System.Drawing.Size(167, 21);
            this.chkMaintainAspectRatio.TabIndex = 11;
            this.chkMaintainAspectRatio.Text = "Maintain Aspect Ratio";
            this.chkMaintainAspectRatio.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(576, 213);
            this.Controls.Add(this.cmbFileExtension);
            this.Controls.Add(this.pbResize);
            this.Controls.Add(this.btnResize);
            this.Controls.Add(this.gbSaveOptions);
            this.Controls.Add(this.btnSelectDirectory);
            this.Controls.Add(this.lblPhotoDirectory);
            this.Controls.Add(this.txtPhotoDirectory);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Batch Image Resizer";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.gbSaveOptions.ResumeLayout(false);
            this.gbSaveOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWidth)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtPhotoDirectory;
        private System.Windows.Forms.Label lblPhotoDirectory;
        private System.Windows.Forms.Button btnSelectDirectory;
        private System.Windows.Forms.GroupBox gbSaveOptions;
        private System.Windows.Forms.Button btnResize;
        private System.Windows.Forms.ProgressBar pbResize;
        private System.Windows.Forms.NumericUpDown nudHeight;
        private System.Windows.Forms.Label lblNewHeight;
        private System.Windows.Forms.NumericUpDown nudWidth;
        private System.Windows.Forms.Label lblNewWidth;
        private ComboBox cmbFileExtension;
        private CheckBox chkMaintainAspectRatio;
        private Label lblGuide;
    }
}

