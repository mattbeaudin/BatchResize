using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows.Forms;

namespace BatchResize
{
    public partial class MainForm : Form
    {
        private string _originalDirectory;
        private string[] _originalFiles;
        private Size _resizeTo;

        private const string _copyDirectory = "resized";

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _resizeTo = new Size(1280, 720);
        }

        private void OpenFolderBrowserDialog()
        {
            using (var dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.SelectedPath))
                {
                    _originalDirectory = dialog.SelectedPath;
                    //_outputDirectory = Path.Combine(_originalDirectory, "Resized");

                    txtPhotoDirectory.Text = _originalDirectory;
                    _originalFiles = Directory.GetFiles(dialog.SelectedPath, "*.jpg");

                    btnResize.Enabled = true;

                    InitializeSizeSettings();
                }
            }
        }

        private bool ProcessImages()
        {
            if (Directory.Exists(_originalDirectory))
            {
                for (var i = 0; i < _originalFiles.Length; i++)
                {
                    var original = Image.FromFile(_originalFiles[i]);
                    var newImage = ResizeImage(original);
                    original.Dispose();
                    
                    if (rbOverwrite.Checked)
                    {
                        // Delete before saving new
                        if (File.Exists(_originalFiles[i]))
                            File.Delete(_originalFiles[i]);

                        newImage.Save(_originalFiles[i], ImageFormat.Jpeg);
                    }

                    newImage.Dispose();

                    pbResize.PerformStep();
                }
            } else
            {
                return false;
            }

            return true;
        }

        private Image ResizeImage(Image image)
        {
            var newImage = new Bitmap(_resizeTo.Width, _resizeTo.Height);

            using (var graphics = Graphics.FromImage(newImage))
            {
                graphics.DrawImage(image, 0, 0, _resizeTo.Width, _resizeTo.Height);
            }

            return newImage;
        }

        private void InitializeSizeSettings()
        {
            _resizeTo = Image.FromFile(_originalFiles[0]).Size;
            nudWidth.Value = _resizeTo.Width;
            nudHeight.Value = _resizeTo.Height;
        }

        private void InitializeProgressBar()
        {
            pbResize.Value = 0;
            pbResize.Maximum = _originalFiles.Length;
            pbResize.Step = 1;
        }

        private void btnSelectDirectory_Click(object sender, EventArgs e)
        {
            OpenFolderBrowserDialog();
        }

        private void btnResize_Click(object sender, EventArgs e)
        {
            InitializeProgressBar();

            bool result = ProcessImages();

            var finalPath = _originalDirectory + ((rbCopy.Checked) ? "/" + _copyDirectory : "");

            if (result)
                MessageBox.Show("New resized photos have been saved to " + finalPath, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("I don't really know what happened...", "Something went horribly wrong.", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void nudWidth_ValueChanged(object sender, EventArgs e)
        {
            _resizeTo.Width = (int)nudWidth.Value;
        }

        private void nudHeight_ValueChanged(object sender, EventArgs e)
        {
            _resizeTo.Height = (int)nudHeight.Value;
        }
    }
}
