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
    public class ResultViewModel : BaseViewModel
    {
        private IDataProvider dataProvider;
        public ObservableCollection<RideData> AllRides { get; set; }
        public Command C_BackToMenu { get; private set; }
        public ResultViewModel(IDataProvider service)
        {
            this.dataProvider = service;
            this.AllRides = new ObservableCollection<RideData>();
            AllRides = service.GetAllRides();

            this.C_BackToMenu = new Command(BackToMenu); 
        }
        private async void BackToMenu()
        {
            await Shell.Current.GoToAsync("//MainPage");
        }

        public void SetAllRides()
        {
            this.AllRides = dataProvider.GetAllRides();
        }
    }
}
