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
    /// Interaction logic for EditLogView.xaml
    /// </summary>
    public partial class EditLogView : UserControl
    {
        public EditLogView()
        {
            InitializeComponent();
        }

        private void EditLog_Click(object sender, RoutedEventArgs e)
        {
            var bindingExpression = logTime.GetBindingExpression(TextBox.TextProperty);
            bindingExpression.UpdateSource();
        }
    }
}
