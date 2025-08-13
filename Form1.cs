using DevTurret.Classes;
using Emgu.CV;          
using Emgu.CV.CvEnum;  
using Emgu.CV.Structure;

namespace DevTurret
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void openBtn_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Images (*.jpg; *.png; *.bmp)|*.jpg;*.png;*.bmp|All files (*.*)|*.*";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        imgPath.Text = openFileDialog.FileName;
                        Bitmap image = new Bitmap(openFileDialog.FileName);


                        using (Mat imgMat = CvInvoke.Imread(openFileDialog.FileName))
                        {
                            Mat grayMat = new Mat();
                            CvInvoke.CvtColor(imgMat, grayMat, ColorConversion.Bgr2Gray);

                            ImageViewerForm origImg = new ImageViewerForm(image); origImg.Show();
                            ImageViewerForm grayImg = new ImageViewerForm(grayMat.ToBitmap()); grayImg.Show();

                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка: {ex.Message}");
                    }
                }
            }
        }
    }
}
