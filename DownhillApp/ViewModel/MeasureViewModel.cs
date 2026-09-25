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
        public ObservableCollection<string> AllGroup { get; set; }
        public ObservableCollection<string> AllData { get; set; }
        public Command C_BackToMenu { get; private set; }
        public Command C_StartMeasure { get; private set; }
        public Command C_StopMeasure { get; private set; }
        public Command C_AddNewGroup { get; private set; }
        public Command C_SaveRide { get; set; }

        private static string IPAddress = "http://192.168.4.11";
        private string currentData;
        private string currentGps;
        private string groupName;
        private string selectedGroup;

        public MeasureViewModel()
        {
            this.AllGroup = new ObservableCollection<string>();
            this.AllData = new ObservableCollection<string>();
            this.C_BackToMenu = new Command(BackToMenu);
            this.C_StartMeasure = new Command(() => StartMeasure());
            this.C_StopMeasure = new Command(StopMeasure);
            this.C_AddNewGroup = new Command(AddNewGroup);
            this.C_SaveRide = new Command(SaveRide);
            this._httpClient = new();
            this.timer = Application.Current.Dispatcher.CreateTimer();
            this.currentData = "";
            this.currentData = "";
            this.groupName = "";
            this.selectedGroup = "";
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

        public string SelecteGroup
        {
            get { return this.selectedGroup; }
            set
            {
                this.selectedGroup = value;
                OnPropertyChange();
            }
        }

        private async void BackToMenu()
        {
            await Shell.Current.GoToAsync("//MainPage");
        }

        private async Task GetData()
        {
            currentData = await _httpClient.GetStringAsync(IPAddress + "/sendData");
            currentGps = await _httpClient.GetStringAsync(IPAddress + "/sendGps");

            AllData.Add(currentData);
            AllData.Add(currentGps);
        }

        public void AddNewGroup()
        {
            AllGroup.Add(this.GroupName);
            this.GroupName = "";
        }

        private async void SaveRide()
        {
            //ide még kell database
            await Shell.Current.GoToAsync("//MainPage");
        }

        private void StartMeasure()
        {
            timer.Interval = TimeSpan.FromMilliseconds(250);
            timer.Tick += async (x, y) =>
            {
                await GetData();
            };

            timer.Start();
        }

        private void StopMeasure()
        {
            timer.Stop();
        }
    }
}
