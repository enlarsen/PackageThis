# Export to Mshc File dialog
Enter the required fields and click OK to package (zip) all downloaded files to a .mshc help file (this is the help file format for Visual Studio 2010 & 2012). We also create the associated .msha (install manifest) file which can be used later to install the help on a PC.

Note: Once export is finished you will find a duplicate .msha file called "HelpContentSetup.msha" since Help Viewer 1.0 (VS 2010 RTM with no service pack) requires this fixed name. Using any other name will cause your help install to fail.

![](MSHC Export Dialog_https://sites.google.com/site/mshcpics/_/rsrc/1350613664029/home/PackageThis006.png)

## Mshc File
Click the "browse" button to enter a name and directory for the output .mshc help file.

## Vendor Name
Help packages are stored internally in the help store under this name and is required for installing and uninstalling a help package. Do not use the reserved name "Microsoft" otherwise your help package will require signing to install. 

## Product Name & Book Name
Both Product name and Book name will be displayed in Help Manager when you install and uninstall help.

## Root Topic Title Suffix
This will make your new package easier to locate in the TOC. When set, the root topic(s) of the new .mshc help packaged will have its topic TOC title appended with this text. 

## Root Topic Parent
This defaults to "-1" (if left blank) which lets your package parent into the root of the VS Table of Contents. To parent under another TOC node, enter the ID of that node (use the topic's "Microsoft.Help.Id" meta tag). 

Warning: Your package topics may not appear in the VS Table of Contents (TOC)...
* If the specified parent topic ID does not exist in the target collection
* If the parent topic contains a different TopicVersion (see Topic Version below)

## Topic Version
Set a Topic Version number to stamp all files. This will allow all topics to show correctly in the Table of Contents.

Every topic contains a TopicVersion meta tag 
eg. <!-- meta name="Microsoft.Help.TopicVersion" content="110" /-->. Help topics from different versions of help (eg. Blend 3 and Blend 4) may have duplicate help IDs (meta tag "Microsoft.Help.Id"), and the TopicVersion tag helps keep the different versions separate. Unfortunately if there is a mixture of TopicVersion tags across the topics in the help package you will find that topics may be not show in the TOC. To fix this we overriding the TopicVersion of all topics and recommend you parent the help package in the root of the TOC (TocParent="-1").

