using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;

namespace BatchResize.Util
{
    public class ImageProcessor
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
            _frmMain.BeginInvoke(
                (MethodInvoker)
                (() => imageFormat = ImageExtensions.GetImageFormat((string) _frmMain.cmbFileExtension.SelectedItem)));

            // Disable controls so user can't harm the process.
            _frmMain.BeginInvoke((MethodInvoker)(() => _frmMain.ToggleControls(false)));

            // Reset progress bar if starting from beginning.
            if (startPos == 0)
                _frmMain.BeginInvoke((MethodInvoker)(() => _frmMain.InitializeProgressBar()));

            try
            {
                var outputDir = _frmMain.OriginalDirectory;

                var copy = false;
                _frmMain.BeginInvoke((MethodInvoker) (() => copy = _frmMain.rbCopy.Checked));

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
                        // Increment startPos to update where loop will start if process fails.
                        startPos++;

                        // Update ProgressBar
                        _frmMain.BeginInvoke((MethodInvoker)(() => _frmMain.pbResize.PerformStep()));

                        // Take original image and resize it.
                        var original = Image.FromFile(Path.Combine(_frmMain.OriginalDirectory, _frmMain.OriginalFiles[i]));
                        var newImage = ResizeImage(original);
                        original.Dispose();

                        var outputPath = Path.Combine(outputDir, _frmMain.OriginalFiles[i]);

                        // Delete before saving new (update)
                        if (File.Exists(outputPath))
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
                        _frmMain.BeginInvoke((MethodInvoker) (() => _frmMain.ToggleControls(true)));
                        return false;
                }
            }

            // Turn controls back on.
            _frmMain.BeginInvoke((MethodInvoker) (() => _frmMain.ToggleControls(true)));
            return true;
        }

        /// <summary>
        /// Determines whether image is landscape or portrait and resizes them accordingly.
        /// </summary>
        /// <param name="image">Original image to resize.</param>
        /// <returns>Properly resized image.</returns>
        public Image ResizeImage(Image image)
        {
            var width = (int) Math.Round(_frmMain.ResizeWidth);
            var height = (int) Math.Round(_frmMain.ResizeHeight);

            return image.Width > image.Height ? ResizeLandscape(width, height, image) : ResizePortrait(width, height, image);
        }

        /// <summary>
        /// Resizes image with dimensions of a landscape photo.
        /// </summary>
        /// <param name="width">Width of new image</param>
        /// <param name="height">Height of new image</param>
        /// <param name="image">Original image to resize.</param>
        /// <returns>Properly resized image.</returns>
        private Image ResizeLandscape(int width, int height, Image image)
        {
            var newImage = new Bitmap(width, height);

            using (var graphics = Graphics.FromImage(newImage))
            {
                graphics.DrawImage(image, 0, 0, width, height);
            }

            return newImage;
        }

        /// <summary>
        /// Resizes image with dimensions of a portrait photo.
        /// </summary>
        /// <param name="width">Width of new image</param>
        /// <param name="height">Height of new image</param>
        /// <param name="image">Original image to resize.</param>
        /// <returns>Properly resized image.</returns>
        private Image ResizePortrait(int width, int height, Image image)
        {
            var newImage = new Bitmap(height, width);

            using (var graphics = Graphics.FromImage(newImage))
            {
                graphics.DrawImage(image, 0, 0, height, width);
            }

            return newImage;
        }
    }
}
