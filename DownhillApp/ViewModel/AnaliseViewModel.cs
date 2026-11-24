using DownhillApp.Model;
using DownhillApp.Service;
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
        private IGeminiAiService geminiService;
        public ObservableCollection<ChartPoint> DataSelectedAcc { get; set; }
        public ObservableCollection<ChartPoint> DataSelectedRot { get; set; }
        public ObservableCollection<ChartPoint> DataBestAcc { get; set; }
        public ObservableCollection<HeightProfilePoint> DataForHeight { get; set; }
        public ObservableCollection<Point> StartPoint { get; set; }
        public ObservableCollection<Point> EndPoint { get; set; }
        public Command C_BackToMenu { get; private set; }
        public Command C_Gemini { get; private set; }
        public Command C_Statistic { get; set; }
        private RideData bestRide;
        private RideData selectedRide;
        private double minHeight;
        private List<double> distance;
        private List<double> allDistance;
        private double maxDistance;
        private string geminiResponse;
        public double[,] corrAccData;
        public double[,] corrRotData;
        public AnaliseViewModel(IGeminiAiService service)
        {
            this.geminiService = service;
            
            this.DataSelectedAcc = new ObservableCollection<ChartPoint>();
            this.DataSelectedRot = new ObservableCollection<ChartPoint>();
            this.DataBestAcc = new ObservableCollection<ChartPoint>();
            this.DataForHeight = new ObservableCollection<HeightProfilePoint>();
            this.StartPoint = new ObservableCollection<Point>();
            this.EndPoint = new ObservableCollection<Point>();

            this.C_BackToMenu = new Command(BackToMenu);
            this.C_Gemini = new Command(() => GetResponse());
            this.bestRide = new RideData();
            this.selectedRide = new RideData();
            this.minHeight = 0.0;
            this.distance = new List<double>();
            this.allDistance = new List<double>();
            this.maxDistance = 0.0;
            this.geminiResponse = "";
            this.corrAccData = new double[6,6];
            this.corrRotData = new double[6,6];
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

        public string GeminiResponse
        {
            get { return this.geminiResponse; }
            set
            {
                this.geminiResponse = value;
                OnPropertyChange();
            }
        }

        private async void BackToMenu()
        {
            await Shell.Current.GoToAsync("//MainPage");
        }

        private async void GetResponse()
        {
            this.GeminiResponse = "Waiting for result...";
            string promt = $"There are two measures downhill rides in the same track. I give you the acceleration, rotation, time data and the coordinates." +
                $"This is the list with the coordinates of the track: {this.SelectedRide.Position} and the height for each coordinate {this.SelectedRide.Height}" +
                $"First is the Selected Ride:" +
                $"Acceleration: {this.SelectedRide.AccelerationJson}" +
                $"Rotation: {this.SelectedRide.Rotation}" +
                $"Time: {this.SelectedRide.Time}" +
                $"The second one is the best measured ride in the same track:" +
                $"Acceleration: {this.BestRide.AccelerationJson}" +
                $"Rotation: {this.BestRide.Rotation}" +
                $"Time: {this.BestRide.Time}" +
                $"Simulate the track with the given coordinates and analise the Selected ride and give 2 or 3 bulletpointed respose about the performance of the rider." +
                $"After that analise the best ride as well and compare it with the selected ride and give 2 or 3 response about the main differences in the two measures." +
                $"The goal is to give the rider a releable response about his performance and show him how to get better." +
                $"The form of the response should look like this:" +
                $"Selected Ride analised:" +
                $"-" +
                $"-" +
                $"-" +
                $"Difference between the best ride:" +
                $"-" +
                $"-" +
                $"-";
            this.GeminiResponse = await this.geminiService.GetResponse(promt);
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
                if (Distance[i] <= 0.5)
                {
                    Distance[i] = 0.0;
                }
                AllDistance.Add(AllDistance[i - 1] + Distance[i]);
                DataForHeight.Add(new HeightProfilePoint(AllDistance[i], Math.Round(SelectedRide.Height[i], 2),0.0));
                DataForHeight[i].Grade = ((DataForHeight[i].Height - DataForHeight[i - 1].Height) / Distance[i]) * 100.0;
            }
            MaxDistance = AllDistance.Last();
            
            DataSelectedAcc.Clear();
            DataSelectedRot.Clear();
            DataBestAcc.Clear();
            
            StartPoint.Clear();
            EndPoint.Clear();
            for (int i = 0; i < SelectedRide.Acceleration.Count; i++)
            {
                double d = AllDistance[i];
                DataSelectedAcc.Add(new ChartPoint(d, SelectedRide.Acceleration[i][0], SelectedRide.Acceleration[i][1], SelectedRide.Acceleration[i][2]));
                DataSelectedRot.Add(new ChartPoint(d, SelectedRide.Rotation[i][0], SelectedRide.Rotation[i][1], SelectedRide.Rotation[i][2]));
            }
            for(int i= 0; i<bestRide.Acceleration.Count; i++)
            {
                double d = AllDistance[i];
                DataBestAcc.Add(new ChartPoint(d, BestRide.Acceleration[i][0], BestRide.Acceleration[i][1], BestRide.Acceleration[i][2]));
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

        public void SetCorreletionData()
        {
            //Aceleration
            double SelectedAccXAvg = SelectedRide.Acceleration.Average(x => x[0]);
            double SelectedAccYAvg = SelectedRide.Acceleration.Average(x => x[1]);
            double SelectedAccZAvg = SelectedRide.Acceleration.Average(x => x[2]);
            double BestAccXAvg = BestRide.Acceleration.Average(x => x[0]);
            double BestAccYAvg = BestRide.Acceleration.Average(x => x[1]);
            double BestAccZAvg = BestRide.Acceleration.Average(x => x[2]);

            double[] tempAvg = [SelectedAccXAvg, SelectedAccYAvg, SelectedAccZAvg, BestAccXAvg, BestAccYAvg, BestAccZAvg];
            double[][] tempData =
                [
                SelectedRide.Acceleration.Select(x => (double)x[0]).ToArray(),
                SelectedRide.Acceleration.Select(x => (double)x[1]).ToArray(),
                SelectedRide.Acceleration.Select(x => (double)x[2]).ToArray(),
                BestRide.Acceleration.Select(x => (double)x[0]).ToArray(),
                BestRide.Acceleration.Select(x => (double)x[1]).ToArray(),
                BestRide.Acceleration.Select(x => (double)x[2]).ToArray()
                ];

            
            for(int i = 0; i<6; i++)
            {
                for(int e = 0; e<6; e++)
                {
                    int n = Math.Min(tempData[i].Length , tempData[e].Length);
                    double numerator = 0;
                    double denominatorX = 0;
                    double denominatorY = 0;

                    for(int j = 0; j<n; j++)
                    {
                        double dx = tempData[i][j] - tempAvg[i];
                        double dy = tempData[e][j] - tempAvg[e];

                        numerator += dx * dy;
                        denominatorX += dx * dx;
                        denominatorY += dy * dy;
                    }

                    double denominator = Math.Sqrt(denominatorX * denominatorY);
                    corrAccData[i, e] = numerator/ denominator;
                }
            }

            //Rotation
            double SelectedRotXAvg = SelectedRide.Rotation.Average(x => x[0]);
            double SelectedRotYAvg = SelectedRide.Rotation.Average(x => x[1]);
            double SelectedRotZAvg = SelectedRide.Rotation.Average(x => x[2]);
            double BestRotXAvg = BestRide.Rotation.Average(x => x[0]);
            double BestRotYAvg = BestRide.Rotation.Average(x => x[1]);
            double BestRotZAvg = BestRide.Rotation.Average(x => x[2]);

            tempAvg = [SelectedRotXAvg, SelectedRotYAvg, SelectedRotZAvg, BestRotXAvg, BestRotYAvg, BestRotZAvg];
            tempData =
                [
                SelectedRide.Rotation.Select(x => (double)x[0]).ToArray(),
                SelectedRide.Rotation.Select(x => (double)x[1]).ToArray(),
                SelectedRide.Rotation.Select(x => (double)x[2]).ToArray(),
                BestRide.Rotation.Select(x => (double)x[0]).ToArray(),
                BestRide.Rotation.Select(x => (double)x[1]).ToArray(),
                BestRide.Rotation.Select(x => (double)x[2]).ToArray()
                ];


            for (int i = 0; i < 6; i++)
            {
                for (int e = 0; e < 6; e++)
                {
                    int n = Math.Min(tempData[i].Length, tempData[e].Length);
                    double numerator = 0;
                    double denominatorX = 0;
                    double denominatorY = 0;

                    for (int j = 0; j < n; j++)
                    {
                        double dx = tempData[i][j] - tempAvg[i];
                        double dy = tempData[e][j] - tempAvg[e];

                        numerator += dx * dy;
                        denominatorX += dx * dx;
                        denominatorY += dy * dy;
                    }

                    double denominator = Math.Sqrt(denominatorX * denominatorY);
                    corrRotData[i, e] = numerator / denominator;
                }
            }
        }

    }
}