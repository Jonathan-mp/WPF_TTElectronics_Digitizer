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
            _model.FileDestination = null;


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

                var list = infofiles.Select(x => new cFileInfo { FullName = x.Name.Split('.')[0], Family = x.Directory.Name,  Check2Add = true }).ToList();

                _model.PDF2Add = new ObservableCollection<cFileInfo>(list);

                       
                   
               
                

               

                //activeWindow.FindChild<WebBrowser>("pdfview").Navigate(_model.FileDestination.FullPathWithExtension);
            }
            catch
            {
                _model.FileDestination = null;
            }





        }


        #endregion









        #region --------TEST

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



        public async void ShowTest()
        {

            foreach (var item in _model.PDF2Add)
            {
                _model.IsMsgVisible = true;
               await activeWindow.ShowMessageAsync(item.Check2Add.ToString(), item.FullName);
                //await Task.Delay(1000);
                _model.IsMsgVisible = false;
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
