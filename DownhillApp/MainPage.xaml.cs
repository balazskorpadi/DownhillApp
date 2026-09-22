using DownhillApp.ViewModel;

namespace DownhillApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            this.BindingContext = new MainpageViewModel();
        }
    }
}
