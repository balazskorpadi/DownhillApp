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
}