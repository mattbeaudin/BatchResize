using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using BatchResize.Classes;

namespace BatchResize
{
    public partial class MainForm : Form
    {
        private string _originalDirectory;
        private string[] _originalFiles;
        private string _outputDirectory;

        private decimal _resizeWidth = 1920;
        private decimal _resizeHeight = 1080;

        /// <summary>
        /// Form constructor. Sets up default form selections for easier use.
        /// </summary>
        public MainForm()
        {
            InitializeComponent();

            // Load ImageExtensions into cmbFileExtensions and select the most common extension.
            foreach (var format in ImageExtensions.AvailableFormats)
                cmbFileExtension.Items.Add(format);

            cmbFileExtension.SelectedIndex = 2;
            btnSelectDirectory.Select();

            nudWidth.Value = _resizeWidth;
            nudHeight.Value = _resizeHeight;
        }

        /// <summary>
        /// Setup initial values for NumericUpDowns.
        /// Done by finding the first landscape photo and selecting it's size.
        /// </summary>
        private void InitializeResizeSettings()
        {
            var imageData = Image.FromFile(Path.Combine(_originalDirectory, _originalFiles[0]));

            for (var i = 1; i < _originalFiles.Length; i++)
            {
                if ( imageData.Width > imageData.Height )
                    break;

                // Make sure to dispose so file is let go.
                imageData.Dispose();
                imageData = Image.FromFile(Path.Combine(_originalDirectory, _originalFiles[i]));
            }

            // Set private vars as well as controls.
            _resizeWidth = imageData.Width;
            _resizeHeight = imageData.Height;

            nudWidth.Value = _resizeWidth;
            nudHeight.Value = _resizeHeight;
            imageData.Dispose();
        }

        /// <summary>
        /// Setup progress bar with maximum amount of files.
        /// </summary>
        private void InitializeProgressBar()
        {
            pbResize.Value = 0;
            pbResize.Maximum = _originalFiles.Length;
            pbResize.Step = 1;
        }

        /// <summary>
        /// Open Folder Browser Dialog so users can select a folder and what type of photo they'd like to resize.
        /// </summary>
        private void OpenFolderBrowserDialog()
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
                    _originalFiles = Directory.EnumerateFiles(path, "*" + cmbFileExtension.SelectedItem,
                                    SearchOption.TopDirectoryOnly)
                                    .Select(file => file.Substring(nameIndex)).ToArray();

