using System;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Windows.Forms;

namespace BatchResize.Util
{
    public class ImageProcessor
    {
        public string OriginalDirectory;
        public string[] OriginalFiles;
        public string OutputDirectory;

        public decimal ResizeWidth;
        public decimal ResizeHeight;

        /// <summary>
        /// ImageProcessor constructor.
        /// </summary>
        public ImageProcessor()
        {
            ResizeWidth = 1920;
            ResizeHeight = 1080;
        }

        /// <summary>
        /// Gets first landscape image in OriginalFiles. If none are found, grab last portrait image.
        /// </summary>
        /// <returns>First landscape image or last portrait image.</returns>
        public Image GetFirstLandscape()
        {
            var imageData = Image.FromFile(Path.Combine(OriginalDirectory, OriginalFiles[0]));

            for (var i = 1; i < ImageCount(); i++)
            {
                if (imageData.Width > imageData.Height)
                    break;

                imageData.Dispose();
                imageData = Image.FromFile(Path.Combine(OriginalDirectory, OriginalFiles[i]));
            }

            return imageData;
        }

        /// <summary>
        /// Loads files into OriginalFiles after path is selected.
        /// </summary>
        /// <param name="path">Path of the directory to take files from.</param>
        /// <param name="ext">File extension to grab files with.</param>
        public void LoadFiles(string path, string ext)
        {
            // Only get the file name, not the directory.
            var nameIndex = path.Length;

            if (!path.EndsWith(Path.DirectorySeparatorChar.ToString()))
                nameIndex++;

            // TODO -> Add SearchOptions for user to select.
            OriginalFiles = Directory.EnumerateFiles(path, "*" + ext,
                    SearchOption.TopDirectoryOnly)
                .Select(file => file.Substring(nameIndex)).ToArray();

            if (OriginalFiles.Length == 0)
            {
                MessageBox.Show($"No files with extension '{ext}' in directory.",
                    "File Type Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            OriginalDirectory = path;
        }

        /// <summary>
        /// Gets the number of images that have been loaded in.
        /// </summary>
        /// <returns>Number of loaded images.</returns>
        public int ImageCount()
        {
            return OriginalFiles.Length;
        }

        /// <summary>
        /// Runs resize process on given image.
        /// </summary>
        /// <param name="index">Index of image to resize.</param>
        /// <param name="outputDir">Directory to output image to.</param>
        /// <param name="imageFormat">Image format of new image.</param>
        /// <returns>If process succeeded or not.</returns>
        public bool ProcessImage(int index, string outputDir, ImageFormat imageFormat)
        {
            try
            {
                var original = Image.FromFile(Path.Combine(OriginalDirectory, OriginalFiles[index]));
                var newImage = ResizeImage(original);

                CopyPropertiesTo(original, newImage);

                original.Dispose();

                var outputPath = Path.Combine(outputDir, OriginalFiles[index]);

                // Delete before saving new (update)
                if ( File.Exists(outputPath) )
                {
                    File.Delete(outputPath);
                }

                newImage.Save(outputPath, imageFormat);

                newImage.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }

            return true;
        }

        public void CopyPropertiesTo(Image original, Image newImage)
        {
            // Copy properties from original file into new file
            foreach (var property in original.PropertyItems)
            {
                newImage.SetPropertyItem(property);
            }
        }

        /// <summary>
        /// Determines whether image is landscape or portrait and resizes them accordingly.
        /// </summary>
        /// <param name="image">Original image to resize.</param>
        /// <returns>Properly resized image.</returns>
        public Image ResizeImage(Image image)
        {
            var width = (int) Math.Round(ResizeWidth);
            var height = (int) Math.Round(ResizeHeight);

            return image.Width > image.Height ? DoResize(width, height, image) : DoResize(height, width, image);
        }

        /// <summary>
        /// Resizes image with new dimensions width and height.
        /// To resize a landscape photo, send parameters normally.
        /// To resize a portrait photo, send resize height as param width and resize width as param height.
        /// </summary>
        /// <param name="width">Width of new image</param>
        /// <param name="height">Height of new image</param>
        /// <param name="image">Original image to resize.</param>
        /// <returns>Properly resized image.</returns>
        private Image DoResize(int width, int height, Image image)
        {
            var newImage = new Bitmap(width, height);

            using (var graphics = Graphics.FromImage(newImage))
            {
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                graphics.CompositingQuality = CompositingQuality.HighQuality;

                graphics.DrawImage(image, 0, 0, width, height);
            }

            return newImage;
        }

        /// <summary>
        /// Gets aspect ratio to find new height/width.
        /// </summary>
        /// <returns>Aspect ratio</returns>
        public decimal GetAspectRatio()
        {
            return ResizeWidth / ResizeHeight;
        }
    }
}
