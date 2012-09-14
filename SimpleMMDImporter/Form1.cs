using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SimpleMMDImporter
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            openFileDialog.Filter = "MMDモデルファイル(*.pmd)|*.pmd|MMDモーションファイル(*.vmd)|*.vmd";
            openFileDialog.Title = "開くファイルを選択してください";
        }

        private void buttonRefer_Click(object sender, EventArgs e)
        {
            switch (openFileDialog.ShowDialog())
            {
                case DialogResult.OK:
                    textBox.Text = openFileDialog.FileName;
                    break;
                default:
                    break;
            }
        }

        private void buttonConvert_Click(object sender, EventArgs e)
        {

        }
    }
}
