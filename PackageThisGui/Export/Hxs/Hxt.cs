// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace PackageThis
{
    public class Hxt
    {
        private bool Disposed;
        private XmlTextWriter writer;
        // Constructor
        public Hxt(string filePath, Encoding encoding)
        {

            Disposed = false;
            writer = new XmlTextWriter(filePath, encoding);

            // Write header
            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument();
            writer.WriteDocType("HelpTOC", null, "MS-Help://Hx/Resources/HelpTOC.DTD", null);
            writer.WriteStartElement("HelpTOC");
            writer.WriteAttributeString("DTDVersion", "1.0");
            writer.Flush();

        }

        // Destructor
        ~Hxt() { Dispose(false); }

        // Free resources immediately
        protected void Dispose(bool Disposing)
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
                
            }
        }

        public void WriteStartNode(string title, string url)
        {

            writer.WriteStartElement("HelpTOCNode");
            if (string.IsNullOrEmpty(title) != true)
            {
                writer.WriteAttributeString("Title", title);
            }
            if (string.IsNullOrEmpty(url) != true)
            {
                writer.WriteAttributeString("Url", url);
            }
            writer.Flush();
        }

        public void WriteEndNode()
        {
            writer.WriteEndElement();
            writer.Flush();
        }

        public void Close()
        {
            // Write footer
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();
            // Free resources
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
