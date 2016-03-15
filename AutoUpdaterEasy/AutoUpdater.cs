using System;
using System.Net;
using System.Threading;
using AutoUpdaterEasy.Entities;
using AutoUpdaterEasy.Exceptions;

namespace AutoUpdaterEasy
{
    public class AutoUpdater
    {
        public static void Initialize(string url, string currentVersion)
        {
            Instance = new AutoUpdater(url, currentVersion);
            Instance.Start();
        }
        public static AutoUpdater Instance { get; private set; }


        private Thread _thread;
        private int _progress;
        private bool _isRunning;
        private bool _isCancel;
        private bool _isAccepted;
        public event EventHandler NewUpdate;
        public event EventHandler<DownloadProgressChangedEventArgs> DownloadProgressChanged;
        public event EventHandler DownloadCompleted;
        public event EventHandler<MessageArgs> Error;
        private readonly string _url;
        private readonly string _currentVersion;
        private bool _await;
        private JsonConfig _jsonConfig;

        private AutoUpdater(string url, string currentVersion)
        {
            _url = url;
            _currentVersion = currentVersion;
            _jsonConfig = JsonConfig.Factory();
        }

        private void Start()
        {
            if (_isRunning) return;
            _isCancel = false;
            _thread = new Thread(Run);
            _isRunning = true;
            _thread.Start();
        }

        public void Join()
        {
            if (!_isRunning) return;
            _thread.Join();
        }
        public void Stop()
        {
            if (!_isRunning) return;            
            _isCancel = true;
            _thread.Join();
            _thread = null;
            GC.Collect();
        }
        private void Sleep(int sec)
        {
            for (var i = 1; i <= sec; i++)
            {
                if(_isCancel)Thread.CurrentThread.Abort();
                Thread.Sleep(1000);
                if (_isCancel) Thread.CurrentThread.Abort();
            }
        }

        private void Waiting()
        {
            while (_await)
            {                
                Thread.Sleep(1000);
            }
        }

        public void Accept()
        {
            _await = false;
            _isAccepted = true;
        }

        public void Reject()
        {
            _isAccepted = false;
            _await = false;            
        }

        private void Run()
        {            
            try
            {
                _isRunning = true;
                while (!_isCancel)
                {
                    try
                    {
                        _jsonConfig = JsonConfig.Factory(_url);
                        _jsonConfig.Save();
                        if (!_jsonConfig.IsNewVersion(_currentVersion))
                        {
                            Sleep(_jsonConfig.GetMilliseconds()/1000);
                            continue;
                        }

                        if (!_jsonConfig.ForceUpdate)
                        {
                            _await = true;
                            OnNewUpdate();
                            Waiting();
                            if (!_isAccepted)
                            {
                                Sleep(_jsonConfig.GetMilliseconds() / 1000);
                                continue;
                            }
                        }

                        _progress = 0;
                        _jsonConfig.ProgressChanged += (e, a) => OnDownloadProgressChanged(a);
                        _jsonConfig.DownloadCompleted += (e, a) => OnDownloadCompleted();
                        _await = true;
                        _jsonConfig.Download();
                        Waiting();
                        if (_progress != 100) continue;
                        _isCancel = true;
                    }
                    catch (DnsNotResolveException ex)
                    {
                        Sleep(_jsonConfig.GetMilliseconds()/1000);
                        OnError(ex.Message);
                    }
                    catch (ProtocolErrorException ex)
                    {
                        Sleep(_jsonConfig.GetMilliseconds() / 1000);
                        OnError(ex.Message);
                    }
                    catch (ConnectionFailureException ex)
                    {
                        Sleep(_jsonConfig.GetMilliseconds() / 1000);
                        OnError(ex.Message);
                    }
                    catch (Exception ex)
                    {
                        Sleep(_jsonConfig.GetMilliseconds() / 1000);
                        OnError(ex.Message);
                    }
                }
            }
            catch (ThreadAbortException)
            {
                _isRunning = false;
            }
            catch (ThreadInterruptedException)
            {
                _isRunning = false;
            }
            _isRunning = false;
        }

        public void Unzip()
        {
            
        }

        protected virtual void OnDownloadProgressChanged(DownloadProgressChangedEventArgs e)
        {
            _progress = e.ProgressPercentage;
            DownloadProgressChanged?.Invoke(this, e);
        }

        protected virtual void OnDownloadCompleted()
        {
            _await = false;
            DownloadCompleted?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnError(string e)
        {
            Error?.Invoke(this, new MessageArgs(e));
        }

        protected virtual void OnNewUpdate()
        {
            NewUpdate?.Invoke(this, EventArgs.Empty);
        }
    }
}
