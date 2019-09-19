using System.Windows;
using System.Windows.Threading;
using Serilog;

namespace TTMS.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public void ApplicationExceptionHandler(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            Log.Logger.Error(e.Exception, "Unhandled exception");
            e.Handled = true;
        }
    }
}
