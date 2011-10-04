using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using MS.OSM.Querys.OSM2DB.Controller;

namespace MS.OSM.Querys.OSM2DB
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainController _mainController = new MainController();

        private List<string> _selectedFilters = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = _mainController.Model;
            InitSelectedFiltersList();
        }

        private void InitSelectedFiltersList()
        {
            foreach (var item in SelectedItemsList.Children)
            {
                CheckBox cb = item as CheckBox;

                if (cb != null)
                {
                    _selectedFilters.Add((string)cb.Content);
                }
            }
        }

        private void ButtonLoadClick(object sender, RoutedEventArgs e)
        {
            if (_mainController.Phase != MainControllerStates.None)
            {
                MessageBox.Show("Data already loaded. To load other file press Clear first.");
                return;
            }

            string path = textBoxPath.Text;

            if (!File.Exists(path))
            {
                StatusItem.Content = "Invalid file!";
                return;
            }
            
            buttonClear.IsEnabled = false;
            StatusItem.Content = "Loading data from file... please wait!";

            _mainController.LoadOSMFile(path, _selectedFilters, a => ButtonLoadFileClickCallback());
        }

      
        private void ButtonLoadFileClickCallback()
        {
            if (!this.Dispatcher.CheckAccess())
            {
                Dispatcher.BeginInvoke(new Action(ButtonLoadFileClickCallback), null);
                return;
            }
            buttonClear.IsEnabled = true;
            DBLoadButton.IsEnabled = true;

            StatusItem.Content = "Ready";
        }

        private void ButtonClearClick(object sender, RoutedEventArgs e)
        {
            _mainController.Clear();
            DBLoadButton.IsEnabled = false;
        }

        private void DBLoadButtonClick(object sender, RoutedEventArgs e)
        {
            StatusItem.Content = "Loading data to database... please wait!";
            _mainController.LoadOSMToDatabase(a => ButtonLoadDbClickCallback());
        }

        private void ButtonLoadDbClickCallback()
        {
            if (!this.Dispatcher.CheckAccess())
            {
                Dispatcher.BeginInvoke(new Action(ButtonLoadDbClickCallback), null);
                return;
            }

            StatusItem.Content = "Ready";
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;

            if (cb.Content != null)
                _selectedFilters.Add((string)cb.Content);    
            
        }

        private void CheckBox_UnChecked(object sender, RoutedEventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            
            if (cb.Content != null)
                _selectedFilters.Remove((string) cb.Content);
        }
    }
}
