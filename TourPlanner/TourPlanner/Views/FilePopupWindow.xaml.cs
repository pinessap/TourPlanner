using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TourPlanner.Views
{
    /// <summary>
    /// Interaction logic for FilePopupWindow.xaml
    /// </summary>
    public partial class FilePopupWindow : Window
    {
        public FilePopupWindow()
        {
            InitializeComponent();
        }

        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);
        private void ControlBar_OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowInteropHelper helper = new WindowInteropHelper(this);
            SendMessage(helper.Handle, 161, 2, 0);
        }

        private void ControlBar_OnMouseEnter(object sender, MouseEventArgs e)
        {
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        }

        private void BtnMinimize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void BtnMaximize_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = this.WindowState == WindowState.Normal ? WindowState.Maximized : WindowState.Normal;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Window window = Window.GetWindow(this);
            window.Closing += Window_Closing;
            window.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Raise a custom event to notify the main window to reload
            MainReloadRequested?.Invoke(this, EventArgs.Empty);
        }

        public event EventHandler MainReloadRequested;
    }
}
