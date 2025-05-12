using System.Windows;

namespace VerySimpleCppPad.Services
{
    public interface IMessageBoxService
    {
        void Show(string message, string caption, MessageBoxButton? messageBoxButton = null, MessageBoxImage? messageBoxImage = null);
        string? ShowInputBox(string message, string caption, string defaultValue = "");
    }
}