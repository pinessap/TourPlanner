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

        private readonly ITourFactory _tourFactory;

        public ObservableCollection<Tour> Tours { get; set; }


        private ICommand _importCommand = null!;
        public ICommand ImportCommand => _importCommand ??= new RelayCommand(Import);

        private ICommand _importOverrideCommand = null!;
        public ICommand ImportOverrideCommand => _importOverrideCommand ??= new RelayCommand(ImportOverride);

        private ICommand _exportCommand = null!;
        public ICommand ExportCommand => _exportCommand ??= new RelayCommand(Export);

        private string _pathValue = null!;
        public FilePopupWindowVm()
        {
            _tourFactory = TourFactory.Instance;
            Tours = new ObservableCollection<Tour>();

        }

        private void Import(object commandParameter)
        {
            HandleException(() => _tourFactory.ImportAppend());
        }

        private void ImportOverride(object commandParameter)
        {
            HandleException(() => _tourFactory.ImportOverride());
        }

        private void Export(object commandParameter)
        {
            HandleException(() => _tourFactory.Export(_tourFactory.GetTours()));
        }
    }
}
