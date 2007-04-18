// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;

namespace PackageThis
{
    public class Hxc
    {
        // Constructor
        public Hxc(string name, string title, string langId, string version, string copyright, string outputDirectory, Encoding encoding)
        {
            if (!Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception ex)
                {
                    throw ex;
                    
                }
                
            }
            try
            {


                System.Xml.XmlTextWriter writer = new System.Xml.XmlTextWriter(Path.Combine(outputDirectory, name+".hxc"), null);
                writer.Formatting = Formatting.Indented;
                writer.WriteStartDocument();
                writer.WriteDocType("HelpCollection", null, "MS-Help://Hx/Resources/HelpCollection.dtd", null);

                writer.WriteStartElement("HelpCollection");
                writer.WriteAttributeString("DTDVersion", "1.0");
                writer.WriteAttributeString("FileVersion", version);
                writer.WriteAttributeString("LangId", langId);
                writer.WriteAttributeString("Title", title);
                writer.WriteAttributeString("Copyright", copyright);

                writer.WriteStartElement("CompilerOptions");
                writer.WriteAttributeString("OutputFile", string.Format("{0}{1}", name, ".HxS"));
                writer.WriteAttributeString("CreateFullTextIndex", "Yes");
                writer.WriteAttributeString("CompileResult", "Hxs");
                writer.WriteAttributeString("StopWordFile", "msdnFTSstop_Unicode.stp");

                writer.WriteStartElement("IncludeFile");
                writer.WriteAttributeString("File", string.Format("{0}{1}", name, ".HxF"));
                writer.WriteEndElement();

                writer.WriteEndElement();

                writer.WriteStartElement("TOCDef");
                writer.WriteAttributeString("File", string.Format("{0}{1}", name, ".HxT"));
                writer.WriteEndElement();

                writer.WriteStartElement("KeywordIndexDef");
                writer.WriteAttributeString("File", string.Format("{0}{1}", name, "K.HxK"));
                writer.WriteEndElement();
                writer.WriteStartElement("KeywordIndexDef");
                writer.WriteAttributeString("File", string.Format("{0}{1}", name, "F.HxK"));
                writer.WriteEndElement();
                writer.WriteStartElement("KeywordIndexDef");
                writer.WriteAttributeString("File", string.Format("{0}{1}", name, "N.HxK"));
                writer.WriteEndElement();
                writer.WriteStartElement("KeywordIndexDef");
                writer.WriteAttributeString("File", string.Format("{0}{1}", name, "A.HxK"));
                writer.WriteEndElement();
                writer.WriteStartElement("KeywordIndexDef");
                writer.WriteAttributeString("File", string.Format("{0}{1}", name, "S.HxK"));
                writer.WriteEndElement();
                writer.WriteStartElement("KeywordIndexDef");
                writer.WriteAttributeString("File", string.Format("{0}{1}", name, "B.HxK"));
                writer.WriteEndElement();

                //  <ItemMoniker Name="!DefaultTOC" ProgId="HxDs.HxHierarchy" InitData="AnyString" />
                writer.WriteStartElement("ItemMoniker");
                writer.WriteAttributeString("Name", "!DefaultTOC");
                writer.WriteAttributeString("ProgId", "HxDs.HxHierarchy");
                writer.WriteAttributeString("InitData", "AnyString");
                writer.WriteEndElement();

                //  <ItemMoniker Name="!DefaultFullTextSearch" ProgId="HxDs.HxFullTextSearch" InitData="AnyString" />
                writer.WriteStartElement("ItemMoniker");
                writer.WriteAttributeString("Name", "!DefaultFullTextSearch");
                writer.WriteAttributeString("ProgId", "HxDs.HxFullTextSearch");
                writer.WriteAttributeString("InitData", "AnyString");
                writer.WriteEndElement();

                //  <ItemMoniker Name="!DefaultAssociativeIndex" ProgId="HxDs.HxIndex" InitData="A" />
                writer.WriteStartElement("ItemMoniker");
                writer.WriteAttributeString("Name", "!DefaultAssociativeIndex");
                writer.WriteAttributeString("ProgId", "HxDs.HxIndex");
                writer.WriteAttributeString("InitData", "A");
                writer.WriteEndElement();

                //  <ItemMoniker Name="!DefaultKeywordIndex" ProgId="HxDs.HxIndex" InitData="K" />
                writer.WriteStartElement("ItemMoniker");
                writer.WriteAttributeString("Name", "!DefaultKeywordIndex");
                writer.WriteAttributeString("ProgId", "HxDs.HxIndex");
                writer.WriteAttributeString("InitData", "K");
                writer.WriteEndElement();

                //  <ItemMoniker Name="!DefaultContextWindowIndex" ProgId="HxDs.HxIndex" InitData="F" />
                writer.WriteStartElement("ItemMoniker");
                writer.WriteAttributeString("Name", "!DefaultContextWindowIndex");
                writer.WriteAttributeString("ProgId", "HxDs.HxIndex");
                writer.WriteAttributeString("InitData", "F");
                writer.WriteEndElement();

                //  <ItemMoniker Name="!DefaultNamedUrlIndex" ProgId="HxDs.HxIndex" InitData="NamedUrl" />
                writer.WriteStartElement("ItemMoniker");
                writer.WriteAttributeString("Name", "!DefaultNamedUrlIndex");
                writer.WriteAttributeString("ProgId", "HxDs.HxIndex");
                writer.WriteAttributeString("InitData", "NamedUrl");
                writer.WriteEndElement();

                //  <ItemMoniker Name="!DefaultSearchWindowIndex" ProgId="HxDs.HxIndex" InitData="S" />
                writer.WriteStartElement("ItemMoniker");
                writer.WriteAttributeString("Name", "!DefaultSearchWindowIndex");
                writer.WriteAttributeString("ProgId", "HxDs.HxIndex");
                writer.WriteAttributeString("InitData", "S");
                writer.WriteEndElement();


                //  <ItemMoniker Name="!DefaultDynamicLinkIndex" ProgId="HxDs.HxIndex" InitData="B" />
                writer.WriteStartElement("ItemMoniker");
                writer.WriteAttributeString("Name", "!DefaultDynamicLinkIndex");
                writer.WriteAttributeString("ProgId", "HxDs.HxIndex");
                writer.WriteAttributeString("InitData", "B");
                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.Flush();
                writer.Close();
                writer = null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            

        }
    }
}
