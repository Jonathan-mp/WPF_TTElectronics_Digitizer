using System;
using System.Windows.Media.Imaging;
using WPF_TTElectronics.Services;
using System.IO;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using System.Windows.Controls;
using System.Threading.Tasks;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using WPF_TTElectronics.Models;
using System.Diagnostics;
using WPF_TTElectronics.Helpers;
using System.Linq;
using Xceed.Wpf.Toolkit;

namespace WPF_TTElectronics.ViewModels
{
    class SearchViewModel : HelperClosePDFProcess
    {
       MetroWindow activeWindow = Application.Current.Windows.OfType<Views.MainBaseWindowsView>().FirstOrDefault();
       MetroDialogSettings s_err = new MetroDialogSettings { NegativeButtonText = "Cancel", AffirmativeButtonText = "Aceptar", ColorScheme = MetroDialogColorScheme.Inverted };


        public SearchModel _model { get; set; }
        //HelperClosePDFProcess killer = new HelperClosePDFProcess();
        public PreviewViewModel vm_preview { get; set; }
        public string tmpFolder { get; set; }



        //HelperClosePDFProcess killer = new HelperClosePDFProcess();


        public SearchViewModel()
        {
           //var killer = new HelperClosePDFProcess();
            var xmlhelper = new HelperPaths();

            _model = (_model != null) ? _model : new SearchModel();
            tmpFolder = $@"{Path.GetTempPath()}TTElectronics_tmp\";
            if (!Directory.Exists($@"{tmpFolder}"))
                Directory.CreateDirectory($@"{tmpFolder}");

            Visibilities(true);
            Task.Factory.StartNew(() => AcrobatProcess());

            
        }















        private RelayCommand _showTest;
        public ICommand ShowTestCommand
        {
            get
            {
                if (_showTest == null)
                {
                    _showTest = new RelayCommand(param => this.ShowTest(), param => this.CanTest);
                }

                return _showTest;
            }
        }

        public bool CanTest
        {
            get { return true; }
        }

        public void ShowTest()
        {
            if (_model.FullPathToSearch == null)
                return;

            try
            {
                if (File.Exists($"{_model.FullPathToSearch}.pdf"))
                {
                   
                    File.Copy($@"{_model.FullPathToSearch}.pdf", $@"{tmpFolder}{_model.FileNameAdded}.pdf", true );
                    activeWindow.FindChild<WebBrowser>("pdfview").Navigate($@"{tmpFolder}{_model.FileNameAdded}.pdf");
                    _model.Header = $@"TTElectronics_tmp\{_model.FileNameAdded}";
                   
                    Visibilities(false);
                }
                else
                    ShowErrorMessage("File NOT Found");
            }
            catch
            {

                return;
            }

         

        }

















        private RelayCommand _showClosePDF;
        public ICommand ShowClosePDFCommand
        {
            get
            {
                if (_showClosePDF == null)
                {
                    _showClosePDF = new RelayCommand(param => this.ShowClosePDF(), param => this.CanClosePDF);
                }

                return _showClosePDF;
            }
        }

        public bool CanClosePDF
        {
            get { return true; }
        }

        public async void ShowClosePDF()
        {

            AcrobatProcess();
            await Task.Delay(500);
            if (File.Exists($@"{tmpFolder}{_model.FileNameAdded}.pdf"))
                    File.Delete($@"{tmpFolder}{_model.FileNameAdded}.pdf");

          
         
           
            activeWindow.FindChild<WatermarkComboBox>("comboBox").SelectedItem = null;
            _model.FolderToSearch = null;
            _model.ModelToSearch = string.Empty;
            _model.CodeDateToSearch = string.Empty;
            Visibilities(true);



        }









        public void Visibilities(bool searchCtrlsPresented)
        {
            _model.SearchStackVisibility = (searchCtrlsPresented) ? Visibility.Visible : Visibility.Hidden;
            _model.AddPageStackVisibility = (searchCtrlsPresented) ? Visibility.Hidden : Visibility.Visible;

        }












        #region <-------ShowScanAndAddCommand, CanScanAndAdd and ShowScanAndAdd------->

        private RelayCommand _showScanAndAdd;
        public ICommand ShowScanAndAddCommand
        {
            get
            {
                if (_showScanAndAdd == null)
                {
                    _showScanAndAdd = new RelayCommand(param => this.ShowScanAndAdd(), param => this.CanScanAndAdd);
                }

                return _showScanAndAdd;
            }
        }

        public bool CanScanAndAdd
        {
            get { return true; }
        }

        public async void ShowScanAndAdd()
        {
            _model.IsMsgVisible = true;
            var pAsync = await activeWindow.ShowProgressAsync("Status", "Adding Files...");

            AcrobatProcess();   
            await Task.Delay(500);

            //_model.IsMsgVisible = true;
            //var pAsync = await activeWindow.ShowProgressAsync("Status", "Scanning...");
            //pAsync.SetIndeterminate();
            //var scanner = new ScannerService();


            var converter = new ScannerImageConverter(tmpFolder);
        
            await Task.Factory.StartNew(() => converter.AddToExistingPDF($@"{tmpFolder}01.pdf", $@"{tmpFolder}02.pdf"));

            activeWindow.FindChild<WebBrowser>("pdfview").Navigate($@"{tmpFolder}02.pdf");

            await pAsync.CloseAsync();
            _model.IsMsgVisible = false;




            //await Task.Factory.StartNew(async () =>
            //{

            //    var file = scanner.ScanAll(pAsync);
            //    if (file.Count == 0)
            //    {
            //        await pAsync.CloseAsync();
            //        return;
            //    }

            //    converter.SavePDFsOn(file);
            //    await pAsync.CloseAsync();
            //    _model.IsMsgVisible = false;
            //    activeWindow.FindChild<WebBrowser>("pdfview").Navigate($@"{tmpFolder}Preview.pdf");




            //});

















            //if (_model.PreviewIsChecked)
            //{
            //    var pv = new Views.PreviewView();
            //    pv.pdfview.Navigate($@"{tmpFolder}Preview.pdf");
            //    vm_preview = new PreviewViewModel(_model.FullPathToSearch);
            //    pv.DataContext = vm_preview;
            //    pv.Closing += (s, e) =>
            //    {

            //        killer.AcrobatProcess();
            //        activeWindow.FindChild<WebBrowser>("pdfview").Navigate($@"{tmpFolder}Preview.pdf");


            //    };


            //    pv.ShowDialog();
            //    _model.IsMsgVisible = true;
            //    var r = await activeWindow.ShowMessageAsync($@"Family: {_model.FolderToSearch.Title}", $@"Would you like to add this scanned document to {_model.FileNameAdded}.pdf", MessageDialogStyle.AffirmativeAndNegative);
            //    _model.IsMsgVisible = false;




            //}




        }

        #endregion




























        public async void ShowErrorMessage(string message = "default message")
        {
            await activeWindow.ShowMessageAsync("Error", message, MessageDialogStyle.Affirmative, s_err);

        }

    }
}
