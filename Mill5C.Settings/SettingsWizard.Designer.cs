namespace Mill5C.Settings
{
    partial class SettingsWizard
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
            this.ContentPanel = new System.Windows.Forms.Panel();
            this.CancelButton_ = new System.Windows.Forms.Button();
            this.NextFinishButton = new System.Windows.Forms.Button();
            this.BackButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // ContentPanel
            // 
            this.ContentPanel.Dock = System.Windows.Forms.DockStyle.Top;
            this.ContentPanel.Location = new System.Drawing.Point(0, 0);
            this.ContentPanel.Name = "ContentPanel";
            this.ContentPanel.Size = new System.Drawing.Size(514, 321);
            this.ContentPanel.TabIndex = 0;
            // 
            // CancelButton_
            // 
            this.CancelButton_.Location = new System.Drawing.Point(428, 327);
            this.CancelButton_.Name = "CancelButton_";
            this.CancelButton_.Size = new System.Drawing.Size(75, 23);
            this.CancelButton_.TabIndex = 1;
            this.CancelButton_.Text = "Cancel";
            this.CancelButton_.UseVisualStyleBackColor = true;
            this.CancelButton_.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // NextFinishButton
            // 
            this.NextFinishButton.Location = new System.Drawing.Point(347, 327);
            this.NextFinishButton.Name = "NextFinishButton";
            this.NextFinishButton.Size = new System.Drawing.Size(75, 23);
            this.NextFinishButton.TabIndex = 2;
            this.NextFinishButton.Text = "Next";
            this.NextFinishButton.UseVisualStyleBackColor = true;
            this.NextFinishButton.Click += new System.EventHandler(this.NextFinishButton_Click);
            // 
            // BackButton
            // 
            this.BackButton.Location = new System.Drawing.Point(266, 327);
            this.BackButton.Name = "BackButton";
            this.BackButton.Size = new System.Drawing.Size(75, 23);
            this.BackButton.TabIndex = 3;
            this.BackButton.Text = "Back";
            this.BackButton.UseVisualStyleBackColor = true;
            this.BackButton.Click += new System.EventHandler(this.BackButton_Click);
            // 
            // SettingsWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(514, 359);
            this.Controls.Add(this.BackButton);
            this.Controls.Add(this.NextFinishButton);
            this.Controls.Add(this.CancelButton_);
            this.Controls.Add(this.ContentPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SettingsWizard";
            this.Text = "Simulation Wizard";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel ContentPanel;
        private System.Windows.Forms.Button CancelButton_;
        private System.Windows.Forms.Button NextFinishButton;
        private System.Windows.Forms.Button BackButton;
    }
}