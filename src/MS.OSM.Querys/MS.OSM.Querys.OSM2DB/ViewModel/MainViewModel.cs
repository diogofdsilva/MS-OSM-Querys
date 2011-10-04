using System;
using System.ComponentModel;

namespace MS.OSM.Querys.OSM2DB.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public MainViewModel()
        {
        }

        private string _queryString;

        public string QueryString
        {
            get { return _queryString; }
            set { _queryString = value; NotifyPropertyChanged("QueryString"); }
        }

        //NodesLeft
        private int _nodesLeft;
        public int NodesLeft
        {
            get { return _nodesLeft; }
            set { _nodesLeft = value; NotifyPropertyChanged("NodesLeft"); }
        }


        //MaxNodesLeft
        private int _maxNodesLeft;
        public int MaxNodesLeft
        {
            get { return _maxNodesLeft; }
            set { _maxNodesLeft = value; NotifyPropertyChanged("MaxNodesLeft"); }
        }

        //WaysLeft
        private int _waysLeft;
        public int WaysLeft
        {
            get { return _waysLeft; }
            set { _waysLeft = value; NotifyPropertyChanged("WaysLeft"); }
        }


        //MaxWaysLeft
        private int _maxWaysLeft;
        public int MaxWaysLeft
        {
            get { return _maxWaysLeft; }
            set { _maxWaysLeft = value; NotifyPropertyChanged("MaxWaysLeft"); }
        }


        //Ellapsed time
        private TimeSpan _elapsed;

        public TimeSpan Elapsed
        {
            get { return _elapsed; }
            set { _elapsed = value; NotifyPropertyChanged("Elapsed"); }
        }


        #region INotifyPropertyChanged

        //INotifyPropertyChanged members
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (null != handler)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
        
        #endregion INotifyPropertyChanged
    }
}