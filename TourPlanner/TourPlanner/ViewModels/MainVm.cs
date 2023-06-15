using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;
using TourPlanner.BusinessLayer;
using TourPlanner.Models;
using GalaSoft.MvvmLight.Messaging;
using TourPlanner.Messages;

namespace TourPlanner.ViewModels
{
    public class MainVm : ViewModelBase
    {

        /*public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return true;
        }*/
        public void Execute(object parameter)
        {
            // Send a message to MainViewModel to switch to AddTourView
            Messenger.Default.Send(new SwitchViewMessage(typeof(AddTourVm)));
        }

        public void ExecuteWithTour(object parameter)
        {
            // Retrieve the selected tour
            var selectedTour = CurrentTour;

            // Send a message to MainViewModel to switch to TourDetailsView
            Messenger.Default.Send(new SwitchViewMessage(typeof(TourDetailsVm), selectedTour));
        }


        /// <summary>
        /// Connection to our business logic
        /// </summary>
        private readonly ITourFactory _tourFactory;

        private Tour _currentTour = null!;


        /// <summary>
        /// The ObservableCollection bound to the ListBox that lists the tour names
        /// </summary>
        public ObservableCollection<Tour> Tours { get; set; }

        /// <summary>
        /// The tour currently selected
        /// </summary>
        public Tour CurrentTour
        {
            get => _currentTour;
            set
            {
                if (_currentTour != value)
                {
                    _currentTour = value;
                    RaisePropertyChangedEvent();
                }
            }
        }

        public MainVm()
        {
            _tourFactory = TourFactory.Instance;
            Tours = new ObservableCollection<Tour>();

            // Fill list box with all tours in DB
            var allTours = _tourFactory.GetTours();

            FillListBox(allTours);

        }

        

        /// <summary>
        /// Replaces ListBox content with all provided tours
        /// </summary>
        /// <param name="toursToDisplay">List of the tours we want our ListBox to display</param>
        private void FillListBox(List<Tour> toursToDisplay)
        {
            Tours.Clear();

            foreach (var item in toursToDisplay)
            {
                Tours.Add(item);
            }
        }



    }
}
