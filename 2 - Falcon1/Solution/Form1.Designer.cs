﻿using System.Windows.Forms;

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
            this.Key3TextBox = new Keygen.LogInNormalTextBox();
            this.Key2TextBox = new Keygen.LogInNormalTextBox();
            this.KeyTextBox = new Keygen.LogInNormalTextBox();
            this.NameTextBox = new Keygen.LogInNormalTextBox();
            this.ButtonTextBox = new Keygen.LogInNormalTextBox();
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
            this.GenerateButton.Location = new System.Drawing.Point(33, 265);
            this.GenerateButton.Name = "GenerateButton";
            this.GenerateButton.Size = new System.Drawing.Size(295, 35);
            this.GenerateButton.TabIndex = 5;
            this.GenerateButton.Text = "GENERATE";
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
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Username";
            // 
            // Key3TextBox
            // 
            this.Key3TextBox.BackColor = System.Drawing.Color.Transparent;
            this.Key3TextBox.BackgroundColour = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
            this.Key3TextBox.BorderColour = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.Key3TextBox.Location = new System.Drawing.Point(249, 306);
            this.Key3TextBox.MaxLength = 32767;
            this.Key3TextBox.Multiline = false;
            this.Key3TextBox.Name = "Key3TextBox";
            this.Key3TextBox.ReadOnly = true;
            this.Key3TextBox.Size = new System.Drawing.Size(79, 29);
            this.Key3TextBox.Style = Keygen.LogInNormalTextBox.Styles.NotRounded;
            this.Key3TextBox.TabIndex = 8;
            this.Key3TextBox.Text = "---";
            this.Key3TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Key3TextBox.TextColour = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Key3TextBox.UseSystemPasswordChar = false;
            // 
            // Key2TextBox
            // 
            this.Key2TextBox.BackColor = System.Drawing.Color.Transparent;
            this.Key2TextBox.BackgroundColour = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
            this.Key2TextBox.BorderColour = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.Key2TextBox.Location = new System.Drawing.Point(98, 306);
            this.Key2TextBox.MaxLength = 32767;
            this.Key2TextBox.Multiline = false;
            this.Key2TextBox.Name = "Key2TextBox";
            this.Key2TextBox.ReadOnly = true;
            this.Key2TextBox.Size = new System.Drawing.Size(145, 29);
            this.Key2TextBox.Style = Keygen.LogInNormalTextBox.Styles.NotRounded;
            this.Key2TextBox.TabIndex = 7;
            this.Key2TextBox.Text = "---";
            this.Key2TextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.Key2TextBox.TextColour = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Key2TextBox.UseSystemPasswordChar = false;
            // 
            // KeyTextBox
            // 
            this.KeyTextBox.BackColor = System.Drawing.Color.Transparent;
            this.KeyTextBox.BackgroundColour = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
            this.KeyTextBox.BorderColour = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.KeyTextBox.Location = new System.Drawing.Point(33, 306);
            this.KeyTextBox.MaxLength = 32767;
            this.KeyTextBox.Multiline = false;
            this.KeyTextBox.Name = "KeyTextBox";
            this.KeyTextBox.ReadOnly = true;
            this.KeyTextBox.Size = new System.Drawing.Size(59, 29);
            this.KeyTextBox.Style = Keygen.LogInNormalTextBox.Styles.NotRounded;
            this.KeyTextBox.TabIndex = 4;
            this.KeyTextBox.Text = "---";
            this.KeyTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.KeyTextBox.TextColour = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.KeyTextBox.UseSystemPasswordChar = false;
            // 
            // NameTextBox
            // 
            this.NameTextBox.BackColor = System.Drawing.Color.Transparent;
            this.NameTextBox.BackgroundColour = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
            this.NameTextBox.BorderColour = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.NameTextBox.Location = new System.Drawing.Point(35, 178);
            this.NameTextBox.MaxLength = 32767;
            this.NameTextBox.Multiline = false;
            this.NameTextBox.Name = "NameTextBox";
            this.NameTextBox.ReadOnly = false;
            this.NameTextBox.Size = new System.Drawing.Size(295, 29);
            this.NameTextBox.Style = Keygen.LogInNormalTextBox.Styles.NotRounded;
            this.NameTextBox.TabIndex = 2;
            this.NameTextBox.Text = "Jack Kowalski";
            this.NameTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NameTextBox.TextColour = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.NameTextBox.UseSystemPasswordChar = false;
            // 
            // ButtonTextBox
            // 
            this.ButtonTextBox.BackColor = System.Drawing.Color.Transparent;
            this.ButtonTextBox.BackgroundColour = System.Drawing.Color.FromArgb(((int)(((byte)(42)))), ((int)(((byte)(42)))), ((int)(((byte)(42)))));
            this.ButtonTextBox.BorderColour = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.ButtonTextBox.Location = new System.Drawing.Point(33, 341);
            this.ButtonTextBox.MaxLength = 32767;
            this.ButtonTextBox.Multiline = false;
            this.ButtonTextBox.Name = "ButtonTextBox";
            this.ButtonTextBox.ReadOnly = true;
            this.ButtonTextBox.Size = new System.Drawing.Size(297, 29);
            this.ButtonTextBox.Style = Keygen.LogInNormalTextBox.Styles.NotRounded;
            this.ButtonTextBox.TabIndex = 9;
            this.ButtonTextBox.Text = "-- WAITING --";
            this.ButtonTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ButtonTextBox.TextColour = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.ButtonTextBox.UseSystemPasswordChar = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(359, 478);
            this.Controls.Add(this.ButtonTextBox);
            this.Controls.Add(this.Key3TextBox);
            this.Controls.Add(this.Key2TextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.GenerateButton);
            this.Controls.Add(this.KeyTextBox);
            this.Controls.Add(this.NameTextBox);
            this.Controls.Add(this.pictureBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Crackme 1 - Keygen";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_Closing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_Closed);
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private LogInNormalTextBox NameTextBox;
        private LogInNormalTextBox KeyTextBox;
        private Button GenerateButton;
        private Label label1;
        private LogInNormalTextBox Key2TextBox;
        private LogInNormalTextBox Key3TextBox;
        private LogInNormalTextBox ButtonTextBox;
    }
}

