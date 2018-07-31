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
using System.Threading.Tasks;

namespace WPF_TTElectronics.ViewModels
{
    public class MainBaseWindowsViewModel : HelperClosePDFProcess
    {

        MetroWindow activeWindow = Application.Current.Windows.OfType<Views.MainBaseWindowsView>().FirstOrDefault();
        public MainBaseModel _model { get; set; }

        public MainBaseWindowsViewModel()
        {
            _model = _model  ?? new MainBaseModel();
            
            if (!Directory.Exists($@"{_model.TempFolder}"))
                Directory.CreateDirectory($@"{_model.TempFolder}");

            activeWindow.Closed += (s,e) =>{
           
                AcrobatProcess();
                ClearTempFolder(_model.TempFolder);
            };
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








        #region --------ShowSearchView and ShowSearchCommand

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
            if (activeWindow.FindChild<TransitioningContentControl>("contentControl").Content is SearchView)
                return;
            activeWindow.FindChild<TransitioningContentControl>("contentControl").Content = new SearchView();
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
            activeWindow.FindChild<TransitioningContentControl>("contentControl").Content = new ScanView();
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
            if (activeWindow.FindChild<TransitioningContentControl>("contentControl").Content is ViewView)
                return;
            activeWindow.FindChild<TransitioningContentControl>("contentControl").Content = new ViewView(); //v_view;
        }



        #endregion



        #region --------ShowAddPDFView and ShowAddPDFCommand

        private RelayCommand _showAddPDFCommand;
        public ICommand ShowAddPDFCommand
        {
            get
            {
                if (_showAddPDFCommand == null)
                {
                    _showAddPDFCommand = new RelayCommand(param => this.ShowAddPDF(), param => this.CanAddPDF);
                }

                return _showAddPDFCommand;
            }
        }

        public bool CanAddPDF
        {
            get { return true; }
        }



        public void ShowAddPDF()
        {
            if (activeWindow.FindChild<TransitioningContentControl>("contentControl").Content is AddPDFView)
                return;
            activeWindow.FindChild<TransitioningContentControl>("contentControl").Content = new AddPDFView(); 
        }



        #endregion





        















































     





    }
}
