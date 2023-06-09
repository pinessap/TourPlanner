﻿using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TourPlanner.BusinessLayer;
using TourPlanner.Configuration;
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
        private readonly ITourPlannerBl _tourPlannerBl;



        /// <summary>
        /// The Log currently selected
        /// </summary>
        ///
        private TourLog _currentLog = null!;
        public TourLog CurrentLog
        {
            get => _currentLog;
            set
            {
                if (_currentLog != value)
                {
                    _currentLog = value;
                    RaisePropertyChangedEvent();
                }
            }
        }


        private ICommand _deleteCommand = null!;

        /// <summary>
        /// The ICommand bound to the "Delete selected tour" button
        /// </summary>
        public ICommand DeleteCommand => _deleteCommand ??= new RelayCommand(Delete);

        /// <summary>
        /// The ObservableCollection bound to the ListBox that lists the tour names
        /// </summary>
        public ObservableCollection<Tour> Tours { get; set; }

        private ICommand _generateReportCommand = null!;

        public ICommand GenerateReportCommand => _generateReportCommand ??= new RelayCommand(GenerateReport);



        public TourDetailsVm()
        {
            // Subscribe to the SwitchViewMessage
            Messenger.Default.Register<SwitchViewMessage>(this, HandleSwitchViewMessage);

            _tourPlannerBl = TourPlannerBlFactory.Instance;
            Tours = new ObservableCollection<Tour>();

          
        }

        /// <summary>
        /// Function called when "Delete selected tour" button is pressed
        /// </summary>
        /// <param name="commandParameter">Gets automatically assigned by ICommand, dunno what's in there tbh but who cares</param>
        private void Delete(object commandParameter)
        {
            HandleException(() =>
            {
                _tourPlannerBl.Delete(SelectedTour);
                SuccessMessage = "Deleting Tour was successful!";
            });
        }


        private void GenerateReport(object commandParameter)
        {

            HandleException(() =>
            {
                _tourPlannerBl.GenerateSingleReport(SelectedTour);
                SuccessMessage = "Generating Report was successful!";
            });

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

        public void ExecuteToEditLog(object parameter)
        {
            // Retrieve the selected log and tour
            var selectedLog = CurrentLog;
            var selectedTour = SelectedTour;

            // Send a message to MainViewModel to switch to View
            Messenger.Default.Send(new SwitchViewMessage(typeof(EditLogVm), selectedTour, selectedLog));
        }

        private void HandleSwitchViewMessage(SwitchViewMessage message)
        {
            if (message.ViewModelType == typeof(TourDetailsVm))
            {
                SelectedTour = message.SelectedTour;
                Trace.WriteLine("PATH:");
                Trace.WriteLine(SelectedTour.PathToRouteImage);
                //SelectedTour.PathToRouteImage = "C:\\Users\\inesp\\OneDrive - FH Technikum Wien\\INFORMATIK\\Semester 4\\SWEN2\\project\\TourPlanner\\Pictures\\50_Route.png";
               
                Trace.WriteLine("PATH:");
                Trace.WriteLine(SelectedTour.PathToRouteImage);
                // Handle the selected tour in TourDetailsView
            }
        }
    }
}
