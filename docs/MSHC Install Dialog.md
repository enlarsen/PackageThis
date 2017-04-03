# Install Mshc Help File dialog
This dialog allows you to install your newly created .mshc help into an existing help collection.

![](MSHC Install Dialog_https://sites.google.com/site/mshcpics/_/rsrc/1350614945232/home/PackageThis007.png)

## .MSHA File

When PackageThis creates a .mshc help file it also creates a matching .msha (install manifest) file. The .msha file is passed to the Help Manager application to install our .mshc help file. Click the browse button to select the desired .msha file. 

Note: You will also find a duplicate .msha file called "HelpContentSetup.msha" as Help Viewer 1.0 (VS 2010 RTM with no service pack) requires this fixed name. Using any other name will cause install to fail.

## Help Viewer 1.x

Select this option to install into VS 2010 RTM (HV 1.0) or VS 2010 SP1 (HV 1.1) help.

The VS 2010 help catalog is identified by the Product/Version/Locale combination of  "vs/100/en-US" (your help system may be non-English and use a different locale). The Reset button will return you to the default values should you forget them.

## Help Viewer 2.x
Select this option to install into VS 2012 help.

The VS 2012 help catalog is identified by the Catalog/Locale combination of  "VisualStudio11/en-US" (your help system may be non-English and use a different locale). The Reset button will return you to the default values should you forget them.

## Open Help Manager link
Click this link to open the selected Help Manager (see Target Help Catalog radio buttons). You may want to do this to uninstalled an older version of help. Or see what help is currently installed.

## Install Button
Installs your help into the target help collection.

For HV 1.x we pass the .msha file and parameters to 
{{  c:\program files\Microsoft Help Viewer\v1.0\HelpLibManager.exe }}

For HV 2.x we pass the .msha file and parameters to 
{{ c:\program files\Microsoft Help Viewer\v2.0\HlpCtntMgr.exe }}

Note that HV 2.x runs help installs in the background. You will see a message in the Windows taskbar tray when the help installation has completed. You can also monitor the install by opening the HV 2.x help manager UI (in VS 2012 Help Viewer).

### Notes:
* Before installing help you must first uninstall any old help. Otherwise help appears to install OK but the new changes are not visible.
* When installing help that does not match the current Visual Studio help locale you must first do some work.  See also:
	* [Language and Branding](http://mshcmigrate.helpmvp.com/news/languageandbranding)
	* [Language Packs for Help Viewer 1.x](http://mshcmigrate.helpmvp.com/news/languagepacksformicrosofthelpviewer10)
