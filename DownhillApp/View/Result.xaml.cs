using DownhillApp.Service;
using DownhillApp.ViewModel;

namespace DownhillApp.View;

public partial class Result : ContentPage
{
	public Result()
	{
		InitializeComponent();
		var service = ServiceHandler.Service.GetService<IDataProvider>();
		this.BindingContext = new ResultViewModel(service);
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		if(this.BindingContext is ResultViewModel vm)
		{
			vm.SetAllRides();
		}
    }
}