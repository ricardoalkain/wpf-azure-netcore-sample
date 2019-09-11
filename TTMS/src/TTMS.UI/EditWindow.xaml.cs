using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;
using Microsoft.Win32;
using TTMS.Data.Models;

namespace TTMS.UI
{
    /// <summary>
    /// Interaction logic for EditWindow.xaml
    /// </summary>
    public partial class EditWindow : UserControl
    {
        public EditWindow()
        {
            InitializeComponent();
        }

        public void LoadPicture(object sender, RoutedEventArgs args)
        {
            var dlg = new OpenFileDialog();
            dlg.Title = "Select traveler's picture";
            dlg.Filter = "All supported graphics|*.jpg;*.jpeg;*.png;*.bmp|" +
                         "JPEG (*.jpg;*.jpeg)|*.jpg;*.jpeg|" +
                         "Portable Network Graphic (*.png)|*.png|" +
                         "Bitmap (*.bmp)|*.bmp";

            if (dlg.ShowDialog() == true)
            {
                (btPicture.Template.FindName("imPicture", btPicture) as Image).Source = new BitmapImage(new Uri(dlg.FileName));
            }
        }
    }
}
