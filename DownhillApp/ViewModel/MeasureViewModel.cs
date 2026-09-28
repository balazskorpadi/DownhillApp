using DownhillApp.Model;
using DownhillApp.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DownhillApp.ViewModel
{
    public class MeasureViewModel : BaseViewModel
    {
        private readonly HttpClient _httpClient;
        private IDispatcherTimer timer;
        private IDataProvider dataProvider;
        public ObservableCollection<string> AllGroup { get; set; }
        public ObservableCollection<string> AllMeasure { get; set; }
        public Command C_BackToMenu { get; private set; }
        public Command C_StartMeasure { get; private set; }
        public Command C_StopMeasure { get; private set; }
        public Command C_AddNewGroup { get; private set; }
        public Command C_SaveRide { get; set; }

        private static string IPAddress = "http://192.168.4.11";
        private string currentData;
        private string currentGps;
        private List<string> allData;
        private List<string> allGps;
        private string groupName;
        private string selectedGroup;
        private string name;
        //private TimeSpan rideTime;

        public MeasureViewModel(IDataProvider service)
        {
            this.dataProvider = service;

            this.AllGroup = dataProvider.GetAllGroupName();
            this.AllMeasure = new ObservableCollection<string>();          
            this.C_BackToMenu = new Command(BackToMenu);
            this.C_StartMeasure = new Command(() => StartMeasure());
            this.C_StopMeasure = new Command(StopMeasure);
            this.C_AddNewGroup = new Command(() => AddNewGroup());
            this.C_SaveRide = new Command(() => SaveRide());
            this._httpClient = new();
            this.timer = Application.Current.Dispatcher.CreateTimer();
            this.currentData = "";
            this.currentData = "";
            this.allData = new List<string>();
            this.allGps = new List<string>();
            this.groupName = "";
            this.selectedGroup = "";
            this.name = "";
            //this.rideTime = TimeSpan.Zero;
        }

        public string GroupName
        {
            get { return this.groupName; }
            set
            {
                this.groupName = value;
                OnPropertyChange();
            }
        }

        public string SelectedGroup
        {
            get { return this.selectedGroup; }
            set
            {
                this.selectedGroup = value;
                OnPropertyChange();
            }
        }

        public string Name
        {
            get { return this.name; }
            set
            {
                this.name = value;
                OnPropertyChange();
            }
        }

        private async void BackToMenu()
        {
            //await _httpClient.GetAsync(IPAddress + "/stop");
            SetDefault();
            await Shell.Current.GoToAsync("//MainPage");
        }

        private async Task GetData()
        {
            currentData = await _httpClient.GetStringAsync(IPAddress + "/sendData");
            currentGps = await _httpClient.GetStringAsync(IPAddress + "/sendGps");

            AllMeasure.Add(currentData);
            AllMeasure.Add(currentGps);
            allData.Add(currentData);
            allGps.Add(currentGps);
        }

        private async void AddNewGroup()
        {
            if(this.GroupName == "" || this.GroupName == null)
            {
                await Shell.Current.DisplayAlert("ERROR","Empty field!","OK");
            }
            else
            {
                AllGroup.Add(this.GroupName);
                dataProvider.Create(new Group(this.groupName));
            }
            this.GroupName = "";
        }

        private async void SaveRide()
        {
            if(this.SelectedGroup == null || this.Name == "" || AllMeasure.Count == 0)
            {
                await Shell.Current.DisplayAlert("ERROR", "Empty field!", "OK");
            }
            else
            {
                //adatmentés kell még ide
                RideData temp = SaveData();
                SetDefault();
                await Shell.Current.GoToAsync("//MainPage");
            }
        }

        private async void StartMeasure()
        {
            await _httpClient.GetAsync(IPAddress + "/start");
            
            timer.Interval = TimeSpan.FromMilliseconds(250);
            timer.Tick += async (x, y) =>
            {
                await GetData();
            };

            timer.Start();
        }

        private async void StopMeasure()
        {
            await _httpClient.GetAsync(IPAddress + "/stop");
            timer.Stop();
        }

        private void SetDefault()
        {           
            AllMeasure = new ObservableCollection<string>();
            allData = new List<string>();
            allGps = new List<string>();
            Name = "";
            SelectedGroup = "";
        }

        private RideData SaveData()
        {
            RideData result = new RideData();
            result.Name = this.Name;
            result.GrpupName = this.GroupName;
            result.Time = 0; //ki kell még javítani
            
            //data: acc:x;acc:y;acc:z;gir:x;gir:y;gir:z
            foreach(string s in allData)
            {
                string[] temp = s.Split(';');
                float[] tempAcc = { float.Parse(temp[0]), float.Parse(temp[1]), float.Parse(temp[2]) };
                float[] tempGir = { float.Parse(temp[3]), float.Parse(temp[4]), float.Parse(temp[5]) };
                result.Acceleration.Add(tempAcc);
                result.Rotation.Add(tempGir);
            }

            /*gps:
            latitude: [2]
            latutude direction: [3]
            longtitude: [4]
            longtitude direction: [5]
            waterhight: [9]*/
            foreach(string s in allGps)
            {
                string[] temp = s.Split(',');
                float[] coordinates = { ConvertCordinate(float.Parse(temp[2]), temp[3]), ConvertCordinate(float.Parse(temp[4]), temp[5]) };
                result.Position.Add(coordinates);
                result.Height.Add(float.Parse(temp[9]));

            }

            return result;
        }

        private float ConvertCordinate(float rawValue, string direction)
        {
            float degree = rawValue / 100;
            float minutes = rawValue % 100;
            float result = degree + (minutes / 60);
            if(direction == "S" || direction == "W")
            {
                result = -result;
            }
            return result;
        }

        public void SetGroupNames()
        {
            this.AllGroup = dataProvider.GetAllGroupName();
        }
    }
}
