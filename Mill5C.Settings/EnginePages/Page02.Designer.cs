namespace Mill5C.Settings.EnginePages
{
    partial class Page02
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.saveFileDialogOutput = new System.Windows.Forms.SaveFileDialog();
            this.groupBoxOutput = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SelectPathButton = new System.Windows.Forms.Button();
            this.PathTextBox = new System.Windows.Forms.TextBox();
            this.groupBoxOutput.SuspendLayout();
            this.SuspendLayout();
            // 
            // saveFileDialogOutput
            // 
            this.saveFileDialogOutput.DefaultExt = "*.mat";
            this.saveFileDialogOutput.Filter = "Material files (*.mat)|*.mat";
            this.saveFileDialogOutput.Title = "Save material";
            // 
            // groupBoxOutput
            // 
            this.groupBoxOutput.Controls.Add(this.label1);
            this.groupBoxOutput.Controls.Add(this.SelectPathButton);
            this.groupBoxOutput.Controls.Add(this.PathTextBox);
            this.groupBoxOutput.Location = new System.Drawing.Point(3, 3);
            this.groupBoxOutput.Name = "groupBoxOutput";
            this.groupBoxOutput.Size = new System.Drawing.Size(509, 314);
            this.groupBoxOutput.TabIndex = 0;
            this.groupBoxOutput.TabStop = false;
            this.groupBoxOutput.Text = "Output";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(361, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Select output file or leave empty if you don\'t want to save the result material";
            // 
            // SelectPathButton
            // 
            this.SelectPathButton.Location = new System.Drawing.Point(428, 39);
            this.SelectPathButton.Name = "SelectPathButton";
            this.SelectPathButton.Size = new System.Drawing.Size(75, 23);
            this.SelectPathButton.TabIndex = 2;
            this.SelectPathButton.Text = "...";
            this.SelectPathButton.UseVisualStyleBackColor = true;
            this.SelectPathButton.Click += new System.EventHandler(this.SelectPathButton_Click);
            // 
            // PathTextBox
            // 
            this.PathTextBox.Location = new System.Drawing.Point(6, 41);
            this.PathTextBox.Name = "PathTextBox";
            this.PathTextBox.Size = new System.Drawing.Size(416, 20);
            this.PathTextBox.TabIndex = 0;
            // 
            // Page02
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBoxOutput);
            this.Name = "Page02";
            this.Size = new System.Drawing.Size(515, 320);
            this.groupBoxOutput.ResumeLayout(false);
            this.groupBoxOutput.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SaveFileDialog saveFileDialogOutput;
        private System.Windows.Forms.GroupBox groupBoxOutput;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button SelectPathButton;
        private System.Windows.Forms.TextBox PathTextBox;
    }
}
