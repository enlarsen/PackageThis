// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using MSHelpCompiler;



namespace PackageThis
{
    public class Hxs : ICompilable
    {
        private string projectFile;
        private string projectRoot;
        private string outputFile;

        public Hxs(string projectFile, string projectRoot, string outputFile)
        {
            this.projectFile = projectFile;
            this.projectRoot = projectRoot;
            this.outputFile = outputFile;
        }

        void ICompilable.Compile(IProgressReporter progressReporter)
        {
            HxComp hxsCompiler = new HxComp();
            hxsCompiler.Initialize();
            //MSHelpCompilerError compilerError = new MSHelpCompilerError();
            CompMsg compilerError;
 
            compilerError = new CompMsg(progressReporter);
            
            int i = hxsCompiler.AdviseCompilerMessageCallback(compilerError);


            hxsCompiler.Compile(projectFile, projectRoot, outputFile, 0);
            //if (compilerError.HxErrorType != MSHelpCompilerError.ErrorType.None)
            //{
            //    Console.WriteLine(compilerError.ShortErrorDescription);
            //}
            hxsCompiler.UnadviseCompilerMessageCallback(i);
            hxsCompiler = null;
        }


        public void Decompile()
        {
        }



    }



}
