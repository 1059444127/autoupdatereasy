# Auto Updater Easy™
====================

* https://www.nuget.org/packages/AutoUpdaterEasy/p
* https://github.com/cecon/autoupdatereasy

Description
-----------

This is a very easy auto update for any .Net applications. To use simply install Unzip package from Nuget:


* [Install-Package AutoUpdaterEasy](https://www.nuget.org/packages/AutoUpdaterEasy/)

Usage
-----

```C#
		/// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            AutoUpdater.Initialize("http://yourdomain/yourapp/config.json", Application.ProductVersion);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new UiMain());
            AutoUpdater.Instance.Stop();
        }
```

*Json file model [config.json]:
***The package name always be 'update.zip'

```
{
	"packageUrl":"http://yourdomain/yourapp/update.zip",
	"forceUpdate":"true",
	"checkEvery":"10",
	"processKill":"myapp",
	"output":".\",
	"version" : "1.0.0.1",
	"checkEveryType":"second"
}
```
### Summary of config json

 Key                                   | Description
---------------------------------------|-------------------------------------
packageUrl                             | URL of the package containing the updated application. `[required]` 
forceUpdate                            | Indicator to force the update without the user's iteration. `[optional] [default 'False']`
checkEvery                             | It indicates the library to check for updates every X range. `[optional] [default '30']`
intervalType                           | defines the type of range: `second, minute, hour or day`. `[optional] [default 'minute']`
output                                 | Determines the extraction directory of package files. `[optional] [default '.\']`
version                                | Current version of the upgrade package. `[required]`
processKill                            | Name of the process to be closed before extracting the update files.
---

**the key "output" can be overridden by setting "OutputPath" directly into your app.config

### Summary of Events

 Event                                 | Description
---------------------------------------|-------------------------------------
NewUpdate                              | It is triggered when the key "forceUpdate" is set to "False" and a new update is found.
DownloadProgressChanged                | It is triggered when a download is first, so your application can display the status of the download update.
DownloadCompleted                      | It is triggered when a download is complete, should be taken into account the percentage of the download progress, so, if it is different from "100", then the download failed.
Error                                  | It is triggered when an error occurs.
---


### IIS Web.config
```
<?xml version="1.0"?>
 
<configuration>
    <system.webServer>
        <staticContent>
            <mimeMap fileExtension=".json" mimeType="application/json" />
     </staticContent>
    </system.webServer>
</configuration>
```
By
------

* [Eduardo Cecon](https://github.com/cecon) founder.