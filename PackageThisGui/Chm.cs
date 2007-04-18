// Copyright (c) Microsoft Corporation.  All rights reserved.
//
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Xsl;

namespace PackageThis
{
    class Chm : ICompilable
    {
        private string withinChmDir;
        private string rawDir;
        private string chmDir;
        private string chmSubDir = "html";

        private string baseName;

        private string workingDir;
        private string title;
        private string chmFile;
        private string locale;
        private Content contentDataSet;
        private TreeNodeCollection nodes;
        private Dictionary<string, string> links;

        private string defaultPage = null;

        private IProgressReporter progressReporter = null;

        public int expectedLines = 0;

        // {0} = filename with full path (c:\file.chm)
        // {1} = filename without extension
        // {2} = LCID
        // {3} = default page
        // {4} = title
        static string template = "[OPTIONS]\n" +
            "Auto Index=Yes\n" +
            "Compatibility=1.1 or later\n" +
            "Compiled file={0}\n" +
            "Contents file={1}.hhc\n" +
            "Create CHI file=No\n" +
            "Default Window=msdn\n" +
            "Default topic={3}\n" +
            "Display compile progress=Yes\n" +
            "Enhanced decompilation=Yes\n" +
            "Error log file={1}.log\n" +
            "Full-text search=Yes\n" +
            "Index file={1}.hhk\n" +
            "Language=0x{2:x}\n" + // in hex, eg. 0x0409
            "Title={4}\n\n" +

            "[WINDOWS]\n" +
            "msdn=\"{4}\",\"{1}.hhc\",\"{1}.hhk\",\"{3}\",\"{3}\",,\"MSDN Library\",,\"MSDN Online\",0x73520,240,0x60387e,[30,30,770,540],0x30000,,,,,,0\n\n" +


            "[INFOTYPES]\n";


        static private Stream resourceStream = typeof(AppController).Assembly.GetManifestResourceStream(
            "PackageThis.Extra.chm.xslt");
        static private XmlReader transformFile = XmlReader.Create(resourceStream);
        static private XslCompiledTransform xform = null;


        public Chm(string workingDir, string title, string chmFile, string locale, TreeNodeCollection nodes,
            Content contentDataSet, Dictionary<string, string> links)
        {
            this.workingDir = workingDir;
            this.title = title;
            this.chmFile = chmFile;
            this.locale = locale;
            this.nodes = nodes;
            this.contentDataSet = contentDataSet;
            this.links = links;

            this.rawDir = Path.Combine(workingDir, "raw");
            this.chmDir = Path.Combine(workingDir, "chm");
            this.withinChmDir = Path.Combine(chmDir, chmSubDir);

            this.baseName = Path.GetFileNameWithoutExtension(chmFile);

            if (xform == null)
            {
                xform = new XslCompiledTransform(true);
                xform.Load(transformFile);
            }


        }


