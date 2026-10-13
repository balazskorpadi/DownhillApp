using DownhillApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownhillApp.ViewModel
{
    public class AnaliseViewModel : BaseViewModel
    {
        public ObservableCollection<Point> DataSelectedX {  get; set; }
        public ObservableCollection<Point> DataSelectedY { get; set; }
        public ObservableCollection<Point> DataSelectedZ { get; set; }
        public Command C_BackToMenu { get; private set; }
        private RideData bestRide;
        private RideData selectedRide;
        public AnaliseViewModel()
        {
            DataSelectedX = new ObservableCollection<Point>();
            DataSelectedY = new ObservableCollection<Point>();
            DataSelectedZ = new ObservableCollection<Point>();

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

        public void SetData()
        {
            DataSelectedX.Clear();
            DataSelectedY.Clear();
            DataSelectedZ.Clear();
            for (int i = 0; i < SelectedRide.Acceleration.Count; i++)
            {
                double time = (double)i * ((double)(SelectedRide.Time) / (double)(SelectedRide.Acceleration.Count));
                DataSelectedX.Add(new Point(time, SelectedRide.Acceleration[i][0]));
                DataSelectedY.Add(new Point(time, SelectedRide.Acceleration[i][1]));
                DataSelectedZ.Add(new Point(time, SelectedRide.Acceleration[i][2]));
            }
        }
    }
}
