using DownhillApp.Service;
using DownhillApp.ViewModel;

namespace DownhillApp.View;

public partial class Measure : ContentPage
{
	public Measure()
	{
		InitializeComponent();
		var service = ServiceHandler.Service.GetService<IDataProvider>();
		this.BindingContext = new MeasureViewModel(service);
	}

    protected override void OnAppearing()
    {
		base.OnAppearing();
		//saját kód0
    }
}