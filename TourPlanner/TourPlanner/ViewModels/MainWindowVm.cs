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

        public void SwitchToMainView(object commandParameter)
        {
            // Close the current user control
            CurrentView = null;

            // Create the new MainView and MainVm instances
            var mainVm = new MainVm();
            var mainView = new MainView();
            mainView.DataContext = mainVm;

            // Set the new MainView as the current view
            CurrentView = mainView;
        }


        /// <summary>
        /// The ICommand bound to the "Search" button
        /// </summary>
        ///
        private ICommand _searchCommand = null!;
        public ICommand SearchCommand => _searchCommand ??= new RelayCommand(Search);

        private string _searchValue = null!;

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

        private void Search(object commandParameter)
        {
            CurrentView = null;

            var mainVm = new MainVm(SearchValue);
            mainVm.SearchValue = SearchValue;
            var mainView = new MainView();
            mainView.DataContext = mainVm;
            CurrentView = mainView;
        }

        public MainWindowVm()
        {
            MainViewModel = new MainVm();

            CurrentView = MainViewModel;

            Messenger.Default.Register<SwitchViewMessage>(this, HandleSwitchViewMessage);
       
        }

        private void HandleSwitchViewMessage(SwitchViewMessage message)
        {
            CurrentView = null;
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
            else if (message.ViewModelType == typeof(EditTourVm))
            {
                var selectedTour = message.SelectedTour;
                var editTourVm = new EditTourVm();
                editTourVm.SelectedTour = selectedTour;
                var editTourView = new EditTourView();
                editTourView.DataContext = editTourVm;
                CurrentView = editTourView;

            }
            else if (message.ViewModelType == typeof(AddLogVm))
            {
                var selectedTour = message.SelectedTour;
                var addLogVm = new AddLogVm();
                addLogVm.SelectedTour = selectedTour;
                var addLogView = new AddLogView();
                addLogView.DataContext = addLogVm;
                CurrentView = addLogView;
            }
            else if (message.ViewModelType == typeof(EditLogVm))
            {
                var selectedLog = message.SelectedLog;
                var selectedTour = message.SelectedTour;
                var editLogVm = new EditLogVm();
                editLogVm.SelectedLog = selectedLog;
                editLogVm.SelectedTour = selectedTour;
                var editLogView = new EditLogView();
                editLogView.DataContext = editLogVm;
                CurrentView = editLogView;


            }
        }

    }
}
