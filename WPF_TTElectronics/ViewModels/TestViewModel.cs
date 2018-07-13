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
using WPF_TTElectronics.Models;

namespace WPF_TTElectronics.ViewModels
{
    public class TestViewModel
    {

        public TestModel  _model { get; set; }
        MetroWindow activeWindow = Application.Current.Windows.OfType<Views.MainBaseWindowsView>().FirstOrDefault();
        Helpers.HelperClosePDFProcess killer;


        public TestViewModel()
        {
            _model = new TestModel();
            killer = new Helpers.HelperClosePDFProcess();
          
            

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
            if (_model.FileNameSelected == null)
                return;
            var fullView = new Views.FullWindowView();
            fullView.Fullpdfview.Navigate($@"{_model.FileNameSelected.FullPathWithExtension}");
            var vm_fullView = new FullWindowModel() {TitleWindow= $@"OPENED FILE: {_model.FileNameSelected.FullName}"};
            fullView.DataContext = vm_fullView;
            //fullView.Closing += (s, e) =>
            //{

            //   // killer.AcrobatProcess();
            //    //activeWindow.FindChild<WebBrowser>("pdfview").Navigate($"{_model.FileNameSelected.FullPathWithExtension}");


            //};
            fullView.ShowDialog();

          


        }



       
        #endregion

    }
}
