using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using BatchResize;
using BatchResize.Util;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class ImageProcessorTests
    {
        private readonly MainForm _frmMain;
        private readonly ImageProcessor _imageProcessor;

        public ImageProcessorTests()
        {
            _frmMain = new MainForm();
            _imageProcessor = new ImageProcessor(_frmMain);
        }

        [TestMethod]
        public void ResizeLandscape()
        {
            const int newWidth = 1920;
            const int newHeight = 1080;

            _frmMain.ResizeWidth = newWidth;
            _frmMain.ResizeHeight = newHeight;

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

            _frmMain.ResizeWidth = newWidth;
            _frmMain.ResizeHeight = newHeight;

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

            _frmMain.ResizeWidth = newWidth;
            _frmMain.ResizeHeight = newHeight;

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

            _frmMain.ResizeWidth = newWidth;
            _frmMain.ResizeHeight = newHeight;

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
