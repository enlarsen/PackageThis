// Copyright (c) Microsoft Corporation.  All rights reserved.
//
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Globalization;
using System.Xml;
using System.IO;
using System.Web;
using System.Windows.Forms;
using System.Xml.Xsl;
using MSHelpCompiler;
using ContentServiceLibrary;

// Version variables in this code are collection + "." + version, but ContentItem requires a
// version and collection, eg. version="10", collection="MSDN"

namespace PackageThis
{
    public class MtpsNode
    {
        public string navAssetId;
        public string navLocale;
        public string navVersion;

        public string targetAssetId;
        public string targetContentId;
        public string targetLocale;
        public string targetVersion;

        public string title;

        public bool external;


        public MtpsNode(string navAssetId, string navLocale, string navVersion,
            string targetContentId, string targetAssetId, string targetLocale, 
            string targetVersion, string title)
        {
            this.navAssetId = navAssetId;
            this.navLocale = navLocale;
            this.navVersion = navVersion;

            this.targetContentId = targetContentId;

            this.targetAssetId = targetAssetId.ToLower().StartsWith("assetid:") == true ?
                targetAssetId.Remove(0,8) : targetAssetId;

            this.targetLocale = targetLocale;
            this.targetVersion = targetVersion;
            

            this.title = title;

            this.external = targetAssetId.ToLower().Contains("http:");
        }

    }

    public class AppController
    {
        private string application = "PackageThisGui";
        private string topNode;
        private string locale;
        private string version;
        private TreeView tocControl;
        private string workingDir;

        private string withinHxsDir;
        private string rawDir;
        private string hxsDir;
        private string hxsSubDir = "html";

        public Dictionary<string, string> links = new Dictionary<string, string>();


        static private Stream resourceStream = typeof(AppController).Assembly.GetManifestResourceStream(
            "PackageThis.Extra.hxs.xslt");
        static private XmlReader transformFile = XmlReader.Create(resourceStream);
        static private  XslCompiledTransform xform = null;

        // static private StreamWriter sw;

        public AppController(string topNode, string locale, string version, TreeView tocControl, string workingDir)
        {
            this.topNode = topNode;
            this.locale = locale;
            this.version = version;
            this.tocControl = tocControl;
            this.workingDir = workingDir;

            this.rawDir = Path.Combine(workingDir, "raw");
            this.hxsDir = Path.Combine(workingDir, "hxs");
            this.withinHxsDir = Path.Combine(hxsDir, hxsSubDir);


            if (xform == null)
            {
                xform = new XslCompiledTransform(true);
                xform.Load(transformFile);
            }

            Directory.CreateDirectory(rawDir);

            ContentItem contentItem = lookupTOCNode(topNode, locale, version);

            processNodeList(contentItem, tocControl.Nodes);
        }

        ContentItem lookupTOCNode(string contentIdentifier, string locale, string version)
        {
            string[] splitVersion = version.Split(new char[] {'.'});

            ContentItem contentItem = new ContentItem(contentIdentifier, locale, splitVersion[1], 
                splitVersion[0], application);
            contentItem.Load(false, false);

            return contentItem;
        }

