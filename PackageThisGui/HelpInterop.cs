// Copyright (c) Microsoft Corporation.  All rights reserved.
//

/* This file was decompiled from the generated interop assembly with .NET Reflector
 * to simplify the binary distribution somewhat.
 * 
 * Small changes were necessary to compile (items commented out).
 */

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;


namespace MSHelpCompiler
{
    [ComImport, CoClass(typeof(HxCompClass)), Guid("314111B5-A502-11D2-BBCA-00C04F8EC294")]
    public interface HxComp : IHxComp
    {
    }

    [ComImport, /*ClassInterface(0), */ Guid("314111B4-A502-11D2-BBCA-00C04F8EC294") /*, TypeLibType(2) */]
    public class HxCompClass // : IHxComp, HxComp
    {/*
        // Methods
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(2)]
        public virtual extern int AdviseCompilerMessageCallback([In, MarshalAs(UnmanagedType.Interface)] IHxCompError pHxCompError);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(3)]
        public virtual extern void Compile([In, MarshalAs(UnmanagedType.BStr)] string ProjectFile, [In, MarshalAs(UnmanagedType.BStr)] string ProjectRoot, [In, MarshalAs(UnmanagedType.BStr)] string OutputFile, [In] int dwFlags);
        [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(4)]
        public virtual extern string Decompile([In, MarshalAs(UnmanagedType.BStr)] string CompiledFile, [In, MarshalAs(UnmanagedType.BStr)] string ProjectRoot, [In] int dwFlags);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(1)]
        public virtual extern void Initialize();
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(5)]
        public virtual extern void UnadviseCompilerMessageCallback([In] int pdwCookie);

        // Properties
        [DispId(100)]
        public virtual short LangId 
        { 
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(100)] 
            get; [param: In] 
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(100)] 
            set; 
        } */
    }

    [Guid("314111F8-A502-11D2-BBCA-00C04F8EC294")]
    public enum HxCompErrorSeverity
    {
        HxCompErrorSeverity_Information,
        HxCompErrorSeverity_Warning,
        HxCompErrorSeverity_Error,
        HxCompErrorSeverity_Fatal
    }

    [Guid("F9C06234-8EC6-4F13-8B54-DCA7C6B62302")]
    public enum HxCompStatus
    {
        HxCompStatus_Continue,
        HxCompStatus_Cancel
    }

    [ComImport, Guid("314111B5-A502-11D2-BBCA-00C04F8EC294"), TypeLibType(0x10c0)]
    public interface IHxComp
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(1)]
        void Initialize();
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(2)]
        int AdviseCompilerMessageCallback([In, MarshalAs(UnmanagedType.Interface)] IHxCompError pHxCompError);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(3)]
        void Compile([In, MarshalAs(UnmanagedType.BStr)] string ProjectFile, [In, MarshalAs(UnmanagedType.BStr)] string ProjectRoot, [In, MarshalAs(UnmanagedType.BStr)] string OutputFile, [In] int dwFlags);
        [return: MarshalAs(UnmanagedType.BStr)]
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(4)]
        string Decompile([In, MarshalAs(UnmanagedType.BStr)] string CompiledFile, [In, MarshalAs(UnmanagedType.BStr)] string ProjectRoot, [In] int dwFlags);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(5)]
        void UnadviseCompilerMessageCallback([In] int pdwCookie);
        
        [DispId(100)]
        short LangId 
        { 
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(100)] 
            get; [param: In] 
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(100)] 
            set; 
        }
    }

    [ComImport, TypeLibType(0x10c0), Guid("314111F9-A502-11D2-BBCA-00C04F8EC294")]
    public interface IHxCompError
    {
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(1)]
        void ReportError([In, MarshalAs(UnmanagedType.BStr)] string TaskItemString, [In, MarshalAs(UnmanagedType.BStr)] string Filename, [In] int nLineNum, [In] int nCharNum, [In] HxCompErrorSeverity Severity, [In, MarshalAs(UnmanagedType.BStr)] string DescriptionString);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(2)]
        void ReportMessage([In] HxCompErrorSeverity Severity, [In, MarshalAs(UnmanagedType.BStr)] string DescriptionString);
        [MethodImpl(MethodImplOptions.InternalCall, MethodCodeType = MethodCodeType.Runtime), DispId(3)]
        HxCompStatus QueryStatus();
    }
}


