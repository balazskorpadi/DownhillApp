using DownhillApp.Service;
using DownhillApp.ViewModel;

namespace DownhillApp.View;

public partial class AllGroup : ContentPage
{
	public AllGroup()
	{
		InitializeComponent();
		var service = ServiceHandler.Service.GetService<IDataProvider>();
		this.BindingContext = new AllGroupViewModel(service);
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
		if(this.BindingContext is AllGroupViewModel vm)
		{
			vm.SetDatas();
		}
    }

}