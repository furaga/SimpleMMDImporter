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
            saveFileDialog.Filter = "CSVファイル(*.csv)|*.csv";
            saveFileDialog.Title = "保存先を選択してください";
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

        MMDModel.MMDModel model;

        private void buttonConvert_Click(object sender, EventArgs e)
        {
//            model = new MMDModel.MMDModel(textBox.Text, @"C:\Users\furaga\Desktop\test.csv", 1.0f);
            switch (saveFileDialog.ShowDialog())
            {
                case DialogResult.OK:
                    model = new MMDModel.MMDModel(textBox.Text, saveFileDialog.FileName, 1.0f);
                    break;
                default:
                    break;
            }
//            Close();
        }
    }
}
