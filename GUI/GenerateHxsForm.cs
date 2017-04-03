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
    public partial class GenerateHxsForm : Form
    {
        public GenerateHxsForm()
        {
            InitializeComponent();
        }

        private void GenerateHxsForm_Load(object sender, EventArgs e)
        {
            // Add three years of copyright strings to the copyright control since no one knows how
            // to type the copyright symbol.

            for (int i = -1; i < 2; i++)
            {
                CopyrightComboBox.Items.Add("©" + (DateTime.Now.Year + i).ToString() +
                    ". All rights reserved.");
            }
            
            // Select the current year by default.

            CopyrightComboBox.SelectedItem = CopyrightComboBox.Items[1];

        }

        private void Browse_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileTextBox.Text = saveFileDialog1.FileName;
                this.ActiveControl = TitleTextBox;

            }
        }

        // Make sure the OK button is disabled until there is some
        // text in each of the three text boxes.

        private void TextChanged_Event(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(FileTextBox.Text) == false &&
                String.IsNullOrEmpty(TitleTextBox.Text) == false &&
                String.IsNullOrEmpty(CopyrightComboBox.Text) == false)
                OK.Enabled = true;
            else
                OK.Enabled = false;

        }
    }
}