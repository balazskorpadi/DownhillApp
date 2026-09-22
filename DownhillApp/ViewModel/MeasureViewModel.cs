using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace DownhillApp.ViewModel
{
    public class MeasureViewModel : BaseViewModel
    {
        private readonly HttpClient _httpClient;
        private static string IPAddress = "192.168.4.11";
        public Command C_BackToMenu { get; private set; }
        public Command C_StartMeasure { get; private set; }

        public MeasureViewModel()
        {
            this.C_BackToMenu = new Command(BackToMenu);
            this.C_StartMeasure = new Command(() => StartMeasure());
            _httpClient = new();
        }

        private async void BackToMenu()
        {
            await Shell.Current.GoToAsync("//MainPage");
        }

        private async Task StartMeasure()
        {
            await _httpClient.GetAsync("http://" + IPAddress + "/led");
;        }
    }
}
