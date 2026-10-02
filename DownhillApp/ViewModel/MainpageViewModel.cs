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
        public Command C_SeeResults { get; private set; }
        public Command C_AllGroups { get; private set; }
        public Command C_AllRides { get; private set; }

        public MainpageViewModel()
        {
            this.C_NewMeasure = new Command(ToMeasurePage);
            this.C_SeeResults = new Command(ToSeeResults);
            this.C_AllGroups = new Command(ToAllGroups);

        }

        private async void ToMeasurePage()
        {
            await Shell.Current.GoToAsync("//Measure");
        }

        private async void ToSeeResults()
        {
            await Shell.Current.GoToAsync("//Result");
        }

        private async void ToAllGroups()
        {
            await Shell.Current.GoToAsync("//AllGroups");
        }
    }
}
