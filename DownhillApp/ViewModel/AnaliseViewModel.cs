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
        public ObservableCollection<Point> DataBestX { get; set; }
        public ObservableCollection<Point> DataBestY { get; set; }
        public ObservableCollection<Point> DataBestZ { get; set; } 
        public ObservableCollection<Point> DataHeight { get; set; }
        public ObservableCollection<Point> StartPoint { get; set; }
        public ObservableCollection<Point> EndPoint { get; set; }
        public Command C_BackToMenu { get; private set; }
        private RideData bestRide;
        private RideData selectedRide;
        public AnaliseViewModel()
        {
            this.DataSelectedX = new ObservableCollection<Point>();
            this.DataSelectedY = new ObservableCollection<Point>();
            this.DataSelectedZ = new ObservableCollection<Point>();
            this.DataBestX = new ObservableCollection<Point>();
            this.DataBestY = new ObservableCollection<Point>();
            this.DataBestZ = new ObservableCollection<Point>();
            this.DataHeight = new ObservableCollection<Point>();
            this.StartPoint = new ObservableCollection<Point>();
            this.EndPoint = new ObservableCollection<Point>();

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
            DataBestX.Clear();
            DataBestY.Clear();
            DataBestZ.Clear();
            DataHeight.Clear();
            StartPoint.Clear();
            EndPoint.Clear();
            for (int i = 0; i < SelectedRide.Acceleration.Count; i++)
            {
                double time = (double)i * ((double)(SelectedRide.Time) / (double)(SelectedRide.Acceleration.Count));
                DataSelectedX.Add(new Point(time, SelectedRide.Acceleration[i][0]));
                DataSelectedY.Add(new Point(time, SelectedRide.Acceleration[i][1]));
                DataSelectedZ.Add(new Point(time, SelectedRide.Acceleration[i][2]));
                DataBestX.Add(new Point(time, BestRide.Acceleration[i][0]));
                DataBestY.Add(new Point(time, BestRide.Acceleration[i][1]));
                DataBestZ.Add(new Point(time, BestRide.Acceleration[i][2]));
                DataHeight.Add(new Point(time, Math.Round(SelectedRide.Height[i],2)));
            }
            StartPoint.Add(DataHeight.First());
            EndPoint.Add(DataHeight.Last());
        }
    }
}
