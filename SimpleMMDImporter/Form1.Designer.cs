namespace SimpleMMDImporter
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
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
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
            this.textBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonConvert = new System.Windows.Forms.Button();
            this.buttonRefer = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // textBox
            // 
            this.textBox.Location = new System.Drawing.Point(69, 24);
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(254, 19);
            this.textBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "ファイル名";
            // 
            // buttonConvert
            // 
            this.buttonConvert.Location = new System.Drawing.Point(280, 51);
            this.buttonConvert.Name = "buttonConvert";
            this.buttonConvert.Size = new System.Drawing.Size(98, 24);
            this.buttonConvert.TabIndex = 2;
            this.buttonConvert.Text = "変換";
            this.buttonConvert.UseVisualStyleBackColor = true;
            this.buttonConvert.Click += new System.EventHandler(this.buttonConvert_Click);
            // 
            // buttonRefer
            // 
            this.buttonRefer.Location = new System.Drawing.Point(329, 22);
            this.buttonRefer.Name = "buttonRefer";
            this.buttonRefer.Size = new System.Drawing.Size(49, 23);
            this.buttonRefer.TabIndex = 3;
            this.buttonRefer.Text = "参照";
            this.buttonRefer.UseVisualStyleBackColor = true;
            this.buttonRefer.Click += new System.EventHandler(this.buttonRefer_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(228, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "pmd, vmdファイルをエクセルファイルに変換します";
            // 
            // openFileDialog
            // 
            this.openFileDialog.FileName = "openFileDialog";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(393, 86);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.buttonRefer);
            this.Controls.Add(this.buttonConvert);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBox);
            this.Name = "Form1";
            this.Text = "SimpleMMDImporter";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonConvert;
        private System.Windows.Forms.Button buttonRefer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
    }
}

