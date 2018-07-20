using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using WPF_TTElectronics.Models;
using System.Windows.Threading;
using WPF_TTElectronics.UserControls;
using System.IO;
using WPF_TTElectronics.Helpers;

namespace WPF_TTElectronics.ViewModels
{
    public class MainBaseWindowsViewModel : HelperClosePDFProcess
    {

        MetroWindow activeWindow = Application.Current.Windows.OfType<Views.MainBaseWindowsView>().FirstOrDefault();

  




        public MainBaseModel _model { get; set; }



        private ScanView v_scan { get; set; }
        private SearchView v_search { get; set; }
        private ViewView v_view { get; set; }
        private TestView v_test { get; set; }








        public MainBaseWindowsViewModel()
        {
            v_scan = (v_scan != null) ? v_scan : new ScanView();
            v_search = (v_search != null) ? v_search : new SearchView();
            v_view = (v_view != null) ? v_view : new ViewView();
            v_test = (v_test != null) ? v_test : new TestView();
           
                

            

            activeWindow.Closed += (s,e) =>{
                // var killer = new Helpers.HelperClosePDFProcess();
                // killer.AcrobatProcess();
                AcrobatProcess();
                ClearTempFolder();
            };

           


            _model = (_model != null) ? _model : new MainBaseModel();

        }

        


        #region --------ShowAbout and ShowAboutCommand

        private RelayCommand _showAboutCommand;
        public ICommand ShowAboutCommand
        {
            get
            {
                if (_showAboutCommand == null)
                {
                    _showAboutCommand = new RelayCommand(param => this.ShowAbout(), param => this.CanAbout);
                }

                return _showAboutCommand;
            }
        }

        public bool CanAbout
        {
            get { return _model.About; }
        }


        /// <summary>
        ///  if "_model.About" is true, swaps to false its value, which is used to show a little windows with software information which will be closed after 8 seconds
        /// </summary>
        public void ShowAbout()
        {

            if (_model.About)
            {
                var aboutview = new Views.AboutView();
                this._model.About = false;
                aboutview.Show();
            }
        }



        #endregion








        #region --------ShowSearch and ShowSearchCommand

        private RelayCommand _showSearchCommand;
        public ICommand ShowSearchCommand
        {
            get
            {
                if (_showSearchCommand == null)
                {
                    _showSearchCommand = new RelayCommand(param => this.ShowSearch(), param => this.CanSearch);
                }

                return _showSearchCommand;
            }
        }

        public bool CanSearch
        {
            get { return true; }
        }


    
        public void ShowSearch()
        {
            // v_search = new SearchView();
            if (activeWindow.FindChild<TransitioningContentControl>("contentControl").Content is SearchView)
                return;
            activeWindow.FindChild<TransitioningContentControl>("contentControl").Content = new SearchView(); //v_search;
            ClearTempFolder();
        }



        #endregion







        #region --------ShowScanView and ShowScanViewCommand

        private RelayCommand _showScanCommand;
        public ICommand ShowScanCommand
        {
            get
            {
                if (_showScanCommand == null)
                {
                    _showScanCommand = new RelayCommand(param => this.ShowScan(), param => this.CanScan);
                }

                return _showScanCommand;
            }
        }

        public bool CanScan
        {
            get { return true; }
        }



        public void ShowScan()
        {
            if (activeWindow.FindChild<TransitioningContentControl>("contentControl").Content is ScanView)
                return;

            activeWindow.FindChild<TransitioningContentControl>("contentControl").Content = new ScanView(); // _model.V_Scan; //v_scan; //new ScanView(); //v_scan;
        }



        #endregion









        #region --------ShowViewView and ShowViewViewCommand

        private RelayCommand _showViewCommand;
        public ICommand ShowViewsCommand
        {
            get
            {
                if (_showViewCommand == null)
                {
                    _showViewCommand = new RelayCommand(param => this.ShowView(), param => this.CanView);
                }

                return _showViewCommand;
            }
        }

        public bool CanView
        {
            get { return true; }
        }



        public void ShowView()
        {
            // v_view = new ViewView();
            if (activeWindow.FindChild<TransitioningContentControl>("contentControl").Content is ViewView)
                return;
            activeWindow.FindChild<TransitioningContentControl>("contentControl").Content = new ViewView(); //v_view;
        }



        #endregion







        public void ClearTempFolder()
        {
            if(Directory.GetFiles($@"{Path.GetTempPath()}TTElectronics_tmp\") != null)
            Array.ForEach(Directory.GetFiles($@"{Path.GetTempPath()}TTElectronics_tmp\"), delegate (string path) { File.Delete(path); });

        }















































        #region --------ShowViewView and ShowViewViewCommand

        private RelayCommand _showTestCommand;
        public ICommand ShowTestCommand
        {
            get
            {
                if (_showTestCommand == null)
                {
                    _showTestCommand = new RelayCommand(param => this.ShowTest(), param => this.CanTest);
                }

                return _showTestCommand;
            }
        }

        public bool CanTest
        {
            get { return true; }
        }



        public void ShowTest()
        {
            activeWindow.FindChild<TransitioningContentControl>("contentControl").Content = new TestView(); //v_test; //new TestView();  //v_test;
        }



        #endregion






    }
}
