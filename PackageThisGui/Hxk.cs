// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;

namespace PackageThis
{
    public class Hxk
    {
        // Constructor
        public Hxk(string name, string indexName, string outputDirectory)
        {
            if (indexName.Length != 1)
                throw new ArgumentException("indexName too long (should be one character).");

            if(!(indexName.ToLower()[0] >= 'a' && indexName.ToLower()[0] <= 'z'))
                throw new ArgumentException("indexName is not between 'a' and 'z'");

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
                using (StreamWriter writer = new StreamWriter(Path.Combine(outputDirectory, name + indexName + ".hxk")))
                {
                    writer.Write(string.Format("<?xml version=\"1.0\" encoding=\"utf-8\"?><!DOCTYPE HelpIndex SYSTEM \"MS-Help://Hx/Resources/HelpIndex.dtd\"><HelpIndex Name=\"{0}\" DTDVersion=\"1.0\" />", indexName));
                    writer.Flush();
                    writer.Close();
                }
 
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
