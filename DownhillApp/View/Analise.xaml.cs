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
        vm.C_Statistic = new Command(() => 
        { 
            vm.SetStatisticData();
            CreateBasicStatistic(vm.statistic);
            CreateMSE(vm.mse);
            GenerateMatrixAcc(vm.corrAccData);
            GenerateMatrixRot(vm.corrRotData);
            l1.IsVisible = true;
            l2.IsVisible = true;
            l3.IsVisible = true;
            l4.IsVisible = true;
        });

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

    protected override void OnAppearing()
    {
        base.OnAppearing();
        CorrAcc.RowDefinitions.Clear();
        CorrAcc.ColumnDefinitions.Clear();
        CorrAcc.Children.Clear();
        CorrRot.RowDefinitions.Clear();
        CorrRot.ColumnDefinitions.Clear();
        CorrRot.Children.Clear();
        mseValues.RowDefinitions.Clear();
        mseValues.ColumnDefinitions.Clear();
        mseValues.Children.Clear();
        statBasic.RowDefinitions.Clear();
        statBasic.ColumnDefinitions.Clear();
        statBasic.Children.Clear();
        l1.IsVisible = false;
        l2.IsVisible = false;
        l3.IsVisible = false;
        l4.IsVisible = false;
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

        var blackcell = new Label
        {
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center,
            BackgroundColor = Colors.Black,
            Padding = 8
        };

		CorrAcc.SetRow(blackcell, 0);
		CorrAcc.SetColumn(blackcell, 0);
		CorrAcc.Children.Add(blackcell);

		string[] cellName = ["Selected X", "Selected Y", "Selected Z", "Best X", "Best Y", "Best Z"];

		for(int i= 0; i<6; i++)
		{
			var cell = new Label
			{
				Text = cellName[i],
				HorizontalTextAlignment = TextAlignment.Center,
				VerticalTextAlignment = TextAlignment.Center,
				BackgroundColor = Colors.Black,
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
                BackgroundColor = Colors.Black,
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

        var blackcell = new Label
        {
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center,
            BackgroundColor = Colors.Black,
            Padding = 8
        };

        CorrRot.SetRow(blackcell, 0);
        CorrRot.SetColumn(blackcell, 0);
        CorrRot.Children.Add(blackcell);

        string[] cellName = ["Selected X", "Selected Y", "Selected Z", "Best X", "Best Y", "Best Z"];

        for (int i = 0; i < 6; i++)
        {
            var cell = new Label
            {
                Text = cellName[i],
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
				BackgroundColor = Colors.Black,
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
				BackgroundColor = Colors.Black,
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

	private void CreateMSE(double[] values)
	{
		mseValues.RowDefinitions.Clear();
		mseValues.ColumnDefinitions.Clear();
		mseValues.Children.Clear();

		for(int i =0; i<7; i++)
		{
			mseValues.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
		}
        mseValues.RowDefinitions.Add(new RowDefinition(GridLength.Star));
        mseValues.RowDefinitions.Add(new RowDefinition(GridLength.Star));

        var blackcell = new Label
        {
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center,
            BackgroundColor = Colors.Black,
            Padding = 8
        };

        mseValues.SetRow(blackcell, 0);
        mseValues.SetColumn(blackcell, 0);
        mseValues.Children.Add(blackcell);

        string[] colName = ["Acceleration X", "Acceleration Y", "Acceleration Z", "Rotation X", "Rotation Y", "Rotation Z"];

        for (int i = 0; i<6; i++)
		{
            var cell = new Label
            {
                Text = colName[i],
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                BackgroundColor = Colors.Black,
                Padding = 8
            };

            mseValues.SetColumn(cell, i + 1);
            mseValues.SetRow(cell, 0);
            mseValues.Children.Add(cell);

            var cell2 = new Label
			{
                Text = values[i].ToString("F2"),
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                BackgroundColor = Colors.Black,
                Padding = 8
            };

			mseValues.SetColumn(cell2,i + 1);
			mseValues.SetRow(cell2,1);
			mseValues.Children.Add(cell2);
		}

		var msecell = new Label
		{
			Text = "MSE value",
			HorizontalTextAlignment = TextAlignment.Center,
			VerticalTextAlignment = TextAlignment.Center,
            BackgroundColor = Colors.Black,
			Padding = 8
		};

		mseValues.SetColumn(msecell,0);
		mseValues.SetRow(msecell,1);
		mseValues.Children.Add(msecell);
	}

    private void CreateBasicStatistic(double[,] datas)
    {
        statBasic.RowDefinitions.Clear();
        statBasic.ColumnDefinitions.Clear();
        statBasic.Children.Clear();

        for (int i = 0; i < 4; i++)
        {
            statBasic.RowDefinitions.Add(new RowDefinition(GridLength.Star));
            statBasic.ColumnDefinitions.Add(new ColumnDefinition(GridLength.Star));
        }

        var blackcell = new Label
        {
            HorizontalTextAlignment = TextAlignment.Center,
            VerticalTextAlignment = TextAlignment.Center,
            BackgroundColor = Colors.Black,
            Padding = 8
        };

        statBasic.SetRow(blackcell, 0);
        statBasic.SetColumn(blackcell, 0);
        statBasic.Children.Add(blackcell);

        string[] colName = ["Acceleration X", "Acceleration Y", "Acceleration Z"];
        string[] rowName = ["Max value", "Distance from Start", "Difference from Best Ride"];

        for(int i = 0; i<3; i++)
        {
            var cell = new Label
            {
                Text = colName[i],
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                BackgroundColor = Colors.Black,
                Padding = 8
            };

            statBasic.SetRow(cell, 0);
            statBasic.SetColumn(cell, i+1);
            statBasic.Children.Add(cell);

            var cell2 = new Label
            {
                Text = rowName[i],
                HorizontalTextAlignment = TextAlignment.Center,
                VerticalTextAlignment = TextAlignment.Center,
                BackgroundColor = Colors.Black,
                Padding = 8
            };

            statBasic.SetRow(cell2, i+1);
            statBasic.SetColumn(cell2, 0);
            statBasic.Children.Add(cell2);
        }

        for (int i = 0; i < 3; i++)
        {
            for (int e = 0; e < 3; e++)
            {
                var cell = new Label
                {
                    Text = datas[i, e].ToString("F2"),
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    BackgroundColor = Colors.Black,
                    Padding = 8
                };

                statBasic.SetColumn(cell, e + 1);
                statBasic.SetRow(cell, i + 1);
                statBasic.Children.Add(cell);
            }
        }
    }
}