        public void Create()
        {
            if (Directory.Exists(chmDir) == true)
            {
                Directory.Delete(chmDir, true);
            }

            Directory.CreateDirectory(chmDir);
            Directory.CreateDirectory(withinChmDir);

            foreach (string file in Directory.GetFiles(rawDir))
            {
                File.Copy(file, Path.Combine(withinChmDir, Path.GetFileName(file)), true);
            }

            Hhk hhk = new Hhk(Path.Combine(chmDir, baseName + ".hhk"), locale);

            foreach (DataRow row in contentDataSet.Tables["Item"].Rows)
            {
                if (Int32.Parse(row["Size"].ToString()) != 0)
                {
                    Transform(row["ContentId"].ToString(),
                        row["Metadata"].ToString(), row["VersionId"].ToString(), contentDataSet);

                    XmlDocument document = new XmlDocument();

                    document.LoadXml(row["Metadata"].ToString());

                    XmlNamespaceManager nsManager = new XmlNamespaceManager(document.NameTable);
                    nsManager.AddNamespace("se", "urn:mtpg-com:mtps/2004/1/search");
                    nsManager.AddNamespace("xhtml", "http://www.w3.org/1999/xhtml");

                    XmlNodeList xmlNodes = document.SelectNodes("//xhtml:meta[@name='MSHKeywordK']/@content", nsManager);

                    foreach (XmlNode xmlNode in xmlNodes)
                    {
                        hhk.Add(xmlNode.InnerText, 
                            Path.Combine(chmSubDir, row["ContentId"].ToString() + ".htm"), 
                            row["Title"].ToString());
                    }
                }
            }

            hhk.Save();

            int lcid = new CultureInfo(locale).LCID;
            

            // Create TOC
            Hhc hhc = new Hhc(Path.Combine(chmDir, baseName + ".hhc"), locale);
            CreateHhc(nodes, hhc, contentDataSet);
            hhc.Close();

            using (FileStream fileStream = new FileStream(Path.Combine(chmDir, baseName + ".hhp"),
                FileMode.Create, FileAccess.Write, FileShare.None))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream))
                {
                    streamWriter.Write(template, chmFile, baseName, lcid, defaultPage, title);
                }
            }


            Stream rs = typeof(AppController).Assembly.GetManifestResourceStream(
                "PackageThis.Extra.Classic.css");

            FileStream fs = new FileStream(Path.Combine(withinChmDir, "Classic.css"),
                FileMode.Create, FileAccess.Write);

            StreamWriter sw = new StreamWriter(fs);
            StreamReader sr = new StreamReader(rs);

            sw.Write(sr.ReadToEnd());
            sw.Close();
            sr.Close();

            int numFiles = Directory.GetFiles(chmDir, "*", SearchOption.AllDirectories).Length;

            expectedLines = numFiles + 15;

        }

        public void Compile(IProgressReporter progressReporter)
        {
            this.progressReporter = progressReporter;

            // Use registry to find the compiler and invoke as a separate process.
            string key = @"HKEY_CURRENT_USER\Software\Microsoft\HTML Help Workshop";

            string install = (string)Registry.GetValue(key, "InstallDir", null);

            if (install == null)
            {
                // TODO: throw this instead to decouple from winforms.
                MessageBox.Show("You need to install the HTML Help Workshop.");
                return;
            }
            
            
            
            Process compileProcess = new Process();

            compileProcess.StartInfo.FileName = Path.Combine(install, "hhc.exe");
            compileProcess.StartInfo.Arguments = baseName + ".hhp";
            compileProcess.StartInfo.CreateNoWindow = true;
            compileProcess.StartInfo.WorkingDirectory = chmDir;
            compileProcess.StartInfo.UseShellExecute = false;
            compileProcess.StartInfo.RedirectStandardOutput = true;
//            compileProcess.OutputDataReceived += new DataReceivedEventHandler(CompilerOutputHandler);

            
            
            compileProcess.Start();

            StreamReader streamReader = compileProcess.StandardOutput;

//            compileProcess.BeginOutputReadLine();
//            compileProcess.WaitForExit();

            string line;

            // The UI doesn't update because stdout isn't flushed, so for now, just toss
            // the message and call the progressReporter with the same
            // message.
            while(streamReader.EndOfStream != true)
            {
                line = streamReader.ReadLine();

                // if (String.IsNullOrEmpty(line) == false)
                {
                    progressReporter.ProgressMessage("Compiling");
                }
            }

            compileProcess.Close();

        }

        public void CreateHhc(TreeNodeCollection nodeCollection, Hhc hhc, Content contentDataSet)
        {
            bool opened = false; // Keep track of opening or closing of TOC entries

            foreach (TreeNode node in nodeCollection)
            {
                if (node.Checked == true)
                {
                    MtpsNode mtpsNode = node.Tag as MtpsNode;

                    DataRow row = contentDataSet.Tables["Item"].Rows.Find(mtpsNode.targetAssetId);
                    string Url;

                    if (Int32.Parse(row["Size"].ToString()) == 0)
                        Url = null;
                    else
                    {
                        Url = Path.Combine(chmSubDir,
                            row["ContentId"].ToString() + ".htm");
                        
                        // Save the first page we see in the TOC as the default page as required
                        // by the chm.
                        if(defaultPage == null)
                            defaultPage = Url;
                    }


                    hhc.WriteStartNode(mtpsNode.title, Url);

                    opened = true;
                }
                if (node.Nodes.Count != 0 || node.Tag != null)
                {
                    CreateHhc(node.Nodes, hhc, contentDataSet);
                }
                if (opened)
                {
                    opened = false;
                    hhc.WriteEndNode();
                }
            }

        }

        public void Transform(string contentId, string metadataXml, string versionId, Content contentDataSet)
        {
            XsltArgumentList arguments = new XsltArgumentList();
            Link link = new Link(contentDataSet, links);
            XmlDocument metadata = new XmlDocument();
            string filename = Path.Combine(withinChmDir, contentId + ".htm");
            StreamReader sr = new StreamReader(filename);

            int codePage = new CultureInfo(locale).TextInfo.ANSICodePage;

            // We use these fallbacks because &nbsp; is unknown under DBCS like Japanese
            // and translated to ? by default.
            Encoding encoding = Encoding.GetEncoding(codePage,
                new EncoderReplacementFallback(" "),
                new DecoderReplacementFallback(" "));


            string xml = sr.ReadToEnd();
            sr.Close();



            metadata.LoadXml(metadataXml);

            arguments.AddParam("metadata", "", metadata.CreateNavigator());
            arguments.AddParam("version", "", versionId);
            arguments.AddParam("locale", "", locale);
            arguments.AddParam("charset", "", encoding.WebName);

            arguments.AddExtensionObject("urn:Link", link);

            TextReader tr = new StringReader(xml);
            XmlReader xr = XmlReader.Create(tr);

            using (StreamWriter sw = new StreamWriter(filename, false, encoding))
            {
                try
                {
                    xform.Transform(xr, arguments, sw);

                }
                catch (Exception ex)
                {
                    return;
                }
            }


        }


        private void CompilerOutputHandler(object sendingProcess, DataReceivedEventArgs message)
        {
            if (progressReporter != null)
            {
                progressReporter.ProgressMessage(message.Data);
            } 
        }



    }
}
