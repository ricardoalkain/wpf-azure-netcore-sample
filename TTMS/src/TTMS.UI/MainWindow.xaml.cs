using System.Windows;

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
