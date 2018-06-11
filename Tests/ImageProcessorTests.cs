using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using BatchResize.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class ImageProcessorTests
    {
        private readonly ImageProcessor _imageProcessor;

        public ImageProcessorTests()
        {
            _imageProcessor = new ImageProcessor();
        }

        [TestMethod]
        public void GetFirstLandscapeImage()
        {
            const int expectedWidth = 1280;
            const int expectedHeight = 720;

            _imageProcessor.LoadFiles(Path.Combine(Environment.CurrentDirectory, "..\\..\\Resources"), ".png");
            var first = _imageProcessor.GetFirstLandscape();

            Assert.AreEqual(expectedWidth, first.Width);
            Assert.AreEqual(expectedHeight, first.Height);
            first.Dispose();
        }

        [TestMethod]
        public void ResizeLandscape()
        {
            const int newWidth = 1920;
            const int newHeight = 1080;

            _imageProcessor.ResizeWidth = newWidth;
            _imageProcessor.ResizeHeight = newHeight;

            var original = Image.FromFile(Path.Combine(Environment.CurrentDirectory, "..\\..\\Resources\\Test1.png"));
            var newImage = _imageProcessor.ResizeImage(original);
            original.Dispose();

            Assert.AreEqual(newWidth, newImage.Width);
            Assert.AreEqual(newHeight, newImage.Height);
            newImage.Dispose();
        }

        [TestMethod]
        public void ResizePortrait()
        {
            const int newWidth = 1920;
            const int newHeight = 1080;

            _imageProcessor.ResizeWidth = newWidth;
            _imageProcessor.ResizeHeight = newHeight;

            var original = Image.FromFile(Path.Combine(Environment.CurrentDirectory, "..\\..\\Resources\\Test2.png"));
            var newImage = _imageProcessor.ResizeImage(original);
            original.Dispose();

            // Switch newHeight/newWidth because of portrait photo
            Assert.AreEqual(newHeight, newImage.Width);
            Assert.AreEqual(newWidth, newImage.Height);
            newImage.Dispose();
        }

        [TestMethod]
        public void OverwriteImage()
        {
            const int newWidth = 1280;
            const int newHeight = 720;

            _imageProcessor.ResizeWidth = newWidth;
            _imageProcessor.ResizeHeight = newHeight;

            var outputPath = Path.Combine(Environment.CurrentDirectory, "..\\..\\Resources\\Test1.png");

            var original = Image.FromFile(outputPath);
            var newImage = _imageProcessor.ResizeImage(original);
            original.Dispose();

            // Delete before saving new (update)
            if (File.Exists(outputPath))
                File.Delete(outputPath);

            newImage.Save(outputPath, ImageFormat.Png);
            newImage.Dispose();

            Assert.IsTrue(File.Exists(outputPath));
        }

        [TestMethod]
        public void CopyImage()
        {
            const int newWidth = 1280;
            const int newHeight = 720;

            _imageProcessor.ResizeWidth = newWidth;
            _imageProcessor.ResizeHeight = newHeight;

            var original = Image.FromFile(Path.Combine(Environment.CurrentDirectory, "..\\..\\Resources\\Test2.png"));
            var newImage = _imageProcessor.ResizeImage(original);
            original.Dispose();

            var outputPath = Path.Combine(Environment.CurrentDirectory, "..\\..\\Resources\\CopyDir\\Test2.png");

            // Delete before saving new (update)
            if (File.Exists(outputPath))
                File.Delete(outputPath);

            newImage.Save(outputPath, ImageFormat.Png);
            newImage.Dispose();

            Assert.IsTrue(File.Exists(outputPath));
        }

        [TestMethod]
        public void CopyPropertiesTo()
        {
            var original = Image.FromFile(Path.Combine(Environment.CurrentDirectory, "..\\..\\Resources\\Test1.png"));
            var newImage = _imageProcessor.ResizeImage(original);

            _imageProcessor.CopyPropertiesTo(original, newImage);

            foreach (var id in original.PropertyIdList)
            {
                Assert.IsTrue(newImage.GetPropertyItem(id).Id == id);
            }
        }
    }
}
