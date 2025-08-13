using DevTurret.Classes;
using Emgu.CV;          
using Emgu.CV.CvEnum;  
using Emgu.CV.Structure;
using System.Drawing;
using System.Drawing.Imaging;

namespace DevTurret
{
    public partial class Form1 : Form
    {
        const string logImgPath = @"C:\Users\Shapi\Desktop\DevTurret\Image\Log";
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

                        MatBuilder builder = new MatBuilder(imgPath.Text);
                        Mat changedImg = builder.SetCanny(10,100).Build();

                        ImageViewerForm viewForm = new ImageViewerForm(changedImg.ToBitmap()); viewForm.Show();

                        changedImg.ToBitmap().Save(Path.Combine(logImgPath, "test.jpeg"), ImageFormat.Jpeg);
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
