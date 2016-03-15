# Auto Updater Easy™
====================

* http://nuget.org/packages/unzip
* https://github.com/yallie/unzip

Description
-----------

This is a very easy auto update for any .Net applications. To use simply install Unzip package from Nuget:


* [Install-Package Unzip](http://nuget.org/packages/unzip)

Usage
-----

```C#
using (var unzip = new Unzip("zyan-sources.zip"))
{
	// list all files in the archive
	foreach (var fileName in unzip.FileNames)
	{
		Console.WriteLine(fileName);
	}

	// extract single file to a specified location
	unzip.Extract(@"source\Zyan.Communication\ZyanConnection.cs", "test.cs");

	// extract file to a stream
	unzip.Extract(@"source\Zyan.Communication\ZyanProxy.cs", stream);

	// extract all files from zip archive to a directory
	unzip.ExtractToDirectory(outputDirectory);
}
```

By
------

* [Eduardo Cecon](https://github.com/cecon) founder.