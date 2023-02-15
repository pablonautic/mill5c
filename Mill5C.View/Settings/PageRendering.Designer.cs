namespace Mill5C.View.Window.Settings
{
    partial class PageRendering
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.NoneMatRadioButton = new System.Windows.Forms.RadioButton();
            this.PostSimulationMatRadioButton = new System.Windows.Forms.RadioButton();
            this.RealTimeMatRadioButton = new System.Windows.Forms.RadioButton();
            this.PathRendererGroupBox = new System.Windows.Forms.GroupBox();
            this.NonePathRadioButton = new System.Windows.Forms.RadioButton();
            this.StandardPathRadioButton = new System.Windows.Forms.RadioButton();
            this.CutterRendererGroupBox = new System.Windows.Forms.GroupBox();
            this.NoneCutterRadioButton = new System.Windows.Forms.RadioButton();
            this.StandardCutterRadioButton = new System.Windows.Forms.RadioButton();
            this.CubesRadioButton = new System.Windows.Forms.RadioButton();
            this.SpheresRadioButton = new System.Windows.Forms.RadioButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1.SuspendLayout();
            this.PathRendererGroupBox.SuspendLayout();
            this.CutterRendererGroupBox.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Controls.Add(this.NoneMatRadioButton);
            this.groupBox1.Controls.Add(this.PostSimulationMatRadioButton);
            this.groupBox1.Controls.Add(this.RealTimeMatRadioButton);
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(250, 150);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Material Renderer";
            // 
            // NoneMatRadioButton
            // 
            this.NoneMatRadioButton.AutoSize = true;
            this.NoneMatRadioButton.Location = new System.Drawing.Point(6, 65);
            this.NoneMatRadioButton.Name = "NoneMatRadioButton";
            this.NoneMatRadioButton.Size = new System.Drawing.Size(51, 17);
            this.NoneMatRadioButton.TabIndex = 0;
            this.NoneMatRadioButton.Text = "None";
            this.NoneMatRadioButton.UseVisualStyleBackColor = true;
            // 
            // PostSimulationMatRadioButton
            // 
            this.PostSimulationMatRadioButton.AutoSize = true;
            this.PostSimulationMatRadioButton.Checked = true;
            this.PostSimulationMatRadioButton.Location = new System.Drawing.Point(6, 19);
            this.PostSimulationMatRadioButton.Name = "PostSimulationMatRadioButton";
            this.PostSimulationMatRadioButton.Size = new System.Drawing.Size(97, 17);
            this.PostSimulationMatRadioButton.TabIndex = 1;
            this.PostSimulationMatRadioButton.TabStop = true;
            this.PostSimulationMatRadioButton.Text = "Post Simulation";
            this.PostSimulationMatRadioButton.UseVisualStyleBackColor = true;
            // 
            // RealTimeMatRadioButton
            // 
            this.RealTimeMatRadioButton.AutoSize = true;
            this.RealTimeMatRadioButton.Location = new System.Drawing.Point(6, 42);
            this.RealTimeMatRadioButton.Name = "RealTimeMatRadioButton";
            this.RealTimeMatRadioButton.Size = new System.Drawing.Size(73, 17);
            this.RealTimeMatRadioButton.TabIndex = 0;
            this.RealTimeMatRadioButton.Text = "Real Time";
            this.RealTimeMatRadioButton.UseVisualStyleBackColor = true;
            // 
            // PathRendererGroupBox
            // 
            this.PathRendererGroupBox.Controls.Add(this.NonePathRadioButton);
            this.PathRendererGroupBox.Controls.Add(this.StandardPathRadioButton);
            this.PathRendererGroupBox.Location = new System.Drawing.Point(259, 3);
            this.PathRendererGroupBox.Name = "PathRendererGroupBox";
            this.PathRendererGroupBox.Size = new System.Drawing.Size(250, 150);
            this.PathRendererGroupBox.TabIndex = 1;
            this.PathRendererGroupBox.TabStop = false;
            this.PathRendererGroupBox.Text = "Path Renderer";
            // 
            // NonePathRadioButton
            // 
            this.NonePathRadioButton.AutoSize = true;
            this.NonePathRadioButton.Location = new System.Drawing.Point(7, 43);
            this.NonePathRadioButton.Name = "NonePathRadioButton";
            this.NonePathRadioButton.Size = new System.Drawing.Size(51, 17);
            this.NonePathRadioButton.TabIndex = 1;
            this.NonePathRadioButton.TabStop = true;
            this.NonePathRadioButton.Text = "None";
            this.NonePathRadioButton.UseVisualStyleBackColor = true;
            // 
            // StandardPathRadioButton
            // 
            this.StandardPathRadioButton.AutoSize = true;
            this.StandardPathRadioButton.Checked = true;
            this.StandardPathRadioButton.Location = new System.Drawing.Point(7, 19);
            this.StandardPathRadioButton.Name = "StandardPathRadioButton";
            this.StandardPathRadioButton.Size = new System.Drawing.Size(68, 17);
            this.StandardPathRadioButton.TabIndex = 0;
            this.StandardPathRadioButton.TabStop = true;
            this.StandardPathRadioButton.Text = "Standard";
            this.StandardPathRadioButton.UseVisualStyleBackColor = true;
            // 
            // CutterRendererGroupBox
            // 
            this.CutterRendererGroupBox.Controls.Add(this.NoneCutterRadioButton);
            this.CutterRendererGroupBox.Controls.Add(this.StandardCutterRadioButton);
            this.CutterRendererGroupBox.Location = new System.Drawing.Point(3, 159);
            this.CutterRendererGroupBox.Name = "CutterRendererGroupBox";
            this.CutterRendererGroupBox.Size = new System.Drawing.Size(250, 150);
            this.CutterRendererGroupBox.TabIndex = 2;
            this.CutterRendererGroupBox.TabStop = false;
            this.CutterRendererGroupBox.Text = "Cutter Renderer";
            // 
            // NoneCutterRadioButton
            // 
            this.NoneCutterRadioButton.AutoSize = true;
            this.NoneCutterRadioButton.Location = new System.Drawing.Point(7, 44);
            this.NoneCutterRadioButton.Name = "NoneCutterRadioButton";
            this.NoneCutterRadioButton.Size = new System.Drawing.Size(51, 17);
            this.NoneCutterRadioButton.TabIndex = 1;
            this.NoneCutterRadioButton.Text = "None";
            this.NoneCutterRadioButton.UseVisualStyleBackColor = true;
            // 
            // StandardCutterRadioButton
            // 
            this.StandardCutterRadioButton.AutoSize = true;
            this.StandardCutterRadioButton.Checked = true;
            this.StandardCutterRadioButton.Location = new System.Drawing.Point(7, 20);
            this.StandardCutterRadioButton.Name = "StandardCutterRadioButton";
            this.StandardCutterRadioButton.Size = new System.Drawing.Size(68, 17);
            this.StandardCutterRadioButton.TabIndex = 0;
            this.StandardCutterRadioButton.TabStop = true;
            this.StandardCutterRadioButton.Text = "Standard";
            this.StandardCutterRadioButton.UseVisualStyleBackColor = true;
            // 
            // CubesRadioButton
            // 
            this.CubesRadioButton.AutoSize = true;
            this.CubesRadioButton.Checked = true;
            this.CubesRadioButton.Location = new System.Drawing.Point(3, 3);
            this.CubesRadioButton.Name = "CubesRadioButton";
            this.CubesRadioButton.Size = new System.Drawing.Size(55, 17);
            this.CubesRadioButton.TabIndex = 2;
            this.CubesRadioButton.TabStop = true;
            this.CubesRadioButton.Text = "Cubes";
            this.CubesRadioButton.UseVisualStyleBackColor = true;
            // 
            // SpheresRadioButton
            // 
            this.SpheresRadioButton.AutoSize = true;
            this.SpheresRadioButton.Location = new System.Drawing.Point(3, 26);
            this.SpheresRadioButton.Name = "SpheresRadioButton";
            this.SpheresRadioButton.Size = new System.Drawing.Size(64, 17);
            this.SpheresRadioButton.TabIndex = 3;
            this.SpheresRadioButton.Text = "Spheres";
            this.SpheresRadioButton.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.CubesRadioButton);
            this.panel1.Controls.Add(this.SpheresRadioButton);
            this.panel1.Location = new System.Drawing.Point(127, 19);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(117, 63);
            this.panel1.TabIndex = 4;
            // 
            // PageRendering
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.CutterRendererGroupBox);
            this.Controls.Add(this.PathRendererGroupBox);
            this.Controls.Add(this.groupBox1);
            this.Name = "PageRendering";
            this.Size = new System.Drawing.Size(515, 320);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.PathRendererGroupBox.ResumeLayout(false);
            this.PathRendererGroupBox.PerformLayout();
            this.CutterRendererGroupBox.ResumeLayout(false);
            this.CutterRendererGroupBox.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox PathRendererGroupBox;
        private System.Windows.Forms.GroupBox CutterRendererGroupBox;
        private System.Windows.Forms.RadioButton PostSimulationMatRadioButton;
        private System.Windows.Forms.RadioButton RealTimeMatRadioButton;
        private System.Windows.Forms.RadioButton NoneMatRadioButton;
        private System.Windows.Forms.RadioButton NonePathRadioButton;
        private System.Windows.Forms.RadioButton StandardPathRadioButton;
        private System.Windows.Forms.RadioButton NoneCutterRadioButton;
        private System.Windows.Forms.RadioButton StandardCutterRadioButton;
        private System.Windows.Forms.RadioButton SpheresRadioButton;
        private System.Windows.Forms.RadioButton CubesRadioButton;
        private System.Windows.Forms.Panel panel1;
    }
}
