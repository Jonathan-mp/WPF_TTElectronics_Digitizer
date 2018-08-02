using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPF_TTElectronics.Helpers;
using WPF_TTElectronics.Models;
using WPF_TTElectronics.Services;

namespace WPF_TTElectronics.ViewModels
{
    public class AddPDFViewModel : HelperClosePDFProcess
    {
        MetroWindow activeWindow = Application.Current.Windows.OfType<Views.MainBaseWindowsView>().FirstOrDefault();
        MetroDialogSettings s_err = new MetroDialogSettings { NegativeButtonText = "Cancel", AffirmativeButtonText = "Aceptar", ColorScheme = MetroDialogColorScheme.Inverted, AnimateHide = false, AnimateShow = false };
        MetroDialogSettings s_without_animation = new MetroDialogSettings { AnimateHide = false, AnimateShow = false };
        Regex nameFormat = new Regex(@"[\d]{1,8}_[\d]{1,8}$");


        public AddPDFModel _model { get; set; }

        public AddPDFViewModel()
        {
           _model = _model ?? new AddPDFModel();

        }


        #region --------ShowBrowseDestination and ShowBrowseDestinationCommand

        private RelayCommand _showBrowseDestinationCommand;
        public ICommand ShowBrowseDestinationCommand
        {
            get
            {
                if (_showBrowseDestinationCommand == null)
                {
                    _showBrowseDestinationCommand = new RelayCommand(param => this.ShowBrowseDestination(), param => this.CanBrowseDestination);
                }

                return _showBrowseDestinationCommand;
            }
        }

        public bool CanBrowseDestination
        {
            get { return true; }
        }


 
        public void ShowBrowseDestination()
        {
            var openFileDialog = new OpenFileDialog() { Filter = "PDF Files|*.pdf", InitialDirectory = $@"{_model.ComboItems[0].FolderPath}" };
            if (openFileDialog.ShowDialog() != true)
                return;
            AcrobatProcess();
            Task.Delay(500);

            var file = new FileInfo(openFileDialog.FileName);
            App.Current.Dispatcher.Invoke(() => {
              
                File.Copy(file.FullName, $"{_model.TempFolder}{file.Name}", true);
            });
            

            if (nameFormat.IsMatch(file.Name.Split('.')[0]) != true)
            {
                ShowErrorMessage("Incorrect Format","The next format was expected => {Model}_{DateCode}.pdf");
                return;
            }


            try
            {

                _model.DestinationFile = new cFileInfo()
                {
                    FullName = file.Name.Split('.')[0],
                    Model = file.Name.Split('_')[0],
                    DateCode = file.Name.Split('_')[1].Split('.')[0],
                    Family = file.Directory.Name,
                    FullPathWithExtension = file.FullName,
                    TimeCreation = file.CreationTime.ToString("d"),
                    TimeLastAccess = file.LastAccessTime.ToString("d"),
                    TimeLastWrite =  file.LastWriteTime.ToString("d")
                };

                activeWindow.FindChild<WebBrowser>("pdfview").Navigate($"{_model.DestinationFile.FullPathWithExtension}");
            }
            catch
            {
                _model.DestinationFile = null;
            }
        }



        #endregion




        #region --------ShowClosePDFDocument and ShowClosePDFDocumentCommand

        private RelayCommand _showClosePDFDocumentCommand;
        public ICommand ShowClosePDFDocumentCommand
        {
            get
            {
                if (_showClosePDFDocumentCommand == null)
                {
                    _showClosePDFDocumentCommand = new RelayCommand(param => this.ShowClosePDFDocument(), param => this.CanClosePDFDocument);
                }

                return _showClosePDFDocumentCommand;
            }
        }

        public bool CanClosePDFDocument
        {
            get { return true; }
        }



        public void ShowClosePDFDocument()
        {
            activeWindow.FindChild<WebBrowser>("pdfview").Navigate("about:blank");
            AcrobatProcess();
            Task.Delay(500);
            _model.DestinationFile = null;
        }



        #endregion




        #region --------ShowSelectPDF2Add and ShowSelectPDF2AddCommand

        private RelayCommand _showSelectPDF2AddCommand;
        public ICommand ShowSelectPDF2Addommand
        {
            get
            {
                if (_showSelectPDF2AddCommand == null)
                {
                    _showSelectPDF2AddCommand = new RelayCommand(param => this.ShowSelectPDF2Add(), param => this.CanSelectPDF2Add);
                }

                return _showSelectPDF2AddCommand;
            }
        }

        public bool CanSelectPDF2Add
        {
            get { return true; }
        }



