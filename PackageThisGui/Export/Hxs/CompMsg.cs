// Copyright (c) Microsoft Corporation.  All rights reserved.
//
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using MSHelpCompiler;


namespace PackageThis
{
    class CompMsg : IHxCompError
    {
        private IProgressReporter progressReporter;

        public CompMsg(IProgressReporter progressReporter)
        {
            this.progressReporter = progressReporter;
        }


        public void ReportMessage(MSHelpCompiler.HxCompErrorSeverity Severity, string DescriptionString)
        {
            progressReporter.ProgressMessage(DescriptionString);
        }

        public void ReportError(string TaskItemString, string Filename, int nLineNum, int nCharNum, MSHelpCompiler.HxCompErrorSeverity Severity, string DescriptionString)
        {
            //Console.ForegroundColor = ConsoleColor.Red;
            //Console.WriteLine(DescriptionString);
            //Console.ResetColor();
        }

        public MSHelpCompiler.HxCompStatus QueryStatus()
        {
            return new MSHelpCompiler.HxCompStatus();
        }

    }



}
