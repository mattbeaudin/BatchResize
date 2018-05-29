using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace BatchResize.Util
{
    class ImageProcessor
    {
        private static MainForm _frmMain;

        /// <summary>
        /// ImageProcessor constructor.
        /// </summary>
        /// <param name="frmMain">Reference to MainForm to use for UI updates.</param>
        public ImageProcessor(MainForm frmMain)
        {
            _frmMain = frmMain;
        }

        /// <summary>
        /// Goes through each file in directory and updates them with a resized version of the original.
        /// </summary>
        /// <param name="startPos">What folder in the directory to start resizing from. Useful for Retry and skipping a file that can't be loaded.</param>
        /// <returns>Whether or not the processing succeeded.</returns>
        public bool ProcessImages(int startPos = 0)
        {
            // Get ImageFormat to save new images as.
            var imageFormat = ImageFormat.Jpeg;
            _frmMain.Invoke(
                (MethodInvoker)
                (() => imageFormat = ImageExtensions.GetImageFormat((string) _frmMain.cmbFileExtension.SelectedItem)));

            // Disable controls so user can't harm the process.
            _frmMain.Invoke((MethodInvoker)(() => _frmMain.ToggleControls(false)));

            // Reset progress bar if starting from beginning.
            if (startPos == 0)
                _frmMain.Invoke((MethodInvoker)(() => _frmMain.InitializeProgressBar()));

            try
            {
                var outputDir = _frmMain.OriginalDirectory;

                var copy = false;
                _frmMain.Invoke((MethodInvoker) (() => copy = _frmMain.rbCopy.Checked));

                if (copy)
                    outputDir = _frmMain.OutputDirectory;

                if (string.IsNullOrWhiteSpace(outputDir) || !Directory.Exists(outputDir))
                {
                    MessageBox.Show("No output directory was selected. Please select one before resizing images", "Something went wrong.",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                if (Directory.Exists(_frmMain.OriginalDirectory))
                {
                    for (var i = startPos; i < _frmMain.OriginalFiles.Length; i++)
                    {
                        startPos++;
                        _frmMain.Invoke((MethodInvoker)(() => _frmMain.pbResize.PerformStep()));

                        var original = Image.FromFile(Path.Combine(_frmMain.OriginalDirectory, _frmMain.OriginalFiles[i]));
                        var newImage = ResizeImage(original);
                        original.Dispose();

                        var outputPath = Path.Combine(outputDir, _frmMain.OriginalFiles[i]);

                        // Delete before saving new (update)
                        if (File.Exists(outputPath))
                            File.Delete(outputPath);

                        newImage.Save(outputPath, ImageFormat.Jpeg);

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
                        _frmMain.Invoke((MethodInvoker) (() => _frmMain.ToggleControls(true)));
                        return false;
                }
            }

            // Turn controls back on.
            _frmMain.Invoke((MethodInvoker) (() => _frmMain.ToggleControls(true)));
            return true;
        }

        /// <summary>
        /// Determines whether image is landscape or portrait and resizes them accordingly.
        /// </summary>
        /// <param name="image">Original image to resize.</param>
        /// <returns>Properly resized image.</returns>
        public Image ResizeImage(Image image)
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
            var newImage = new Bitmap((int)_frmMain.ResizeWidth, (int)_frmMain.ResizeHeight);

            using (var graphics = Graphics.FromImage(newImage))
            {
                graphics.DrawImage(image, 0, 0, (int)_frmMain.ResizeWidth, (int)_frmMain.ResizeHeight);
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
            var newImage = new Bitmap((int)_frmMain.ResizeHeight, (int)_frmMain.ResizeWidth);

            using (var graphics = Graphics.FromImage(newImage))
            {
                graphics.DrawImage(image, 0, 0, (int)_frmMain.ResizeHeight, (int)_frmMain.ResizeWidth);
            }

            return newImage;
        }
    }
}
