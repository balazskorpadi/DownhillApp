using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DownhillApp.ViewModel
{
    public class MainpageViewModel : BaseViewModel
    {
        public Command C_NewMeasure { get; private set; }

        public MainpageViewModel()
        {
            this.C_NewMeasure = new Command(ToMeasurePage);
        }

        private async void ToMeasurePage()
        {
            await Shell.Current.GoToAsync("//Measure");
        }
    }
}