        void processNodeList(ContentItem contentItem, TreeNodeCollection tnCollection)
        {
            XmlDocument xmlDocument = new XmlDocument();
            XmlNamespaceManager nsm = new XmlNamespaceManager(xmlDocument.NameTable);
            nsm.AddNamespace("toc", "urn:mtpg-com:mtps/2004/1/toc");
            nsm.AddNamespace("mtps", "http://msdn2.microsoft.com/mtps");
            nsm.AddNamespace("asp", "http://msdn2.microsoft.com/asp");
            nsm.AddNamespace("mshelp", "http:/msdn.microsoft.com/mshelp");

            if(string.IsNullOrEmpty(contentItem.toc) == true)
                return;

            xmlDocument.LoadXml(contentItem.toc);

            XmlNodeList nodes = xmlDocument.SelectNodes("/toc:Node/toc:Node", nsm);


            foreach(XmlNode node in nodes)
            {
                string title = GetAttribute(node.Attributes["toc:Title"]);

                string target = HttpUtility.UrlDecode(GetAttribute(node.Attributes["toc:Target"]));


                string targetLocale = GetAttribute(node.Attributes["toc:TargetLocale"]);
                string targetVersion = GetAttribute(node.Attributes["toc:TargetVersion"]);

                string subTree = HttpUtility.UrlDecode(GetAttribute(node.Attributes["toc:SubTree"]));
                string subTreeVersion = GetAttribute(node.Attributes["toc:SubTreeVersion"]);
                string subTreeLocale = GetAttribute(node.Attributes["toc:SubTreeLocale"]);
                string isPhantom = GetAttribute(node.Attributes["toc:IsPhantom"]);

                if (isPhantom != "true" && title != "@PhantomNode" && string.IsNullOrEmpty(title) != true)
                {
                    TreeNode treeNode = tnCollection.Add(title);

                    MtpsNode mtpsNode = new MtpsNode(subTree, subTreeLocale, subTreeVersion,
                        contentItem.contentId, target, targetLocale, targetVersion, title);

                    treeNode.Tag = mtpsNode;

                    // Mark nodes red that point outside this server
                    if (mtpsNode.external == true)
                    {
                        treeNode.ForeColor = System.Drawing.Color.Red;

                        //treeNode.NodeFont = new System.Drawing.Font(tocControl.Font, System.Drawing.FontStyle.Italic);
                    }


                    if (subTree != null)
                    {
                        // Add a + as the title so any node with subnodes is expandable.
                        // Only load the subnodes when user expands this node.
                        // We rely on Tag == null rather than Text == "+" in case
                        // there really is a node with a title of "+".
                        treeNode.Nodes.Add("+");

                    }
                }
                else
                {
                    if (subTree != null)
                    {
                        // TODO: add a ContentItem constructor that takes a combined 
                        // version string: MSDN.10, Office.12, etc.

                        string[] splitVersion = subTreeVersion.Split(new char[] {'.'});
                        ContentItem childContentItem = new ContentItem(subTree, subTreeLocale, 
                            splitVersion[1], splitVersion[0], application);
                        childContentItem.Load(false);

                        processNodeList(childContentItem, tnCollection);
                    }
                }
            }

        }



        private string GetAttribute(XmlAttribute attribute)
        {
            return (attribute == null ? null : attribute.Value);
        }

        public void ExpandNode(TreeNode node)
        {
            if (node.Nodes[0].Tag == null)
            {
                ContentItem contentItem;
                MtpsNode mtpsNode = node.Tag as MtpsNode;

                try
                {
                    contentItem = lookupTOCNode(mtpsNode.navAssetId, mtpsNode.navLocale,
                        mtpsNode.navVersion);
                    processNodeList(contentItem, node.Nodes);
 
                }
                catch
                {
//                    mtpsNode.external = true;
//                    node.ForeColor = System.Drawing.Color.Red;
                }
                
                node.Nodes.Remove(node.Nodes[0]); // This removes the node labeled "+"

            }
        }

        public void UncheckNodes(TreeNode node)
        {

            // Events are created even if the checked state doesn't change.
            // That confuses the event handler because it assumes that the
            // event is only fired on a state change.


            if(node.Checked == true)
                node.Checked = false;

            foreach (TreeNode currentNode in node.Nodes)
            {
                if(currentNode.Checked == true)
                    currentNode.Checked = false;

                if (currentNode.Nodes != null)
                    UncheckNodes(currentNode);
            }
        }


