namespace TTMS.UI.Services
{
    public interface IMessageBoxService
    {
        bool Confirm(string message, string title = null);
        void Error(string message, string title = null);
        void Show(string message, string title = null);
        void Warning(string message, string title = null);
    }
}