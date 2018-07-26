using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using BatchResize.Properties;
using BatchResize.Util;

namespace BatchResize
{
    public partial class MainForm : Form
    {
        public readonly ImageProcessor ImageProcessor;
        private bool _themeSet;

        /// <summary>
        /// Form constructor. Sets up default form selections for easier use.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            ImageProcessor = new ImageProcessor();

            // Load ImageExtensions into cmbFileExtensions and select the most common extension.
            foreach (var format in ImageExtensions.AvailableFormats)
            {
                cmbFileExtension.Items.Add(format);
            }

            // Select most common use case (jpg)
            cmbFileExtension.SelectedIndex = 2;

            // Focus on button instead of textbox
            btnSelectDirectory.Select();

            nudWidth.Value = ImageProcessor.ResizeWidth;
            nudHeight.Value = ImageProcessor.ResizeHeight;

            LoadThemeSettings();
            SetTheme();
        }

        /// <summary>
        /// Setup initial values for NumericUpDowns.
        /// Done by finding the first landscape photo and selecting it's size.
        /// </summary>
        public void InitializeResizeSettings()
        {
            var imageData = ImageProcessor.GetFirstLandscape();

            // Set private vars as well as controls.
            ImageProcessor.ResizeWidth = imageData.Width;
            ImageProcessor.ResizeHeight = imageData.Height;

            nudWidth.Value = ImageProcessor.ResizeWidth;
            nudHeight.Value = ImageProcessor.ResizeHeight;
            imageData.Dispose();
        }

        /// <summary>
        /// Setup progress bar with maximum amount of files.
        /// </summary>
        public void InitializeProgressBar()
        {
            pbResize.Value = 0;
            pbResize.Maximum = ImageProcessor.OriginalFiles.Length;
            pbResize.Step = 1;
        }

        /// <summary>
        /// Button event that fires OpenSelectDirectoryDialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectDirectory_Click(object sender, EventArgs e)
        {
            OpenSelectDirectoryDialog();
        }