        public bool WriteContent(TreeNode node, Content contentDataSet)
        {
            DataRow row;
            MtpsNode mtpsNode = node.Tag as MtpsNode;
            
            string[] splitVersion = mtpsNode.targetVersion.Split(new char[] {'.'});
            
            ContentItem contentItem = new ContentItem("AssetId:" + mtpsNode.targetAssetId, mtpsNode.targetLocale,
                splitVersion[1], splitVersion[0], application);

            try
            {
                contentItem.Load(true);
            }
                
            catch
            {
                node.ForeColor = System.Drawing.Color.Red;
                return false; // tell the event handler to reject the click.
            }

            if (contentDataSet.Tables["Item"].Rows.Find(mtpsNode.targetAssetId) == null)
            {
                row = contentDataSet.Tables["Item"].NewRow();
                row["ContentId"] = contentItem.contentId;
                row["Title"] = mtpsNode.title;
                row["VersionId"] = mtpsNode.targetVersion;
                row["AssetId"] = mtpsNode.targetAssetId;
                row["Pictures"] = contentItem.numImages;
                row["Size"] = contentItem.xml == null ? 0 : contentItem.xml.Length;

                // If we get no meta/search data, plug in NOP search data because
                // we can't LoadXml an empty string nor pass null navigators to
                // the transform.
                if (string.IsNullOrEmpty(contentItem.metadata) == true)
                    contentItem.metadata = "<se:search xmlns:se=\"urn:mtpg-com:mtps/2004/1/search\" />";
                
                row["Metadata"] = contentItem.metadata;

                contentDataSet.Tables["Item"].Rows.Add(row);
            }
            if (contentDataSet.Tables["ItemInstance"].Rows.Find(node.FullPath) == null)
            {
                row = contentDataSet.Tables["ItemInstance"].NewRow();
                row["ContentId"] = contentItem.contentId;
                row["FullPath"] = node.FullPath;
                contentDataSet.Tables["ItemInstance"].Rows.Add(row);
            }
            foreach (string imageFilename in contentItem.ImageFilenames)
            {
                row = contentDataSet.Tables["Picture"].NewRow();
                row["ContentId"] = contentItem.contentId;
                row["Filename"] = imageFilename;
            }


            if (string.IsNullOrEmpty(contentItem.links) == false)
            {
                XmlDocument linkDoc = new XmlDocument();
                XmlNamespaceManager nsm = new XmlNamespaceManager(linkDoc.NameTable);
                nsm.AddNamespace("k", "urn:mtpg-com:mtps/2004/1/key");
                nsm.AddNamespace("mtps", "urn:msdn-com:public-content-syndication");

                linkDoc.LoadXml(contentItem.links);

                XmlNodeList nodes = linkDoc.SelectNodes("//mtps:link", nsm);

                foreach (XmlNode xmlNode in nodes)
                {
                    XmlNode assetIdNode = xmlNode.SelectSingleNode("mtps:assetId", nsm);
                    XmlNode contentIdNode = xmlNode.SelectSingleNode("k:contentId", nsm);

                    if (assetIdNode == null || contentIdNode == null)
                        continue;

                    string assetId = assetIdNode.InnerText;
                    string contentId = contentIdNode.InnerText;

                    if (string.IsNullOrEmpty(assetId) == false)
                    {
                        // Remove "assetId:" from front
                        assetId = HttpUtility.UrlDecode(assetIdNode.InnerText.Remove(0, "assetId:".Length));

                        if (links.ContainsKey(assetId) == false)
                        {
                            links.Add(assetId, contentId);
                        }
                    }

                }
            }
            contentItem.Write(rawDir);

            return true;

            
        }

        public void RemoveContent(TreeNode node, Content contentDataSet)
        {
            if (node.Tag != null)
            {
                MtpsNode mtpsNode = node.Tag as MtpsNode;

                DataRow row = contentDataSet.Tables["ItemInstance"].Rows.Find(node.FullPath);

                if (row != null)
                {
                    DataRow parentRow = row.GetParentRow("FK_Item_ItemInstance");
                    contentDataSet.Tables["ItemInstance"].Rows.Remove(row);
                    int count = parentRow.GetChildRows("FK_Item_ItemInstance").Length;

                    if (count == 0) // If there are no refs to this item, delete it and its files
                    {
                        foreach (string file in Directory.GetFiles(rawDir, 
                            parentRow["ContentId"].ToString() + "*"))
                        {
                            File.Delete(file);
                        }
                        contentDataSet.Tables["Item"].Rows.Remove(parentRow);
                    }

                }

            }
        }

        public void CreateChm(string chmFile, string title, string locale, Content contentDataSet)
        {
            Chm chm = new Chm(workingDir, title,
                chmFile, locale, tocControl.Nodes, contentDataSet, links);

            chm.Create();

            ExportProgressForm progressForm = new ExportProgressForm(chm, chm.expectedLines);

            progressForm.ShowDialog();

        }

