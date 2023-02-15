namespace Mill5C.Settings.EnginePages
{
    partial class Page01
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
            this.StrategyGroupBox = new System.Windows.Forms.GroupBox();
            this.labelParallel = new System.Windows.Forms.Label();
            this.ParallelDepthNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.ThreadCountLbl = new System.Windows.Forms.Label();
            this.ThreadsNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.MultithreadedPERadioButton = new System.Windows.Forms.RadioButton();
            this.MultithreadedNormalRadioButton = new System.Windows.Forms.RadioButton();
            this.SinglethreadedRadioButton = new System.Windows.Forms.RadioButton();
            this.ManualRadioButton = new System.Windows.Forms.RadioButton();
            this.InterpolatorGroupBox = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.PositionDeltaLbl = new System.Windows.Forms.Label();
            this.OrientationDeltaLbl = new System.Windows.Forms.Label();
            this.PositionDeltaTextBox = new System.Windows.Forms.TextBox();
            this.OrientationDeltaTextBox = new System.Windows.Forms.TextBox();
            this.LCInterpolatorRadioButton = new System.Windows.Forms.RadioButton();
            this.MaterialGroupBox = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.CenterLabel = new System.Windows.Forms.Label();
            this.EdgeLengthLbl = new System.Windows.Forms.Label();
            this.CenterTextBox = new System.Windows.Forms.TextBox();
            this.EdgeLengthTextBox = new System.Windows.Forms.TextBox();
            this.StopCondTextBox = new System.Windows.Forms.TextBox();
            this.StopConditionSizeLbl = new System.Windows.Forms.Label();
            this.OctreeRadioButton = new System.Windows.Forms.RadioButton();
            this.FilesGroupBox = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.DownFileButton = new System.Windows.Forms.Button();
            this.UpFileButton = new System.Windows.Forms.Button();
            this.DeleteFileButton = new System.Windows.Forms.Button();
            this.AddFileButton = new System.Windows.Forms.Button();
            this.FilesListBox = new System.Windows.Forms.ListBox();
            this.openFileDialogFiles = new System.Windows.Forms.OpenFileDialog();
            this.StrategyGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ParallelDepthNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ThreadsNumericUpDown)).BeginInit();
            this.InterpolatorGroupBox.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.MaterialGroupBox.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.FilesGroupBox.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // StrategyGroupBox
            // 
            this.StrategyGroupBox.Controls.Add(this.labelParallel);
            this.StrategyGroupBox.Controls.Add(this.ParallelDepthNumericUpDown);
            this.StrategyGroupBox.Controls.Add(this.ThreadCountLbl);
            this.StrategyGroupBox.Controls.Add(this.ThreadsNumericUpDown);
            this.StrategyGroupBox.Controls.Add(this.MultithreadedPERadioButton);
            this.StrategyGroupBox.Controls.Add(this.MultithreadedNormalRadioButton);
            this.StrategyGroupBox.Controls.Add(this.SinglethreadedRadioButton);
            this.StrategyGroupBox.Controls.Add(this.ManualRadioButton);
            this.StrategyGroupBox.Location = new System.Drawing.Point(3, 3);
            this.StrategyGroupBox.Name = "StrategyGroupBox";
            this.StrategyGroupBox.Size = new System.Drawing.Size(250, 150);
            this.StrategyGroupBox.TabIndex = 0;
            this.StrategyGroupBox.TabStop = false;
            this.StrategyGroupBox.Text = "Simulation type";
            // 
            // labelParallel
            // 
            this.labelParallel.AutoSize = true;
            this.labelParallel.Location = new System.Drawing.Point(16, 116);
            this.labelParallel.Name = "labelParallel";
            this.labelParallel.Size = new System.Drawing.Size(86, 13);
            this.labelParallel.TabIndex = 7;
            this.labelParallel.Text = "Parallelism depth";
            // 
            // ParallelDepthNumericUpDown
            // 
            this.ParallelDepthNumericUpDown.Location = new System.Drawing.Point(108, 114);
            this.ParallelDepthNumericUpDown.Name = "ParallelDepthNumericUpDown";
            this.ParallelDepthNumericUpDown.Size = new System.Drawing.Size(115, 20);
            this.ParallelDepthNumericUpDown.TabIndex = 6;
            this.ParallelDepthNumericUpDown.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // ThreadCountLbl
            // 
            this.ThreadCountLbl.AutoSize = true;
            this.ThreadCountLbl.Location = new System.Drawing.Point(16, 67);
            this.ThreadCountLbl.Name = "ThreadCountLbl";
            this.ThreadCountLbl.Size = new System.Drawing.Size(76, 13);
            this.ThreadCountLbl.TabIndex = 5;
            this.ThreadCountLbl.Text = "Threads count";
            // 
            // ThreadsNumericUpDown
            // 
            this.ThreadsNumericUpDown.Location = new System.Drawing.Point(108, 65);
            this.ThreadsNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.ThreadsNumericUpDown.Name = "ThreadsNumericUpDown";
            this.ThreadsNumericUpDown.Size = new System.Drawing.Size(115, 20);
            this.ThreadsNumericUpDown.TabIndex = 4;
            this.ThreadsNumericUpDown.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // MultithreadedPERadioButton
            // 
            this.MultithreadedPERadioButton.AutoSize = true;
            this.MultithreadedPERadioButton.Location = new System.Drawing.Point(7, 91);
            this.MultithreadedPERadioButton.Name = "MultithreadedPERadioButton";
            this.MultithreadedPERadioButton.Size = new System.Drawing.Size(224, 17);
            this.MultithreadedPERadioButton.TabIndex = 3;
            this.MultithreadedPERadioButton.Text = "Multi threaded Octree (Parallel Extensions)";
            this.MultithreadedPERadioButton.UseVisualStyleBackColor = true;
            // 
            // MultithreadedNormalRadioButton
            // 
            this.MultithreadedNormalRadioButton.AutoSize = true;
            this.MultithreadedNormalRadioButton.Location = new System.Drawing.Point(7, 43);
            this.MultithreadedNormalRadioButton.Name = "MultithreadedNormalRadioButton";
            this.MultithreadedNormalRadioButton.Size = new System.Drawing.Size(222, 17);
            this.MultithreadedNormalRadioButton.TabIndex = 2;
            this.MultithreadedNormalRadioButton.Text = "Multi threaded Octree (standard algorithm)";
            this.MultithreadedNormalRadioButton.UseVisualStyleBackColor = true;
            // 
            // SinglethreadedRadioButton
            // 
            this.SinglethreadedRadioButton.AutoSize = true;
            this.SinglethreadedRadioButton.Checked = true;
            this.SinglethreadedRadioButton.Location = new System.Drawing.Point(108, 20);
            this.SinglethreadedRadioButton.Name = "SinglethreadedRadioButton";
            this.SinglethreadedRadioButton.Size = new System.Drawing.Size(99, 17);
            this.SinglethreadedRadioButton.TabIndex = 1;
            this.SinglethreadedRadioButton.TabStop = true;
            this.SinglethreadedRadioButton.Text = "Single threaded";
            this.SinglethreadedRadioButton.UseVisualStyleBackColor = true;
            // 
            // ManualRadioButton
            // 
            this.ManualRadioButton.AutoSize = true;
            this.ManualRadioButton.Location = new System.Drawing.Point(7, 20);
            this.ManualRadioButton.Name = "ManualRadioButton";
            this.ManualRadioButton.Size = new System.Drawing.Size(95, 17);
            this.ManualRadioButton.TabIndex = 0;
            this.ManualRadioButton.Text = "Manual control";
            this.ManualRadioButton.UseVisualStyleBackColor = true;
            // 
            // InterpolatorGroupBox
            // 
            this.InterpolatorGroupBox.Controls.Add(this.tableLayoutPanel1);
            this.InterpolatorGroupBox.Controls.Add(this.LCInterpolatorRadioButton);
            this.InterpolatorGroupBox.Location = new System.Drawing.Point(259, 4);
            this.InterpolatorGroupBox.Name = "InterpolatorGroupBox";
            this.InterpolatorGroupBox.Size = new System.Drawing.Size(250, 150);
            this.InterpolatorGroupBox.TabIndex = 1;
            this.InterpolatorGroupBox.TabStop = false;
            this.InterpolatorGroupBox.Text = "Interpolator";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.PositionDeltaLbl, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.OrientationDeltaLbl, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.PositionDeltaTextBox, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.OrientationDeltaTextBox, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(7, 65);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(237, 50);
            this.tableLayoutPanel1.TabIndex = 1;
            // 
            // PositionDeltaLbl
            // 
            this.PositionDeltaLbl.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.PositionDeltaLbl.AutoSize = true;
            this.PositionDeltaLbl.Location = new System.Drawing.Point(3, 6);
            this.PositionDeltaLbl.Name = "PositionDeltaLbl";
            this.PositionDeltaLbl.Size = new System.Drawing.Size(72, 13);
            this.PositionDeltaLbl.TabIndex = 0;
            this.PositionDeltaLbl.Text = "Position Delta";
            // 
            // OrientationDeltaLbl
            // 
            this.OrientationDeltaLbl.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.OrientationDeltaLbl.AutoSize = true;
            this.OrientationDeltaLbl.Location = new System.Drawing.Point(3, 31);
            this.OrientationDeltaLbl.Name = "OrientationDeltaLbl";
            this.OrientationDeltaLbl.Size = new System.Drawing.Size(86, 13);
            this.OrientationDeltaLbl.TabIndex = 1;
            this.OrientationDeltaLbl.Text = "Orientation Delta";
            // 
            // PositionDeltaTextBox
            // 
            this.PositionDeltaTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PositionDeltaTextBox.Location = new System.Drawing.Point(121, 3);
            this.PositionDeltaTextBox.Name = "PositionDeltaTextBox";
            this.PositionDeltaTextBox.Size = new System.Drawing.Size(113, 20);
            this.PositionDeltaTextBox.TabIndex = 2;
            this.PositionDeltaTextBox.Text = "1";
            // 
            // OrientationDeltaTextBox
            // 
            this.OrientationDeltaTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OrientationDeltaTextBox.Location = new System.Drawing.Point(121, 28);
            this.OrientationDeltaTextBox.Name = "OrientationDeltaTextBox";
            this.OrientationDeltaTextBox.Size = new System.Drawing.Size(113, 20);
            this.OrientationDeltaTextBox.TabIndex = 3;
            this.OrientationDeltaTextBox.Text = "0.02";
            // 
            // LCInterpolatorRadioButton
            // 
            this.LCInterpolatorRadioButton.AutoSize = true;
            this.LCInterpolatorRadioButton.Checked = true;
            this.LCInterpolatorRadioButton.Location = new System.Drawing.Point(7, 19);
            this.LCInterpolatorRadioButton.Name = "LCInterpolatorRadioButton";
            this.LCInterpolatorRadioButton.Size = new System.Drawing.Size(84, 17);
            this.LCInterpolatorRadioButton.TabIndex = 0;
            this.LCInterpolatorRadioButton.TabStop = true;
            this.LCInterpolatorRadioButton.Text = "Linear-Cubic";
            this.LCInterpolatorRadioButton.UseVisualStyleBackColor = true;
            // 
            // MaterialGroupBox
            // 
            this.MaterialGroupBox.Controls.Add(this.tableLayoutPanel2);
            this.MaterialGroupBox.Controls.Add(this.OctreeRadioButton);
            this.MaterialGroupBox.Location = new System.Drawing.Point(4, 160);
            this.MaterialGroupBox.Name = "MaterialGroupBox";
            this.MaterialGroupBox.Size = new System.Drawing.Size(249, 150);
            this.MaterialGroupBox.TabIndex = 2;
            this.MaterialGroupBox.TabStop = false;
            this.MaterialGroupBox.Text = "Material";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Controls.Add(this.CenterLabel, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.EdgeLengthLbl, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.CenterTextBox, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.EdgeLengthTextBox, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.StopCondTextBox, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.StopConditionSizeLbl, 0, 2);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(7, 43);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 3;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(236, 75);
            this.tableLayoutPanel2.TabIndex = 4;
            // 
            // CenterLabel
            // 
            this.CenterLabel.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.CenterLabel.AutoSize = true;
            this.CenterLabel.Location = new System.Drawing.Point(3, 6);
            this.CenterLabel.Name = "CenterLabel";
            this.CenterLabel.Size = new System.Drawing.Size(38, 13);
            this.CenterLabel.TabIndex = 0;
            this.CenterLabel.Text = "Center";
            // 
            // EdgeLengthLbl
            // 
            this.EdgeLengthLbl.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.EdgeLengthLbl.AutoSize = true;
            this.EdgeLengthLbl.Location = new System.Drawing.Point(3, 31);
            this.EdgeLengthLbl.Name = "EdgeLengthLbl";
            this.EdgeLengthLbl.Size = new System.Drawing.Size(64, 13);
            this.EdgeLengthLbl.TabIndex = 1;
            this.EdgeLengthLbl.Text = "Edge length";
            // 
            // CenterTextBox
            // 
            this.CenterTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CenterTextBox.Location = new System.Drawing.Point(121, 3);
            this.CenterTextBox.Name = "CenterTextBox";
            this.CenterTextBox.Size = new System.Drawing.Size(112, 20);
            this.CenterTextBox.TabIndex = 2;
            this.CenterTextBox.Text = "0.00;0.00;0.00";
            // 
            // EdgeLengthTextBox
            // 
            this.EdgeLengthTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.EdgeLengthTextBox.Location = new System.Drawing.Point(121, 28);
            this.EdgeLengthTextBox.Name = "EdgeLengthTextBox";
            this.EdgeLengthTextBox.Size = new System.Drawing.Size(112, 20);
            this.EdgeLengthTextBox.TabIndex = 3;
            this.EdgeLengthTextBox.Text = "80";
            // 
            // StopCondTextBox
            // 
            this.StopCondTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.StopCondTextBox.Location = new System.Drawing.Point(121, 53);
            this.StopCondTextBox.Name = "StopCondTextBox";
            this.StopCondTextBox.Size = new System.Drawing.Size(112, 20);
            this.StopCondTextBox.TabIndex = 5;
            this.StopCondTextBox.Text = "0.5";
            // 
            // StopConditionSizeLbl
            // 
            this.StopConditionSizeLbl.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.StopConditionSizeLbl.AutoSize = true;
            this.StopConditionSizeLbl.Location = new System.Drawing.Point(3, 56);
            this.StopConditionSizeLbl.Name = "StopConditionSizeLbl";
            this.StopConditionSizeLbl.Size = new System.Drawing.Size(96, 13);
            this.StopConditionSizeLbl.TabIndex = 4;
            this.StopConditionSizeLbl.Text = "Stop condition size";
            // 
            // OctreeRadioButton
            // 
            this.OctreeRadioButton.AutoSize = true;
            this.OctreeRadioButton.Checked = true;
            this.OctreeRadioButton.Location = new System.Drawing.Point(7, 20);
            this.OctreeRadioButton.Name = "OctreeRadioButton";
            this.OctreeRadioButton.Size = new System.Drawing.Size(57, 17);
            this.OctreeRadioButton.TabIndex = 0;
            this.OctreeRadioButton.TabStop = true;
            this.OctreeRadioButton.Text = "Octree";
            this.OctreeRadioButton.UseVisualStyleBackColor = true;
            // 
            // FilesGroupBox
            // 
            this.FilesGroupBox.Controls.Add(this.tableLayoutPanel3);
            this.FilesGroupBox.Controls.Add(this.FilesListBox);
            this.FilesGroupBox.Location = new System.Drawing.Point(259, 160);
            this.FilesGroupBox.Name = "FilesGroupBox";
            this.FilesGroupBox.Size = new System.Drawing.Size(250, 150);
            this.FilesGroupBox.TabIndex = 3;
            this.FilesGroupBox.TabStop = false;
            this.FilesGroupBox.Text = "Path files";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 4;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel3.Controls.Add(this.DownFileButton, 3, 0);
            this.tableLayoutPanel3.Controls.Add(this.UpFileButton, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.DeleteFileButton, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.AddFileButton, 0, 0);
            this.tableLayoutPanel3.Location = new System.Drawing.Point(7, 114);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(237, 30);
            this.tableLayoutPanel3.TabIndex = 3;
            // 
            // DownFileButton
            // 
            this.DownFileButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DownFileButton.Location = new System.Drawing.Point(180, 3);
            this.DownFileButton.Name = "DownFileButton";
            this.DownFileButton.Size = new System.Drawing.Size(54, 24);
            this.DownFileButton.TabIndex = 5;
            this.DownFileButton.Text = "Down";
            this.DownFileButton.UseVisualStyleBackColor = true;
            this.DownFileButton.Click += new System.EventHandler(this.DownFileButton_Click);
            // 
            // UpFileButton
            // 
            this.UpFileButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UpFileButton.Location = new System.Drawing.Point(121, 3);
            this.UpFileButton.Name = "UpFileButton";
            this.UpFileButton.Size = new System.Drawing.Size(53, 24);
            this.UpFileButton.TabIndex = 4;
            this.UpFileButton.Text = "Up";
            this.UpFileButton.UseVisualStyleBackColor = true;
            this.UpFileButton.Click += new System.EventHandler(this.UpFileButton_Click);
            // 
            // DeleteFileButton
            // 
            this.DeleteFileButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DeleteFileButton.Location = new System.Drawing.Point(62, 3);
            this.DeleteFileButton.Name = "DeleteFileButton";
            this.DeleteFileButton.Size = new System.Drawing.Size(53, 24);
            this.DeleteFileButton.TabIndex = 3;
            this.DeleteFileButton.Text = "Delete";
            this.DeleteFileButton.UseVisualStyleBackColor = true;
            this.DeleteFileButton.Click += new System.EventHandler(this.DeleteFileButton_Click);
            // 
            // AddFileButton
            // 
            this.AddFileButton.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AddFileButton.Location = new System.Drawing.Point(3, 3);
            this.AddFileButton.Name = "AddFileButton";
            this.AddFileButton.Size = new System.Drawing.Size(53, 24);
            this.AddFileButton.TabIndex = 2;
            this.AddFileButton.Text = "Add";
            this.AddFileButton.UseVisualStyleBackColor = true;
            this.AddFileButton.Click += new System.EventHandler(this.FilesButton_Click);
            // 
            // FilesListBox
            // 
            this.FilesListBox.FormattingEnabled = true;
            this.FilesListBox.HorizontalScrollbar = true;
            this.FilesListBox.Location = new System.Drawing.Point(7, 17);
            this.FilesListBox.Name = "FilesListBox";
            this.FilesListBox.Size = new System.Drawing.Size(237, 95);
            this.FilesListBox.TabIndex = 0;
            this.FilesListBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.FilesListBox_DragDrop);
            // 
            // openFileDialogFiles
            // 
            this.openFileDialogFiles.Filter = "5C Paths|*.*";
            this.openFileDialogFiles.Multiselect = true;
            // 
            // Page01
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.FilesGroupBox);
            this.Controls.Add(this.MaterialGroupBox);
            this.Controls.Add(this.InterpolatorGroupBox);
            this.Controls.Add(this.StrategyGroupBox);
            this.Name = "Page01";
            this.Size = new System.Drawing.Size(515, 320);
            this.StrategyGroupBox.ResumeLayout(false);
            this.StrategyGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ParallelDepthNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ThreadsNumericUpDown)).EndInit();
            this.InterpolatorGroupBox.ResumeLayout(false);
            this.InterpolatorGroupBox.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.MaterialGroupBox.ResumeLayout(false);
            this.MaterialGroupBox.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.FilesGroupBox.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox StrategyGroupBox;
        private System.Windows.Forms.RadioButton ManualRadioButton;
        private System.Windows.Forms.RadioButton MultithreadedNormalRadioButton;
        private System.Windows.Forms.RadioButton SinglethreadedRadioButton;
        private System.Windows.Forms.RadioButton MultithreadedPERadioButton;
        private System.Windows.Forms.GroupBox InterpolatorGroupBox;
        private System.Windows.Forms.RadioButton LCInterpolatorRadioButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label PositionDeltaLbl;
        private System.Windows.Forms.Label OrientationDeltaLbl;
        private System.Windows.Forms.TextBox PositionDeltaTextBox;
        private System.Windows.Forms.TextBox OrientationDeltaTextBox;
        private System.Windows.Forms.NumericUpDown ThreadsNumericUpDown;
        private System.Windows.Forms.GroupBox MaterialGroupBox;
        private System.Windows.Forms.RadioButton OctreeRadioButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Label CenterLabel;
        private System.Windows.Forms.Label EdgeLengthLbl;
        private System.Windows.Forms.TextBox CenterTextBox;
        private System.Windows.Forms.TextBox EdgeLengthTextBox;
        private System.Windows.Forms.TextBox StopCondTextBox;
        private System.Windows.Forms.Label StopConditionSizeLbl;
        private System.Windows.Forms.GroupBox FilesGroupBox;
        private System.Windows.Forms.Button AddFileButton;
        private System.Windows.Forms.ListBox FilesListBox;
        private System.Windows.Forms.OpenFileDialog openFileDialogFiles;
        private System.Windows.Forms.Label ThreadCountLbl;
        private System.Windows.Forms.Label labelParallel;
        private System.Windows.Forms.NumericUpDown ParallelDepthNumericUpDown;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Button DownFileButton;
        private System.Windows.Forms.Button UpFileButton;
        private System.Windows.Forms.Button DeleteFileButton;
    }
}
