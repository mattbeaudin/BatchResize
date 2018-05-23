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

        private decimal _resizeWidth = 1920;
        private decimal _resizeHeight = 1080;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            nudWidth.Value = _resizeWidth;
            nudHeight.Value = _resizeHeight;
            cmbFileExtension.SelectedIndex = 4;
        }

        private void InitializeResizeSettings()
        {
            var imageData = Image.FromFile(_originalFiles[0]);

            for (var i = 1; i < _originalFiles.Length; i++)
            {
                if ( imageData.Width > imageData.Height )
                    break;

                imageData.Dispose();
                imageData = Image.FromFile(_originalFiles[i]);
            }

            _resizeWidth = imageData.Width;
            _resizeHeight = imageData.Height;

            nudWidth.Value = _resizeWidth;
            nudHeight.Value = _resizeHeight;
            imageData.Dispose();
        }

        private void InitializeProgressBar()
        {
            pbResize.Value = 0;
            pbResize.Maximum = _originalFiles.Length;
            pbResize.Step = 1;
        }

        private void OpenFolderBrowserDialog()
        {
            using (var dialog = new FolderBrowserDialog())
            {
                if ( dialog.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.SelectedPath) )
                {
                    _originalDirectory = dialog.SelectedPath;

                    txtPhotoDirectory.Text = _originalDirectory;
                    _originalFiles = Directory.GetFiles(dialog.SelectedPath, "*" + cmbFileExtension.SelectedItem);

                    if ( _originalFiles.Length == 0 )
                    {
                        MessageBox.Show($"No files with extension '{cmbFileExtension.SelectedItem}' in directory.",
                            "File Type Exception", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        return;
                    }

                    btnResize.Enabled = true;
                    InitializeResizeSettings();
                }
            }
        }

        private bool ProcessImages(int startPos = 0)
        {
            ToggleControls(false);

            if ( startPos == 0 )
                InitializeProgressBar();

            try
            {
                if ( Directory.Exists(_originalDirectory) )
                {
                    for (var i = startPos; i < _originalFiles.Length; i++)
                    {
                        startPos++;
                        pbResize.PerformStep();
                        var original = Image.FromFile(_originalFiles[i]);
                        var newImage = ResizeImage(original);
                        original.Dispose();

                        // Delete before saving new
                        if ( File.Exists(_originalFiles[i]) )
                            File.Delete(_originalFiles[i]);

                        newImage.Save(_originalFiles[i], ImageFormat.Jpeg);

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
                DialogResult result = MessageBox.Show(ex.Message, "File in use Exception",
                    MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);

                switch (result)
                {
                    case DialogResult.Retry:
                        return ProcessImages();
                    case DialogResult.Ignore:
                        return ProcessImages(startPos);
                    default:
                        return false;
                }
            }

            ToggleControls(true);
            return true;
        }

        private void ToggleControls(bool state)
        {
            btnSelectDirectory.Enabled = state;
            cmbFileExtension.Enabled = state;
            btnResize.Enabled = state;
            nudWidth.Enabled = state;
            nudHeight.Enabled = state;
        }

        private Image ResizeImage(Image image)
        {
            return image.Width > image.Height ? ResizeLandscape(image) : ResizePortrait(image);
        }

        private Image ResizeLandscape(Image image)
        {
            var newImage = new Bitmap((int) _resizeWidth, (int) _resizeHeight);

            using (var graphics = Graphics.FromImage(newImage))
            {
                graphics.DrawImage(image, 0, 0, (int)_resizeWidth, (int)_resizeHeight);
            }

            return newImage;
        }

        private Image ResizePortrait(Image image)
        {
            var newImage = new Bitmap((int)_resizeHeight, (int)_resizeWidth);

            using (var graphics = Graphics.FromImage(newImage))
            {
                graphics.DrawImage(image, 0, 0, (int)_resizeHeight, (int)_resizeWidth);
            }

            return newImage;
        }

        private void btnSelectDirectory_Click(object sender, EventArgs e)
        {
            OpenFolderBrowserDialog();
        }

        private void btnResize_Click(object sender, EventArgs e)
        {
            bool result = ProcessImages();

            if ( result )
                MessageBox.Show("New resized photos have been saved to " + _originalDirectory, "Success", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            else
                MessageBox.Show("Files were not changed as process was aborted.", "Something went wrong.",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

            pbResize.Value = 0;
        }

        private void nudWidth_ValueChanged(object sender, EventArgs e)
        {
            _resizeWidth = (int) nudWidth.Value;
        }

        private void nudHeight_ValueChanged(object sender, EventArgs e)
        {
            _resizeHeight = (int) nudHeight.Value;
        }
    }
}
