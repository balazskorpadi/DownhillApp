using DownhillApp.Service;
using Microsoft.Extensions.Logging;
using Syncfusion.Maui.Core.Hosting;

namespace DownhillApp
{
    public static class API_Keys
    {
        public static string API_Gemini { get; set; }
        public static string API_Syncfusion { get; set; }
        public static void SetKeys()
        {
            string[] temp = File.ReadAllLines("C:\\DeveloperKeys\\DownhillKey.txt");
            API_Gemini = temp[1].Split(':')[1];  
            API_Syncfusion = temp[2].Split(':')[1];
        }
    }
    public static class ServiceHandler
    {
        public static IServiceProvider Service { get; set; } 
    }
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            API_Keys.SetKeys();
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureSyncfusionCore()
#if IOS
                .UseMauiMaps()
#endif
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
            builder.Services.AddSingleton<IGeminiAiService, GeminiAiService>();

            var app = builder.Build();
            ServiceHandler.Service = app.Services;
            ILocalDataBase loadDatas = ServiceHandler.Service.GetService<ILocalDataBase>();
            loadDatas.LoadAtTheStart();
            return app;
        }
    }
}