        public void ShowSelectPDF2Add()
        {
            var openFileDialog = new OpenFileDialog() { Filter = "PDF Files|*.pdf", InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop), Multiselect=true };
            if (openFileDialog.ShowDialog() != true)
                return;

            try
            {
                var infofiles = openFileDialog.FileNames.Select(x => new FileInfo(x)).ToList();

                var list = infofiles.Select(x => new cFileInfo { FullName = x.Name.Split('.')[0], Family = x.Directory.Name, FullPathWithExtension = x.FullName, Check2Add = true }).ToList();
                _model.PDF2Add = new ObservableCollection<cFileInfo>(list);
            }
            catch
            {
                _model.DestinationFile = null;
            }
        }


        #endregion




        #region --------ShowAddingPage and ShowAddingPageCommand

        private RelayCommand _showAddingPageCommand;
        public ICommand ShowAddingPageCommand
        {
            get
            {
                if (_showAddingPageCommand == null)
                {
                    _showAddingPageCommand = new RelayCommand(param => this.ShowAddingPage(), param => this.CanAddingPage);
                }

                return _showAddingPageCommand;
            }
        }

        public bool CanAddingPage
        {
            get { return true; }
        }



        public async void ShowAddingPage()
        {
            if (_model.PDF2Add == null || _model.DestinationFile ==  null)
                return;
            if (_model.PDF2Add.Where(w => w.Check2Add != false).Select(w => w).Count() == 0)
                return;
            var x = await activeWindow.ShowProgressAsync("Starting to Add Pages", "", false, s_without_animation);
            try
            {
                var converter = new ScannerImageConverter(_model.TempFolder);
                _model.IsMsgVisible = true;
              

                AcrobatProcess();
                await Task.Delay(500);

                foreach (var item in _model.PDF2Add)
                    if (item.Check2Add != false)
                        await Task.Factory.StartNew(() => converter.AddToExistingPDF(item.FullPathWithExtension, $@"{_model.TempFolder}{_model.DestinationFile.FullName}.pdf", x));

                activeWindow.FindChild<WebBrowser>("pdfview").Navigate($"{_model.TempFolder}{_model.DestinationFile.FullName}.pdf");
                await x.CloseAsync();
                _model.IsMsgVisible = false;
            }
            catch (Exception ex)
            {
                await x.CloseAsync();
                ShowErrorMessage(message: ex.Message);
            }

           
        }


        #endregion



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
            if (_model.FileSelected == null)
                return;
            var fullView = new Views.FullWindowView();
            fullView.Fullpdfview.Navigate($@"{_model.FileSelected.FullPathWithExtension}");
            var vm_fullView = new FullWindowModel() { TitleWindow = $@"OPENED FILE: {_model.FileSelected.FullName}" };
            fullView.DataContext = vm_fullView;
            fullView.ShowDialog();
        }




        #endregion



        #region --------ShowSave and ShowSaveCommand

        private RelayCommand _showSaveCommand;
        public ICommand ShowSaveCommand
        {
            get
            {
                if (_showSaveCommand == null)
                {
                    _showSaveCommand = new RelayCommand(param => this.ShowSave(), param => this.CanSave);
                }

                return _showSaveCommand;
            }
        }

        public bool CanSave
        {
            get { return true; }
        }



        public async void ShowSave()
        {
            if (_model.PDF2Add == null || _model.DestinationFile == null)
                return;
            if (_model.PDF2Add.Where(w => w.Check2Add != false).Select(w => w).Count() == 0)
                return;
            _model.IsMsgVisible = true;
            var x = await activeWindow.ShowProgressAsync("Saving file", $"", false, s_without_animation);
            AcrobatProcess();
            await Task.Delay(500);

            try
            {
                
                File.Copy($"{_model.TempFolder}{_model.DestinationFile.FullName}.pdf", $"{_model.DestinationFile.FullPathWithExtension}", true);
                x.SetTitle("File saved!");
                x.SetMessage($"{_model.DestinationFile.FullName}.pdf saved successfully");
                activeWindow.FindChild<WebBrowser>("pdfview").Navigate($"{_model.TempFolder}{_model.DestinationFile.FullName}.pdf");
                await Task.Delay(1500);
                await x.CloseAsync();
                _model.IsMsgVisible = false;
                
                
            }
            catch (Exception ex)
            {
                await x.CloseAsync();
                _model.IsMsgVisible = false;
                ShowErrorMessage(message:ex.Message);
            }
        }




        #endregion

        public async void ShowErrorMessage(string title = "Error", string message = "default message")
        {
            _model.IsMsgVisible = true;
            await activeWindow.ShowMessageAsync(title, message, MessageDialogStyle.Affirmative, s_err);
            _model.IsMsgVisible = false;
        }

    }
}
