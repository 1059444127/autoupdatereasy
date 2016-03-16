using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using AutoUpdaterEasy;
using AutoUpdaterEasy.Entities;
using AutoUpdaterView;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutoUpdaterEasyTest
{
    [TestClass]
    public class DownloadFileTest
    {
        private static int _percent;
        [TestMethod]
        public void ShouldDownloadFile()
        {
            AutoUpdater.Initialize("http://update.arcnet.com.br/autoupdatereasytest/config.json", "1.0.0.0");            
            _percent = 0;
            AutoUpdater.Instance.DownloadProgressChanged += (e, a) => { Percent(a.ProgressPercentage); };
            AutoUpdater.Instance.DownloadCompleted += (e, a) => { Debug.WriteLine("FIM DO DOWNLOAD"); };
            AutoUpdater.Instance.Error += (e, a) => { Debug.WriteLine(a.Message); };
            AutoUpdater.Instance.Join();
            Assert.IsTrue(File.Exists($@".\{JsonConfig.Factory().PackageFileName}"));
        }

        [TestMethod]
        public void ShouldUnzipDownloadedFile()
        {
            AutoUpdater.Initialize("http://update.arcnet.com.br/autoupdatereasytest/config.json", "1.0.0.0");
            _percent = 0;
            AutoUpdater.Instance.DownloadProgressChanged += (e, a) => { Percent(a.ProgressPercentage); };
            AutoUpdater.Instance.DownloadCompleted += (e, a) => { Debug.WriteLine("FIM DO DOWNLOAD"); };
            AutoUpdater.Instance.Error += (e, a) => { Debug.WriteLine(a.Message); };
            AutoUpdater.Instance.Join();
            Assert.IsTrue(File.Exists($@".\{JsonConfig.Factory().PackageFileName}"));

            var form = new UiMain();
            var uIMainLoad = form.GetType().GetMethod("UiMain_Load", BindingFlags.NonPublic | BindingFlags.Instance);
            var tmrMainTick = form.GetType().GetMethod("TmrMain_Tick", BindingFlags.NonPublic | BindingFlags.Instance);
            Assert.IsNotNull(uIMainLoad);
            Assert.IsNotNull(tmrMainTick);
            uIMainLoad.Invoke(form, new object[] {this, EventArgs.Empty});
            try
            {
                tmrMainTick.Invoke(form, new object[] { this, EventArgs.Empty });
            }
            catch (TargetInvocationException ex)
            {
                
                throw ex.InnerException;
            }            
        }
        private static void Percent(int percent)
        {
            if (_percent == percent) return;
            _percent = percent;
            Debug.WriteLine($"{percent}%");
        }
    }
}
