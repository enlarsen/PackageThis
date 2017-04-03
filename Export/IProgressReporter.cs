// Copyright (c) Microsoft Corporation.  All rights reserved.

using System;
using System.Collections.Generic;
using System.Text;

namespace PackageThis
{
    public interface IProgressReporter
    {
        void ProgressMessage(string message);
    }
}
