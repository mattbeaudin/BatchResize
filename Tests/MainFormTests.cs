using System;
using System.IO;
using BatchResize;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests
{
    [TestClass]
    public class MainFormTests
    {
        private readonly MainForm _frmMain;

        public MainFormTests()
        {
            _frmMain = new MainForm();
        }

        [TestMethod]
        public void OpenFiles()
        {
            _frmMain.cmbFileExtension.SelectedIndex = _frmMain.cmbFileExtension.Items.IndexOf(".png");
            _frmMain.LoadFiles(Path.Combine(Environment.CurrentDirectory, "..\\..\\Resources"));

            Assert.IsTrue(_frmMain.ImageProcessor.ImageCount() == 2);
        }

        [TestMethod]
        public void InitializeResizeSettings()
        {
            const int initialWidth = 1920;
            const int initialHeight = 1080;

            const int actualWidth = 1280;
            const int actualHeight = 720;

            Assert.AreEqual(initialWidth, _frmMain.ImageProcessor.ResizeWidth);
            Assert.AreEqual(initialHeight, _frmMain.ImageProcessor.ResizeHeight);

            _frmMain.cmbFileExtension.SelectedIndex = _frmMain.cmbFileExtension.Items.IndexOf(".png");
            _frmMain.LoadFiles(Path.Combine(Environment.CurrentDirectory, "..\\..\\Resources"));

            _frmMain.InitializeResizeSettings();

            Assert.AreEqual(actualWidth, _frmMain.ImageProcessor.ResizeWidth);
            Assert.AreEqual(actualHeight, _frmMain.ImageProcessor.ResizeHeight);
        }

        [TestMethod]
        public void CalculateHeightUsingAspectRatio()
        {
            const int initialWidth1 = 1920;
            const int initialHeight1 = 1080;
            const int width1 = 1280;
            const int height1 = 720;

            _frmMain.nudWidth.Value = initialWidth1;
            _frmMain.nudHeight.Value = initialHeight1;

            _frmMain.chkMaintainAspectRatio.Checked = true;

            _frmMain.nudWidth.Value = width1;

            Assert.AreEqual(height1, Math.Round(_frmMain.ImageProcessor.ResizeHeight));

            const int initialWidth2 = 1600;
            const int initialHeight2 = 1200;
            const int width2 = 2200;
            const int height2 = 1650;

            _frmMain.chkMaintainAspectRatio.Checked = false;

            _frmMain.nudWidth.Value = initialWidth2;
            _frmMain.nudHeight.Value = initialHeight2;

            _frmMain.chkMaintainAspectRatio.Checked = true;

            _frmMain.nudWidth.Value = width2;

            Assert.AreEqual(height2, Math.Round(_frmMain.ImageProcessor.ResizeHeight));
        }

        [TestMethod]
        public void CalculateWidthUsingAspectRatio()
        {
            const int initialWidth1 = 1920;
            const int initialHeight1 = 1080;
            const int width1 = 1280;
            const int height1 = 720;

            _frmMain.nudWidth.Value = initialWidth1;
            _frmMain.nudHeight.Value = initialHeight1;

            _frmMain.chkMaintainAspectRatio.Checked = true;

            _frmMain.nudHeight.Value = height1;

            Assert.AreEqual(width1, Math.Round(_frmMain.ImageProcessor.ResizeWidth));

            const int initialWidth2 = 1600;
            const int initialHeight2 = 1200;
            const int width2 = 2200;
            const int height2 = 1650;

            _frmMain.chkMaintainAspectRatio.Checked = false;

            _frmMain.nudWidth.Value = initialWidth2;
            _frmMain.nudHeight.Value = initialHeight2;

            _frmMain.chkMaintainAspectRatio.Checked = true;

            _frmMain.nudHeight.Value = height2;

            Assert.AreEqual(width2, Math.Round(_frmMain.ImageProcessor.ResizeWidth));
        }
    }
}
