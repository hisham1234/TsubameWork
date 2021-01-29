namespace DirectioinAPI
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
            this.pick_Lat = new System.Windows.Forms.TextBox();
            this.pick_Lon = new System.Windows.Forms.TextBox();
            this.pickup_Lat = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dest_Lon = new System.Windows.Forms.TextBox();
            this.dest_Lat = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.pickup_Lon = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.Route1_Dist = new System.Windows.Forms.Label();
            this.Route1_Dur = new System.Windows.Forms.Label();
            this.Route2_Dist = new System.Windows.Forms.Label();
            this.Route2_Dur = new System.Windows.Forms.Label();
            this.isHighWay = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(138, 176);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "計算";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // pick_Lat
            // 
            this.pick_Lat.Location = new System.Drawing.Point(138, 51);
            this.pick_Lat.Name = "pick_Lat";
            this.pick_Lat.Size = new System.Drawing.Size(100, 19);
            this.pick_Lat.TabIndex = 1;
            // 
            // pick_Lon
            // 
            this.pick_Lon.Location = new System.Drawing.Point(346, 51);
            this.pick_Lon.Name = "pick_Lon";
            this.pick_Lon.Size = new System.Drawing.Size(100, 19);
            this.pick_Lon.TabIndex = 2;
            // 
            // pickup_Lat
            // 
            this.pickup_Lat.AutoSize = true;
            this.pickup_Lat.Location = new System.Drawing.Point(31, 58);
            this.pickup_Lat.Name = "pickup_Lat";
            this.pickup_Lat.Size = new System.Drawing.Size(73, 12);
            this.pickup_Lat.TabIndex = 3;
            this.pickup_Lat.Text = "集荷場所_Lat";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 119);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "行き先_Lat";
            // 
            // dest_Lon
            // 
            this.dest_Lon.Location = new System.Drawing.Point(346, 112);
            this.dest_Lon.Name = "dest_Lon";
            this.dest_Lon.Size = new System.Drawing.Size(100, 19);
            this.dest_Lon.TabIndex = 5;
            // 
            // dest_Lat
            // 
            this.dest_Lat.Location = new System.Drawing.Point(138, 112);
            this.dest_Lat.Name = "dest_Lat";
            this.dest_Lat.Size = new System.Drawing.Size(100, 19);
            this.dest_Lat.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(265, 119);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "行き先_Lon";
            // 
            // pickup_Lon
            // 
            this.pickup_Lon.AutoSize = true;
            this.pickup_Lon.Location = new System.Drawing.Point(265, 58);
            this.pickup_Lon.Name = "pickup_Lon";
            this.pickup_Lon.Size = new System.Drawing.Size(75, 12);
            this.pickup_Lon.TabIndex = 8;
            this.pickup_Lon.Text = "集荷場所_Lon";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(136, 281);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(39, 12);
            this.label1.TabIndex = 9;
            this.label1.Text = "ルート1";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(344, 281);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(39, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "ルート2";
            // 
            // Route1_Dist
            // 
            this.Route1_Dist.AutoSize = true;
            this.Route1_Dist.Location = new System.Drawing.Point(124, 325);
            this.Route1_Dist.Name = "Route1_Dist";
            this.Route1_Dist.Size = new System.Drawing.Size(73, 12);
            this.Route1_Dist.TabIndex = 11;
            this.Route1_Dist.Text = "集荷場所_Lat";
            // 
            // Route1_Dur
            // 
            this.Route1_Dur.AutoSize = true;
            this.Route1_Dur.Location = new System.Drawing.Point(124, 364);
            this.Route1_Dur.Name = "Route1_Dur";
            this.Route1_Dur.Size = new System.Drawing.Size(73, 12);
            this.Route1_Dur.TabIndex = 12;
            this.Route1_Dur.Text = "集荷場所_Lat";
            // 
            // Route2_Dist
            // 
            this.Route2_Dist.AutoSize = true;
            this.Route2_Dist.Location = new System.Drawing.Point(344, 325);
            this.Route2_Dist.Name = "Route2_Dist";
            this.Route2_Dist.Size = new System.Drawing.Size(73, 12);
            this.Route2_Dist.TabIndex = 13;
            this.Route2_Dist.Text = "集荷場所_Lat";
            // 
            // Route2_Dur
            // 
            this.Route2_Dur.AutoSize = true;
            this.Route2_Dur.Location = new System.Drawing.Point(344, 364);
            this.Route2_Dur.Name = "Route2_Dur";
            this.Route2_Dur.Size = new System.Drawing.Size(73, 12);
            this.Route2_Dur.TabIndex = 14;
            this.Route2_Dur.Text = "集荷場所_Lat";
            // 
            // isHighWay
            // 
            this.isHighWay.AutoSize = true;
            this.isHighWay.Location = new System.Drawing.Point(346, 183);
            this.isHighWay.Name = "isHighWay";
            this.isHighWay.Size = new System.Drawing.Size(72, 16);
            this.isHighWay.TabIndex = 15;
            this.isHighWay.Text = "高速道路";
            this.isHighWay.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(778, 590);
            this.Controls.Add(this.isHighWay);
            this.Controls.Add(this.Route2_Dur);
            this.Controls.Add(this.Route2_Dist);
            this.Controls.Add(this.Route1_Dur);
            this.Controls.Add(this.Route1_Dist);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pickup_Lon);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dest_Lon);
            this.Controls.Add(this.dest_Lat);
            this.Controls.Add(this.pickup_Lat);
            this.Controls.Add(this.pick_Lon);
            this.Controls.Add(this.pick_Lat);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox pick_Lat;
        private System.Windows.Forms.TextBox pick_Lon;
        private System.Windows.Forms.Label pickup_Lat;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox dest_Lon;
        private System.Windows.Forms.TextBox dest_Lat;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label pickup_Lon;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label Route1_Dist;
        private System.Windows.Forms.Label Route1_Dur;
        private System.Windows.Forms.Label Route2_Dist;
        private System.Windows.Forms.Label Route2_Dur;
        private System.Windows.Forms.CheckBox isHighWay;
    }
}

