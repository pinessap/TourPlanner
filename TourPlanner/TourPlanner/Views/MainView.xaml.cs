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
using TourPlanner.Models;
using TourPlanner.ViewModels;

namespace TourPlanner.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {



        public MainView()
        {
            InitializeComponent();
        }


        private void ListBoxItem_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            // Find the parent MainView
            var mainView = FindVisualParent<MainView>((DependencyObject)sender);

            // Retrieve the DataContext of the MainView (which should be an instance of MainVm)
            var viewModel = (MainVm)mainView.DataContext;


            // Execute the command in View1ViewModel to switch to TourDetailsView
            viewModel.ExecuteWithTour(null);

           
        }

        private static T FindVisualParent<T>(DependencyObject child) where T : DependencyObject
        {
            var parentObject = VisualTreeHelper.GetParent(child);

            if (parentObject == null)
                return null;

            if (parentObject is T parent)
                return parent;

            return FindVisualParent<T>(parentObject);
        }

        private void AddTour_Click(object sender, RoutedEventArgs e)
        {
            // Find the parent MainView
            var mainView = FindVisualParent<MainView>((DependencyObject)sender);

            // Retrieve the DataContext of the MainView (which should be an instance of MainVm)
            var viewModel = (MainVm)mainView.DataContext;

            // Execute the command in View1ViewModel to switch to AddTourView
            viewModel.Execute(null);
        }

        




    }
}
