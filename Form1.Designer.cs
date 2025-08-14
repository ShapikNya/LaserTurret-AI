namespace DevTurret
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            openBtn = new Button();
            imgPath = new Label();
<<<<<<< HEAD
            cameraBtn = new Button();
=======
>>>>>>> a20105c934463e9ad5cdeee966de2f06756b45da
            SuspendLayout();
            // 
            // openBtn
            // 
            openBtn.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            openBtn.Location = new Point(12, 397);
            openBtn.Name = "openBtn";
            openBtn.Size = new Size(124, 41);
            openBtn.TabIndex = 0;
            openBtn.Text = "Open";
            openBtn.UseVisualStyleBackColor = true;
            openBtn.Click += openBtn_Click;
            // 
            // imgPath
            // 
            imgPath.AutoSize = true;
            imgPath.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            imgPath.Location = new Point(12, 9);
            imgPath.Name = "imgPath";
            imgPath.Size = new Size(59, 28);
            imgPath.TabIndex = 1;
            imgPath.Text = "Path: ";
            // 
<<<<<<< HEAD
            // cameraBtn
            // 
            cameraBtn.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            cameraBtn.Location = new Point(142, 397);
            cameraBtn.Name = "cameraBtn";
            cameraBtn.Size = new Size(136, 41);
            cameraBtn.TabIndex = 2;
            cameraBtn.Text = "Camera";
            cameraBtn.UseVisualStyleBackColor = true;
            cameraBtn.Click += cameraBtn_Click;
            // 
=======
>>>>>>> a20105c934463e9ad5cdeee966de2f06756b45da
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
<<<<<<< HEAD
            Controls.Add(cameraBtn);
=======
>>>>>>> a20105c934463e9ad5cdeee966de2f06756b45da
            Controls.Add(imgPath);
            Controls.Add(openBtn);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button openBtn;
        private Label imgPath;
<<<<<<< HEAD
        private Button cameraBtn;
=======
>>>>>>> a20105c934463e9ad5cdeee966de2f06756b45da
    }
}
