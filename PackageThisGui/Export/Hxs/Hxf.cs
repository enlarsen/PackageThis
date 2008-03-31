// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.IO;
using System.Xml;
using System.Text;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace PackageThis
{
    public class Hxf : TextWriter
    {

        private bool Disposed;
        private XmlTextWriter writer;

        // Constructor
        public Hxf(string filePath, Encoding encoding)
        {
         
            Disposed = false;
            writer = new XmlTextWriter(filePath, encoding);

            // Write header
            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument();
            writer.WriteDocType("HelpFileList", null, "MS-Help://Hx/Resources/HelpFileList.DTD", null);
            writer.WriteStartElement("HelpFileList");
            writer.WriteAttributeString("DTDVersion", "1.0");
            writer.Flush();
            
        }

        // Destructor (equivalent to Finalize() without the need to call base.Finalize())
        ~Hxf() { Dispose(false); }

        // Free resources immediately
        protected override void Dispose(bool Disposing)
        {
            if (!Disposed)
            {
                if (Disposing)
                {
                }
                // Close file
                writer.Close();
                writer = null;
                // Disposed
                Disposed = true;
                // Parent disposing
                base.Dispose(Disposing);
            }
        }

        // Close the file
        public override void Close()
        {
            // Write footer
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            // Free resources
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Implement Encoding() method from TextWriter
        public override Encoding Encoding
        {
            get
            {
                return (Encoding.Unicode);
            }
        }

        // Implement WriteLine() method from TextWriter (remove MethodImpl attribute for single-threaded app)
        // Use stack trace and reflection to get the calling class and method
        [MethodImpl(MethodImplOptions.Synchronized)]
        public override void WriteLine(string url)
        {
            writer.WriteStartElement("File");
            writer.WriteAttributeString("Url", url);
            writer.WriteEndElement();
            writer.Flush();
        }

    }
}
