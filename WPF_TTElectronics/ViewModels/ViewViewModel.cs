using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPF_TTElectronics.Helpers;
using WPF_TTElectronics.Models;

namespace WPF_TTElectronics.ViewModels
{
    class ViewViewModel : HelperClosePDFProcess
    {

        public ViewModel _model { get; set; }
        MetroWindow activeWindow = Application.Current.Windows.OfType<Views.MainBaseWindowsView>().FirstOrDefault();


        public ViewViewModel()
        {
            _model = _model ?? new ViewModel();
        }


        #region --------ShowFullScreen and ShowFullScreenCommand

        private RelayCommand _showFullScreenCommand;
        public ICommand ShowFullScreenCommand
        {
            get
            {
                if (_showFullScreenCommand == null)
                {
                    _showFullScreenCommand = new RelayCommand(param => this.ShowFullScreen(), param => this.CanFullScreen);
                }

                return _showFullScreenCommand;
            }
        }

        public bool CanFullScreen
        {
            get { return true; }
        }



        public void ShowFullScreen()
        {
            if (_model.FileNameSelected == null)
                return;
            var fullView = new Views.FullWindowView();
            fullView.Fullpdfview.Navigate($@"{_model.FileNameSelected.FullPathWithExtension}");
            var vm_fullView = new FullWindowModel() { TitleWindow = $@"OPENED FILE: {_model.FileNameSelected.FullName}" };
            fullView.DataContext = vm_fullView;
            fullView.Closing += (s, e) =>
            {
                AcrobatProcess();
                activeWindow.FindChild<WebBrowser>("pdfview").Navigate($"{_model.FileNameSelected.FullPathWithExtension}");

            };
            fullView.ShowDialog();
        }




        #endregion

    }
}
