using DownhillApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownhillApp.ViewModel
{
    public class AnaliseViewModel: BaseViewModel
    {
        public Command C_BackToMenu { get; private set; }
        private RideData bestRide;
        private RideData selectedRide;
        public AnaliseViewModel()
        {
            this.C_BackToMenu = new Command(BackToMenu);
            
            this.bestRide = new RideData();
            this.selectedRide = new RideData();
        }

        public RideData BestRide
        {
            get { return this.bestRide; }
            set
            {
                this.bestRide = value;
                OnPropertyChange();
            }
        }

        public RideData SelectedRide
        {
            get { return this.selectedRide; }
            set
            {
                this.selectedRide = value;
                OnPropertyChange();
            }
        }

        private async void BackToMenu()
        {
            await Shell.Current.GoToAsync("//MainPage");
        }
    }
}