        public void CreateHxs(string hxsFile, string title, string copyright, string locale, 
            Content contentDataSet)
        {
            if (Directory.Exists(hxsDir) == true)
            {
                Directory.Delete(hxsDir, true);
            }

            Directory.CreateDirectory(hxsDir);
            Directory.CreateDirectory(withinHxsDir);

            foreach (string file in Directory.GetFiles(rawDir))
            {
                File.Copy(file, Path.Combine(withinHxsDir, Path.GetFileName(file)), true);
            }
            
            // This will be used as a base name for forming all of the MSHelp files.
            string baseFilename = Path.GetFileNameWithoutExtension(hxsFile);

            foreach (DataRow row in contentDataSet.Tables["Item"].Rows)
            {
                if (Int32.Parse(row["Size"].ToString()) != 0)
                {
                    Transform(row["ContentId"].ToString(),
                        row["Metadata"].ToString(), row["VersionId"].ToString(), contentDataSet);
                }
            }


            // Create TOC
            Hxt hxt = new Hxt(Path.Combine(hxsDir, baseFilename + ".hxt"), Encoding.UTF8);
            CreateHxt(tocControl.Nodes, hxt, contentDataSet);
            hxt.Close();

            
            CreateHxks(baseFilename);

            WriteExtraFiles();

            Hxf hxf = new Hxf(Path.Combine(hxsDir, baseFilename + ".hxf"), Encoding.UTF8);

            string[] files = Directory.GetFiles(hxsDir, "*", SearchOption.AllDirectories);

            foreach(string file in files)
            {
                hxf.WriteLine(file.Replace(hxsDir, ""));
            }

            hxf.Close();

            string lcid = new CultureInfo(locale).LCID.ToString();
            Hxc hxc = new Hxc(baseFilename, title, lcid, "1.0", copyright, hxsDir, Encoding.UTF8);

            int numHtmlFiles = Directory.GetFiles(hxsDir, "*.htm", SearchOption.AllDirectories).Length;
            int numFiles = Directory.GetFiles(hxsDir, "*", SearchOption.AllDirectories).Length;

            // This gives the number of information lines output by the compiler. It
            // was determined experimentally, and should give some means of making an
            // accurate progress bar during a compile.
            // Actual equation is numInfoLines = 2*numHtmlFiles + (numFiles - numHtmlFiles) + 6
            // After factoring, we get this equation
            int expectedLines = numHtmlFiles + numFiles + 6;
            
            Hxs hxs = new Hxs(Path.Combine(Path.GetFullPath(hxsDir), baseFilename + ".hxc"),
                Path.GetFullPath(hxsDir),
                Path.GetFullPath(hxsFile));


            ExportProgressForm hxsProgressForm = new ExportProgressForm(hxs, expectedLines);

            hxsProgressForm.ShowDialog();


        }
        public void CreateHxt(TreeNodeCollection nodeCollection, Hxt hxt, Content contentDataSet)
        {
            bool opened = false; // Keep track of opening or closing of TOC entries in the .hxt

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
                        Url = Path.Combine(hxsSubDir,
                            row["ContentId"].ToString() + ".htm");
                    

                    hxt.WriteStartNode(mtpsNode.title, Url);
                        
                    opened = true;
                }
                if (node.Nodes.Count != 0 || node.Tag != null)
                {
                   CreateHxt(node.Nodes, hxt, contentDataSet);
                }
                if (opened)
                {
                    opened = false;
                    hxt.WriteEndNode();
                }
            }

        }

        void CreateHxks(string baseFileName)
        {
            
            Hxk hxk = new Hxk(baseFileName, "A", hxsDir);
            hxk = new Hxk(baseFileName, "B", hxsDir);
            hxk = new Hxk(baseFileName, "F", hxsDir);
            hxk = new Hxk(baseFileName, "K", hxsDir);
            hxk = new Hxk(baseFileName, "N", hxsDir);
            hxk = new Hxk(baseFileName, "S", hxsDir);
        }
        
        // Includes stoplist and stylesheet
        void WriteExtraFiles()
        {
            Stream resourceStream = typeof(Program).Assembly.GetManifestResourceStream(
                "PackageThis.Extra.Classic.css");

            WriteExtraFile(resourceStream, "Classic.css");

            // TODO: Locate stop lists for other locales and add them to the project.
            resourceStream = typeof(Program).Assembly.GetManifestResourceStream(
                "PackageThis.Extra.msdnFTSstop_Unicode.stp");

            WriteExtraFile(resourceStream, "msdnFTSstop_Unicode.stp");

        }
        
        void WriteExtraFile(Stream resourceStream, string filename)
        {
            FileStream fs = new FileStream(Path.Combine(hxsDir, filename), 
                FileMode.Create, FileAccess.Write);

            StreamWriter sw = new StreamWriter(fs);
            StreamReader sr = new StreamReader(resourceStream);

            sw.Write(sr.ReadToEnd());
            sw.Close();
            sr.Close();

        }

        public void Transform(string contentId, string metadataXml, string versionId, Content contentDataSet)
        {
            XsltArgumentList arguments = new XsltArgumentList();
            Link link = new Link(contentDataSet, links);
            XmlDocument metadata = new XmlDocument();
            string filename = Path.Combine(withinHxsDir, contentId + ".htm");
            StreamReader sr = new StreamReader(filename);

            string xml = sr.ReadToEnd();
            sr.Close();

            metadata.LoadXml(metadataXml);

            arguments.AddParam("metadata", "", metadata.CreateNavigator());
            arguments.AddParam("version", "", versionId);
            arguments.AddParam("locale", "", locale);

            arguments.AddExtensionObject("urn:Link", link);

            TextReader tr = new StringReader(xml);
            XmlReader xr = XmlReader.Create(tr);

            using (StreamWriter sw = new StreamWriter(filename, false, Encoding.UTF8))
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


    }
}
