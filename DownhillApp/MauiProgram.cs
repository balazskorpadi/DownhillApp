using DownhillApp.Service;
using Microsoft.Extensions.Logging;

namespace DownhillApp
{
    public static class ServiceHandler
    {
        public static IServiceProvider Service { get; set; } 
    }
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton<ILocalDataBase, LocalDataBase>();
            builder.Services.AddSingleton<IDataProvider, DataProvider>();

            var app = builder.Build();
            ServiceHandler.Service = app.Services;
            return app;
        }
    }
}
