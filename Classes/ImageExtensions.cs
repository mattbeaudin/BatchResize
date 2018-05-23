using System.Drawing.Imaging;

namespace BatchResize.Classes
{
    public class ImageExtensions
    {
        public static string[] AvailableFormats =
        {
            ".bmp",
            ".gif",
            ".jpg",
            ".jpeg",
            ".png",
            ".tiff",
            ".wmf"
        };

        /// <summary>
        /// Finds a ImageFormat using the param ext.
        /// </summary>
        /// <param name="ext">File extension of what ImageFormat you want to return.</param>
        /// <returns>Found ImageFormat of type ext.</returns>
        public static ImageFormat GetImageFormat(string ext)
        {
            switch (ext)
            {
                case ".bmp":
                    return ImageFormat.Bmp;
                case ".gif":
                    return ImageFormat.Gif;
                case ".jpg":
                case ".jpeg":
                    return ImageFormat.Jpeg;
                case ".png":
                    return ImageFormat.Png;
                case ".tiff":
                    return ImageFormat.Tiff;
                case ".wmf":
                    return ImageFormat.Wmf;
                default:
                    return ImageFormat.Jpeg;
            }
        }
    }
}
