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
using WIA;

namespace WPF_TTElectronics.ViewModels
{
    class SearchViewModel : HelperClosePDFProcess
    {
       MetroWindow activeWindow = Application.Current.Windows.OfType<Views.MainBaseWindowsView>().FirstOrDefault();
       MetroDialogSettings s_err = new MetroDialogSettings { NegativeButtonText = "Cancel", AffirmativeButtonText = "Aceptar", ColorScheme = MetroDialogColorScheme.Inverted, AnimateHide = false, AnimateShow = false };
       MetroDialogSettings s_without_animation = new MetroDialogSettings { AnimateHide = false, AnimateShow = false };


        public SearchModel _model { get; set; }


     






        public SearchViewModel()
        {



            _model = (_model != null) ? _model : new SearchModel();

            Visibilities(true);
            Task.Factory.StartNew(() => AcrobatProcess());

            
        }















        private RelayCommand _showSearch;
        public ICommand ShowSearchCommand
        {
            get
            {
                if (_showSearch == null)
                {
                    _showSearch = new RelayCommand(param => this.ShowSearch(), param => this.CanSearch);
                }

                return _showSearch;
            }
        }

        public bool CanSearch
        {
            get { return true; }
        }

        public void ShowSearch()
        {
            if (_model.FullPathToSearch == null)
                return;

            try
            {
                if (File.Exists($"{_model.FullPathToSearch}.pdf"))
                {
                   
                    File.Copy($@"{_model.FullPathToSearch}.pdf", $@"{_model.TempFolder}{_model.FileNameAdded}.pdf", true );
                    activeWindow.FindChild<WebBrowser>("pdfview").Navigate($@"{_model.TempFolder}{_model.FileNameAdded}.pdf");
                    //_model.Header = $@"tmp\{_model.FileNameAdded}";
                   
                    Visibilities(false);
                }
                else
                    ShowErrorMessage("File NOT Found");
            }
            catch(Exception ex)
            {

                ShowErrorMessage(message:ex.Message);
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
            //if (File.Exists($@"{_model.TempFolder}{_model.FileNameAdded}.pdf"))
              //      File.Delete($@"{_model.TempFolder}{_model.FileNameAdded}.pdf");

          
         
           
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
            var scanner = new ScannerService();
            var pAsync = await activeWindow.ShowProgressAsync("Status", "Starting...", false, s_without_animation);
            pAsync.SetIndeterminate();
        
            var converter = new ScannerImageConverter(_model.TempFolder);

            switch (_model.AutoAdd)
            {
                case true:
                    await Task.Factory.StartNew(async () =>
                    {

                        try
                        {
                            var filee = scanner.ScanAll(pAsync, _model.ColorSetting);
                            if (filee.Count == 0)
                            {
                                await pAsync.CloseAsync();
                                return;
                            }

                            converter.SavePDFsOn(filee);
                            await pAsync.CloseAsync();
                            _model.IsMsgVisible = false;
                            AcrobatProcess();
                            await Task.Delay(500);

                            pAsync.SetMessage("Adding pages to file...");
                            await Task.Factory.StartNew(() => converter.AddToExistingPDF($@"{_model.TempFolder}Preview.pdf", $@"{_model.TempFolder}{_model.FileNameAdded}.pdf"));
                            App.Current.Dispatcher.Invoke(() => {
                                activeWindow.FindChild<WebBrowser>("pdfview").Navigate($@"{_model.TempFolder}{_model.FileNameAdded}.pdf");
                            });
                            await pAsync.CloseAsync();
                            _model.IsMsgVisible = false;
                        }
                        catch (Exception ex)
                        {
                            await pAsync.CloseAsync();


                            App.Current.Dispatcher.Invoke(() => {
                                ShowErrorMessage(ex.Message);
                            });


                        }
                    });
                    break;
                case false:
                    var file = await Task<ImageFile>.Factory.StartNew(() => scanner.Scan(pAsync, _model.ColorSetting)).ContinueWith(async (t) =>
                    {
                    try
                    {
                        if (t != null)
                        {
                            var scannedImage = converter.ConvertScannedImage(t.Result);
                            var saveResult = converter.SaveOnPDF(scannedImage as BitmapFrame);
                            AcrobatProcess();
                            await Task.Delay(500);

                                pAsync.SetMessage("Adding pages to file...");
                            await Task.Factory.StartNew(() => converter.AddToExistingPDF($@"{_model.TempFolder}Preview.pdf", $@"{_model.TempFolder}{_model.FileNameAdded}.pdf"));
                                App.Current.Dispatcher.Invoke(() => {
                                    activeWindow.FindChild<WebBrowser>("pdfview").Navigate($@"{_model.TempFolder}{_model.FileNameAdded}.pdf");
                                });
                               
                            await pAsync.CloseAsync();
                            _model.IsMsgVisible = false;
                        }
                        else
                        {

                        }
                    }
                    catch (Exception ex)
                    {
                        await pAsync.CloseAsync();

                        App.Current.Dispatcher.Invoke(()=> {
                            ShowErrorMessage(ex.Message);
                        });

                      

                        }
            });
                    break;

                
                default:
                    break;
            }

           // _model.IsMsgVisible = false;
          

        }

        #endregion




        #region <-------ShowSaveCommand, CanSave and ShowSave------->

        private RelayCommand _showSave;
        public ICommand ShowSaveCommand
        {
            get
            {
                if (_showSave == null)
                {
                    _showSave = new RelayCommand(param => this.ShowSave(), param => this.CanSave);
                }

                return _showSave;
            }
        }

        public bool CanSave
        {

            get { return true; }
        }

        public async void ShowSave()
        {
            _model.IsMsgVisible = true;
            AcrobatProcess();
            await Task.Delay(500);
            var saved = false;


            try
            {

                File.Copy($"{_model.TempFolder}{_model.FileNameAdded}.pdf", $"{_model.FullPathToSearch}.pdf", true);
                saved = true;
                //_model.Header = _model.FileNameAdded;
                var c = await activeWindow.ShowProgressAsync("Status!", $"{_model.FileNameAdded}.pdf saved successfully", false);
                await Task.Delay(1000);
                await c.CloseAsync();
                _model.IsMsgVisible = false;


               // _model.Header = _model.FileNameAdded;
              







            }
            catch (Exception ex)
            {

                await App.Current.Dispatcher.InvokeAsync(() =>
                {
                     ShowErrorMessage(ex.Message);
                });


            }
            finally
            {
                activeWindow.FindChild<WebBrowser>("pdfview").Navigate((saved) ? $"{_model.FullPathToSearch}.pdf" : $"{_model.TempFolder}{_model.FileNameAdded}.pdf");
               
              
            }

            
     
        }


        #endregion

























        public async void ShowErrorMessage(string message = "default message")
        {
            _model.IsMsgVisible = true;
            await activeWindow.ShowMessageAsync("Error", message, MessageDialogStyle.Affirmative, s_err);
            _model.IsMsgVisible = false;

        }

    }
}
