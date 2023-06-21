using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
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
using System.Windows.Forms;
using TourPlanner.Configuration;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;


namespace TourPlanner.Views
{
    /// <summary>
    /// Interaction logic for ConfigSettingsWindow.xaml
    /// </summary>
    public partial class ConfigSettingsWindow : Window
    {
        public ConfigSettingsWindow()
        {
            InitializeComponent();

            DbSettingToStrings(AppConfigManager.Settings.DbConnection);

            MapQuestApiKeyTextBox.Text = AppConfigManager.Settings.MapQuestApiKey;
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
            window.Close();
        }

        private void SelectTemplateDirectory_Click(object sender, RoutedEventArgs e)
        {

            Trace.WriteLine("pre test:");
            Trace.WriteLine(AppConfigManager.Settings.TemplateDirectory);
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.SelectedPath = AppConfigManager.Settings.TemplateDirectory;

                DialogResult result = dialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    // User clicked the "OK" button
                    // Perform actions for the selected directory
                    // Handle the selected directory path
                    string selectedDirectory = dialog.SelectedPath;

                    // Update the corresponding setting
                    AppConfigManager.Settings.TemplateDirectory = selectedDirectory;
                    Trace.WriteLine("post test:");
                    Trace.WriteLine(AppConfigManager.Settings.TemplateDirectory);
                }

            }
        }

        private void SelectPictureDirectory_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.SelectedPath = AppConfigManager.Settings.PictureDirectory;

                DialogResult result = dialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    // User clicked the "OK" button
                    // Perform actions for the selected directory
                    // Handle the selected directory path
                    string selectedDirectory = dialog.SelectedPath;

                    // Update the corresponding setting
                    AppConfigManager.Settings.PictureDirectory = selectedDirectory;
                }

            }
        }

        private void SelectOutputDirectory_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.SelectedPath = AppConfigManager.Settings.OutputDirectory;

                DialogResult result = dialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    // User clicked the "OK" button
                    // Perform actions for the selected directory
                    // Handle the selected directory path
                    string selectedDirectory = dialog.SelectedPath;

                    // Update the corresponding setting
                    AppConfigManager.Settings.OutputDirectory = selectedDirectory;
                }

            }
        }

        private void SelectLogFilePath_Click(object sender, RoutedEventArgs e)
        {
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.SelectedPath = AppConfigManager.Settings.LogfilePath;

                DialogResult result = dialog.ShowDialog();

                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    // User clicked the "OK" button
                    // Perform actions for the selected directory
                    // Handle the selected directory path
                    string selectedDirectory = dialog.SelectedPath;

                    // Update the corresponding setting
                    AppConfigManager.Settings.LogfilePath = selectedDirectory;
                }

            }
        }

        private void SaveDirectory_Click(Object sender, RoutedEventArgs e)
        {
            string host = HostTextBox.Text;
            string database = DatabaseTextBox.Text;
            string username = UsernameTextBox.Text;
            string password = PasswordTextBox.Text;

            string mapKey = MapQuestApiKeyTextBox.Text;

            AppConfigManager.Settings.DbConnection = DbConnectionString(host, database, username, password);
            AppConfigManager.Settings.MapQuestApiKey = mapKey;


            AppConfigManager.SaveSettings();
            Trace.WriteLine("post post test:");
            Trace.WriteLine(AppConfigManager.Settings.TemplateDirectory);
            Window window = Window.GetWindow(this);
            window.Close();
        }

        private string DbConnectionString(string host, string database, string username, string password)
        {
            return string.Format("Host={0}; Database={1}; Username={2}; Password={3}", host, database, username, password);
        }

        public void DbSettingToStrings(string dbConnection)
        {

            var values = dbConnection.Split(';');
            foreach (var value in values)
            {
                var keyValue = value.Split('=');
                if (keyValue.Length == 2)
                {
                    var key = keyValue[0].Trim().ToLower();
                    var val = keyValue[1].Trim();

                    // Assign the values to the corresponding text boxes based on the key
                    if (key == "host")
                    {
                        HostTextBox.Text = val;
                    }
                    else if (key == "database")
                    {
                        DatabaseTextBox.Text = val;
                    }
                    else if (key == "username")
                    {
                        UsernameTextBox.Text = val;
                    }
                    else if (key == "password")
                    {
                        PasswordTextBox.Text = val;
                    }
                }
            }

        }



    }
}
