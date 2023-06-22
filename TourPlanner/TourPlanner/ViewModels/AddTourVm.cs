using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TourPlanner.BusinessLayer;
using TourPlanner.Models;

namespace TourPlanner.ViewModels
{
    public class AddTourVm : ViewModelBase
    {
        /// <summary>
        /// Connection to our business logic
        /// </summary>
        private readonly ITourPlannerBl _tourPlannerBl;

        private Tour _currentTour = null!; // Diese dinger sind nur "= null!" um die warnungen im konstruktur zu unterbinden, hat keine programmrelevanz :)
     
        private ICommand _addCommand = null!;

        /// <summary>
        /// The ObservableCollection bound to the ListBox that lists the tour names
        /// </summary>
        public ObservableCollection<Tour> Tours { get; set; }

        /// <summary>
        /// The ICommand bound to the "Add dummy tour" button
        /// </summary>
        public ICommand AddCommand => _addCommand ??= new RelayCommand(Add);

        private string _tourImageSource;
        public string TourImageSource
        {
            get { return _tourImageSource; }
            set
            {
                _tourImageSource = value;
                RaisePropertyChangedEvent();
            }
        }


        private string _tourName;
        public string TourName
        {
            get { return _tourName; }
            set
            {
                _tourName = value;
                RaisePropertyChangedEvent();
            }
        }

        private string _tourDescription;
        public string TourDescription
        {
            get { return _tourDescription; }
            set
            {
                _tourDescription = value;
                RaisePropertyChangedEvent();
            }
        }

        private string _fromLocation;
        public string TourFrom
        {
            get { return _fromLocation; }
            set
            {
                _fromLocation = value;
                RaisePropertyChangedEvent();
            }
        }

        private string _toLocation;
        public string TourTo
        {
            get { return _toLocation; }
            set
            {
                _toLocation = value;
                RaisePropertyChangedEvent();
            }
        }

        private Tour.TransportTypes _transportType;
        public Tour.TransportTypes TourTransportType
        {
            get { return _transportType; }
            set
            {
                _transportType = value;
                RaisePropertyChangedEvent();
            }
        }

        private bool _isCarSelected;
        public bool IsCarSelected
        {
            get { return _isCarSelected; }
            set
            {
                _isCarSelected = value;
                RaisePropertyChangedEvent();

                if (value)
                    TourTransportType = Tour.TransportTypes.Car;
            }
        }

        private bool _isWalkingSelected;
        public bool IsWalkingSelected
        {
            get { return _isWalkingSelected; }
            set
            {
                _isWalkingSelected = value;
                RaisePropertyChangedEvent();

                if (value)
                    TourTransportType = Tour.TransportTypes.Walking;
            }
        }

        private bool _isBicycleSelected;
        public bool IsBicycleSelected
        {
            get { return _isBicycleSelected; }
            set
            {
                _isBicycleSelected = value;
                RaisePropertyChangedEvent();

                if (value)
                    TourTransportType = Tour.TransportTypes.Bicycle;
            }
        }


        public AddTourVm()
        {
            _tourPlannerBl = TourPlannerBlFactory.Instance;
            Tours = new ObservableCollection<Tour>();
            
        }

        /// <summary>
        /// Function called when "Add dummy tour" button is pressed
        /// </summary>
        /// <param name="commandParameter">Gets automatically assigned by ICommand, dunno what's in there tbh but who cares</param>
        private async void Add(object commandParameter)
        {
            // Get existing tours and either set the lastTourId to 0 or whatever the last id in the DB is
            /*var existingTours = _tourPlannerBl.GetTours();
            var lastTourId = existingTours.Count > 0 ? _tourPlannerBl.GetTours().Last().TourId : 0;

            string name = TourName;

            //BUG: If the database is empty, but the autoincrement field is not at 0, the "new" first entry might f.ex. get the idValue 7 instead of the assumed value 1
            var newTourName = name + " " + (lastTourId + 1);*/
            Trace.WriteLine("tour name: " + TourName);

            
            var tourToAdd = new Tour
            {
                Name = TourName,
                Description = TourDescription,
                FromLocation = TourFrom,
                ToLocation = TourTo,
                EstimatedTime = null,
                TourDistance = null,
                TransportType = TourTransportType,
                Logs = new List<TourLog>
                {
                    /*new()
                    {
                        Comment = "Help me pls I am dying",
                        Difficulty = 10,
                        Duration = new TimeSpan(99, 5, 0),
                        Rating = 5f,
                        Time = new DateTime(2000, 12, 31, 0, 0, 0, DateTimeKind.Utc)
                    }*/
                }
            };

                
            await HandleExceptionAsync( () => _tourPlannerBl.Add(tourToAdd));

            TourImageSource = tourToAdd.PathToRouteImage;




        }
    }
}
