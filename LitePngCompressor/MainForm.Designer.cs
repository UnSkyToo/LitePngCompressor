namespace LitePngCompressor
{
    partial class MainForm
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
            this.BtnGo = new System.Windows.Forms.Button();
            this.BtnInput = new System.Windows.Forms.Button();
            this.BtnOutput = new System.Windows.Forms.Button();
            this.LabelInput = new System.Windows.Forms.Label();
            this.LabelOutput = new System.Windows.Forms.Label();
            this.ListFiles = new System.Windows.Forms.ListBox();
            this.ProgressBarCompress = new System.Windows.Forms.ProgressBar();
            this.LabelInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // BtnGo
            // 
            this.BtnGo.Location = new System.Drawing.Point(678, 337);
            this.BtnGo.Name = "BtnGo";
            this.BtnGo.Size = new System.Drawing.Size(90, 43);
            this.BtnGo.TabIndex = 0;
            this.BtnGo.Text = "Go";
            this.BtnGo.UseVisualStyleBackColor = true;
            this.BtnGo.Click += new System.EventHandler(this.BtnGo_Click);
            // 
            // BtnInput
            // 
            this.BtnInput.Location = new System.Drawing.Point(486, 337);
            this.BtnInput.Name = "BtnInput";
            this.BtnInput.Size = new System.Drawing.Size(90, 43);
            this.BtnInput.TabIndex = 1;
            this.BtnInput.Text = "Input";
            this.BtnInput.UseVisualStyleBackColor = true;
            this.BtnInput.Click += new System.EventHandler(this.BtnInput_Click);
            // 
            // BtnOutput
            // 
            this.BtnOutput.Location = new System.Drawing.Point(582, 337);
            this.BtnOutput.Name = "BtnOutput";
            this.BtnOutput.Size = new System.Drawing.Size(90, 43);
            this.BtnOutput.TabIndex = 1;
            this.BtnOutput.Text = "Output";
            this.BtnOutput.UseVisualStyleBackColor = true;
            this.BtnOutput.Click += new System.EventHandler(this.BtnOutput_Click);
            // 
            // LabelInput
            // 
            this.LabelInput.AutoSize = true;
            this.LabelInput.Location = new System.Drawing.Point(12, 337);
            this.LabelInput.Name = "LabelInput";
            this.LabelInput.Size = new System.Drawing.Size(53, 12);
            this.LabelInput.TabIndex = 2;
            this.LabelInput.Text = "Input  :";
            // 
            // LabelOutput
            // 
            this.LabelOutput.AutoSize = true;
            this.LabelOutput.Location = new System.Drawing.Point(12, 352);
            this.LabelOutput.Name = "LabelOutput";
            this.LabelOutput.Size = new System.Drawing.Size(53, 12);
            this.LabelOutput.TabIndex = 2;
            this.LabelOutput.Text = "Output :";
            // 
            // ListFiles
            // 
            this.ListFiles.FormattingEnabled = true;
            this.ListFiles.ItemHeight = 12;
            this.ListFiles.Location = new System.Drawing.Point(2, 2);
            this.ListFiles.Name = "ListFiles";
            this.ListFiles.Size = new System.Drawing.Size(766, 304);
            this.ListFiles.TabIndex = 3;
            // 
            // ProgressBarCompress
            // 
            this.ProgressBarCompress.Location = new System.Drawing.Point(2, 313);
            this.ProgressBarCompress.Name = "ProgressBarCompress";
            this.ProgressBarCompress.Size = new System.Drawing.Size(766, 18);
            this.ProgressBarCompress.TabIndex = 4;
            // 
            // LabelInfo
            // 
            this.LabelInfo.AutoSize = true;
            this.LabelInfo.Location = new System.Drawing.Point(12, 368);
            this.LabelInfo.Name = "LabelInfo";
            this.LabelInfo.Size = new System.Drawing.Size(29, 12);
            this.LabelInfo.TabIndex = 5;
            this.LabelInfo.Text = "Done";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(771, 385);
            this.Controls.Add(this.LabelInfo);
            this.Controls.Add(this.ProgressBarCompress);
            this.Controls.Add(this.ListFiles);
            this.Controls.Add(this.LabelOutput);
            this.Controls.Add(this.LabelInput);
            this.Controls.Add(this.BtnOutput);
            this.Controls.Add(this.BtnInput);
            this.Controls.Add(this.BtnGo);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Lite Png Compressor";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyDown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button BtnGo;
        private System.Windows.Forms.Button BtnInput;
        private System.Windows.Forms.Button BtnOutput;
        private System.Windows.Forms.Label LabelInput;
        private System.Windows.Forms.Label LabelOutput;
        private System.Windows.Forms.ListBox ListFiles;
        private System.Windows.Forms.ProgressBar ProgressBarCompress;
        private System.Windows.Forms.Label LabelInfo;
    }
}

