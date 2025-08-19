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
            label1 = new Label();
            cameraBtn = new Button();
            imgPath = new Label();
            imgYoloBtn = new Button();
            cameraYoloBtn = new Button();
            label2 = new Label();
            label3 = new Label();
            SuspendLayout();
            // 
            // openBtn
            // 
            openBtn.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            openBtn.Location = new Point(44, 209);
            openBtn.Name = "openBtn";
            openBtn.Size = new Size(124, 41);
            openBtn.TabIndex = 0;
            openBtn.Text = "Open";
            openBtn.UseVisualStyleBackColor = true;
            openBtn.Click += openBtn_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label1.Location = new Point(71, 149);
            label1.Name = "label1";
            label1.Size = new Size(243, 28);
            label1.TabIndex = 1;
            label1.Text = "EmguCv + Каскады Хаара";
            // 
            // cameraBtn
            // 
            cameraBtn.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            cameraBtn.Location = new Point(204, 209);
            cameraBtn.Name = "cameraBtn";
            cameraBtn.Size = new Size(136, 41);
            cameraBtn.TabIndex = 2;
            cameraBtn.Text = "Camera";
            cameraBtn.UseVisualStyleBackColor = true;
            cameraBtn.Click += cameraBtn_Click;
            // 
            // imgPath
            // 
            imgPath.AutoSize = true;
            imgPath.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            imgPath.Location = new Point(80, 403);
            imgPath.Name = "imgPath";
            imgPath.Size = new Size(0, 28);
            imgPath.TabIndex = 3;
            // 
            // imgYoloBtn
            // 
            imgYoloBtn.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            imgYoloBtn.Location = new Point(440, 209);
            imgYoloBtn.Name = "imgYoloBtn";
            imgYoloBtn.Size = new Size(124, 41);
            imgYoloBtn.TabIndex = 4;
            imgYoloBtn.Text = "Open";
            imgYoloBtn.UseVisualStyleBackColor = true;
            imgYoloBtn.Click += imgYoloBtn_Click;
            // 
            // cameraYoloBtn
            // 
            cameraYoloBtn.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            cameraYoloBtn.Location = new Point(612, 209);
            cameraYoloBtn.Name = "cameraYoloBtn";
            cameraYoloBtn.Size = new Size(136, 41);
            cameraYoloBtn.TabIndex = 5;
            cameraYoloBtn.Text = "Camera";
            cameraYoloBtn.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label2.Location = new Point(440, 149);
            label2.Name = "label2";
            label2.Size = new Size(308, 28);
            label2.TabIndex = 6;
            label2.Text = "Emgu CV + YoloDotNet +Yolo11n";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 204);
            label3.Location = new Point(21, 403);
            label3.Name = "label3";
            label3.Size = new Size(59, 28);
            label3.TabIndex = 7;
            label3.Text = "Path: ";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(cameraYoloBtn);
            Controls.Add(imgYoloBtn);
            Controls.Add(imgPath);
            Controls.Add(cameraBtn);
            Controls.Add(label1);
            Controls.Add(openBtn);
            Name = "Form1";
            Text = "Dev";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button openBtn;
        private Label label1;
        private Button cameraBtn;
        private Label imgPath;
        private Button imgYoloBtn;
        private Button cameraYoloBtn;
        private Label label2;
        private Label label3;
    }
}
