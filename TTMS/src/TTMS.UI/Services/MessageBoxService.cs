using System.Windows;

namespace TTMS.UI.Services
{
    public class MessageBoxService : IMessageBoxService
    {
        public void Show(string message, string title = null)
        {
            MessageBox.Show(Application.Current.MainWindow, message, title ?? "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void Error(string message, string title = null)
        {
            MessageBox.Show(Application.Current.MainWindow, message, title ?? "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void Warning(string message, string title = null)
        {
            MessageBox.Show(Application.Current.MainWindow, message, title ?? "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public bool Confirm(string message, string title = null)
        {
            var result = MessageBox.Show(Application.Current.MainWindow, message, title ?? "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

            return (result == MessageBoxResult.Yes);
        }
    }
}
