// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;

namespace PackageThis
{
    public class Link
    {
        private Content contentDataSet;
        private Dictionary<string, string> links;

        public Link(Content contentDataSet, Dictionary<string, string> links)
        {
            this.contentDataSet = contentDataSet;
            this.links = links;
        }

        // Called by the transform to lookup an href. If it begins with "AssetId:", we lookup
        // its aKeyword.
        public string Resolve(string href, string version, string locale, bool returnContentId)
        {
            if(href.ToLower().StartsWith("assetid:") == true)
            {
                string assetId = HttpUtility.UrlDecode(href.Remove(0, "assetid:".Length).ToLower());

                DataRow row = contentDataSet.Tables["Item"].Rows.Find(assetId);

                if (row == null)
                {
                    string target = assetId;

                    if (links.ContainsKey(assetId) == true)
                        target = links[assetId];

                    // Added d=ide for a view that hides the TOC.
                    // TODO: change from msdn2 when possible.

                    return "http://msdn2.microsoft.com/library/" + target + "(" + version + "," +
                        locale + ",d=ide).aspx";
                }

                if (returnContentId == true)
                    return row["ContentId"].ToString();
                else
                    return assetId;                
            }
            return href;
        } 
    }
}
