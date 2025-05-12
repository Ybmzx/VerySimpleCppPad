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

namespace VerySimpleCppPad.Views
{
    /// <summary>
    /// InputBox.xaml 的交互逻辑
    /// </summary>
    public partial class InputBox : Window
    {
        public string? Result { get; set; } = null;

        public InputBox(string message, string caption, string defaultValue = "")
        {
            InitializeComponent();

            this.Title = caption;
            this.Message.Text = message;
            this.InputTextBox.Text = defaultValue;
        }

        private void OKButtonClicked(object sender, RoutedEventArgs e)
        {
            Result = InputTextBox.Text;
            this.Close();
        }

        private void CancelButtonClicked(object sender, RoutedEventArgs e)
        {
            Result = null;
            this.Close();
        }

        public static string? Show(string message, string caption, string defaultValue = "")
        {
            var inputBox = new InputBox(message, caption, defaultValue);
            inputBox.ShowDialog();
            return inputBox.Result;
        }

    }
}
