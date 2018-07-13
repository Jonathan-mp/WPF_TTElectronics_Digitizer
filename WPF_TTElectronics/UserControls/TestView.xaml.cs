using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WPF_TTElectronics.Models;

namespace WPF_TTElectronics.UserControls
{
    /// <summary>
    /// Interaction logic for TestView.xaml
    /// </summary>
    public partial class TestView : UserControl
    {
        public TestView()
        {
            InitializeComponent();

        




        }

        private async void list_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {






            await Task.Factory.StartNew(() =>
            {
                App.Current.Dispatcher.Invoke(() =>
                {
                     pdfview.Navigate((list.SelectedValue!= null)? ((cFileInfo)list.SelectedValue).FullPathWithExtension : "about:blank");
                });

              



            });






        }


    }
}
