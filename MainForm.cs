using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using BatchResize.Util;

namespace BatchResize
{
    public partial class MainForm : Form
    {
        public string OriginalDirectory;
        public string[] OriginalFiles;
        public string OutputDirectory;
        private ImageProcessor _imageProcessor;

        public decimal ResizeWidth = 1920;
        public decimal ResizeHeight = 1080;

        /// <summary>
        /// Form constructor. Sets up default form selections for easier use.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            _imageProcessor = new ImageProcessor(this);

            // Load ImageExtensions into cmbFileExtensions and select the most common extension.
            foreach (var format in ImageExtensions.AvailableFormats)
                cmbFileExtension.Items.Add(format);

            cmbFileExtension.SelectedIndex = 2;
            btnSelectDirectory.Select();

            nudWidth.Value = ResizeWidth;
            nudHeight.Value = ResizeHeight;
        }

        /// <summary>
        /// Setup initial values for NumericUpDowns.
        /// Done by finding the first landscape photo and selecting it's size.
        /// </summary>
        private void InitializeResizeSettings()
        {
            var imageData = Image.FromFile(Path.Combine(OriginalDirectory, OriginalFiles[0]));

            for (var i = 1; i < OriginalFiles.Length; i++)
            {
                if ( imageData.Width > imageData.Height )
                    break;

                // Make sure to dispose so file is let go.
                imageData.Dispose();
                imageData = Image.FromFile(Path.Combine(OriginalDirectory, OriginalFiles[i]));
            }

            // Set private vars as well as controls.
            ResizeWidth = imageData.Width;
            ResizeHeight = imageData.Height;

            nudWidth.Value = ResizeWidth;
            nudHeight.Value = ResizeHeight;
            imageData.Dispose();
        }

        /// <summary>
        /// Setup progress bar with maximum amount of files.
        /// </summary>
        public void InitializeProgressBar()
        {
            pbResize.Value = 0;
            pbResize.Maximum = OriginalFiles.Length;
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
                if ( dialog.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.SelectedPath) )
                {
                    var path = dialog.SelectedPath;

                    // Only get the file name, not the directory.
                    var nameIndex = path.Length;

                    if ( !path.EndsWith(Path.DirectorySeparatorChar.ToString()) )
                        nameIndex++;


                    // TODO -> Add SearchOptions for user to select.
                    OriginalFiles = Directory.EnumerateFiles(path, "*" + cmbFileExtension.SelectedItem,
                                    SearchOption.TopDirectoryOnly)
                                    .Select(file => file.Substring(nameIndex)).ToArray();

                    if ( OriginalFiles.Length == 0 )
                    {
                        MessageBox.Show($"No files with extension '{cmbFileExtension.SelectedItem}' in directory.",
                            "File Type Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        return;
                    }

                    OriginalDirectory = path;
                    txtPhotoDirectory.Text = OriginalDirectory;
                    btnResize.Enabled = true;
                    InitializeResizeSettings();
                }
            }
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
                if (dialog.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.SelectedPath))
                {
                    // If there are files in the directory and user doesn't want to continue, don't set output directory.
                    if (Directory.GetFiles(dialog.SelectedPath).Length > 0)
                    {
                        if (MessageBox.Show(
                                 "There are files in this directory that may be overwritten. Would you like to continue?",
                                 "Files could possibly be overwritten.",
                                 MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                        {
                            return;
                        }
                    }

                    OutputDirectory = dialog.SelectedPath;
                    txtOutputDirectory.Text = OutputDirectory;
                }
            }
        }

        /// <summary>
        /// Toggles controls so user can't press anything while images are being processed.
        /// </summary>
        /// <param name="state">Whether or not controls will be enabled.</param>
        public void ToggleControls(bool state)
        {
            btnSelectDirectory.Enabled = state;
            cmbFileExtension.Enabled = state;
            rbOverwrite.Enabled = state;
            rbCopy.Enabled = state;
            chkMaintainAspectRatio.Enabled = state;
            btnResize.Enabled = state;
            nudWidth.Enabled = state;
            nudHeight.Enabled = state;
            btnCopyDir.Enabled = state;
        }

        /// <summary>
        /// Starts processing images and evaluates the results.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnResize_Click(object sender, EventArgs e)
        {
            // Run ProcessImages and grab result.
            var result = false;

            var thread = new Thread(() =>
            {
                result = _imageProcessor.ProcessImages();

                if (result)
                {
                    if (rbCopy.Checked)
                        MessageBox.Show($"New resized photos have been saved to {OutputDirectory}", "Success",
                       MessageBoxButtons.OK,
                       MessageBoxIcon.Information);
                    else
                        MessageBox.Show($"New resized photos have been saved to {OriginalDirectory}", "Success",
                       MessageBoxButtons.OK,
                       MessageBoxIcon.Information);
                }
                else
                    MessageBox.Show("Files were not changed as process was aborted.", "Something went wrong.",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            });

            thread.Start();
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
                nudHeight.Value = nudWidth.Value / GetAspectRatio();

            // Update private width variable.
            ResizeWidth = Math.Round(nudWidth.Value);
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
                nudWidth.Value = nudHeight.Value * GetAspectRatio();

            // Update private height variable.
            ResizeHeight = Math.Round(nudHeight.Value);
        }

        /// <summary>
        /// Gets aspect ratio to find new height/width.
        /// </summary>
        /// <returns>Aspect ratio</returns>
        private decimal GetAspectRatio()
        {
            return ResizeWidth / ResizeHeight;
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
    }
}
