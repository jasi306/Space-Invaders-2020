namespace Space_Invaders
{
    partial class Form1
    {
        /// <summary>
        /// Wymagana zmienna projektanta.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Wyczyść wszystkie używane zasoby.
        /// </summary>
        /// <param name="disposing">prawda, jeżeli zarządzane zasoby powinny zostać zlikwidowane; Fałsz w przeciwnym wypadku.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Kod generowany przez Projektanta formularzy systemu Windows

        /// <summary>
        /// Metoda wymagana do obsługi projektanta — nie należy modyfikować
        /// jej zawartości w edytorze kodu.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.gameTimer = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.logoBox = new System.Windows.Forms.PictureBox();
            this.OnePlayerButton = new System.Windows.Forms.Button();
            this.TwoPlayersButton = new System.Windows.Forms.Button();
            this.ScoreButton = new System.Windows.Forms.Button();
            this.ExitButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.logoBox)).BeginInit();
            this.SuspendLayout();
            // 
            // gameTimer
            // 
            this.gameTimer.Enabled = true;
            this.gameTimer.Tick += new System.EventHandler(this.gameTimer_Tick);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("MS Gothic", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label1.Location = new System.Drawing.Point(704, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 19);
            this.label1.TabIndex = 0;
            this.label1.Tag = "label1";
            this.label1.Text = "Score: 0";
            this.label1.UseCompatibleTextRendering = true;
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // logoBox
            // 
            this.logoBox.Image = ((System.Drawing.Image)(resources.GetObject("logoBox.Image")));
            this.logoBox.Location = new System.Drawing.Point(12, 11);
            this.logoBox.Name = "logoBox";
            this.logoBox.Size = new System.Drawing.Size(354, 246);
            this.logoBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.logoBox.TabIndex = 1;
            this.logoBox.TabStop = false;
            this.logoBox.Click += new System.EventHandler(this.logoBox_Click);
            // 
            // OnePlayerButton
            // 
            this.OnePlayerButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(213)))), ((int)(((byte)(104)))));
            this.OnePlayerButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 17F);
            this.OnePlayerButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.OnePlayerButton.Location = new System.Drawing.Point(12, 263);
            this.OnePlayerButton.Name = "OnePlayerButton";
            this.OnePlayerButton.Size = new System.Drawing.Size(354, 58);
            this.OnePlayerButton.TabIndex = 2;
            this.OnePlayerButton.Text = "Jeden gracz";
            this.OnePlayerButton.UseVisualStyleBackColor = false;
            this.OnePlayerButton.Click += new System.EventHandler(this.OnePlayerButton_Click);
            // 
            // TwoPlayersButton
            // 
            this.TwoPlayersButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(213)))), ((int)(((byte)(104)))));
            this.TwoPlayersButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 17F);
            this.TwoPlayersButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.TwoPlayersButton.Location = new System.Drawing.Point(12, 287);
            this.TwoPlayersButton.Name = "TwoPlayersButton";
            this.TwoPlayersButton.Size = new System.Drawing.Size(354, 68);
            this.TwoPlayersButton.TabIndex = 3;
            this.TwoPlayersButton.Text = "Dwóch graczy";
            this.TwoPlayersButton.UseVisualStyleBackColor = false;
            this.TwoPlayersButton.Click += new System.EventHandler(this.TwoPlayersButton_Click);
            // 
            // ScoreButton
            // 
            this.ScoreButton.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.ScoreButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(213)))), ((int)(((byte)(104)))));
            this.ScoreButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 17F);
            this.ScoreButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ScoreButton.Location = new System.Drawing.Point(24, 317);
            this.ScoreButton.Name = "ScoreButton";
            this.ScoreButton.Size = new System.Drawing.Size(354, 58);
            this.ScoreButton.TabIndex = 4;
            this.ScoreButton.Text = "Tabela wyników";
            this.ScoreButton.UseVisualStyleBackColor = false;
            // 
            // ExitButton
            // 
            this.ExitButton.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.ExitButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(213)))), ((int)(((byte)(104)))));
            this.ExitButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 17F);
            this.ExitButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ExitButton.Location = new System.Drawing.Point(24, 361);
            this.ExitButton.Name = "ExitButton";
            this.ExitButton.Size = new System.Drawing.Size(354, 58);
            this.ExitButton.TabIndex = 5;
            this.ExitButton.Text = "Wyjdź";
            this.ExitButton.UseVisualStyleBackColor = false;
            this.ExitButton.Click += new System.EventHandler(this.ExitButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(199)))), ((int)(((byte)(213)))), ((int)(((byte)(104)))));
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(782, 553);
            this.Controls.Add(this.ExitButton);
            this.Controls.Add(this.ScoreButton);
            this.Controls.Add(this.TwoPlayersButton);
            this.Controls.Add(this.OnePlayerButton);
            this.Controls.Add(this.logoBox);
            this.Controls.Add(this.label1);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Form1";
            this.Text = "Space Invaders";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.logoBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Timer gameTimer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox logoBox;
        private System.Windows.Forms.Button OnePlayerButton;
        private System.Windows.Forms.Button TwoPlayersButton;
        private System.Windows.Forms.Button ScoreButton;
        private System.Windows.Forms.Button ExitButton;
    }
}

