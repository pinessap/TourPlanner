using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using TourPlanner.BusinessLayer;
using TourPlanner.Messages;
using TourPlanner.Models;
using TourPlanner.ViewModels;
using TourPlanner.Views;
using static TourPlanner.ViewModels.MainVm;
using static TourPlanner.Views.MainView;

namespace TourPlanner.ViewModels
{
    public class MainWindowVm : ViewModelBase
    {

        private ICommand _mainViewCommand = null!;
        public ICommand MainViewCommand => _mainViewCommand ??= new RelayCommand(SwitchToMainView);


        public MainVm MainViewModel { get; set; }

        private object _currentView;

        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                RaisePropertyChangedEvent(nameof(CurrentView));
            }
            
        }

        private void SwitchToMainView(object commandParameter)
        {
            var mainVm = new MainVm();
            var mainView = new MainView();
            mainView.DataContext = mainVm;
            CurrentView = mainView;
        }

        /*
        /// <summary>
        /// Connection to our business logic
        /// </summary>
        private readonly ITourFactory _tourFactory;
        
        private Tour _currentTour = null!; // Diese dinger sind nur "= null!" um die warnungen im konstruktur zu unterbinden, hat keine programmrelevanz :)
        private string _searchValue = null!;
        private ICommand _searchCommand = null!;
        private ICommand _addCommand = null!;
        private ICommand _deleteCommand = null!;
        
        /// <summary>
        /// The ICommand bound to the "Search" button
        /// </summary>
        public ICommand SearchCommand => _searchCommand ??= new RelayCommand(Search);
        
        /// <summary>
        /// The ICommand bound to the "Add dummy tour" button
        /// </summary>
        public ICommand AddCommand => _addCommand ??= new RelayCommand(Add);
        
        /// <summary>
        /// The ICommand bound to the "Delete selected tour" button
        /// </summary>
        public ICommand DeleteCommand => _deleteCommand ??= new RelayCommand(Delete);

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
        
        /// <summary>
        /// The two-way bound search string
        /// </summary>
        public string SearchValue
        {
            get => _searchValue;
            set
            {
                if (_searchValue != value)
                {
                    _searchValue = value;
                    RaisePropertyChangedEvent();
                }
            }
        }*/

        public MainWindowVm()
        {
            MainViewModel = new MainVm();

            CurrentView = MainViewModel;

            Messenger.Default.Register<SwitchViewMessage>(this, HandleSwitchViewMessage);
            /*
            _tourFactory = TourFactory.Instance;
            Tours = new ObservableCollection<Tour>();
            
            // Fill list box with all tours in DB
            var allTours = _tourFactory.GetTours();
            
            FillListBox(allTours);*/
        }

        private void HandleSwitchViewMessage(SwitchViewMessage message)
        {
            if (message.ViewModelType == typeof(TourDetailsVm))
            {

                // Retrieve the selected tour
                var selectedTour = message.SelectedTour;

                // Create an instance of TourDetailsVm
                var tourDetailsVm = new TourDetailsVm();

                // Set the selected tour in the TourDetailsVm
                tourDetailsVm.SelectedTour = selectedTour;

                // Create an instance of TourDetailsView
                var tourDetailsView = new TourDetailsView();

                // Set the DataContext of the TourDetailsView to the TourDetailsVm
                tourDetailsView.DataContext = tourDetailsVm;

                // Set the CurrentView to the TourDetailsView
                CurrentView = tourDetailsView;

            } else if (message.ViewModelType == typeof(AddTourVm))
            {
                var addTourVm = new AddTourVm();
                var addTourView = new AddTourView();
                addTourView.DataContext = addTourVm;
                CurrentView = addTourView;


            }
        }

        /*
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
        private void Search(object commandParameter)
        {
            try
            {
                var foundItems = _tourFactory.Search(SearchValue);
                FillListBox(foundItems);
            }
            catch (Exception ex)
            {
                // TODO: Deal with different exceptions, probably display them in the UI somehow
            }
        }
        
        /// <summary>
        /// Function called when "Add dummy tour" button is pressed
        /// </summary>
        /// <param name="commandParameter">Gets automatically assigned by ICommand, dunno what's in there tbh but who cares</param>
        private void Add(object commandParameter)
        {
            // Get existing tours and either set the lastTourId to 0 or whatever the last id in the DB is
            var existingTours = _tourFactory.GetTours();
            var lastTourId = existingTours.Count > 0 ? _tourFactory.GetTours().Last().TourId : 0;
            
            // BUG: If the database is empty, but the autoincrement field is not at 0, the "new" first entry might f.ex. get the idValue 7 instead of the assumed value 1
            var newTourName = "Tour with Database-ID " + (lastTourId + 1);

            var tourToAdd = new Tour
            {
                Name = newTourName,
                Description = "Mc?",
                FromLocation = "Productivity",
                ToLocation = "Nati's Folterkeller",
                EstimatedTime = new DateTime(2000, 12, 31, 0, 0, 0, DateTimeKind.Utc),
                TourDistance = 12f,
                TransportType = "White candy van",
                Logs = new List<TourLog>
                {
                    new()
                    {
                        Comment = "Help me pls I am dying",
                        Difficulty = 10,
                        Duration = new TimeSpan(99, 5, 0),
                        Rating = 5f,
                        Time = new DateTime(2000, 12, 31, 0, 0, 0, DateTimeKind.Utc)
                    }
                }
            };

            try
            {
                _tourFactory.Add(tourToAdd);
            }
            catch (Exception ex)
            {
                // TODO: Deal with different exceptions, probably display them in the UI somehow
            }

            var addedItems = _tourFactory.GetTours();
            
            FillListBox(addedItems);
        }
        
        /// <summary>
        /// Function called when "Delete selected tour" button is pressed
        /// </summary>
        /// <param name="commandParameter">Gets automatically assigned by ICommand, dunno what's in there tbh but who cares</param>
        private void Delete(object commandParameter)
        {
            try
            {
                _tourFactory.Delete(CurrentTour);
            }
            catch (Exception ex)
            {
                // TODO: Deal with different exceptions, probably display them in the UI somehow
            }
            
            var remainingItems = _tourFactory.GetTours();
            
            FillListBox(remainingItems);
        }*/
    }
}
