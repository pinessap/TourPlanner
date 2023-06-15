using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.Models;

namespace TourPlanner.Messages
{
    public class SwitchViewMessage
    {
        public Type ViewModelType { get; }
        public Tour SelectedTour { get; } = null!;

        public SwitchViewMessage(Type viewModelType)
        {
            ViewModelType = viewModelType;
        }

        public SwitchViewMessage(Type viewModelType, Tour selectedTour)
        {
            ViewModelType = viewModelType;
            SelectedTour = selectedTour;
        }


    }
}
