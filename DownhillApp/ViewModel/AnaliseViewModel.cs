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
        //public ObservableCollection<Point> DataSelectedX {  get; set; }
        //public ObservableCollection<Point> DataSelectedY { get; set; }
        //public ObservableCollection<Point> DataSelectedZ { get; set; }

        public ObservableCollection<ChartPoint> DataSelected { get; set; }
        public ObservableCollection<Point> DataBestX { get; set; }
        public ObservableCollection<Point> DataBestY { get; set; }
        public ObservableCollection<Point> DataBestZ { get; set; } 
        public ObservableCollection<HeightProfilePoint> DataForHeight { get; set; }
        public ObservableCollection<Point> StartPoint { get; set; }
        public ObservableCollection<Point> EndPoint { get; set; }
        public Command C_BackToMenu { get; private set; }
        private RideData bestRide;
        private RideData selectedRide;
        private double minHeight;
        private List<double> distance;
        private List<double> allDistance;
        private double maxDistance;
        public AnaliseViewModel()
        {
            //this.DataSelectedX = new ObservableCollection<Point>();
            //this.DataSelectedY = new ObservableCollection<Point>();
            //this.DataSelectedZ = new ObservableCollection<Point>();
            this.DataSelected = new ObservableCollection<ChartPoint>();
            this.DataBestX = new ObservableCollection<Point>();
            this.DataBestY = new ObservableCollection<Point>();
            this.DataBestZ = new ObservableCollection<Point>();
            this.DataForHeight = new ObservableCollection<HeightProfilePoint>();
            this.StartPoint = new ObservableCollection<Point>();
            this.EndPoint = new ObservableCollection<Point>();

            this.C_BackToMenu = new Command(BackToMenu);
            this.bestRide = new RideData();
            this.selectedRide = new RideData();
            this.minHeight = 0.0;
            this.distance = new List<double>();
            this.allDistance = new List<double>();
            this.maxDistance = 0.0;

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

        public double MinHeight
        {
            get { return this.minHeight; }
            set
            {
                this.minHeight = value;
                OnPropertyChange();
            }
        }

        public List<double> Distance
        {
            get { return this.distance; }
            set
            {
                this.distance = value;
                OnPropertyChange();
            }
        }

        public List<double> AllDistance
        {
            get { return this.allDistance; }
            set
            {
                this.allDistance = value;
                OnPropertyChange();
            }
        }

        public double MaxDistance
        {
            get { return this.maxDistance; }
            set
            {
                this.maxDistance = value;
                OnPropertyChange();
            }
        }

        private async void BackToMenu()
        {
            await Shell.Current.GoToAsync("//MainPage");
        }

        public void SetData()
        {
            DataForHeight.Clear();
            DataForHeight.Add(new HeightProfilePoint(0.0, Math.Round(SelectedRide.Height[0], 2), 0.0));
            Distance.Clear();
            Distance.Add(0);
            AllDistance.Clear();
            AllDistance.Add(0);
            for (int i = 1; i < SelectedRide.Position.Count; i++)
            {
                double time = (double)i * ((double)(SelectedRide.Time) / (double)(SelectedRide.Acceleration.Count));
                Distance.Add(CalculateDistance(SelectedRide.Position[i - 1][0], SelectedRide.Position[i - 1][1], SelectedRide.Position[i][0], SelectedRide.Position[i][1]));
                AllDistance.Add(AllDistance[i - 1] + Distance[i]);
                DataForHeight.Add(new HeightProfilePoint(AllDistance[i], Math.Round(SelectedRide.Height[i], 2),0.0));
                DataForHeight[i].Grade = ((DataForHeight[i].Height - DataForHeight[i - 1].Height) / Distance[i]) * 100.0;
            }
            MaxDistance = AllDistance.Last();
            
            //DataSelectedX.Clear();
            //DataSelectedY.Clear();
            //DataSelectedZ.Clear();
            DataSelected.Clear();
            DataBestX.Clear();
            DataBestY.Clear();
            DataBestZ.Clear();
            
            StartPoint.Clear();
            EndPoint.Clear();
            for (int i = 0; i < SelectedRide.Acceleration.Count; i++)
            {
                double d = AllDistance[i];
                //DataSelectedX.Add(new Point(d, SelectedRide.Acceleration[i][0]));
                //DataSelectedY.Add(new Point(d, SelectedRide.Acceleration[i][1]));
                //DataSelectedZ.Add(new Point(d, SelectedRide.Acceleration[i][2]));
                DataSelected.Add(new ChartPoint(d, SelectedRide.Acceleration[i][0], SelectedRide.Acceleration[i][1], SelectedRide.Acceleration[i][2]));
                DataBestX.Add(new Point(d, BestRide.Acceleration[i][0]));
                DataBestY.Add(new Point(d, BestRide.Acceleration[i][1]));
                DataBestZ.Add(new Point(d, BestRide.Acceleration[i][2]));               
            }
            StartPoint.Add(new Point(DataForHeight.First().X, DataForHeight.First().Height));
            EndPoint.Add(new Point(DataForHeight.Last().X, DataForHeight.Last().Height));
            this.MinHeight = Math.Round(DataForHeight.Min(x => x.Height))-15;
        }

        private double CalculateDistance(double lat1, double long1, double lat2, double long2)
        {
            double lat1Rad = (lat1 * Math.PI) / 180.0;
            double lat2Rad = (lat2 * Math.PI) / 180.0;
            double long1Rad = (long1 * Math.PI) / 180.0;
            double long2Rad = (long2 * Math.PI) / 180.0;
            double latDelta = lat2Rad - lat1Rad;
            double longDelta = long2Rad - long1Rad;
            double temp = Math.Pow(Math.Sin((latDelta / 2)), 2) + (Math.Cos(lat1Rad) * Math.Cos(lat2Rad) * Math.Pow(Math.Sin(longDelta/2),2));
            temp = Math.Clamp(temp, 0, 1.0);
            double result = 2.0 * 6371000.0 * Math.Atan2(Math.Sqrt(temp), Math.Sqrt(1 - temp));
            return result;
        }
    }
}
