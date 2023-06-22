using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using TourPlanner.BusinessLayer;
using TourPlanner.Components;
using TourPlanner.Models;


namespace TourPlanner.ViewModels
{
    class AddLogVm : ViewModelBase
    {
        public Tour SelectedTour { get; set; }

        /// <summary>
        /// Connection to our business logic
        /// </summary>
        private readonly ITourPlannerBl _tourPlannerBl;


        private ICommand _addLogCommand = null!;

        /// <summary>
        /// The ObservableCollection bound to the ListBox that lists the tour names
        /// </summary>
        public ObservableCollection<Tour> Tours { get; set; }

        /// <summary>
        /// The ICommand bound to the "Add dummy tour" button
        /// </summary>
        public ICommand AddLogCommand => _addLogCommand ??= new RelayCommand(AddLog);

        private DateTime _logDateTime;
        public DateTime LogDateTime
        {
            get { return _logDateTime; }
            set
            {
                _logDateTime = value;
                RaisePropertyChangedEvent();
            }
        }

        private string _logTime;
        public string LogTime 
        {
            get { return _logTime; }
            set
            {
                _logTime = value;
                RaisePropertyChangedEvent();
            }
        }

        private string _logComment;
        public string LogComment
        {
            get { return _logComment; }
            set
            {
                _logComment = value;
                RaisePropertyChangedEvent();
            }
        }

        private int _logDifficulty;
        public int LogDifficulty
        {
            get { return _logDifficulty; }
            set
            {
                _logDifficulty = value;
                RaisePropertyChangedEvent();
            }
        }

        private float _logRating;
        public float LogRating
        {
            get { return _logRating; }
            set
            {
                _logRating = value;
                RaisePropertyChangedEvent();
            }
        }

        public AddLogVm()
        {
            LogDateTime = DateTime.UtcNow;
            _tourPlannerBl = TourPlannerBlFactory.Instance;
            Tours = new ObservableCollection<Tour>();
            
        }

        private string _message = null!;

        public new string Message
        {
            get => _message;
            set
            {
                _message = value;
                RaisePropertyChangedEvent(nameof(Message));
            }
        }

    

        /// <summary>
        /// Function called when "Add dummy tour" button is pressed
        /// </summary>
        /// <param name="commandParameter">Gets automatically assigned by ICommand, dunno what's in there tbh but who cares</param>
        private void AddLog(object commandParameter)
        {
            Message = null;

            if (!string.IsNullOrEmpty(LogComment) && !string.IsNullOrEmpty(LogTime) && LogRating != null && LogDifficulty != null && LogTime != null)
            {

                HandleException(() =>
                {
                    var logToAdd = new TourLog
                    {
                        Comment = LogComment,
                        Rating = LogRating,
                        Difficulty = LogDifficulty,
                        Duration = TimeSpan.Parse(LogTime),
                        Time = LogDateTime,
                    };

                    SelectedTour.Logs.Add(logToAdd);
                    _tourPlannerBl.Modify(SelectedTour);
                });
                SuccessMessage = "Adding Log was successful!";
            }
            else
            {
                Trace.WriteLine("ALERT");
                SuccessMessage = null;
                AlertMessage = "Please fill in all the required values.";
                Trace.WriteLine(AlertMessage);
            }



        }

    }
}
