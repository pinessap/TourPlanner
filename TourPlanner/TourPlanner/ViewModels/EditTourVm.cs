﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Messaging;
using TourPlanner.BusinessLayer;
using TourPlanner.Messages;
using TourPlanner.Models;

namespace TourPlanner.ViewModels
{
    public class EditTourVm : ViewModelBase
    {
        /// <summary>
        /// Connection to our business logic
        /// </summary>
        private readonly ITourPlannerBl _tourPlannerBl;

        private Tour _selectedTour;
        public Tour SelectedTour
        {
            get { return _selectedTour; }
            set
            {
                if (_selectedTour != value)
                {
                    _selectedTour = value;
                    RaisePropertyChangedEvent();
                }
            }
        }

        private ICommand _editCommand = null!;

        /// <summary>
        /// The ObservableCollection bound to the ListBox that lists the tour names
        /// </summary>
        public ObservableCollection<Tour> Tours { get; set; }

        /// <summary>
        /// The ICommand bound to the "Add dummy tour" button
        /// </summary>
        public ICommand EditCommand => _editCommand ??= new RelayCommand(Edit);

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

        private string _transportType;
        public string TourTransportType
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
                    TourTransportType = "CAR";
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
                    TourTransportType = "WALKING";
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
                    TourTransportType = "BICYCLE";
            }
        }


        public EditTourVm()
        {
            // Subscribe to the SwitchViewMessage
            Messenger.Default.Register<SwitchViewMessage>(this, HandleSwitchViewMessage);

            _tourPlannerBl = TourPlannerBlFactory.Instance;
            Tours = new ObservableCollection<Tour>();
        }

        /// <summary>
        /// Function called when "Edit selected tour" button is pressed
        /// </summary>
        /// <param name="commandParameter">Gets automatically assigned by ICommand, dunno what's in there tbh but who cares</param>
        private void Edit(object commandParameter)
        {
            HandleException(() => _tourPlannerBl.Modify(SelectedTour));
        }

        private void HandleSwitchViewMessage(SwitchViewMessage message)
        {
            if (message.ViewModelType == typeof(TourDetailsVm))
            {
                SelectedTour = message.SelectedTour;
            }
        }
    }
}
