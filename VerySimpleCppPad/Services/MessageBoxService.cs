using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static VerySimpleCppPad.Services.MessageBoxService;
using System.Windows;
using VerySimpleCppPad.Views;

namespace VerySimpleCppPad.Services
{
    public class MessageBoxService : IMessageBoxService
    {
        public void Show(string message, string caption, MessageBoxButton? messageBoxButton = null, MessageBoxImage? messageBoxImage = null)
        {
            MessageBox.Show(message, caption, messageBoxButton ?? MessageBoxButton.OK, messageBoxImage ?? MessageBoxImage.Information);
        }

        public string? ShowInputBox(string message, string caption, string defaultValue = "")
        {
            return InputBox.Show(message, caption, defaultValue);
        }
    }
}
