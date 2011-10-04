using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using MS.OSM.Querys.DTO;
using MS.OSM.Querys.OSM2DB.Loader;
using MS.OSM.Querys.OSM2DB.OSMFileReader;
using MS.OSM.Querys.OSM2DB.ViewModel;

namespace MS.OSM.Querys.OSM2DB.Controller
{
    class MainController
    {
        private MainViewModel _model;
        private Dictionary<long, Node> _nodes;
        private ConcurrentBag<Way> _ways;
        private Stopwatch _watch;
        private Timer timerRefresh;

        private XmlOSMFileLoader _xmlOSMFileLoader;
        private DBLoader _dbLoader;

        private MainControllerStates _phase;

        public MainController()
        {
            _model = new MainViewModel();

            _nodes = new Dictionary<long, Node>();
            _ways = new ConcurrentBag<Way>();
            _watch = new Stopwatch();

            timerRefresh = new Timer(500);
            timerRefresh.Elapsed += TimerRefreshElapsed;
            timerRefresh.Start();

            _xmlOSMFileLoader = new XmlOSMFileLoader(_ways);
            _dbLoader = new DBLoader(_ways,Model.QueryString);

            _phase = MainControllerStates.None;

            //load current Query String
            var v = ConfigurationManager.ConnectionStrings["CS"];

            if (v != null)
            {
                Model.QueryString = v.ConnectionString;
            }

        }

        public MainViewModel Model
        {
            get { return _model; }
        }

        public MainControllerStates Phase
        {
            get { return _phase; }
        }

        void TimerRefreshElapsed(object sender, ElapsedEventArgs e)
        {
            RefreshModel();
        }

        private void RefreshModel()
        {

            if (_phase == MainControllerStates.None)
            {
                return;
            }

            _model.Elapsed = _watch.Elapsed;

            if (_phase == MainControllerStates.LoadFromFile)
            {
                Model.MaxNodesLeft = _nodes.Count;
                Model.MaxWaysLeft = _ways.Count;
            }

            Model.NodesLeft = _nodes.Count;
            Model.WaysLeft = _ways.Count;
        }


        public void LoadOSMFile(string path, List<string> filters, Action<Task> continuation)
        {
            _phase = MainControllerStates.LoadFromFile;
            _watch.Start();
            var task = _xmlOSMFileLoader.BeginRead(path, filters);
            
            task.ContinueWith(a =>
                                  {
                                      if (a.IsFaulted)
                                      {
                                          MessageBox.Show("Internal Error!");
                                      }

                                      continuation(a);
                                      _model.MaxWaysLeft = _ways.Count;
                                      _watch.Stop();
                                      _phase = MainControllerStates.Idle;
                                      _xmlOSMFileLoader.Dispose();
                                  });
        }

        public void Clear()
        {
            _nodes.Clear();
            _ways = new ConcurrentBag<Way>();
            _xmlOSMFileLoader = new XmlOSMFileLoader(_ways);
            _watch.Reset();
            RefreshModel();
            _phase = MainControllerStates.None;
        }

        public void LoadOSMToDatabase(Action<Task> continuation)
        {
            _phase = MainControllerStates.LoadToDb;
            _watch.Start();

            _dbLoader.QueryString = Model.QueryString;
            var tasks = _dbLoader.BeginLoadWays(4);

            Task.Factory.StartNew(() =>
                                      {
                                          Task.WaitAll(tasks);
                                          continuation(null);
                                          _watch.Stop();
                                          _phase = MainControllerStates.Idle;
                                      });
        }
        
    }

    public enum MainControllerStates
    {
        LoadFromFile, LoadToDb, None, Idle
    }
}
