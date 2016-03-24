# Auto Updater Easy™

* https://www.nuget.org/packages/AutoUpdaterEasy
* https://github.com/cecon/autoupdatereasy

Description
-----------

This is a very easy auto update for any .Net applications. To use simply install Unzip package from Nuget:


* [Install-Package AutoUpdaterEasy](https://www.nuget.org/packages/AutoUpdaterEasy/)

Usage
-----

##### Startup Sample

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
##### Event Sample
```C#
		private void Instance_DownloadCompleted(object sender, EventArgs e)
        {
            if (Progress != 100) return;
            if (InvokeRequired)
            {
                Invoke(new EventHandler(Instance_DownloadCompleted), sender, e);
                return;
            }
            UiUpdaterProgress.Instance.Hide();
            AutoUpdater.Instance.UpdateView();
        }

        private void Instance_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (InvokeRequired)
            {
                Invoke(new EventHandler<DownloadProgressChangedEventArgs>(Instance_DownloadProgressChanged), sender, e);
                return;
            }
            Progress = e.ProgressPercentage;

            UiUpdaterProgress.Instance.Show();
            UiUpdaterProgress.Instance.SetStep(e);
        }
```

####Json file model [config.json]:
`The package always named 'update.zip'`

```
{
	"packageUrl": "http://yourdomain/yourapp/update.zip",
	"packageFileName": "update.zip",
	"packageConfigName": "config.json",
	"forceUpdate": "true",
	"checkEvery": "1",
	"intervalType": "second",
	"output": ".\\",
	"processStart": "yourapp.exe",
	"version": "1.0.0.1"
}
```
#### Summary of config json

 Key                                   | Description
---------------------------------------|-------------------------------------
packageUrl                             | URL of the package containing the updated application. `[required]` 
packageFileName                        | Name of the download file. `[optional] [default 'update.zip']`
packageConfigName                      | Name of the config json file. `[optional] [default 'config.json']`
forceUpdate                            | Indicator to force the update without the user's iteration. `[optional] [default 'False']`
checkEvery                             | It indicates the library to check for updates every X range. `[optional] [default '30']`
intervalType                           | defines the type of range: `second, minute, hour or day`. `[optional] [default 'minute']`
output                                 | Determines the extraction directory of package files. `[optional] [default '.\']`
version                                | Current version of the upgrade package. `[required]`
processStart                           | Name of the process to be started before extracting the update files.
---

`the key "output" can be overridden by setting "OutputPath" directly into your app.config`

#### Summary of Events

 Event                                 | Description
---------------------------------------|-------------------------------------
NewUpdate                              | It is triggered when the key "forceUpdate" is set to "False" and a new update is found.
DownloadProgressChanged                | It is triggered when a download is first, so your application can display the status of the download update.
DownloadCompleted                      | It is triggered when a download is complete, should be taken into account the percentage of the download progress, so, if it is different from "100", then the download failed.
Error                                  | It is triggered when an error occurs.
---

#### Copy always
`
When you install the AutoUpaderEasy nuget repository, the package creates into the your project folder an executable file named "AutoUpdateView.exe". This file will run out of your application to kill him and extract the update package files thus replacing the old files.
You need to ensure that the executable will be copied to the bin folder of your project.
`

![alt tag](https://raw.githubusercontent.com/cecon/autoupdatereasy/master/AutoUpdaterEasy/Resources/Manual.png)

#### IIS Web.config
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
---
`This is necessary to show static json file`

By
------

* [Eduardo Cecon](https://github.com/cecon) founder.
