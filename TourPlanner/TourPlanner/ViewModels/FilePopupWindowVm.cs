using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TourPlanner.BusinessLayer;
using TourPlanner.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TourPlanner.ViewModels
{
    public class FilePopupWindowVm : ViewModelBase
    {

        /// <summary>
        /// Connection to our business logic
        /// </summary>
        private readonly ITourFactory _tourFactory;


        /// <summary>
        /// The ObservableCollection bound to the ListBox that lists the tour names
        /// </summary>
        public ObservableCollection<Tour> Tours { get; set; }

        private ICommand _importCommand = null!;
        public ICommand ImportCommand => _importCommand ??= new RelayCommand(Import);

        private ICommand _importOverrideCommand = null!;
        public ICommand ImportOverrideCommand => _importOverrideCommand ??= new RelayCommand(ImportOverride);

        private ICommand _exportCommand = null!;
        public ICommand ExportCommand => _exportCommand ??= new RelayCommand(Export);

        private string _pathValue = null!;

        /// <summary>
        /// The two-way bound search string
        /// </summary>
        public string PathValue
        {
            get => _pathValue;
            set
            {
                if (_pathValue != value)
                {
                    _pathValue = value;
                    RaisePropertyChangedEvent();
                }
            }
        }
        public FilePopupWindowVm()
        {
            _tourFactory = TourFactory.Instance;
            Tours = new ObservableCollection<Tour>();

        }

        private void Import(object commandParameter)
        {
            try
            {
                _tourFactory.ImportAppend(PathValue);
            }
            catch (Exception ex)
            {
                // TODO: Deal with different exceptions, probably display them in the UI somehow
            }
        }

        private void ImportOverride(object commandParameter)
        {
            try
            {
                _tourFactory.ImportOverride(PathValue);
            }
            catch (Exception ex)
            {
                // TODO: Deal with different exceptions, probably display them in the UI somehow
            }
        }

        private void Export(object commandParameter)
        {
            Trace.WriteLine("exporting ");
            try
            {
                _tourFactory.Export(_tourFactory.GetTours());
            }
            catch (Exception ex)
            {
                // TODO: Deal with different exceptions, probably display them in the UI somehow
            }
        }
    }
}
