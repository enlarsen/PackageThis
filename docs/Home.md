**August 25, 2008 release (1.3.1): Skip certain bad nodes in the TOC (@NoTitle, null target, etc.)**

**What It Does**

Package This is a GUI tool written in C# for creating help files (.chm and .hxs) from the content obtained from the [MSDN Library](http://msdn2.microsoft.com/library) or the [TechNet Library](http://technet.microsoft.com/library) via the [MSDN Content Service](http://services.msdn.microsoft.com/ContentServices/ContentService.asmx). You select the content you want from the table of contents, build a help file, and use the content offline. You are making personalized ebooks of MSDN or TechNet content. Both help file formats also give full text search and keyword search.

The code illustrates how to use the MSDN Content Service to retrieve documentation from MSDN or TechNet. It also shows how to build .hxs files and .chm files programmatically.

**Prerequisites**

Package This requires .NET 2.0, the .hxs SDK (MSHelp 2.0), [part of the Visual Studio 2005 SDK](http://go.microsoft.com/fwlink/?linkid=73702), and the [.chm SDK](http://msdn2.microsoft.com/library/ms669985) (HTML Help). If you just want to create .chm files, you don't need to download the .hxs components (and vice versa).

**Limitations**

There is a bug in HTML Help that will cause the index tab to show incorrect character encodings depending on your default locale. For example, you'll see this problem if you build a Japanese .chm file on a machine with the default locale set to English. If you change the default locale to Japanese (which requires a reboot) and rebuild the .chm file, it won't show this problem (even on an English machine).

To view .hxs files, you'll need an .hxs viewer.

**Content Service Logging**

Package This sends a string ("PackageThisGui") to the server to identify its requests in the web server logs. The idea is to determine whether anyone is actually using the tool.


