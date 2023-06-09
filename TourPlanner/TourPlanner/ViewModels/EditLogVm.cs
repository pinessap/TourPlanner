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
using TourPlanner.Messages;
using TourPlanner.Models;

namespace TourPlanner.ViewModels
{
    class EditLogVm : ViewModelBase
    {
        /// <summary>
        /// Connection to our business logic
        /// </summary>
        private readonly ITourPlannerBl _tourPlannerBl;

        public Tour SelectedTour { get; set; }

        public TourLog SelectedLog { get; set; }

        //private Tour _currentTour = null!; // Diese dinger sind nur "= null!" um die warnungen im konstruktur zu unterbinden, hat keine programmrelevanz :)

        private ICommand _editCommand = null!;

        /// <summary>
        /// The ObservableCollection bound to the ListBox that lists the tour names
        /// </summary>
        public ObservableCollection<Tour> Tours { get; set; }

        /// <summary>
        /// The ICommand bound to the "Add dummy tour" button
        /// </summary>
        public ICommand EditCommand => _editCommand ??= new RelayCommand(EditLog);

        private ICommand _deleteCommand = null!;

        /// <summary>
        /// The ICommand bound to the "Delete selected tour" button
        /// </summary>
        public ICommand DeleteCommand => _deleteCommand ??= new RelayCommand(Delete);

        public EditLogVm()
        {
            // Subscribe to the SwitchViewMessage
            Messenger.Default.Register<SwitchViewMessage>(this, HandleSwitchViewMessage);

            _tourPlannerBl = TourPlannerBlFactory.Instance;
            Tours = new ObservableCollection<Tour>();
        }

        /// <summary>
        /// Function called when "Add dummy tour" button is pressed
        /// </summary>
        /// <param name="commandParameter">Gets automatically assigned by ICommand, dunno what's in there tbh but who cares</param>
        private void EditLog(object commandParameter)
        {
            if (!string.IsNullOrEmpty(SelectedLog.Comment) && SelectedLog.Duration != TimeSpan.Zero &&
                SelectedLog.Rating != null && SelectedLog.Difficulty != null && SelectedLog.Time != null)
            {
                HandleException(() => _tourPlannerBl.Modify(SelectedTour));
                SuccessMessage = "Editing Log was successful!";
            }
            else
            {
                SuccessMessage = null;
                AlertMessage = "Please fill in all the required values.";
            }
        }

        /// <summary>
        /// Function called when "Delete selected log" button is pressed
        /// </summary>
        /// <param name="commandParameter">Gets automatically assigned by ICommand, dunno what's in there tbh but who cares</param>
        private void Delete(object commandParameter)
        {
      
            HandleException(() =>
            {
                SelectedTour.Logs.Remove(SelectedLog);
                _tourPlannerBl.Modify(SelectedTour);
                SuccessMessage = null;
                SuccessMessage = "Deleting Log was successful!";
            });



        }
        private void HandleSwitchViewMessage(SwitchViewMessage message)
        {
            if (message.ViewModelType == typeof(TourDetailsVm))
            {
                SelectedTour = message.SelectedTour;

                SelectedLog = message.SelectedLog;
            }
        }
    }
}


