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

namespace TourPlanner.Views
{
    /// <summary>
    /// Interaction logic for TourDetailsView.xaml
    /// </summary>
    public partial class TourDetailsView : UserControl
    {
        public TourDetailsView()
        {
            InitializeComponent();
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

        private void EditTour_Click(object sender, RoutedEventArgs e)
        {
            // Find the parent View
            var tourDetailsView = FindVisualParent<TourDetailsView>((DependencyObject)sender);

            // Retrieve the DataContext of the View (which should be an instance of TourDetailsVm)
            var viewModel = (TourDetailsVm)tourDetailsView.DataContext;

            // Execute the command in View1ViewModel to switch to AddTourView
            viewModel.ExecuteWithTour(null);
        }

        private void AddLog_Click(object sender, RoutedEventArgs e)
        {
            // Find the parent View
            var tourDetailsView = FindVisualParent<TourDetailsView>((DependencyObject)sender);

            // Retrieve the DataContext of the View (which should be an instance of TourDetailsVm)
            var viewModel = (TourDetailsVm)tourDetailsView.DataContext;

            // Execute the command in View1ViewModel to switch to AddLogView
            viewModel.ExecuteToAddLog(null);
        }

        private void ListBoxLogItem_DoubleClick(object sender, MouseButtonEventArgs e)
        {

            // Find the parent View
            var tourDetailsView = FindVisualParent<TourDetailsView>((DependencyObject)sender);

            // Retrieve the DataContext of the View (which should be an instance of TourDetailsVm)
            var viewModel = (TourDetailsVm)tourDetailsView.DataContext;

            // Execute the command in View1ViewModel to switch to EditLogView
            viewModel.ExecuteToEditLog(null);
        }
    }
}
