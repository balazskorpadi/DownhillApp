using DownhillApp.Model;
using DownhillApp.ViewModel;

namespace DownhillApp.View;

[QueryProperty(nameof(Best), "bestRide")]
[QueryProperty(nameof(Selected), "choosenRide")]
public partial class Analise : ContentPage
{
	public Analise()
	{
		InitializeComponent();
		this.BindingContext = new AnaliseViewModel();
    }

	public RideData Best
	{
		set
		{
			if(this.BindingContext is AnaliseViewModel vm)
			{
				vm.BestRide = value;
			}
		}
	}

	public RideData Selected
	{
		set
		{
			if(this.BindingContext is AnaliseViewModel vm)
			{
				vm.SelectedRide = value;
				vm.SetData();
            }
		}
	}

	private async void GoToMap(object sender, WebNavigatedEventArgs e)
	{
		if(e.Result == WebNavigationResult.Success)
		{
			await DrawRoute();
		}
	}

	private async Task DrawRoute()
	{
		if(BindingContext is AnaliseViewModel vm)
		{
			string data = vm.SelectedRide.PositionJson;
			await SelectedRideMap.EvaluateJavaScriptAsync($"DrawRoute({data})");

        }
	}
}