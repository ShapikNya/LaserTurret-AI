using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevTurret.Classes
{
    public class ImageViewerForm : Form
    {
        public ImageViewerForm(Bitmap image)
        {
            this.Text = "Просмотр изображения";
            this.Size = new Size(800, 600);

            PictureBox pictureBox = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom,
                Image = image
            };

            this.Controls.Add(pictureBox);
        }
    }
}
