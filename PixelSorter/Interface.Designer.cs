namespace PixelSorter {
    partial class Interface {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing ) {
            if( disposing && (components != null) ) {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.bOpenFile = new System.Windows.Forms.Button();
            this.tImageFile = new System.Windows.Forms.TextBox();
            this.nSegmentWidth = new System.Windows.Forms.NumericUpDown();
            this.lSegmentSize = new System.Windows.Forms.Label();
            this.lAlgorithm = new System.Windows.Forms.Label();
            this.cAlgorithm = new System.Windows.Forms.ComboBox();
            this.cMetric = new System.Windows.Forms.ComboBox();
            this.lMetric = new System.Windows.Forms.Label();
            this.lX = new System.Windows.Forms.Label();
            this.cSampling = new System.Windows.Forms.ComboBox();
            this.lSampling = new System.Windows.Forms.Label();
            this.pbProgress = new System.Windows.Forms.ProgressBar();
            this.lProgress = new System.Windows.Forms.Label();
            this.bProcess = new System.Windows.Forms.Button();
            this.bSave = new System.Windows.Forms.Button();
            this.bRevert = new System.Windows.Forms.Button();
            this.cOrder = new System.Windows.Forms.ComboBox();
            this.lOrder = new System.Windows.Forms.Label();
            this.lImageSize = new System.Windows.Forms.Label();
            this.bRandomize = new System.Windows.Forms.Button();
            this.nSegmentHeight = new System.Windows.Forms.NumericUpDown();
            this.tbThreshold = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nSegmentWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nSegmentHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbThreshold)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox
            // 
            this.pictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox.Location = new System.Drawing.Point(221, 12);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(330, 371);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox.TabIndex = 0;
            this.pictureBox.TabStop = false;
            // 
            // bOpenFile
            // 
            this.bOpenFile.Location = new System.Drawing.Point(12, 12);
            this.bOpenFile.Name = "bOpenFile";
            this.bOpenFile.Size = new System.Drawing.Size(85, 23);
            this.bOpenFile.TabIndex = 0;
            this.bOpenFile.Text = "Open image...";
            this.bOpenFile.UseVisualStyleBackColor = true;
            this.bOpenFile.Click += new System.EventHandler(this.bOpenFile_Click);
            // 
            // tImageFile
            // 
            this.tImageFile.Location = new System.Drawing.Point(12, 41);
            this.tImageFile.Name = "tImageFile";
            this.tImageFile.ReadOnly = true;
            this.tImageFile.Size = new System.Drawing.Size(203, 20);
            this.tImageFile.TabIndex = 2;
            // 
            // nSegmentWidth
            // 
            this.nSegmentWidth.Location = new System.Drawing.Point(88, 221);
            this.nSegmentWidth.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.nSegmentWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nSegmentWidth.Name = "nSegmentWidth";
            this.nSegmentWidth.Size = new System.Drawing.Size(50, 20);
            this.nSegmentWidth.TabIndex = 14;
            this.nSegmentWidth.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            // 
            // lSegmentSize
            // 
            this.lSegmentSize.AutoSize = true;
            this.lSegmentSize.Location = new System.Drawing.Point(12, 223);
            this.lSegmentSize.Name = "lSegmentSize";
            this.lSegmentSize.Size = new System.Drawing.Size(70, 13);
            this.lSegmentSize.TabIndex = 13;
            this.lSegmentSize.Text = "Segment size";
            this.lSegmentSize.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lAlgorithm
            // 
            this.lAlgorithm.AutoSize = true;
            this.lAlgorithm.Location = new System.Drawing.Point(32, 117);
            this.lAlgorithm.Name = "lAlgorithm";
            this.lAlgorithm.Size = new System.Drawing.Size(50, 13);
            this.lAlgorithm.TabIndex = 5;
            this.lAlgorithm.Text = "Algorithm";
            this.lAlgorithm.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cAlgorithm
            // 
            this.cAlgorithm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cAlgorithm.FormattingEnabled = true;
            this.cAlgorithm.Items.AddRange(new object[] {
            "WholeImage",
            "Row",
            "Column",
            "Segment"});
            this.cAlgorithm.Location = new System.Drawing.Point(88, 114);
            this.cAlgorithm.Name = "cAlgorithm";
            this.cAlgorithm.Size = new System.Drawing.Size(127, 21);
            this.cAlgorithm.TabIndex = 6;
            // 
            // cMetric
            // 
            this.cMetric.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cMetric.FormattingEnabled = true;
            this.cMetric.Items.AddRange(new object[] {
            "Intensity",
            "Lightness",
            "Luma",
            "Brightness",
            "Hue (HSL)",
            "Hue (Lab)",
            "Chroma (HSL)",
            "Chroma (Lab)",
            "Saturation (HSB)",
            "Saturation (HSI)",
            "Saturation (HSL)",
            "Saturation (Lab)",
            "RedChannel",
            "GreenChannel",
            "BlueChannel",
            "Red",
            "Green",
            "Blue",
            "Cyan",
            "Magenta",
            "Yellow",
            "a (Lab)",
            "b (Lab)"});
            this.cMetric.Location = new System.Drawing.Point(88, 167);
            this.cMetric.Name = "cMetric";
            this.cMetric.Size = new System.Drawing.Size(127, 21);
            this.cMetric.TabIndex = 10;
            // 
            // lMetric
            // 
            this.lMetric.AutoSize = true;
            this.lMetric.Location = new System.Drawing.Point(46, 170);
            this.lMetric.Name = "lMetric";
            this.lMetric.Size = new System.Drawing.Size(36, 13);
            this.lMetric.TabIndex = 9;
            this.lMetric.Text = "Metric";
            this.lMetric.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lX
            // 
            this.lX.AutoSize = true;
            this.lX.Location = new System.Drawing.Point(147, 223);
            this.lX.Name = "lX";
            this.lX.Size = new System.Drawing.Size(12, 13);
            this.lX.TabIndex = 15;
            this.lX.Text = "x";
            this.lX.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cSampling
            // 
            this.cSampling.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cSampling.FormattingEnabled = true;
            this.cSampling.Items.AddRange(new object[] {
            "Center",
            "Average (Mean)",
            "Average (Median)",
            "Maximum",
            "Minimum",
            "Random"});
            this.cSampling.Location = new System.Drawing.Point(88, 194);
            this.cSampling.Name = "cSampling";
            this.cSampling.Size = new System.Drawing.Size(127, 21);
            this.cSampling.TabIndex = 12;
            // 
            // lSampling
            // 
            this.lSampling.AutoSize = true;
            this.lSampling.Location = new System.Drawing.Point(32, 197);
            this.lSampling.Name = "lSampling";
            this.lSampling.Size = new System.Drawing.Size(50, 13);
            this.lSampling.TabIndex = 11;
            this.lSampling.Text = "Sampling";
            this.lSampling.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pbProgress
            // 
            this.pbProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.pbProgress.Location = new System.Drawing.Point(12, 324);
            this.pbProgress.Name = "pbProgress";
            this.pbProgress.Size = new System.Drawing.Size(203, 23);
            this.pbProgress.TabIndex = 18;
            // 
            // lProgress
            // 
            this.lProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lProgress.AutoSize = true;
            this.lProgress.Location = new System.Drawing.Point(12, 308);
            this.lProgress.Name = "lProgress";
            this.lProgress.Size = new System.Drawing.Size(50, 13);
            this.lProgress.TabIndex = 17;
            this.lProgress.Text = "lProgress";
            // 
            // bProcess
            // 
            this.bProcess.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bProcess.Location = new System.Drawing.Point(12, 353);
            this.bProcess.Name = "bProcess";
            this.bProcess.Size = new System.Drawing.Size(126, 30);
            this.bProcess.TabIndex = 19;
            this.bProcess.Text = "Process";
            this.bProcess.UseVisualStyleBackColor = true;
            this.bProcess.Click += new System.EventHandler(this.bProcess_Click);
            // 
            // bSave
            // 
            this.bSave.Location = new System.Drawing.Point(123, 67);
            this.bSave.Name = "bSave";
            this.bSave.Size = new System.Drawing.Size(92, 23);
            this.bSave.TabIndex = 4;
            this.bSave.Text = "Save image...";
            this.bSave.UseVisualStyleBackColor = true;
            this.bSave.Click += new System.EventHandler(this.bSave_Click);
            // 
            // bRevert
            // 
            this.bRevert.Location = new System.Drawing.Point(123, 12);
            this.bRevert.Name = "bRevert";
            this.bRevert.Size = new System.Drawing.Size(92, 23);
            this.bRevert.TabIndex = 1;
            this.bRevert.Text = "Show Original";
            this.bRevert.UseVisualStyleBackColor = true;
            this.bRevert.Click += new System.EventHandler(this.bRevert_Click);
            // 
            // cOrder
            // 
            this.cOrder.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cOrder.FormattingEnabled = true;
            this.cOrder.Items.AddRange(new object[] {
            "Ascending",
            "Descending",
            "Ascending Reflected",
            "Descending Reflected",
            "Ascending Thresholded",
            "Descending Thresholded",
            "Random"});
            this.cOrder.Location = new System.Drawing.Point(88, 141);
            this.cOrder.Name = "cOrder";
            this.cOrder.Size = new System.Drawing.Size(127, 21);
            this.cOrder.TabIndex = 8;
            // 
            // lOrder
            // 
            this.lOrder.AutoSize = true;
            this.lOrder.Location = new System.Drawing.Point(49, 144);
            this.lOrder.Name = "lOrder";
            this.lOrder.Size = new System.Drawing.Size(33, 13);
            this.lOrder.TabIndex = 7;
            this.lOrder.Text = "Order";
            this.lOrder.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lImageSize
            // 
            this.lImageSize.AutoSize = true;
            this.lImageSize.Location = new System.Drawing.Point(12, 72);
            this.lImageSize.Name = "lImageSize";
            this.lImageSize.Size = new System.Drawing.Size(58, 13);
            this.lImageSize.TabIndex = 3;
            this.lImageSize.Text = "lImageSize";
            // 
            // bRandomize
            // 
            this.bRandomize.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bRandomize.Location = new System.Drawing.Point(144, 353);
            this.bRandomize.Name = "bRandomize";
            this.bRandomize.Size = new System.Drawing.Size(71, 30);
            this.bRandomize.TabIndex = 20;
            this.bRandomize.Text = "Randomize";
            this.bRandomize.UseVisualStyleBackColor = true;
            this.bRandomize.Click += new System.EventHandler(this.bRandomize_Click);
            // 
            // nSegmentHeight
            // 
            this.nSegmentHeight.Location = new System.Drawing.Point(165, 221);
            this.nSegmentHeight.Maximum = new decimal(new int[] {
            256,
            0,
            0,
            0});
            this.nSegmentHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nSegmentHeight.Name = "nSegmentHeight";
            this.nSegmentHeight.Size = new System.Drawing.Size(50, 20);
            this.nSegmentHeight.TabIndex = 16;
            this.nSegmentHeight.Value = new decimal(new int[] {
            16,
            0,
            0,
            0});
            // 
            // tbThreshold
            // 
            this.tbThreshold.LargeChange = 10;
            this.tbThreshold.Location = new System.Drawing.Point(88, 247);
            this.tbThreshold.Maximum = 99;
            this.tbThreshold.Minimum = 1;
            this.tbThreshold.Name = "tbThreshold";
            this.tbThreshold.Size = new System.Drawing.Size(127, 45);
            this.tbThreshold.TabIndex = 21;
            this.tbThreshold.TickFrequency = 10;
            this.tbThreshold.Value = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 247);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 22;
            this.label1.Text = "Threshold";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Interface
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(563, 395);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbThreshold);
            this.Controls.Add(this.nSegmentHeight);
            this.Controls.Add(this.bRandomize);
            this.Controls.Add(this.lImageSize);
            this.Controls.Add(this.cOrder);
            this.Controls.Add(this.lOrder);
            this.Controls.Add(this.bRevert);
            this.Controls.Add(this.bSave);
            this.Controls.Add(this.bProcess);
            this.Controls.Add(this.lProgress);
            this.Controls.Add(this.pbProgress);
            this.Controls.Add(this.cSampling);
            this.Controls.Add(this.lSampling);
            this.Controls.Add(this.lX);
            this.Controls.Add(this.cMetric);
            this.Controls.Add(this.lMetric);
            this.Controls.Add(this.cAlgorithm);
            this.Controls.Add(this.lAlgorithm);
            this.Controls.Add(this.lSegmentSize);
            this.Controls.Add(this.nSegmentWidth);
            this.Controls.Add(this.tImageFile);
            this.Controls.Add(this.bOpenFile);
            this.Controls.Add(this.pictureBox);
            this.Name = "Interface";
            this.Text = "PixelSorter";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nSegmentWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nSegmentHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.tbThreshold)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Button bOpenFile;
        private System.Windows.Forms.TextBox tImageFile;
        private System.Windows.Forms.NumericUpDown nSegmentWidth;
        private System.Windows.Forms.Label lSegmentSize;
        private System.Windows.Forms.Label lAlgorithm;
        private System.Windows.Forms.ComboBox cAlgorithm;
        private System.Windows.Forms.ComboBox cMetric;
        private System.Windows.Forms.Label lMetric;
        private System.Windows.Forms.Label lX;
        private System.Windows.Forms.ComboBox cSampling;
        private System.Windows.Forms.Label lSampling;
        private System.Windows.Forms.ProgressBar pbProgress;
        private System.Windows.Forms.Label lProgress;
        private System.Windows.Forms.Button bProcess;
        private System.Windows.Forms.Button bSave;
        private System.Windows.Forms.Button bRevert;
        private System.Windows.Forms.ComboBox cOrder;
        private System.Windows.Forms.Label lOrder;
        private System.Windows.Forms.Label lImageSize;
        private System.Windows.Forms.Button bRandomize;
        private System.Windows.Forms.NumericUpDown nSegmentHeight;
        private System.Windows.Forms.TrackBar tbThreshold;
        private System.Windows.Forms.Label label1;
    }
}

