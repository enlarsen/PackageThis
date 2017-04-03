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
    public partial class ExportProgressForm : Form, IProgressReporter
    {
        private ICompilable helpFile;
        private int expectedLines;
        private int lines = 0;


        public ExportProgressForm(ICompilable helpFile, int expectedLines)
        {
            InitializeComponent();

            this.helpFile = helpFile;
            this.expectedLines = expectedLines;
        }

        private void ProgressForm_Load(object sender, EventArgs e)
        {
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;


            helpFile.Compile(this as IProgressReporter);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Close();
            
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
            label1.Text = e.UserState as string;
        }

        void IProgressReporter.ProgressMessage(string message)
        {
            int percent = (lines++ * 100) / expectedLines;
            
            if (percent > 100)
                percent = 100;

            backgroundWorker1.ReportProgress(percent, message);
        }

        // http://blogs.msdn.com/greggm/archive/2005/11/18/494648.aspx
        //
        public override string ToString()
        {
            return "Disabled to make debugger work.";
        }
    }
}