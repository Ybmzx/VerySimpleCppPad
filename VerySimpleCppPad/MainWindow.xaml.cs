using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VerySimpleCppPad.ViewModels;

namespace VerySimpleCppPad
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainWindowVM ViewModel)
        {
            InitializeComponent();
            this.ViewModel = ViewModel;
            this.DataContext = this;
        }

        public MainWindowVM ViewModel { get; }

        private void Drag_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (!File.Exists(ViewModel.CurrentProgramFile)) return;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                // 创建 DataObject 包含链接的 URL
                DataObject data = new DataObject(System.Windows.DataFormats.FileDrop, new string[] { System.IO.Path.GetFullPath(ViewModel.CurrentProgramFile) });

                // 开始拖拽操作
                DragDrop.DoDragDrop(sender as TextBlock, data, DragDropEffects.Copy);
            }
        }
    }
}