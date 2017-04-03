This page is a quick tutorial on how to use PackageThis.

# 1. Select area to download (MSDN or TechNet Library)
![](PackageThis Tutorial_https://sites.google.com/site/mshcpics/home/PackageThis001.png)

# 2. Select a language to download
![](PackageThis Tutorial_https://sites.google.com/site/mshcpics/home/PackageThis002.png)

# 3. Download files (checking items)
To automatically download a page and all sub pages, right-click the required page node and click "Download All". All pages and associated images will be downloaded automatically and corresponding tree items checked. 

![](PackageThis Tutorial_https://sites.google.com/site/mshcpics/home/PackageThis003.png)

To download a single page (and associated images) check the corresponding box.
To remove a single page uncheck the node.
To remove a node's and all its children's downloads, right-click the node and select "Remove All".

## Important: Microsoft recommend you keep package file count < 10,000 files
Some people try and download all files under a root tree node. This can download over 100,000 files and crash the application. Best to download a section; Package; Download another section; Package etc. Microsoft themselves ship large collections made up of several 10,000 file packages.

# 4. Create Help Package
Use the File menu to create either a .CHM or .HxS or .MSHC help file package.
All TreeView checked items will be packed into the output help file.

![](PackageThis Tutorial_https://sites.google.com/site/mshcpics/_/rsrc/1349398466121/home/PackageThis005.png)

# 5. Install VS 2010/2012 Help 
You can run "File > Install Mshc Help File" to install .Mshc help files. Or run the appropriate VS Help Manager to install your help.

# 6. Create Next Package
MSDN holds around a million topics. Don't try and create very large packages. 
Uncheck all items (or restart the application) and begin checking/downloading the next section to package (remember to keep the file count < 10,000 items). 

