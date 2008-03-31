// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Web;

// Because the hhc file is not a valid xml file, we have to write tags directly.

namespace PackageThis
{
    public class Hhc
    {
        private bool Disposed;
        private StreamWriter writer;

        // Constructor
        public Hhc(string filePath, string locale)
        {
            int codePage = new CultureInfo(locale).TextInfo.ANSICodePage;

            Encoding encoding = Encoding.GetEncoding(codePage);
            
            Disposed = false;

            writer = new StreamWriter(filePath, false, encoding);
            writer.WriteLine("<!DOCTYPE HTML PUBLIC \"-//IETF//DTD HTML//EN\"/>\n" +
                "<HTML>\n" +
                "<HEAD>\n" +
                "<META HTTP-EQUIV=\"Content Type\" CONTENT=\"text/html; CHARSET={0}\">\n" + 
                "<meta name=\"GENERATOR\" content=\"Package This\" />\n" +
                "<!-- Sitemap 1.0 -->\n" +
                "</HEAD><BODY>", encoding.WebName);

/*
            writer = new XmlTextWriter(filePath, encoding);

            // Write header
            writer.Formatting = Formatting.Indented;
            writer.WriteStartDocument();
            writer.WriteDocType("HTML", "-//IETF//DTD HTML//EN", null, null);
            writer.WriteStartElement("html");
            writer.WriteStartElement("body");
            writer.WriteStartElement("ul");
            writer.Flush(); */

        }

        // Destructor
        ~Hhc() { Dispose(false); }

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
            title = HttpUtility.HtmlEncode(title);
            url = HttpUtility.HtmlEncode(url);
            writer.WriteLine("<UL>\n" +
                "<LI><OBJECT type=\"text/sitemap\"/>");
                
            
/*            writer.WriteStartElement("li");
            writer.WriteStartElement("object");
            writer.WriteAttributeString("type", "text/sitemap"); */

            if (string.IsNullOrEmpty(title) != true)
            {
                writer.WriteLine("<param name=\"Name\" value=\"" + title + "\">");
                
/*                writer.WriteStartElement("param");
                writer.WriteAttributeString("name", "Name");
                writer.WriteAttributeString("value", title);
                writer.WriteEndElement(); */

            }

            if (string.IsNullOrEmpty(url) != true)
            {
                writer.WriteLine("<param name=\"Local\" value=\"" + url + "\">");
/*                
                writer.WriteStartElement("param");
                writer.WriteAttributeString("name", "Local");
                writer.WriteAttributeString("value", url);
                writer.WriteEndElement(); */
            }
            else
            {
                // creates a folder icon
                writer.WriteLine("<param name=\"ImageNumber\" value=\"1\"/>");

            }

            writer.WriteLine("</OBJECT>");
            //writer.WriteEndElement();
            writer.Flush();
        }

        public void WriteEndNode()
        {
            writer.WriteLine("</UL>");
            //writer.WriteEndElement();
            writer.Flush();
        }

        public void Close()
        {
            // Write footer
            //writer.WriteEndDocument();

            writer.WriteLine("</BODY></HTML>");

            writer.Flush();
            // Free resources
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
