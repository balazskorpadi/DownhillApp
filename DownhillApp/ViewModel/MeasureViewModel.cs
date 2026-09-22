using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownhillApp.ViewModel
{
    public class MeasureViewModel : BaseViewModel
    {
        public Command C_BackToMenu { get; private set; }

        public MeasureViewModel()
        {
            this.C_BackToMenu = new Command(BackToMenu);
        }

        private async void BackToMenu()
        {
            await Shell.Current.GoToAsync("//MainPage");
        }
    }
}
