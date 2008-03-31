// Copyright (c) Microsoft Corporation.  All rights reserved.
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PackageThis
{
    public partial class ExportChmForm : Form
    {
        public ExportChmForm()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(ChmFileTextBox.Text) == false &&
                String.IsNullOrEmpty(TitleTextBox.Text) == false)
                OKBtn.Enabled = true;
            else
                OKBtn.Enabled = false;
        }

        private void BrowseBtn_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                ChmFileTextBox.Text = saveFileDialog1.FileName;
                this.ActiveControl = TitleTextBox;

            }

        }


    }
}