namespace DownhillApp
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("MBEvETIGNwJ0XXVqVFhjW1pNf1ZLQGJLY1B3VWFadVt5flBPa15RRnBeQl9jSXZWcEFnUHhac39WQw==");
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}