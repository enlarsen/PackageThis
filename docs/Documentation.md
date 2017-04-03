![](Documentation_https://sites.google.com/site/mshcpics/_/rsrc/1349399233053/home/PackageThisShot01.png)

# Latest Release
* The main download has not been updated for a long time and is very buggy. Please download from the [patch folder](http://packagethis.codeplex.com/SourceControl/PatchList.aspx) (Note: this is actually a full release version not a patch). Or download from the [PackageThis VS Gallery page](http://visualstudiogallery.msdn.microsoft.com/en-us/626677ab-c35f-4f3d-9f7c-6b471b1195dd?SRC=Home).

# Requirements
* PackageThis 1.3.x+ requires [.NET Framework 4.0](http://msdn.microsoft.com/en-us/netframework/aa569263.aspx)
* Internet connection for downloading content.
* To compile .CHM help [download HTML Help Workshop](http://www.microsoft.com/downloads/en/details.aspx?FamilyID=00535334-c8a6-452f-9aa0-d597d16580cc&displaylang=en)
* To compile .HxS help [download VS 2005 or 2008 SDK](http://www.microsoft.com/downloads/en/details.aspx?FamilyId=59EC6EC3-4273-48A3-BA25-DC925A45584D&displaylang=en) (contains the MS Help 2.x compiler). You need VS 2002\2003\2005\2008 to view help.
* No special requirements for compiling .MSHC help. You will need VS 2010 or VS 2012 installed to view your help package.

# Overview
Select parts of MSDN or Technet library and package to either:
* HTML Help 1.x (.Chm help file) -- General Windows application help.
* MS Help 2.x (.HxS help file) -- VS 2002\2003\2005\2008 help.
* MS Help Viewer 1.x (.MSHC help file) -- VS 2010 help.
* MS Help Viewer 2.x (.MSHC help file) -- VS 2012 help.

# Usage
# Select the source library -- Choose either Technet or MSDN library from the _Library menu_.
# Select a source language -- Choose a source language from the _Locale menu_.
# Select pages to download by checking TreeView menu items. Use the right-click menu to download entire TOC branches.
# Choose Export from the _File menu_. 
## See also [PackageThis Tutorial](PackageThis-Tutorial) | [Schedule a Download](Schedule)

# Trouble Shooting
If help files are not created it's because there are syntax errors in the help source (MSDN is not always perfect).

After Exporting .Chm or .HxS look for the xxx.ProjectSource folder in the output folder (It is your responsibility to delete these folders). They hold the help project source files used to build the help file. Open the xxx.log file to view compiler errors and fix your source.
* Recompile .Chm help -- Run C:\Program Files (x86)\HTML Help Workshop\hhc.exe  .\xxx.ProjectSource\xxx.HHP   (or run the HHW.exe GUI)
* Recompile .HxS help -- Run C:\Program Files (x86)\Common Files\microsoft shared\Help 2.0 Compiler\hxcomp.exe  .\xxx.ProjectSource\xxx.hxc
* Mshc is different. We don't need to set up a project and compile the help. We just need to zip the download and give the zip file a .mshc file extension. See below for more info on Mshc.

# .MSHC Help

* **File > Export to Mshc file** --- Downloads the selected pages to a temp files folder and packages all source to a .MSHC zip file. It also creates _HelpContentSetup.msha_ (Used to install the help). If required you can view the source by renaming the .mshc file to .zip and decompile.
* **File > Install Mshc Help file** -- Opens _Help Library Manager_ (used to install\uninstall help books). This is run in Administrator mode (required for help installation).
**Important Mshc Tips**:
* **Reinstalling** -- Help packages wont install unless you first uninstall the older package (with the same name etc).
* **Install Errors** -- During installation you may see an exception. This means there is a syntax error in the help source. The event log shows the error description but unfortunately not the specific file in error.  To track down the syntax error(s) see [PackageThis notes](http://mshcmigrate.helpmvp.com/news/packagethis)
* **Foreign language help**  -- If you install say English VS 2010 (so your local VS help is en-us), but want to download say Russian (ru-ru) to a vs/100/ru-ru catalog you will find that creating the catalog will fail. See [Language Packs for MS Help Viewer 1.x](http://mshcmigrate.helpmvp.com/news/languagepacksformicrosofthelpviewer10) for the fix.
* **Help Viewer** -- The RTM release of VS 2010 is a very basic help viewer. For a better experience please install VS 2001 SP1 (Help Viewer 1.1 now uses a standalone viewer), or use  [H3Viewer](http://mshcmigrate.helpmvp.com/viewer).
## See also [MSHC Export Dialog](MSHC-Export-Dialog) | [MSHC Install Dialog](MSHC-Install-Dialog)

# Known Problems with PackageThis
* ToDo: Caching of downloads. Recovery so we can see what crashed and continue from crash point.