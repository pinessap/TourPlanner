using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using TourPlanner.BusinessLayer;
using TourPlanner.Models;
using TourPlanner.ViewModels;

namespace TourPlanner.ViewModels
{
    public class MainWindowVm : ViewModelBase
    {
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
        }
        
        public MainWindowVm()
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
        
        /// <summary>
        /// Function called when "Search" button is pressed
        /// </summary>
        /// <param name="commandParameter">Gets automatically assigned by ICommand, dunno what's in there tbh but who cares</param>
        private void Search(object commandParameter)
        {
            var foundItems = _tourFactory.Search(SearchValue);
            
            FillListBox(foundItems);
        }
        
        /// <summary>
        /// Function called when "Add dummy tour" button is pressed
        /// </summary>
        /// <param name="commandParameter">Gets automatically assigned by ICommand, dunno what's in there tbh but who cares</param>
        private void Add(object commandParameter)
        {
            // TODO: Update the UI so we can create a tour from there and then pass it as a new Tour() to the tourFactory
            if (!_tourFactory.Add())
            {
                // TODO: Show an error when adding didn't work for some reason
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
            CurrentTour.Description = "Testing if this works. I should find this maybe hopefully.";
            
            if (!_tourFactory.Modify(CurrentTour))
            {
                // TODO: Show an error when deleting didn't work for some reason
            }
            
            var remainingItems = _tourFactory.GetTours();
            
            FillListBox(remainingItems);
        }
    }
}
