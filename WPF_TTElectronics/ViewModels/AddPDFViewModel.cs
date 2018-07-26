using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Microsoft.Win32;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WPF_TTElectronics.Helpers;
using WPF_TTElectronics.Models;

namespace WPF_TTElectronics.ViewModels
{
    public class AddPDFViewModel : HelperClosePDFProcess
    {
        MetroWindow activeWindow = Application.Current.Windows.OfType<Views.MainBaseWindowsView>().FirstOrDefault();
        MetroDialogSettings s_err = new MetroDialogSettings { NegativeButtonText = "Cancel", AffirmativeButtonText = "Aceptar", ColorScheme = MetroDialogColorScheme.Inverted };
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
            var openFileDialog = new OpenFileDialog() { Filter = "PDF Files|*.pdf", InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) };
            if (openFileDialog.ShowDialog() != true)
                return;

            var file = new FileInfo(openFileDialog.FileName);


            if (nameFormat.IsMatch(file.Name.Split('.')[0]) != true)
            {
                ShowErrorMessage("Incorrect Format","The next format was expected => {Model}_{DateCode}.pdf");
                return;
            }

            

           
          
               


            try
            {


                _model.FileDestination = new cFileInfo()
                {
                    FullName = file.Name.Split('.')[0],
                    Model = file.Name.Split('_')[0],
                    DateCode = file.Name.Split('_')[1].Split('.')[0],
                    Family = file.Directory.Name,
                    FullPathWithExtension = file.FullName,
                    TimeCreation = file.CreationTime, 
                    TimeLastAccess = file.LastAccessTime,
                    TimeLastWrite = file.LastWriteTime
                };

                activeWindow.FindChild<WebBrowser>("pdfview").Navigate(_model.FileDestination.FullPathWithExtension);



            }
            catch
            {
                _model.FileDestination = null;
              
            }



        }



        #endregion




        #region --------ShowBrowseDestination and ShowBrowseDestinationCommand

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
            _model.FileDestination = null;


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
