// Copyright (c) Microsoft Corporation.  All rights reserved.
//
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using PackageThis.com.microsoft.msdn.services;


namespace ContentServiceLibrary
{
    static public class Utility
    {
        // Use the standard root node to get a list of locales available.
        static public SortedDictionary<string, string> GetLocales()
        {
            SortedDictionary<string, string> locales = new SortedDictionary<string, string>();
            getContentRequest request = new getContentRequest();
            ContentService proxy = new ContentService();
            getContentResponse response;

            request.contentIdentifier = rootContentItem.contentId;
            request.locale = "en-US"; // Now required otherwise an exception.

            try
            {
                response = proxy.GetContent(request);
            }
            catch
            {
                locales.Add("English (United States)", "en-us");
                return locales;
            }

            foreach (availableVersionAndLocale av in response.availableVersionsAndLocales)
            {
                // Use the DisplayName as the key instead of the locale because
                // that's how we want the collection sorted.
                string displayName = new CultureInfo(av.locale).DisplayName;

                if (locales.ContainsKey(displayName) == false)
                    locales.Add(displayName, av.locale.ToLower());

            }

            return locales;

        }

    }
}
