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

        private const string _copyDirectory = "resized";

        private bool _changeSize = false;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            nudWidth.Value = _resizeWidth;
            nudHeight.Value = _resizeHeight;
        }

        private void OpenFolderBrowserDialog()
        {
            using (var dialog = new FolderBrowserDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(dialog.SelectedPath))
                {
                    _originalDirectory = dialog.SelectedPath;

                    txtPhotoDirectory.Text = _originalDirectory;
                    _originalFiles = Directory.GetFiles(dialog.SelectedPath, "*.jpg");

                    btnResize.Enabled = true;

                    InitializeSizeSettings();
                }
            }
        }

        private bool ProcessImages(int startPos = 0)
        {
            if (startPos == 0)
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

                        if ( rbOverwrite.Checked )
                        {
                            // Delete before saving new
                            if ( File.Exists(_originalFiles[i]) )
                                File.Delete(_originalFiles[i]);

                            newImage.Save(_originalFiles[i], ImageFormat.Jpeg);
                        }

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
                DialogResult result = MessageBox.Show(ex.Message, "File in use Exception", MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);

                if ( result == DialogResult.Retry )
                    return ProcessImages();

                if ( result == DialogResult.Ignore )
                    return ProcessImages(startPos);

                return false;
            }

            return true;
        }

        private Image ResizeImage(Image image)
        {
            var newImage = new Bitmap((int)_resizeWidth, (int)_resizeHeight);

            using (var graphics = Graphics.FromImage(newImage))
            {
                graphics.DrawImage(image, 0, 0, (int)_resizeWidth, (int)_resizeHeight);
            }

            return newImage;
        }

        private void InitializeSizeSettings()
        {
            _resizeWidth = Image.FromFile(_originalFiles[0]).Size.Width;
            _resizeHeight = Image.FromFile(_originalFiles[0]).Size.Height;
            nudWidth.Value = _resizeWidth;
            nudHeight.Value = _resizeHeight;
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
            bool result = ProcessImages();

            var finalPath = _originalDirectory + (rbCopy.Checked ? "/" + _copyDirectory : "");

            if (result)
                MessageBox.Show("New resized photos have been saved to " + finalPath, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("I don't really know what happened...", "Something went horribly wrong.", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void nudWidth_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if ( chkAspectRatio.Checked && !_changeSize )
                    _changeSize = true;

                if ( _changeSize && chkAspectRatio.Checked )
                {
                    nudHeight.Value = (int) (nudWidth.Value*GetAspectRatio());
                    _changeSize = false;
                }

                _resizeWidth = (int) nudWidth.Value;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                MessageBox.Show("Value must be greater than 0.", "Value error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void nudHeight_ValueChanged(object sender, EventArgs e)
        {
            try {
                if (chkAspectRatio.Checked && !_changeSize)
                    _changeSize = true;

                if (_changeSize && chkAspectRatio.Checked)
                {
                    nudWidth.Value = (int)(nudHeight.Value / GetAspectRatio());
                    _changeSize = false;
                }

                _resizeHeight = (int)nudHeight.Value;
            }
            catch (ArgumentOutOfRangeException ex)
            {
                MessageBox.Show("Value must be greater than 0.", "Value error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private decimal GetAspectRatio()
        {
            return _resizeHeight / _resizeWidth;
        }
    }
}
