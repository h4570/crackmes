using System.Windows.Forms;

namespace Keygen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.GenerateButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.KeyTextBox = new Keygen.LogInNormalTextBox();
            this.ProcessComboBox = new Keygen.LogInComboBox();
            this.RefreshButton = new Keygen.LogInButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Black;
            this.pictureBox1.Image = global::Keygen.Properties.Resources.bg;
            this.pictureBox1.Location = new System.Drawing.Point(1, -1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(360, 480);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // GenerateButton
            // 
            this.GenerateButton.BackColor = System.Drawing.Color.Transparent;
            this.GenerateButton.BackgroundImage = global::Keygen.Properties.Resources.bg;
            this.GenerateButton.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.GenerateButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.GenerateButton.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.GenerateButton.Location = new System.Drawing.Point(33, 281);
            this.GenerateButton.Name = "GenerateButton";
            this.GenerateButton.Size = new System.Drawing.Size(295, 35);
            this.GenerateButton.TabIndex = 5;
            this.GenerateButton.Text = "GET KEY";
            this.GenerateButton.UseVisualStyleBackColor = false;
            this.GenerateButton.Click += new System.EventHandler(this.GenerateButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Black;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(158, 162);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Process";
            // 
            // KeyTextBox
            // 
            this.KeyTextBox.BackColor = System.Drawing.Color.Transparent;
            this.KeyTextBox.BackgroundColour = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
            this.KeyTextBox.BorderColour = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.KeyTextBox.Location = new System.Drawing.Point(31, 322);
            this.KeyTextBox.MaxLength = 32767;
            this.KeyTextBox.Multiline = false;
            this.KeyTextBox.Name = "KeyTextBox";
            this.KeyTextBox.ReadOnly = true;
            this.KeyTextBox.Size = new System.Drawing.Size(297, 29);
            this.KeyTextBox.Style = Keygen.LogInNormalTextBox.Styles.NotRounded;
            this.KeyTextBox.TabIndex = 9;
            this.KeyTextBox.Text = "-- WAITING --";
            this.KeyTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.KeyTextBox.TextColour = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.KeyTextBox.UseSystemPasswordChar = false;
            // 
            // ProcessComboBox
            // 
            this.ProcessComboBox.ArrowColour = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(30)))), ((int)(((byte)(30)))));
            this.ProcessComboBox.BackColor = System.Drawing.Color.Transparent;
            this.ProcessComboBox.BaseColour = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
            this.ProcessComboBox.BorderColour = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.ProcessComboBox.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.ProcessComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ProcessComboBox.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.ProcessComboBox.FontColour = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ProcessComboBox.FormattingEnabled = true;
            this.ProcessComboBox.LineColour = System.Drawing.Color.FromArgb(((int)(((byte)(23)))), ((int)(((byte)(119)))), ((int)(((byte)(151)))));
            this.ProcessComboBox.Location = new System.Drawing.Point(31, 178);
            this.ProcessComboBox.Name = "ProcessComboBox";
            this.ProcessComboBox.Size = new System.Drawing.Size(234, 26);
            this.ProcessComboBox.SqaureColour = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.ProcessComboBox.SqaureHoverColour = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.ProcessComboBox.StartIndex = 0;
            this.ProcessComboBox.TabIndex = 10;
            // 
            // RefreshButton
            // 
            this.RefreshButton.BackColor = System.Drawing.Color.Transparent;
            this.RefreshButton.BaseColour = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
            this.RefreshButton.BorderColour = System.Drawing.Color.FromArgb(((int)(((byte)(25)))), ((int)(((byte)(25)))), ((int)(((byte)(25)))));
            this.RefreshButton.FontColour = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.RefreshButton.HoverColour = System.Drawing.Color.FromArgb(((int)(((byte)(52)))), ((int)(((byte)(52)))), ((int)(((byte)(52)))));
            this.RefreshButton.Location = new System.Drawing.Point(271, 178);
            this.RefreshButton.Name = "RefreshButton";
            this.RefreshButton.PressedColour = System.Drawing.Color.FromArgb(((int)(((byte)(47)))), ((int)(((byte)(47)))), ((int)(((byte)(47)))));
            this.RefreshButton.ProgressColour = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(191)))), ((int)(((byte)(255)))));
            this.RefreshButton.Size = new System.Drawing.Size(64, 26);
            this.RefreshButton.TabIndex = 11;
            this.RefreshButton.Text = "REFRESH";
            this.RefreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(359, 478);
            this.Controls.Add(this.RefreshButton);
            this.Controls.Add(this.ProcessComboBox);
            this.Controls.Add(this.KeyTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.GenerateButton);
            this.Controls.Add(this.pictureBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Crackme 3";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_Closing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_Closed);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private Button GenerateButton;
        private Label label1;
        private LogInNormalTextBox KeyTextBox;
        private LogInComboBox ProcessComboBox;
        private LogInButton RefreshButton;
    }
}

