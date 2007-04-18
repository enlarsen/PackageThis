// Copyright (c) Microsoft Corporation.  All rights reserved.
//
using System;
using System.Collections.Generic;
using System.Text;

namespace ContentServiceLibrary
{
    public class Image
    {
        public string name;
        public string imageFormat;
        public byte[] data;

        public Image(string name, string imageFormat, byte[] data)
        {
            this.name = name;
            this.imageFormat = imageFormat;
            this.data = data;
        }

        public string filename
        {
            get
            {
                return name + "." + imageFormat;
            }
        }
    }
}
