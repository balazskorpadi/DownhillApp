using DownhillApp.ViewModel;

namespace DownhillApp.View;

public partial class Measure : ContentPage
{
	public Measure()
	{
		InitializeComponent();
		this.BindingContext = new MeasureViewModel();
	}
}