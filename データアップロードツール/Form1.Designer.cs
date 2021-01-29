namespace データアップロードツール
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.dtDateTimePickerFrom = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.num_To_H = new System.Windows.Forms.NumericUpDown();
            this.num_To_M = new System.Windows.Forms.NumericUpDown();
            this.num_From_M = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.num_From_H = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dtDateTimePickerTo = new System.Windows.Forms.DateTimePicker();
            this.lblResult = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.num_To_H)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_To_M)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_From_M)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_From_H)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(40, 228);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "アップロードする";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dtDateTimePickerFrom
            // 
            this.dtDateTimePickerFrom.CustomFormat = "yyyy/MM/dd HH:mm";
            this.dtDateTimePickerFrom.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtDateTimePickerFrom.Location = new System.Drawing.Point(28, 43);
            this.dtDateTimePickerFrom.Name = "dtDateTimePickerFrom";
            this.dtDateTimePickerFrom.Size = new System.Drawing.Size(101, 19);
            this.dtDateTimePickerFrom.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 30;
            this.label3.Text = "日付";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(47, 126);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 26;
            this.label1.Text = "計算予約時刻";
            // 
            // num_To_H
            // 
            this.num_To_H.Location = new System.Drawing.Point(194, 143);
            this.num_To_H.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
            this.num_To_H.Name = "num_To_H";
            this.num_To_H.Size = new System.Drawing.Size(42, 19);
            this.num_To_H.TabIndex = 33;
            this.num_To_H.Value = new decimal(new int[] {
            9,
            0,
            0,
            0});
            // 
            // num_To_M
            // 
            this.num_To_M.Location = new System.Drawing.Point(265, 143);
            this.num_To_M.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.num_To_M.Name = "num_To_M";
            this.num_To_M.Size = new System.Drawing.Size(42, 19);
            this.num_To_M.TabIndex = 34;
            // 
            // num_From_M
            // 
            this.num_From_M.Location = new System.Drawing.Point(108, 143);
            this.num_From_M.Maximum = new decimal(new int[] {
            59,
            0,
            0,
            0});
            this.num_From_M.Name = "num_From_M";
            this.num_From_M.Size = new System.Drawing.Size(42, 19);
            this.num_From_M.TabIndex = 32;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(152, 147);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(37, 12);
            this.label10.TabIndex = 37;
            this.label10.Text = "分　～";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(90, 145);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 12);
            this.label6.TabIndex = 35;
            this.label6.Text = "時";
            // 
            // num_From_H
            // 
            this.num_From_H.Location = new System.Drawing.Point(46, 143);
            this.num_From_H.Maximum = new decimal(new int[] {
            23,
            0,
            0,
            0});
            this.num_From_H.Name = "num_From_H";
            this.num_From_H.Size = new System.Drawing.Size(42, 19);
            this.num_From_H.TabIndex = 31;
            this.num_From_H.Value = new decimal(new int[] {
            6,
            0,
            0,
            0});
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(242, 147);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(17, 12);
            this.label9.TabIndex = 36;
            this.label9.Text = "時";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(135, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 12);
            this.label2.TabIndex = 38;
            this.label2.Text = "日　～";
            // 
            // dtDateTimePickerTo
            // 
            this.dtDateTimePickerTo.CustomFormat = "yyyy/MM/dd HH:mm";
            this.dtDateTimePickerTo.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtDateTimePickerTo.Location = new System.Drawing.Point(178, 41);
            this.dtDateTimePickerTo.Name = "dtDateTimePickerTo";
            this.dtDateTimePickerTo.Size = new System.Drawing.Size(101, 19);
            this.dtDateTimePickerTo.TabIndex = 39;
            // 
            // lblResult
            // 
            this.lblResult.AutoSize = true;
            this.lblResult.Location = new System.Drawing.Point(135, 239);
            this.lblResult.Name = "lblResult";
            this.lblResult.Size = new System.Drawing.Size(35, 12);
            this.lblResult.TabIndex = 40;
            this.lblResult.Text = "label4";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(486, 361);
            this.Controls.Add(this.lblResult);
            this.Controls.Add(this.dtDateTimePickerTo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dtDateTimePickerFrom);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.num_To_H);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.num_To_M);
            this.Controls.Add(this.num_From_H);
            this.Controls.Add(this.num_From_M);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label10);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.num_To_H)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_To_M)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_From_M)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_From_H)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DateTimePicker dtDateTimePickerFrom;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown num_To_H;
        private System.Windows.Forms.NumericUpDown num_To_M;
        private System.Windows.Forms.NumericUpDown num_From_M;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown num_From_H;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtDateTimePickerTo;
        private System.Windows.Forms.Label lblResult;
    }
}

