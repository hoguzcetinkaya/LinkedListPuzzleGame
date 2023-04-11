namespace puzzleGame
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            okey = new System.Windows.Forms.Button();
            textBox1 = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // okey
            // 
            okey.BackColor = System.Drawing.SystemColors.Window;
            okey.BackgroundImage = Properties.Resources.check_mark;
            okey.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            okey.Location = new System.Drawing.Point(209, 61);
            okey.Margin = new System.Windows.Forms.Padding(5);
            okey.Name = "okey";
            okey.Size = new System.Drawing.Size(31, 33);
            okey.TabIndex = 19;
            okey.UseVisualStyleBackColor = false;
            okey.Click += okey_Click;
            // 
            // textBox1
            // 
            textBox1.Location = new System.Drawing.Point(33, 61);
            textBox1.Name = "textBox1";
            textBox1.Size = new System.Drawing.Size(168, 33);
            textBox1.TabIndex = 20;
            textBox1.TextChanged += textBox1_TextChanged;
            textBox1.KeyDown += textBox1_KeyDown;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = System.Drawing.SystemColors.Window;
            label1.Location = new System.Drawing.Point(33, 33);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(206, 25);
            label1.TabIndex = 21;
            label1.Text = "KULLANICI ADI GİRİN";
            // 
            // Form2
            // 
            AcceptButton = okey;
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            BackgroundImage = Properties.Resources._2156727b8087808495258eda220b6b0c;
            BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            ClientSize = new System.Drawing.Size(273, 126);
            Controls.Add(label1);
            Controls.Add(textBox1);
            Controls.Add(okey);
            Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(5);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "Form2";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Hoş geldiniz";
            Load += Form2_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button okey;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
    }
}