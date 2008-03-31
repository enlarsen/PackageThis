// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;

// This code refers to title (from the .htm file) which maps to the Name parameter in the .hhk
// <param name="Name" value="The .htm files's title">
// 
namespace PackageThis
{
    class Hhk
    {
        private SortedList<string, KeywordNode> keywords = new SortedList<string, KeywordNode>();
        private string filename;
        private Encoding encoding;

        public Hhk(string filename, string locale)
        {
            this.filename = filename;

            int codePage = new CultureInfo(locale).TextInfo.ANSICodePage;

            encoding = Encoding.GetEncoding(codePage);

        }

        // Pass in the MSHelp kKeyword string (with commas), and this will parse it
        // into the appropriate tree structure.
        public void Add(string kKeyword, string url, string title)
        {
            kKeyword = HttpUtility.HtmlEncode(kKeyword);
            url = HttpUtility.HtmlEncode(url);
            title = HttpUtility.HtmlEncode(title);

            string[] splitKeywords = kKeyword.Split(new string[] { ", " }, 
                StringSplitOptions.RemoveEmptyEntries);

            SortedList<string, KeywordNode> currentList = keywords;

            foreach (string keyword in splitKeywords)
            {
                if (currentList.Keys.Contains(keyword) == false)
                {
                    KeywordNode kwn = new KeywordNode(url, title);

                    currentList.Add(keyword, kwn);
                    currentList = kwn.children;
                }
                else
                {
                    currentList[keyword].duplicates.Add(new KeywordEntry(url, title));
                    currentList = currentList[keyword].children;
                }

            }
        }

        public void Save()
        {
            StreamWriter writer = new StreamWriter(filename, false, encoding);

            writer.WriteLine("<!DOCTYPE HTML PUBLIC \"-//IETF//DTD HTML//EN\"/>\n" +
                "<HTML>\n" + 
                "<!-- Sitemap 1.0 -->\n" +
                "<HEAD><META HTTP-EQUIV=\"Content Type\" CONTENT=\"text/html; CHARSET={0}\">\n" +
                "</HEAD><BODY>\n" +
                "<OBJECT type=\"text/site properties\">\n" +
                "</OBJECT>", encoding.WebName);

            SaveNodes(writer, keywords);

            writer.WriteLine("</BODY></HTML>");
            writer.Close();
        }

        private void SaveNodes(StreamWriter writer, SortedList<string, KeywordNode> keywords)
        {
            writer.WriteLine("<UL>");

            foreach (KeyValuePair<string, KeywordNode> kvp in keywords)
            {
                writer.WriteLine("<LI> <OBJECT type=\"text/sitemap\">");
                writer.WriteLine("<param name=\"Keyword\" value=\"" + kvp.Key + "\">");

                // handle duplicates
                foreach (KeywordEntry keywordEntry in kvp.Value.duplicates)
                {
                    writer.WriteLine("<param name=\"Name\" value=\"" + keywordEntry.title + "\">");
                    writer.WriteLine("<param name=\"Local\" value=\"" + keywordEntry.url + "\">");
                }

                writer.WriteLine("</OBJECT>");

                if (kvp.Value.children.Count != 0)
                {
                    SaveNodes(writer, kvp.Value.children);
                }

                
             }

             writer.WriteLine("</UL>");

        }
    }

    class KeywordEntry
    {
        public string url;
        public string title;

        public KeywordEntry(string url, string title)
        {
            this.url = url;
            this.title = title;
        }
    }

    // A KeywordNode represents a keyword organzied in a tree. Since a keyword can have
    // many duplicate entries (in different files), there is a collection of duplicates. 
    // And a keyword can have child keywords, there is a children collection. Finally, 
    // the leaf node is a KeywordEntry that contains that keyword's actual url and title.
    class KeywordNode
    {
        public List<KeywordEntry> duplicates = new List<KeywordEntry>();
        public SortedList<string, KeywordNode> children = new SortedList<string, KeywordNode>();

        public KeywordNode(string url, string title)
        {
            duplicates.Add(new KeywordEntry(url, title));
        }

    }
}
