using DownhillApp.Model;
using DownhillApp.Service;
using Microsoft.Maui.Animations;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownhillApp.ViewModel
{
    public class ResultViewModel : BaseViewModel
    {
        private IDataProvider dataProvider;
        public ObservableCollection<RideData> AllRides { get; set; }
        public Command C_BackToMenu { get; private set; }
        public Command C_SelectRide { get; private set; }
        public Command C_Delete { get; private set; }

        private RideData selectedRide;
        public ResultViewModel(IDataProvider service)
        {
            this.dataProvider = service;
            this.AllRides = new ObservableCollection<RideData>();
            AllRides = service.GetAllRides();

            this.C_BackToMenu = new Command(BackToMenu);
            this.C_SelectRide = new Command(AnaliseRide);
            this.C_Delete = new Command(Delete);

            this.selectedRide = new RideData();
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

        private async void AnaliseRide()
        {
            if(this.SelectedRide.Name is null)
            {
                await Shell.Current.DisplayAlert("ERROR", "No ride selected", "OK");
            }
            else
            {
                RideData best = new RideData();
                best.Time = int.MaxValue;
                best.GroupName = this.SelectedRide.GroupName;
                foreach(RideData rd in AllRides)
                {
                    if(best.Time > rd.Time && best.GroupName == rd.GroupName)
                    {
                        best = rd;
                    }
                }
                var parameter = new ShellNavigationQueryParameters() { {"choosenRide", this.SelectedRide},{"bestRide", best} };
                await Shell.Current.GoToAsync("//Analise", parameter);
            }
        }

        private void Delete()
        {
            this.dataProvider.Delete(SelectedRide);
            this.SelectedRide = new RideData();
        }

        public void SetAllRides()
        {
            this.AllRides = dataProvider.GetAllRides();
        }
    }
}
