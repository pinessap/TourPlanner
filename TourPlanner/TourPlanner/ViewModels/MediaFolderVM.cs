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
    public class MediaFolderVM : ViewModelBase
    {
        private ITourFactory tourFactory;
        private Tour currentTour;
        private string searchValue;

        private ICommand searchCommand;
        public ICommand SearchCommand => searchCommand ??= new RelayCommand(Search);

        public ObservableCollection<Tour> Tours { get; set; }

        public Tour CurrentTour
        {
            get
            {
                return currentTour;
            }
            set
            {
                if ((currentTour != value) && (value != null))
                {
                    currentTour = value;
                    RaisePropertyChangedEvent(nameof(CurrentTour));
                }
            }
        }

        public string SearchValue
        {
            get { return searchValue; }
            set
            {
                if (searchValue != value)
                {
                    searchValue = value;
                    RaisePropertyChangedEvent(nameof(SearchValue));
                }
            }
        }
        public MediaFolderVM()
        {
            this.tourFactory = TourFactory.GetInstance();
            InitListBox();
        }

        private void InitListBox()
        {
            Tours = new ObservableCollection<Tour>();
            FillListBox();
        }

        private void FillListBox()
        {
            foreach (Tour item in this.tourFactory.GetTours())
            {
                Tours.Add(item);
            }
        }

        private void Search(object commandParameter)
        {
            IEnumerable foundItems = this.tourFactory.Search(SearchValue);
            Tours.Clear();
            foreach (Tour item in foundItems)
            {
                Tours.Add(item);
            }
        }
    }
}
