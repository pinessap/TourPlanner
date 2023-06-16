using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TourPlanner.BusinessLayer;
using TourPlanner.Models;

namespace TourPlanner.ViewModels
{
    class AddLogVm : ViewModelBase
    {
        public Tour SelectedTour { get; set; }

        /// <summary>
        /// Connection to our business logic
        /// </summary>
        private readonly ITourFactory _tourFactory;


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
            _tourFactory = TourFactory.Instance;
            Tours = new ObservableCollection<Tour>();
        }

        /// <summary>
        /// Function called when "Add dummy tour" button is pressed
        /// </summary>
        /// <param name="commandParameter">Gets automatically assigned by ICommand, dunno what's in there tbh but who cares</param>
        private void AddLog(object commandParameter)
        {
            TimeSpan duration = ConvertStringToTimeSpan(LogTime);

            var logToAdd = new TourLog
            {
                Comment = LogComment,
                Rating = LogRating,
                Difficulty = LogDifficulty,
                Duration = duration,
                Time = LogDateTime,
            };

            try
            {
                SelectedTour.Logs.Add(logToAdd);
                _tourFactory.Modify(SelectedTour);
            }
            catch (Exception ex)
            {
                // TODO: Deal with different exceptions, probably display them in the UI somehow
            }

            //var addedItems = _tourFactory.GetTours();

            //FillListBox(addedItems);
        }

        private TimeSpan ConvertStringToTimeSpan(string timeString)
        {
            string[] integerStrings = timeString.Split(',');

            if (integerStrings.Length == 3)
            {
                if (int.TryParse(integerStrings[0], out int firstInteger) &&
                    int.TryParse(integerStrings[1], out int secondInteger) &&
                    int.TryParse(integerStrings[2], out int thirdInteger))
                {
                    return new TimeSpan(firstInteger, secondInteger, thirdInteger);
                }
                else
                {
                    // Parsing failed for one or more integers
                    // Handle the error accordingly
                    return TimeSpan.Zero;
                }
            }
            else
            {
                // The input doesn't contain the expected number of integers
                // Handle the error accordingly
                return TimeSpan.Zero;
            }
        }

    }
}
