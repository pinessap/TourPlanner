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
using System.Windows.Controls;

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
        private readonly ITourPlannerBl _tourPlannerBl;

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

        private ICommand _generateReportCommand = null!;
        public ICommand GenerateReportCommand => _generateReportCommand ??= new RelayCommand(GenerateReport);

        public string SearchValue { get; set; }

      

        public MainVm()
        {
            _tourPlannerBl = TourPlannerBlFactory.Instance;
            Tours = new ObservableCollection<Tour>();


            // Fill list box with all tours in DB
            var allTours = _tourPlannerBl.GetTours();

            FillListBox(allTours);
            Trace.WriteLine("mainvmm " + SearchValue);

        }


        public MainVm(string searchValue)
        {
            _tourPlannerBl = TourPlannerBlFactory.Instance;
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
             HandleException(() =>
             {
                 var foundItems = _tourPlannerBl.Search(searchValue);
                 FillListBox(foundItems);
             });
        }

        private void GenerateReport(object commandParameter) 
        {
            HandleException(() => _tourPlannerBl.GenerateSummarizedReport(_tourPlannerBl.GetTours()));
        }


    }
}
