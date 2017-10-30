namespace Viterbi
{
    partial class Form1
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
            this.editorTB = new System.Windows.Forms.TextBox();
            this.currentWordLabel = new System.Windows.Forms.Label();
            this.tagsLabel = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.status = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.compileStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // editorTB
            // 
            this.editorTB.Location = new System.Drawing.Point(12, 79);
            this.editorTB.Name = "editorTB";
            this.editorTB.Size = new System.Drawing.Size(544, 20);
            this.editorTB.TabIndex = 2;
            this.editorTB.TextChanged += new System.EventHandler(this.editorTB_TextChanged);
            // 
            // currentWordLabel
            // 
            this.currentWordLabel.AutoSize = true;
            this.currentWordLabel.Location = new System.Drawing.Point(91, 40);
            this.currentWordLabel.Name = "currentWordLabel";
            this.currentWordLabel.Size = new System.Drawing.Size(10, 13);
            this.currentWordLabel.TabIndex = 3;
            this.currentWordLabel.Text = ".";
            // 
            // tagsLabel
            // 
            this.tagsLabel.AutoSize = true;
            this.tagsLabel.Location = new System.Drawing.Point(55, 106);
            this.tagsLabel.Name = "tagsLabel";
            this.tagsLabel.Size = new System.Drawing.Size(24, 13);
            this.tagsLabel.TabIndex = 5;
            this.tagsLabel.Text = "<s>";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(12, 131);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(202, 69);
            this.listBox1.TabIndex = 6;
            this.listBox1.SelectedIndexChanged += new System.EventHandler(this.listBox1_SelectedIndexChanged);
            // 
            // status
            // 
            this.status.AutoSize = true;
            this.status.Location = new System.Drawing.Point(55, 203);
            this.status.Name = "status";
            this.status.Size = new System.Drawing.Size(37, 13);
            this.status.TabIndex = 7;
            this.status.Text = "Status";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Editor";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Tags: ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Current Word:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 203);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Status:";
            // 
            // compileStatus
            // 
            this.compileStatus.AutoSize = true;
            this.compileStatus.Location = new System.Drawing.Point(563, 85);
            this.compileStatus.Name = "compileStatus";
            this.compileStatus.Size = new System.Drawing.Size(77, 13);
            this.compileStatus.TabIndex = 12;
            this.compileStatus.Text = "Compile Status";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(729, 432);
            this.Controls.Add(this.compileStatus);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.status);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.tagsLabel);
            this.Controls.Add(this.currentWordLabel);
            this.Controls.Add(this.editorTB);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox editorTB;
        private System.Windows.Forms.Label currentWordLabel;
        private System.Windows.Forms.Label tagsLabel;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.Label status;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label compileStatus;
    }
}

