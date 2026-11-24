using DownhillApp.Model;
using DownhillApp.Service;
using DownhillApp.ViewModel;

namespace DownhillApp.View;

[QueryProperty(nameof(Best), "bestRide")]
[QueryProperty(nameof(Selected), "choosenRide")]
public partial class Analise : ContentPage
{
	IGeminiAiService service = ServiceHandler.Service.GetService<IGeminiAiService>();
	public Analise()
	{
		InitializeComponent();
		AnaliseViewModel vm = new AnaliseViewModel(service);
        vm.C_Statistic = new Command(() => { vm.SetCorreletionData(); GenerateMatrixAcc(vm.corrAccData); GenerateMatrixRot(vm.corrRotData);});

		this.BindingContext = vm;
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


	private void GenerateMatrixAcc(double[,] matrix)
	{		
		CorrAcc.RowDefinitions.Clear();
		CorrAcc.ColumnDefinitions.Clear();
		CorrAcc.Children.Clear();

		for(int i = 0; i<7; i++)
		{
			CorrAcc.RowDefinitions.Add(new RowDefinition(GridLength.Star));
			CorrAcc.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
		}

		string[] cellName = ["Selected X", "Selected Y", "Selected Z", "Best X", "Best Y", "Best Z"];

		for(int i= 0; i<6; i++)
		{
			var cell = new Label
			{
				Text = cellName[i],
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalTextAlignment = TextAlignment.Center,
				Padding = 8
			};

			CorrAcc.SetRow(cell, 0);
			CorrAcc.SetColumn(cell, i+1);
			CorrAcc.Children.Add(cell);

            var cell2 = new Label
            {
                Text = cellName[i],
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                Padding = 8
            };

            CorrAcc.SetRow(cell2, i+1);
			CorrAcc.SetColumn(cell2, 0);
			CorrAcc.Children.Add(cell2);
		}

        for (int i = 0; i < 6; i++)
		{
            for (int e = 0; e < 6; e++)
			{
				var cell = new Label
				{
					Text = matrix[i, e].ToString("F2"),
					HorizontalTextAlignment = TextAlignment.Center,
					VerticalTextAlignment = TextAlignment.Center,
					Background = SetBackGroundColor(matrix[i, e]),
					Padding = 8,
					TextColor = Colors.Black
				};

                CorrAcc.SetRow(cell, i+1);
                CorrAcc.SetColumn(cell, e+1);
                CorrAcc.Children.Add(cell);
            }
        }
    }

    private void GenerateMatrixRot(double[,] matrix)
    {
        CorrRot.RowDefinitions.Clear();
        CorrRot.ColumnDefinitions.Clear();
        CorrRot.Children.Clear();

        for (int i = 0; i < 7; i++)
        {
            CorrRot.RowDefinitions.Add(new RowDefinition(GridLength.Star));
            CorrRot.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
        }

        string[] cellName = ["Selected X", "Selected Y", "Selected Z", "Best X", "Best Y", "Best Z"];

        for (int i = 0; i < 6; i++)
        {
            var cell = new Label
            {
                Text = cellName[i],
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                Padding = 8
            };

            CorrRot.SetRow(cell, 0);
            CorrRot.SetColumn(cell, i + 1);
            CorrRot.Children.Add(cell);

            var cell2 = new Label
            {
                Text = cellName[i],
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                Padding = 8
            };

            CorrRot.SetRow(cell2, i + 1);
            CorrRot.SetColumn(cell2, 0);
            CorrRot.Children.Add(cell2);
        }

        for (int i = 0; i < 6; i++)
        {
            for (int e = 0; e < 6; e++)
            {
                var cell = new Label
                {
                    Text = matrix[i, e].ToString("F2"),
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    Background = SetBackGroundColor(matrix[i, e]),
                    Padding = 8,
                    TextColor = Colors.Black
                };

                CorrRot.SetRow(cell, i + 1);
                CorrRot.SetColumn(cell, e + 1);
                CorrRot.Children.Add(cell);
            }
        }
    }

    private Color SetBackGroundColor(double value)
	{
		value = Math.Clamp(value, -1.0,1.0);
		if (value < 0)
		{
			double t = value + 1.0;
			return new Color(1.0f, (float)t, (float)t);
		}
		else
		{
			double t = value;
			return new Color((float)(1.0-t), (float)(1.0 - t),1.0f);
		}
	}
}