﻿using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TourPlanner.BusinessLayer;
using TourPlanner.Logging;
using TourPlanner.Messages;
using TourPlanner.Models;

namespace TourPlanner.ViewModels
{
    public class TourDetailsVm : ViewModelBase
    {
        public Tour SelectedTour { get; set; }

        /// <summary>
        /// Connection to our business logic
        /// </summary>
        private readonly ITourFactory _tourFactory;

        
        private ICommand _deleteCommand = null!;

        /// <summary>
        /// The ICommand bound to the "Delete selected tour" button
        /// </summary>
        public ICommand DeleteCommand => _deleteCommand ??= new RelayCommand(Delete);

        /// <summary>
        /// The ObservableCollection bound to the ListBox that lists the tour names
        /// </summary>
        public ObservableCollection<Tour> Tours { get; set; }



        public TourDetailsVm()
        {
            // Subscribe to the SwitchViewMessage
            Messenger.Default.Register<SwitchViewMessage>(this, HandleSwitchViewMessage);

            _tourFactory = TourFactory.Instance;
            Tours = new ObservableCollection<Tour>();
        }

        /// <summary>
        /// Function called when "Delete selected tour" button is pressed
        /// </summary>
        /// <param name="commandParameter">Gets automatically assigned by ICommand, dunno what's in there tbh but who cares</param>
        private void Delete(object commandParameter)
        {
            
            try
            {
                _tourFactory.Delete(SelectedTour);
            }
            catch (Exception ex)
            {
                // TODO: Deal with different exceptions, probably display them in the UI somehow
            }

        }

        /// <summary>
        /// Function called when "Edit selected tour" button is pressed
        /// </summary>
        /// <param name="commandParameter">Gets automatically assigned by ICommand, dunno what's in there tbh but who cares</param>
        private void Edit(object commandParameter) //TODO: Edit should refer to a new view with input fields
        {

            try
            {
                _tourFactory.Modify(SelectedTour);
            }
            catch (Exception ex)
            {
                // TODO: Deal with different exceptions, probably display them in the UI somehow
            }

        }

        public void ExecuteWithTour(object parameter)
        {
            // Retrieve the selected tour
            var selectedTour = SelectedTour;

            // Send a message to MainViewModel to switch to EditTourView
            Messenger.Default.Send(new SwitchViewMessage(typeof(EditTourVm), selectedTour));
        }

        public void ExecuteToAddLog(object parameter)
        {
            // Retrieve the selected tour
            var selectedTour = SelectedTour;

            // Send a message to MainViewModel to switch to View
            Messenger.Default.Send(new SwitchViewMessage(typeof(AddLogVm), selectedTour));
        }
        private void HandleSwitchViewMessage(SwitchViewMessage message)
        {
            if (message.ViewModelType == typeof(TourDetailsVm))
            {
                SelectedTour = message.SelectedTour;
                // Handle the selected tour in TourDetailsView
            }
        }
    }
}