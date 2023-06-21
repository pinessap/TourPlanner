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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TourPlanner.ViewModels;
using System.Runtime.InteropServices;
using System.Runtime;
using System.Windows.Interop;
using TourPlanner.Models;
using TourPlanner.Views;
using System.Windows.Controls.Primitives;

namespace TourPlanner
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            DataContext = new MainWindowVm();

            this.WindowStyle = WindowStyle.None;
            this.AllowsTransparency = true;
            this.ResizeMode = ResizeMode.CanResize;
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
            Application.Current.Shutdown();
        }

        private void fileBtn_Click(object sender, RoutedEventArgs e)
        {
            FilePopupWindow popupWindow = new FilePopupWindow();
            popupWindow.MainReloadRequested += PopupWindow_MainReloadRequested;
            popupWindow.ShowDialog();
        }

        private void configBtn_Click(object sender, RoutedEventArgs e)
        {
            ConfigSettingsWindow popupWindow = new ConfigSettingsWindow();
            popupWindow.ShowDialog();
        }

        private void PopupWindow_MainReloadRequested(object sender, EventArgs e)
        {
            if (DataContext is MainWindowVm viewModel)
            {
                // Call the SwitchToMainView method on the view model
                viewModel.SwitchToMainView(null); 
            }
        }

        private bool isResizing = false;
        private Point resizeStartPoint;

        private void MainWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                isResizing = true;
                resizeStartPoint = e.GetPosition(this);
            }
        }

        private void MainWindow_MouseMove(object sender, MouseEventArgs e)
        {
            if (isResizing)
            {
                Point currentPoint = e.GetPosition(this);
                double deltaX = currentPoint.X - resizeStartPoint.X;
                double deltaY = currentPoint.Y - resizeStartPoint.Y;

                this.Width += deltaX;
                this.Height += deltaY;

                resizeStartPoint = currentPoint;
            }
        }

        private void MainWindow_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            isResizing = false;
        }




    }
}
