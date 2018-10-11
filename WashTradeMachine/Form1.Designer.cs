namespace WashTradeMachine
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
            this.lstUsers = new System.Windows.Forms.CheckedListBox();
            this.btnCalculatePermutations = new System.Windows.Forms.Button();
            this.txtPrice = new System.Windows.Forms.TextBox();
            this.txtAmount = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.Repeat = new System.Windows.Forms.Label();
            this.numRepeat = new System.Windows.Forms.NumericUpDown();
            this.btnStop = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numRepeat)).BeginInit();
            this.SuspendLayout();
            // 
            // lstUsers
            // 
            this.lstUsers.CheckOnClick = true;
            this.lstUsers.FormattingEnabled = true;
            this.lstUsers.Location = new System.Drawing.Point(12, 12);
            this.lstUsers.Name = "lstUsers";
            this.lstUsers.Size = new System.Drawing.Size(175, 559);
            this.lstUsers.TabIndex = 14;
            // 
            // btnCalculatePermutations
            // 
            this.btnCalculatePermutations.Location = new System.Drawing.Point(16, 680);
            this.btnCalculatePermutations.Name = "btnCalculatePermutations";
            this.btnCalculatePermutations.Size = new System.Drawing.Size(175, 41);
            this.btnCalculatePermutations.TabIndex = 15;
            this.btnCalculatePermutations.Text = "Calculate Permutations";
            this.btnCalculatePermutations.UseVisualStyleBackColor = true;
            this.btnCalculatePermutations.Click += new System.EventHandler(this.btnCalculatePermutations_Click);
            // 
            // txtPrice
            // 
            this.txtPrice.Location = new System.Drawing.Point(87, 587);
            this.txtPrice.Name = "txtPrice";
            this.txtPrice.Size = new System.Drawing.Size(100, 20);
            this.txtPrice.TabIndex = 16;
            this.txtPrice.Text = "0.05";
            // 
            // txtAmount
            // 
            this.txtAmount.Location = new System.Drawing.Point(87, 613);
            this.txtAmount.Name = "txtAmount";
            this.txtAmount.Size = new System.Drawing.Size(100, 20);
            this.txtAmount.TabIndex = 17;
            this.txtAmount.Text = "1.2";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 590);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "Price";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 620);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "Amount";
            // 
            // Repeat
            // 
            this.Repeat.AutoSize = true;
            this.Repeat.Location = new System.Drawing.Point(13, 646);
            this.Repeat.Name = "Repeat";
            this.Repeat.Size = new System.Drawing.Size(42, 13);
            this.Repeat.TabIndex = 21;
            this.Repeat.Text = "Repeat";
            // 
            // numRepeat
            // 
            this.numRepeat.Location = new System.Drawing.Point(87, 639);
            this.numRepeat.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numRepeat.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numRepeat.Name = "numRepeat";
            this.numRepeat.Size = new System.Drawing.Size(100, 20);
            this.numRepeat.TabIndex = 22;
            this.numRepeat.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(16, 732);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(175, 41);
            this.btnStop.TabIndex = 23;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(198, 785);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.numRepeat);
            this.Controls.Add(this.Repeat);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtAmount);
            this.Controls.Add(this.txtPrice);
            this.Controls.Add(this.btnCalculatePermutations);
            this.Controls.Add(this.lstUsers);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numRepeat)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox lstUsers;
        private System.Windows.Forms.Button btnCalculatePermutations;
        private System.Windows.Forms.TextBox txtPrice;
        private System.Windows.Forms.TextBox txtAmount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label Repeat;
        private System.Windows.Forms.NumericUpDown numRepeat;
        private System.Windows.Forms.Button btnStop;
    }
}