        /// <summary>
        /// Open Folder Browser Dialog so users can select a folder and what type of photo they'd like to resize.
        /// </summary>
        private void OpenSelectDirectoryDialog()
        {
            using (var dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() != DialogResult.OK || string.IsNullOrWhiteSpace(dialog.SelectedPath)) return;

                if (LoadFiles(dialog.SelectedPath))
                {
                    InitializeResizeSettings();
                }
            }
        }

        /// <summary>
        /// Loads files into OriginalFiles after path is selected.
        /// </summary>
        /// <param name="path">Path of the directory to take files from.</param>
        public bool LoadFiles(string path)
        {
            if (!ImageProcessor.LoadFiles(path, (string)cmbFileExtension.SelectedItem)) return false;

            txtPhotoDirectory.Text = ImageProcessor.OriginalDirectory;
            btnResize.Enabled = true;
            return true;
        }

        /// <summary>
        /// Button event that fires OpenCopyDirectoryDialog
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCopyDir_Click(object sender, EventArgs e)
        {
            OpenCopyDirectoryDialog();
        }

        /// <summary>
        /// Open Folder Browser Dialog so users can select where to copy their resized images to.
        /// </summary>
        private void OpenCopyDirectoryDialog()
        {
            using (var dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() != DialogResult.OK || string.IsNullOrWhiteSpace(dialog.SelectedPath)) return;

                // If there are files in the directory and user doesn't want to continue, don't set output directory.
                if (Directory.GetFiles(dialog.SelectedPath).Length > 0)
                {
                    if (MessageBox.Show(
                            Resources.OverwriteFilesWarning,
                            Resources.OverwriteFilesWarningTitle,
                            MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    {
                        return;
                    }
                }

                ImageProcessor.OutputDirectory = dialog.SelectedPath;
                txtOutputDirectory.Text = ImageProcessor.OutputDirectory;
            }
        }

        /// <summary>
        /// Toggles controls so user can't press anything while images are being processed.
        /// </summary>
        /// <param name="state">Whether or not controls will be enabled.</param>
        public void ToggleControls(bool state)
        {
            gbResizeOptions.Enabled = state;
            gbSaveOptions.Enabled = state;

            btnSelectDirectory.Enabled = state;
            cmbFileExtension.Enabled = state;
        }

        /// <summary>
        /// Starts processing images and evaluates the results.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnResize_Click(object sender, EventArgs e)
        {
            ProcessImages();
        }

        /// <summary>
        /// Goes through each file in directory and updates them with a resized version of the original.
        /// </summary>
        public void ProcessImages()
        {
            var imageFormat = ImageExtensions.GetImageFormat((string)cmbFileExtension.SelectedItem);

            ToggleControls(false);

            InitializeProgressBar();

            var outputDir = ImageProcessor.OriginalDirectory;

            if (rbCopy.Checked)
            {
                outputDir = ImageProcessor.OutputDirectory;
            }

            if ( string.IsNullOrWhiteSpace(outputDir) || !Directory.Exists(outputDir) )
            {
                MessageBox.Show(Resources.ErrorNoOutputSelected, Resources.ErrorGenericTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!Directory.Exists(ImageProcessor.OriginalDirectory)) return;

            var worker = new BackgroundWorker();

            // Setup worker task.
            worker.DoWork += (sender, args) =>
            {
                for (var i = 0; i < ImageProcessor.ImageCount(); i++)
                {
                    ImageProcessor.ProcessImage(i, outputDir, imageFormat);
                    Invoke((MethodInvoker)(() => pbResize.PerformStep()));
                }
            };

            // Setup function that runs when worker finishes.
            worker.RunWorkerCompleted += (sender, args) =>
            {
                MessageBox.Show(
                    rbCopy.Checked
                        ? string.Format(Resources.InfoImagesSavedTo, ImageProcessor.OutputDirectory)
                        : string.Format(Resources.InfoImagesSavedTo, ImageProcessor.OriginalDirectory), Resources.SuccessGenericTitle,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);

                ToggleControls(true);
            };

            worker.RunWorkerAsync();
        }

        /// <summary>
        /// Update ResizeWidth when nudWidth changes and update nudHeight if MaintainAspectRatio is checked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nudWidth_ValueChanged(object sender, EventArgs e)
        {
            // Update height using aspect ratio if checked.
            if ( chkMaintainAspectRatio.Checked )
                nudHeight.Value = nudWidth.Value / ImageProcessor.GetAspectRatio();

            // Update private width variable.
            ImageProcessor.ResizeWidth = nudWidth.Value;
        }

        /// <summary>
        /// Update ResizeHeight when nudHeight changes and update nudWidth if MaintainAspectRatio is checked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nudHeight_ValueChanged(object sender, EventArgs e)
        {
            // Update width using aspect ratio if checked.
            if ( chkMaintainAspectRatio.Checked )
                nudWidth.Value = nudHeight.Value * ImageProcessor.GetAspectRatio();

            // Update private height variable.
            ImageProcessor.ResizeHeight = nudHeight.Value;
        }

        /// <summary>
        /// Toggle the visibility of pnlCopyControls.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbCopy_CheckedChanged(object sender, EventArgs e)
        {
            pnlCopyControls.Visible = rbCopy.Checked;
        }

        private void LoadThemeSettings()
        {
            _themeSet = Settings.Default.DarkModeOn;
        }

        /// <summary>
        /// Toggles theme between dark and light.
        /// </summary>
        private void SetTheme()
        {
            var backColor = Color.WhiteSmoke;
            var foreColor = Color.Black;

            if ( _themeSet )
            {
                backColor = Settings.Default.DarkModeBackColor;
                foreColor = Settings.Default.DarkModeForeColor;
            }

            BackColor = backColor;
            ForeColor = foreColor;
            gbResizeOptions.ForeColor = foreColor;
            gbSaveOptions.ForeColor = foreColor;

            btnCopyDir.ForeColor = Color.Black;
            btnResize.ForeColor = Color.Black;
            btnSelectDirectory.ForeColor = Color.Black;
        }

        /// <inheritdoc />
        /// <summary>
        /// Process key presses
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns>Whether or not key press was processed correctly.</returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData != Settings.Default.DarkModeToggleKeys) return base.ProcessCmdKey(ref msg, keyData);

            _themeSet = !_themeSet;
            Settings.Default.DarkModeOn = _themeSet;
            Settings.Default.Save();

            SetTheme();
            return true;

        }
    }
}
