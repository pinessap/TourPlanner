using GalaSoft.MvvmLight.Messaging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TourPlanner.BusinessLayer;
using TourPlanner.Messages;
using TourPlanner.Models;

namespace TourPlanner.ViewModels
{
    public class TourDetailsVm : ViewModelBase
    {
        public Tour SelectedTour { get; set; }

        

        public TourDetailsVm()
        {
            // Subscribe to the SwitchViewMessage
            Messenger.Default.Register<SwitchViewMessage>(this, HandleSwitchViewMessage);
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
