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
    public class ScanViewModel : HelperClosePDFProcess
    {
        MetroWindow activeWindow = Application.Current.Windows.OfType<Views.MainBaseWindowsView>().FirstOrDefault();
        public ScanModel _model { get; set; }
        MetroDialogSettings s_err = new MetroDialogSettings { NegativeButtonText = "Cancel", AffirmativeButtonText = "Aceptar", ColorScheme = MetroDialogColorScheme.Inverted };
       
         //HelperClosePDFProcess killer = new HelperClosePDFProcess();
        
  

        /// <summary>
        /// initialize model
        /// create a object HelperPaths
        /// get Title and Paths to ComboBox Items
        /// get Title and Path to Main folder Container
        /// </summary>
        public ScanViewModel()
        {
            
            _model = (_model != null) ? _model : new ScanModel();
            Task.Factory.StartNew(() => AcrobatProcess());

        }


       





#region <-------ScanCommand, CanScan and Scan ------->

        private RelayCommand _scanCommand;
        public ICommand ScanCommand
        {
            get
            {
                if (_scanCommand == null)
                {
                    _scanCommand = new RelayCommand(param => this.Scan(), param => this.CanScan);
                }

                return _scanCommand;
            }
        }

        
        public bool CanScan
        {
            get { return true; }
        }


  


        public async void Scan()
        {
           
            _model.VisibilityHeader = Visibility.Hidden;
            var pAsync = await activeWindow.ShowProgressAsync("Status", "Starting...");
            pAsync.SetIndeterminate();
            await Task.Delay(500);
           

            var scanner = new ScannerService();

         
                activeWindow.FindChild<WebBrowser>("pdfview").Navigate("about:blank");
                //killer.AcrobatProcess();
                if (File.Exists($"{_model.FoldersContainer.FolderPath}Preview.pdf"))
                    await Task.Factory.StartNew(() => AcrobatProcess()).ContinueWith((t) => File.Delete($"{_model.FoldersContainer.FolderPath}Preview.pdf"));




                var file = await Task<ImageFile>.Factory.StartNew(() => scanner.Scan(pAsync)).ContinueWith(async (t) =>
               {

                   try
                   {
                       if (t != null)
                       {
                           var converter = new ScannerImageConverter(_model.FoldersContainer.FolderPath);
                           _model.OpenedFileName = "Preview";
                           _model.FileNameToSave = _model.OpenedFileName;
                           _model.OpenedFileFolder = _model.FoldersContainer.FolderPath;
                           _model.ScannedImage = converter.ConvertScannedImage(t.Result);
                           var saveResult = converter.SaveOnPDF(_model.ScannedImage as BitmapFrame);

                           _model.FullPathOpenedFile = $@"{_model.FoldersContainer.FolderPath}{_model.OpenedFileName}";
                           await pAsync.CloseAsync();
                           _model.SaveAsEnable = true;
                           _model.VisibilityHeader = Visibility.Visible;
                           App.Current.Dispatcher.Invoke(() => {
                               activeWindow.FindChild<WebBrowser>("pdfview").Navigate($"{_model.FullPathOpenedFile}.pdf");
                           });
                         




                       }
                       else
                       {
                           _model.ScannedImage = null;
                           _model.SaveAsEnable = false;
                       }

                   }
                   catch(Exception ex)
                   {
                       ControlsErrorState();
                       await pAsync.CloseAsync();
                       ShowErrorMessage(ex.Message);
                       _model.ScannedImage = null;
                       _model.VisibilityHeader = (string.IsNullOrWhiteSpace(_model.OpenedFileName)) ? Visibility.Hidden : Visibility.Visible;

                   }

                 

               });

        

                


          
           
          


        }


        //static void AcrobatProcess()
        //{
           
        //        foreach (Process proc in Process.GetProcesses())
        //        {
        //            if (proc.ProcessName.StartsWith("Acro"))
        //            {
        //                string proname = proc.ProcessName.ToString();
        //                if (proc.HasExited == false)
        //                {
        //                    proc.WaitForExit(10000);
        //                    string title = proc.MainWindowTitle.ToString();
        //                    if (title == "Adobe Reader" && proname == "AcroRd32")
        //                    {
        //                        proc.Kill();
        //                        break;
        //                    }
        //                    else
        //                    {
        //                        string title2 = proc.MainWindowTitle.ToString();
        //                        if (title2 == "Adobe Reader" && proname == "AcroRd32")
        //                        {
        //                            proc.Kill();
        //                            break;
        //                        }
        //                    }

        //                }
        //                else
        //                {
        //                    try
        //                    {
        //                        proc.Kill();
        //                        break;
        //                    }
        //                    catch
        //                    {
        //                        break;
        //                    }
        //                }
        //            }
        //        }



            
        //}





#endregion


#region <-------ShowOpenFileCommand, CanOpenFile and ShowOpenFile------->

        private RelayCommand _showOpenFile;
        public ICommand ShowOpenFileCommand
        {
            get
            {
                if (_showOpenFile == null)
                {
                    _showOpenFile = new RelayCommand(param => this.ShowOpenFile(), param => this.CanOpenFile);
                }

                return _showOpenFile;
            }
        }

        public bool CanOpenFile
        {
            get { return true; }
        }

        public void ShowOpenFile()
        {
            try
            {
                var openFileDialog = new OpenFileDialog() { Filter = "PDF Files|*.pdf", InitialDirectory = _model.FoldersContainer.FolderPath };
                if (openFileDialog.ShowDialog() != true)
                    return;
          
                _model.OpenedFileName = Path.GetFileNameWithoutExtension(openFileDialog.FileName);
                _model.FullPathOpenedFile = $@"{Path.GetDirectoryName(openFileDialog.FileName)}\{_model.OpenedFileName}";
                _model.FileNameToSave = _model.OpenedFileName;
                activeWindow.FindChild<WebBrowser>("pdfview").Navigate(openFileDialog.FileName);
                _model.VisibilityHeader = Visibility.Visible;
                _model.SaveAsEnable = true;
            }
            catch (Exception ex)
            {
                //_model.SaveAsEnable = false;

                ControlsErrorState();
                //_model.VisibilityHeader = Visibility.Hidden;
                ShowErrorMessage(ex.Message);
               
                //_model.VisibilityHeader = Visibility.Visible;
            }
          

        }

#endregion



#region <-------ShowSaveAsCommand, CanSaveAs and ShowSaveAs------->

        private RelayCommand _showSaveAs;
        public ICommand ShowSaveAsCommand
        {
            get
            {
                if (_showSaveAs == null)
                {
                    _showSaveAs = new RelayCommand(param => this.ShowSaveAs(), param => this.CanSaveAs);
                }

                return _showSaveAs;
            }
        }

        public bool CanSaveAs
        {
            get { return true; }
        }

        public async void ShowSaveAs()
        {
           
            try
            {
               
                SaveFileDialog MyFiles = new SaveFileDialog() { Filter= "PDF Files|*.pdf", Title= "Save as...",DefaultExt= "*.pdf" };

                if (MyFiles.ShowDialog() != true)
                {
                   // _model.VisibilityHeader = Visibility.Visible;
                    return;
                }
                _model.VisibilityHeader = Visibility.Hidden;
                File.Copy($"{_model.FullPathOpenedFile}.pdf", $"{MyFiles.FileName}", true);

                var c = await activeWindow.ShowProgressAsync("Status!", "File Saved", false);
                await Task.Delay(1000);
                await c.CloseAsync();
                _model.VisibilityHeader = Visibility.Visible;
            }
            catch (Exception ex)
            {
               
                ShowErrorMessage(ex.Message);
               // _model.VisibilityHeader = Visibility.Visible;
            }
           


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
         

            try
            {
                _model.VisibilityHeader = Visibility.Hidden;
                if (File.Exists($@"{_model.FullPathToSave}.pdf"))
                    while (File.Exists($@"{_model.FullPathToSave}.pdf"))
                    {
                      
                        var x = await AskReplaceFile();
                        // _model.VisibilityHeader = Visibility.Visible;
                        if (x == _model.FileNameToSave)
                            continue;
                        else if (string.IsNullOrWhiteSpace(x))
                        {
                            _model.VisibilityHeader = Visibility.Visible;
                            return;
                        }
                        else
                            _model.FileNameToSave = x;
                    }


               

                File.Copy($"{_model.FullPathOpenedFile}.pdf", $"{_model.FullPathToSave}.pdf", false);
                var c = await activeWindow.ShowProgressAsync("Status!", $"{_model.FileNameToSave}.pdf saved on {_model.FolderToSave.Title} Folder ", false);
                await Task.Delay(1000);
                await c.CloseAsync();

     
                _model.OpenedFileName = _model.FileNameToSave;
                activeWindow.FindChild<WebBrowser>("pdfview").Navigate($"{_model.FullPathToSave}.pdf");
                _model.FullPathOpenedFile = _model.FullPathToSave;
                _model.VisibilityHeader = Visibility.Visible;

                if (File.Exists($"{_model.FoldersContainer.FolderPath}Preview.pdf"))
                {
                    await Task.Factory.StartNew(() => AcrobatProcess()).ContinueWith((t) => File.Delete($"{_model.FoldersContainer.FolderPath}Preview.pdf"));
                }


               
            }
            catch (Exception ex)
            {
              
                ShowErrorMessage(ex.Message);
                //_model.FileNameToSave = string.Empty;
                //_model.VisibilityHeader = Visibility.Visible;
            }
        }


#endregion



#region <-------ShowScanAllCommand, CanScanAll and ShowScanAll------->

        private RelayCommand _showScanAll;
        public ICommand ShowScanAllCommand
        {
            get
            {
                if (_showScanAll == null)
                {
                    _showScanAll = new RelayCommand(param => this.ShowScanAll(), param => this.CanScanAll);
                }

                return _showScanAll;
            }
        }

        public bool CanScanAll
        {
            get { return true; }
        }

        public async void ShowScanAll()
        {
            

            _model.VisibilityHeader = Visibility.Hidden;
            var pAsync = await activeWindow.ShowProgressAsync("Status", "Scanning...");
            
            pAsync.SetIndeterminate();
            await Task.Delay(500);



            try
            {
              
                if (File.Exists($"{_model.FoldersContainer.FolderPath}Preview.pdf"))
                {
                    activeWindow.FindChild<WebBrowser>("pdfview").Navigate("about:blank");
                    AcrobatProcess();
                    await Task.Delay(500);
                    File.Delete($"{_model.FoldersContainer.FolderPath}Preview.pdf");
                    //await Task.Factory.StartNew(() => killer.AcrobatProcess()).ContinueWith((t) => File.Delete($"{_model.FoldersContainer.FolderPath}Preview.pdf"));
                }

                var scanner = new ScannerService();
                var converter = new ScannerImageConverter(_model.FoldersContainer.FolderPath);

                await Task.Factory.StartNew(async () =>
                {








                    var file = scanner.ScanAll(pAsync);
                    if (file.Count == 0)
                    {
                        await pAsync.CloseAsync();
                        return;
                    }

                    _model.ScannedImage = null;
                    _model.OpenedFileName = "Preview";
                    _model.FileNameToSave = _model.OpenedFileName;
                    _model.OpenedFileFolder = _model.FoldersContainer.FolderPath;
                    converter.SavePDFsOn(file);
                    if (File.Exists($@"{_model.FoldersContainer.FolderPath}{_model.OpenedFileName}.pdf"))
                        _model.FullPathOpenedFile = $@"{_model.FoldersContainer.FolderPath}{_model.OpenedFileName}";
                    else
                        return;
                  
                });

                if (File.Exists($@"{_model.FullPathOpenedFile}.pdf"))
                {
                    activeWindow.FindChild<WebBrowser>("pdfview").Navigate($"{_model.FullPathOpenedFile}.pdf");
                    _model.SaveAsEnable = true;
                }
                else
                    return;



                await pAsync.CloseAsync();
                _model.VisibilityHeader = Visibility.Visible;

            }
            catch (Exception ex)
            {

                ControlsErrorState();
                await pAsync.CloseAsync();
                ShowErrorMessage(ex.Message);
                _model.ScannedImage = null;
              


            }




        }

#endregion



#region <-------ShowClosePDFCommand, CanClosePDF and ShowClosePDF------->

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
           

            //await Task.Delay(500);

            try
            {
               
                _model.VisibilityHeader = Visibility.Hidden;
                ControlsErrorState();
                await Task.Factory.StartNew(() => AcrobatProcess());

            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message);
            }


        }

        #endregion

        public void ControlsErrorState()
        {
            App.Current.Dispatcher.Invoke(()=> {

                activeWindow.FindChild<WatermarkComboBox>("comboBox").SelectedItem = null;
              
                    _model.SaveAsEnable = false;
                    _model.FileNameToSave = string.Empty;
                    _model.OpenedFileName = string.Empty;
                    _model.OpenedFileFolder = string.Empty;
                    _model.FullPathOpenedFile = string.Empty;
               
            });
           


        }



















        /// <summary>
        /// Asks about other FileNameToSave only if that name already exists on path selected.
        /// </summary>
        /// <returns></returns>
        public async Task<string> AskReplaceFile()
        {
            var r = await activeWindow.ShowInputAsync($"The file named \"{_model.FileNameToSave}\" already exists on this path", "Choose other name to that file.", s_err);
            return r;
        }


        /// <summary>
        /// Shows a MessageAsync (black background) control only when a catch throw an error.
        /// </summary>
        /// <param name="message">
        /// This is the message error.
        /// </param>
        public async void ShowErrorMessage(string message = "default message")
        {

           await  App.Current.Dispatcher.Invoke(async() =>
            {
                _model.VisibilityHeader = Visibility.Hidden;
                await activeWindow.ShowMessageAsync("Error", message, MessageDialogStyle.Affirmative, s_err);
                _model.VisibilityHeader = (string.IsNullOrWhiteSpace(_model.OpenedFileName)) ? Visibility.Hidden : Visibility.Visible;

            });
           

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

        public async void ShowTest()
        {

            


        }














    }
}
