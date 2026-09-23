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
        public ObservableCollection<string> AllGroup { get; set; }
        private readonly HttpClient _httpClient;
        private static string IPAddress = "http://192.168.4.11";
        private string groupName;
        public Command C_BackToMenu { get; private set; }
        public Command C_StartMeasure { get; private set; }
        public Command C_AddNewGroup { get; private set; }
        public Command C_SaveRide { get; set; }


        public MeasureViewModel()
        {
            this.C_BackToMenu = new Command(BackToMenu);
            this.C_StartMeasure = new Command(() => StartMeasure());
            this.C_AddNewGroup = new Command(AddNewGroup);
            this.C_SaveRide = new Command(SaveRide);
            _httpClient = new();
            groupName = "";
            AllGroup = new ObservableCollection<string>();
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

        private async void BackToMenu()
        {
            await Shell.Current.GoToAsync("//MainPage");
        }

        private async Task StartMeasure()
        {
            await _httpClient.GetAsync(IPAddress + "/led");
        }

        public void AddNewGroup()
        {
            AllGroup.Add(this.GroupName);
            this.GroupName = "";
        }

        private async void SaveRide()
        {
            if()
            await Shell.Current.GoToAsync("//MainPage");
        }
    }
}