                    if ( _originalFiles.Length == 0 )
                    {
                        MessageBox.Show($"No files with extension '{cmbFileExtension.SelectedItem}' in directory.",
                            "File Type Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        return;
                    }

                    _originalDirectory = path;
                    txtPhotoDirectory.Text = _originalDirectory;
                    btnResize.Enabled = true;
                    InitializeResizeSettings();
                }
            }
        }

        /// <summary>
        /// Open folder browser dialog to select an output directory.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCopyDir_Click(object sender, EventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.SelectedPath))
                {
                    // If there are files in the directory and user doesn't want to continue, don't set output directory.
                    if ( Directory.GetFiles(dialog.SelectedPath).Length > 0 )
                    {
                        if ( MessageBox.Show(
                                 "There are files in this directory that may be overwritten. Would you like to continue?",
                                 "Files could possibly be overwritten.",
                                 MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No )
                        {
                            return;
                        }
                    }

                    _outputDirectory = dialog.SelectedPath;
                    txtOutputDirectory.Text = _outputDirectory;
                }
            }
        }

        /// <summary>
        /// Goes through each file in directory and updates them with a resized version of the original.
        /// </summary>
        /// <param name="startPos">What folder in the directory to start resizing from. Useful for Retry and skipping a file that can't be loaded.</param>
        /// <returns>Whether or not the processing succeeded.</returns>
        private bool ProcessImages(int startPos = 0)
        {
            // Get ImageFormat to save new images as.
            var imageFormat = ImageExtensions.GetImageFormat((string) cmbFileExtension.SelectedItem);

            // Disable controls so user can't harm the process.
            ToggleControls(false);

            // Reset progress bar if starting from beginning.
            if ( startPos == 0 )
                InitializeProgressBar();

            try
            {
                var outputDir = _originalDirectory;

                if ( rbCopy.Checked )
                    outputDir = _outputDirectory;

                if ( string.IsNullOrWhiteSpace(outputDir) || !Directory.Exists(outputDir) )
                {
                    MessageBox.Show("No output directory was selected. Please select one before resizing images", "Something went wrong.",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                if ( Directory.Exists(_originalDirectory) )
                {
                    for (var i = startPos; i < _originalFiles.Length; i++)
                    {
                        startPos++;
                        pbResize.PerformStep();
                        var original = Image.FromFile(Path.Combine(_originalDirectory, _originalFiles[i]));
                        var newImage = ResizeImage(original);
                        original.Dispose();

                        var outputPath = Path.Combine(outputDir, _originalFiles[i]);

                        // Delete before saving new (update)
                        if ( File.Exists(outputPath) )
                            File.Delete(outputPath);

                        newImage.Save(outputPath, imageFormat);

                        newImage.Dispose();
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (IOException ex)
            {
                // Capture exception if current file is in use by another process.
                DialogResult result = MessageBox.Show($"File is being used by another process. {ex.Message}", "File in use Exception",
                    MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);

                // If user wants to retry, run ProcessImages from beginning.
                // If user wants to ignore, run ProcessImages from next file to skip current one.
                // TODO -> Find a better way to recursively call ProcessImages().
                switch (result)
                {
                    case DialogResult.Retry:
                        return ProcessImages();
                    case DialogResult.Ignore:
                        return ProcessImages(startPos);
                    default:
                        ToggleControls(true);
                        return false;
                }
            }

            // Turn controls back on.
            ToggleControls(true);
            return true;
        }

        /// <summary>
        /// Toggles controls so user can't press anything while images are being processed.
        /// </summary>
        /// <param name="state">Whether or not controls will be enabled.</param>
        private void ToggleControls(bool state)
        {
            btnSelectDirectory.Enabled = state;
            cmbFileExtension.Enabled = state;
            btnResize.Enabled = state;
            nudWidth.Enabled = state;
            nudHeight.Enabled = state;
            btnCopyDir.Enabled = state;
        }

        /// <summary>
        /// Determines whether image is landscape or portrait and resizes them accordingly.
        /// </summary>
        /// <param name="image">Original image to resize.</param>
        /// <returns>Properly resized image.</returns>
        private Image ResizeImage(Image image)
        {
            return image.Width > image.Height ? ResizeLandscape(image) : ResizePortrait(image);
        }

        /// <summary>
        /// Resizes image with dimensions of a landscape photo.
        /// </summary>
        /// <param name="image">Original image to resize.</param>
        /// <returns>Properly resized image.</returns>
        private Image ResizeLandscape(Image image)
        {
            var newImage = new Bitmap((int) _resizeWidth, (int) _resizeHeight);

            using (var graphics = Graphics.FromImage(newImage))
            {
                graphics.DrawImage(image, 0, 0, (int) _resizeWidth, (int) _resizeHeight);
            }

            return newImage;
        }

        /// <summary>
        /// Resizes image with dimensions of a portrait photo.
        /// </summary>
        /// <param name="image">Original image to resize.</param>
        /// <returns>Properly resized image.</returns>
        private Image ResizePortrait(Image image)
        {
            var newImage = new Bitmap((int) _resizeHeight, (int) _resizeWidth);

            using (var graphics = Graphics.FromImage(newImage))
            {
                graphics.DrawImage(image, 0, 0, (int) _resizeHeight, (int) _resizeWidth);
            }

            return newImage;
        }

        /// <summary>
        /// Starts processing images and evaluates the results.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnResize_Click(object sender, EventArgs e)
        {
            // Run ProcessImages and grab result.
            bool result = ProcessImages();

            if ( result )
            {
                if (rbCopy.Checked)
                    MessageBox.Show($"New resized photos have been saved to {_outputDirectory}", "Success",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Information);
                else
                    MessageBox.Show($"New resized photos have been saved to {_originalDirectory}", "Success",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Information);
            }
            else
                MessageBox.Show("Files were not changed as process was aborted.", "Something went wrong.",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

            // Reset progress bar when done.
            pbResize.Value = 0;
        }

        /// <summary>
        /// Opens folder browser dialog to select what directory to process.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectDirectory_Click(object sender, EventArgs e)
        {
            OpenFolderBrowserDialog();
        }

        /// <summary>
        /// Update _resizeWidth when nudWidth changes and update nudHeight if MaintainAspectRatio is checked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nudWidth_ValueChanged(object sender, EventArgs e)
        {
            // Update height using aspect ratio if checked.
            if ( chkMaintainAspectRatio.Checked )
                nudHeight.Value = nudWidth.Value / GetAspectRatio();

            // Update private width variable.
            _resizeWidth = Math.Round(nudWidth.Value);
        }

        /// <summary>
        /// Update _resizeHeight when nudHeight changes and update nudWidth if MaintainAspectRatio is checked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nudHeight_ValueChanged(object sender, EventArgs e)
        {
            // Update width using aspect ratio if checked.
            if ( chkMaintainAspectRatio.Checked )
                nudWidth.Value = nudHeight.Value * GetAspectRatio();

            // Update private height variable.
            _resizeHeight = Math.Round(nudHeight.Value);
        }

        /// <summary>
        /// Gets aspect ratio to find new height/width.
        /// </summary>
        /// <returns>Aspect ratio</returns>
        private decimal GetAspectRatio()
        {
            return _resizeWidth / _resizeHeight;
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
