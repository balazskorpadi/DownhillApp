using DownhillApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownhillApp.ViewModel
{
    public class AnaliseViewModel : BaseViewModel
    {
        public Command C_BackToMenu { get; private set; }
        private RideData bestRide;
        private RideData selectedRide;
        private Dictionary<float, float> data;
        public AnaliseViewModel()
        {
            this.C_BackToMenu = new Command(BackToMenu);
            
            this.bestRide = new RideData();
            this.selectedRide = new RideData();

            this.Data = new Dictionary<float, float>();
            ;
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

        public Dictionary<float, float> Data
        {
            get { return this.data; }
            set
            {
                this.data = value;
                OnPropertyChange();
            }
        }

        private async void BackToMenu()
        {
            await Shell.Current.GoToAsync("//MainPage");
        }

        public void SetData()
        {
            Dictionary<float, float> Temp = new Dictionary<float, float>();
            for (int i = 0; i < SelectedRide.Acceleration.Count; i++)
            {
                Temp.Add(i, SelectedRide.Acceleration[i][2]);
            }
            this.Data = Temp;
        }
    }
}
