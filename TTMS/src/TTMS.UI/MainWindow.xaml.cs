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
using TTMS.Data.Entities;
using TTMS.Data.Repositories;
using TTMS.Data.Services;
using TTMS.UI.Helpers;
using Unity;

namespace TTMS.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void CloseWindowCommand(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }


}
