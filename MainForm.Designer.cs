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
            this.gbResizeOptions = new System.Windows.Forms.GroupBox();
            this.chkMaintainAspectRatio = new System.Windows.Forms.CheckBox();
            this.lblGuide = new System.Windows.Forms.Label();
            this.nudHeight = new System.Windows.Forms.NumericUpDown();
            this.lblNewHeight = new System.Windows.Forms.Label();
            this.nudWidth = new System.Windows.Forms.NumericUpDown();
            this.lblNewWidth = new System.Windows.Forms.Label();
            this.btnResize = new System.Windows.Forms.Button();
            this.pbResize = new System.Windows.Forms.ProgressBar();
            this.cmbFileExtension = new System.Windows.Forms.ComboBox();
            this.gbSaveOptions = new System.Windows.Forms.GroupBox();
            this.pnlCopyControls = new System.Windows.Forms.Panel();
            this.btnCopyDir = new System.Windows.Forms.Button();
            this.lblCopy = new System.Windows.Forms.Label();
            this.txtOutputDirectory = new System.Windows.Forms.TextBox();
            this.rbCopy = new System.Windows.Forms.RadioButton();
            this.rbOverwrite = new System.Windows.Forms.RadioButton();
            this.gbResizeOptions.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWidth)).BeginInit();
            this.gbSaveOptions.SuspendLayout();
            this.pnlCopyControls.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtPhotoDirectory
            // 
            this.txtPhotoDirectory.BackColor = System.Drawing.SystemColors.Window;
            this.txtPhotoDirectory.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.txtPhotoDirectory.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPhotoDirectory.Location = new System.Drawing.Point(9, 24);
            this.txtPhotoDirectory.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtPhotoDirectory.Name = "txtPhotoDirectory";
            this.txtPhotoDirectory.ReadOnly = true;
            this.txtPhotoDirectory.Size = new System.Drawing.Size(242, 19);
            this.txtPhotoDirectory.TabIndex = 0;
            // 
            // lblPhotoDirectory
            // 
            this.lblPhotoDirectory.AutoSize = true;
            this.lblPhotoDirectory.Location = new System.Drawing.Point(9, 7);
            this.lblPhotoDirectory.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPhotoDirectory.Name = "lblPhotoDirectory";
            this.lblPhotoDirectory.Size = new System.Drawing.Size(80, 13);
            this.lblPhotoDirectory.TabIndex = 1;
            this.lblPhotoDirectory.Text = resources.GetString("SelectDirectoryLabelText");
            // 
            // btnSelectDirectory
            // 
            this.btnSelectDirectory.Location = new System.Drawing.Point(318, 22);
            this.btnSelectDirectory.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnSelectDirectory.Name = "btnSelectDirectory";
            this.btnSelectDirectory.Size = new System.Drawing.Size(105, 22);
            this.btnSelectDirectory.TabIndex = 2;
            this.btnSelectDirectory.Text = resources.GetString("SelectDirectoryText");
            this.btnSelectDirectory.UseVisualStyleBackColor = true;
            this.btnSelectDirectory.Click += new System.EventHandler(this.btnSelectDirectory_Click);
            // 
            // gbResizeOptions
            // 
            this.gbResizeOptions.Controls.Add(this.chkMaintainAspectRatio);
            this.gbResizeOptions.Controls.Add(this.lblGuide);
            this.gbResizeOptions.Controls.Add(this.nudHeight);
            this.gbResizeOptions.Controls.Add(this.lblNewHeight);
            this.gbResizeOptions.Controls.Add(this.nudWidth);
            this.gbResizeOptions.Controls.Add(this.lblNewWidth);
            this.gbResizeOptions.Location = new System.Drawing.Point(9, 118);
            this.gbResizeOptions.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.gbResizeOptions.Name = "gbResizeOptions";
            this.gbResizeOptions.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.gbResizeOptions.Size = new System.Drawing.Size(414, 86);
            this.gbResizeOptions.TabIndex = 6;
            this.gbResizeOptions.TabStop = false;
            this.gbResizeOptions.Text = resources.GetString("ResizeOptionsText");
            // 
            // chkMaintainAspectRatio
            // 
            this.chkMaintainAspectRatio.AutoSize = true;
            this.chkMaintainAspectRatio.Location = new System.Drawing.Point(10, 60);
            this.chkMaintainAspectRatio.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.chkMaintainAspectRatio.Name = "chkMaintainAspectRatio";
            this.chkMaintainAspectRatio.Size = new System.Drawing.Size(130, 17);
            this.chkMaintainAspectRatio.TabIndex = 11;
            this.chkMaintainAspectRatio.Text = resources.GetString("MaintainAspectRatioText");
            this.chkMaintainAspectRatio.UseVisualStyleBackColor = true;
            // 
            // lblGuide
            // 
            this.lblGuide.Location = new System.Drawing.Point(137, 10);
            this.lblGuide.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblGuide.Name = "lblGuide";
            this.lblGuide.Size = new System.Drawing.Size(276, 74);
            this.lblGuide.TabIndex = 10;
            this.lblGuide.Text = resources.GetString("GuideText");
            // 
            // nudHeight
            // 
            this.nudHeight.Location = new System.Drawing.Point(71, 35);
            this.nudHeight.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.nudHeight.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudHeight.Name = "nudHeight";
            this.nudHeight.Size = new System.Drawing.Size(54, 20);
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
            this.lblNewHeight.Location = new System.Drawing.Point(69, 19);
            this.lblNewHeight.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblNewHeight.Name = "lblNewHeight";
            this.lblNewHeight.Size = new System.Drawing.Size(63, 13);
            this.lblNewHeight.TabIndex = 8;
            this.lblNewHeight.Text = resources.GetString("NewHeightLabel");
            // 
            // nudWidth
            // 
            this.nudWidth.Location = new System.Drawing.Point(10, 35);
            this.nudWidth.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.nudWidth.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.nudWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudWidth.Name = "nudWidth";
            this.nudWidth.Size = new System.Drawing.Size(54, 20);
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
            this.lblNewWidth.Location = new System.Drawing.Point(8, 19);
            this.lblNewWidth.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblNewWidth.Name = "lblNewWidth";
            this.lblNewWidth.Size = new System.Drawing.Size(60, 13);
            this.lblNewWidth.TabIndex = 6;
            this.lblNewWidth.Text = resources.GetString("NewWidthLabel");
            // 
            // btnResize
            // 
            this.btnResize.Enabled = false;
            this.btnResize.Location = new System.Drawing.Point(9, 209);
            this.btnResize.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnResize.Name = "btnResize";
            this.btnResize.Size = new System.Drawing.Size(91, 27);
            this.btnResize.TabIndex = 7;
            this.btnResize.Text = resources.GetString("ResizeImagesText");
            this.btnResize.UseVisualStyleBackColor = true;
            this.btnResize.Click += new System.EventHandler(this.btnResize_Click);
            // 
            // pbResize
            // 
            this.pbResize.Location = new System.Drawing.Point(104, 209);
            this.pbResize.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pbResize.Name = "pbResize";
            this.pbResize.Size = new System.Drawing.Size(319, 27);
            this.pbResize.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pbResize.TabIndex = 8;
            // 
            // cmbFileExtension
            // 
            this.cmbFileExtension.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFileExtension.FormattingEnabled = true;
            this.cmbFileExtension.Location = new System.Drawing.Point(254, 23);
            this.cmbFileExtension.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.cmbFileExtension.Name = "cmbFileExtension";
            this.cmbFileExtension.Size = new System.Drawing.Size(60, 21);
            this.cmbFileExtension.TabIndex = 9;
            // 
            // gbSaveOptions
            // 
            this.gbSaveOptions.Controls.Add(this.pnlCopyControls);
            this.gbSaveOptions.Controls.Add(this.rbCopy);
            this.gbSaveOptions.Controls.Add(this.rbOverwrite);
            this.gbSaveOptions.Location = new System.Drawing.Point(9, 45);
            this.gbSaveOptions.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.gbSaveOptions.Name = "gbSaveOptions";
            this.gbSaveOptions.Padding = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.gbSaveOptions.Size = new System.Drawing.Size(414, 68);
            this.gbSaveOptions.TabIndex = 10;
            this.gbSaveOptions.TabStop = false;
            this.gbSaveOptions.Text = resources.GetString("SaveOptionsText");
            // 
            // pnlCopyControls
            // 
            this.pnlCopyControls.BackColor = System.Drawing.Color.Transparent;
            this.pnlCopyControls.Controls.Add(this.btnCopyDir);
            this.pnlCopyControls.Controls.Add(this.lblCopy);
            this.pnlCopyControls.Controls.Add(this.txtOutputDirectory);
            this.pnlCopyControls.Location = new System.Drawing.Point(154, 11);
            this.pnlCopyControls.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.pnlCopyControls.Name = "pnlCopyControls";
            this.pnlCopyControls.Size = new System.Drawing.Size(256, 58);
            this.pnlCopyControls.TabIndex = 2;
            this.pnlCopyControls.Visible = false;
            // 
            // btnCopyDir
            // 
            this.btnCopyDir.Location = new System.Drawing.Point(133, 32);
            this.btnCopyDir.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnCopyDir.Name = "btnCopyDir";
            this.btnCopyDir.Size = new System.Drawing.Size(121, 21);
            this.btnCopyDir.TabIndex = 3;
            this.btnCopyDir.Text = resources.GetString("SelectOutputFolderText");
            this.btnCopyDir.UseVisualStyleBackColor = true;
            this.btnCopyDir.Click += new System.EventHandler(this.btnCopyDir_Click);
            // 
            // lblCopy
            // 
            this.lblCopy.AutoSize = true;
            this.lblCopy.Location = new System.Drawing.Point(2, 0);
            this.lblCopy.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCopy.Name = "lblCopy";
            this.lblCopy.Size = new System.Drawing.Size(88, 13);
            this.lblCopy.TabIndex = 2;
            this.lblCopy.Text = resources.GetString("SelectOutputFolderLabelText");
            // 
            // txtOutputDirectory
            // 
            this.txtOutputDirectory.BackColor = System.Drawing.SystemColors.Window;
            this.txtOutputDirectory.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.txtOutputDirectory.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOutputDirectory.Location = new System.Drawing.Point(2, 15);
            this.txtOutputDirectory.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.txtOutputDirectory.Name = "txtOutputDirectory";
            this.txtOutputDirectory.ReadOnly = true;
            this.txtOutputDirectory.Size = new System.Drawing.Size(252, 19);
            this.txtOutputDirectory.TabIndex = 1;
            // 
            // rbCopy
            // 
            this.rbCopy.AutoSize = true;
            this.rbCopy.Location = new System.Drawing.Point(8, 39);
            this.rbCopy.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rbCopy.Name = "rbCopy";
            this.rbCopy.Size = new System.Drawing.Size(113, 17);
            this.rbCopy.TabIndex = 1;
            this.rbCopy.Text = resources.GetString("SaveOptionCopyText");
            this.rbCopy.UseVisualStyleBackColor = true;
            this.rbCopy.CheckedChanged += new System.EventHandler(this.rbCopy_CheckedChanged);
            // 
            // rbOverwrite
            // 
            this.rbOverwrite.AutoSize = true;
            this.rbOverwrite.Checked = true;
            this.rbOverwrite.Location = new System.Drawing.Point(8, 20);
            this.rbOverwrite.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.rbOverwrite.Name = "rbOverwrite";
            this.rbOverwrite.Size = new System.Drawing.Size(142, 17);
            this.rbOverwrite.TabIndex = 0;
            this.rbOverwrite.TabStop = true;
            this.rbOverwrite.Text = resources.GetString("SaveOptionCopyText");
            this.rbOverwrite.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(432, 243);
            this.Controls.Add(this.gbSaveOptions);
            this.Controls.Add(this.cmbFileExtension);
            this.Controls.Add(this.pbResize);
            this.Controls.Add(this.btnResize);
            this.Controls.Add(this.gbResizeOptions);
            this.Controls.Add(this.btnSelectDirectory);
            this.Controls.Add(this.lblPhotoDirectory);
            this.Controls.Add(this.txtPhotoDirectory);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Batch Resize";
            this.gbResizeOptions.ResumeLayout(false);
            this.gbResizeOptions.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudWidth)).EndInit();
            this.gbSaveOptions.ResumeLayout(false);
            this.gbSaveOptions.PerformLayout();
            this.pnlCopyControls.ResumeLayout(false);
            this.pnlCopyControls.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtPhotoDirectory;
        private System.Windows.Forms.Label lblPhotoDirectory;
        private System.Windows.Forms.Button btnSelectDirectory;
        private System.Windows.Forms.GroupBox gbResizeOptions;
        private System.Windows.Forms.Button btnResize;
        private System.Windows.Forms.Label lblNewHeight;
        private System.Windows.Forms.Label lblNewWidth;
        private Label lblGuide;
        private GroupBox gbSaveOptions;
        private Panel pnlCopyControls;
        private Label lblCopy;
        private TextBox txtOutputDirectory;
        private Button btnCopyDir;
        public ComboBox cmbFileExtension;
        public RadioButton rbCopy;
        public RadioButton rbOverwrite;
        public ProgressBar pbResize;
        public CheckBox chkMaintainAspectRatio;
        public NumericUpDown nudHeight;
        public NumericUpDown nudWidth;
    }
}

