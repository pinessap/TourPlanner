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
using System.Diagnostics;

namespace TourPlanner.ViewModels
{
    public class MainVm : ViewModelBase
    {

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

        public string SearchValue { get; set; }
        /*private string _searchValue = null!;

         
        /// <summary>
        /// The two-way bound search string
        /// </summary>
        public string SearchValue
        {
            get => _searchValue;
            set
            {
               // if (_searchValue != value)
                //{
                    _searchValue = value;
                    RaisePropertyChangedEvent();
                //}
            }
        }*/

        public MainVm()
        {
            _tourFactory = TourFactory.Instance;
            Tours = new ObservableCollection<Tour>();


            // Fill list box with all tours in DB
            var allTours = _tourFactory.GetTours();

            FillListBox(allTours);
            Trace.WriteLine("mainvmm " + SearchValue);

        }


        public MainVm(string searchValue)
        {
            _tourFactory = TourFactory.Instance;
            Tours = new ObservableCollection<Tour>();

            Trace.WriteLine("mainvm " + SearchValue);

            Search(searchValue);


            Trace.WriteLine("mainvmm " + SearchValue);



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


        /// <summary>
        /// Function called when "Search" button is pressed
        /// </summary>
        /// <param name="commandParameter">Gets automatically assigned by ICommand, dunno what's in there tbh but who cares</param>
        private void Search(string searchValue)
        {
            try
            {
                var foundItems = _tourFactory.Search(searchValue);
                FillListBox(foundItems);
            }
            catch (Exception ex)
            {
                // TODO: Deal with different exceptions, probably display them in the UI somehow
            }
        }



    }
}
