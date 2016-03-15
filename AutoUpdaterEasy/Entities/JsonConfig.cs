using System;
using System.IO;
using System.Net;
using AutoUpdaterEasy.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace AutoUpdaterEasy.Entities
{
    public class JsonConfig
    {                
        public event EventHandler DownloadCompleted;
        public event EventHandler<DownloadProgressChangedEventArgs> ProgressChanged;
        private const string FilePath = @".\UpdaterConfig.json";
        public const string PackagePath = @".\Update.zip";
        public static JsonConfig Factory(string jsonUrl)
        {
            try
            {
                var file = JsonConvert.DeserializeObject<JsonConfig>(ReadUrl(jsonUrl));
                if (string.IsNullOrWhiteSpace(file.IntervalType))
                {
                    file.IntervalType = "minute";
                }
                if (string.IsNullOrWhiteSpace(file.PackageUrl)) throw new JsonConfigException();
                if (string.IsNullOrWhiteSpace(file.Version)) throw new JsonConfigException();
                if (string.IsNullOrWhiteSpace(file.ProcessKill))
                {
                    file.ProcessKill = "";
                }
                if ("minute,second,hour,day".IndexOf(file.IntervalType, StringComparison.Ordinal) == -1) throw new JsonConfigException();
                return file;
            }
            catch (Exception)
            {
                throw new JsonConfigException();
            }            
        }

        public static JsonConfig Factory()
        {
            try
            {
                var file = new JsonConfig();
                if (File.Exists(FilePath))
                {
                    using (var sr = new StreamReader(FilePath))
                    {
                        file = JsonConvert.DeserializeObject<JsonConfig>(sr.ReadToEnd());
                    }
                }
                if (string.IsNullOrWhiteSpace(file.IntervalType))
                {
                    file.IntervalType = "minute";
                }
                if ("second,minute,hour,day".IndexOf(file.IntervalType, StringComparison.Ordinal) == -1) throw new JsonConfigException();
                return file;
            }
            catch (Exception)
            {
                throw new JsonConfigException();
            }
        }

        public JsonConfig()
        {
            IntervalType = "minute";
            CheckEvery = 30;
        }
        public string PackageUrl { get; set; }
        public bool ForceUpdate { get; set; }
        public string ProcessKill { get; set; }
        public int CheckEvery { get; set; }
        public string IntervalType { get; set; }
        public string Version { get; set; }

        public int GetMilliseconds()
        {
            switch (IntervalType)
            {
                case "second":
                    return CheckEvery*1000;
                case "minute":
                    return CheckEvery * 60 * 1000;
                case "hour":
                    return CheckEvery * 60 * 60 * 1000;
                case "day":
                    return CheckEvery * 24 * 60 * 60 * 1000;
            }
            return 30*60*1000;
        }

        public bool IsNewVersion(string version)
        {                                    
            var currentVersion = new Version(version);
            var newVersion = new Version(Version);
            return newVersion > currentVersion;
        }

        public void Save()
        {
            var json = JsonConvert.SerializeObject(this, new JsonSerializerSettings() {ContractResolver = new CamelCasePropertyNamesContractResolver() });
            using (var sw = new StreamWriter(FilePath, false))
            {
                sw.Write(json);
            }

        }

        public void Download()
        {
            try
            {
                using (var client = new WebClient())
                {
                    client.DownloadProgressChanged += (sender, args) => OnProgressChanged(args);
                    if (File.Exists(PackagePath)) File.Delete(PackagePath);
                    client.DownloadProgressChanged += (e, arg) => OnProgressChanged(arg);
                    client.DownloadFileCompleted += (e, a) => OnDownloadCompleted();
                    client.DownloadFileAsync(new Uri(PackageUrl), PackagePath);
                }
            }
            catch (WebException ex)
            {
                switch (ex.Status)
                {
                    case WebExceptionStatus.ProtocolError:
                        throw new ProtocolErrorException();
                    case WebExceptionStatus.NameResolutionFailure:
                        throw new DnsNotResolveException();
                    case WebExceptionStatus.ConnectFailure:
                        throw new ConnectionFailureException();
                }
                throw;
            }            
        }

        private static string ReadUrl(string jsonUrl)
        {
            try
            {                
                using (var client = new WebClient())
                {
                    return client.DownloadString(jsonUrl);
                }
            }
            catch (WebException ex)
            {
                switch (ex.Status)
                {
                    case WebExceptionStatus.ProtocolError:
                        throw new ProtocolErrorException();                    
                    case WebExceptionStatus.NameResolutionFailure:
                        throw new DnsNotResolveException();
                    case WebExceptionStatus.ConnectFailure:
                        throw new ConnectionFailureException();
                }
                throw;
            }            
        }

        protected virtual void OnDownloadCompleted()
        {
            DownloadCompleted?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnProgressChanged(DownloadProgressChangedEventArgs e)
        {
            ProgressChanged?.Invoke(this, e);
        }
    }
}