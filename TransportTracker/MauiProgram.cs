using Microsoft.Extensions.Logging;
using TransportTracker.Services;

namespace TransportTracker
{
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
            builder.Services.AddSingleton<DatabaseService>();
            builder.Services.AddTransient<RoutesPage>();
            builder.Services.AddTransient<AddRoutePage>();
            builder.Services.AddTransient<DriversPage>();
            builder.Services.AddTransient<AddDriverPage>();
            builder.Services.AddTransient<WorkPage>();
            builder.Services.AddTransient<AddWorkPage>();
            builder.Services.AddTransient<App>();
#if DEBUG
            builder.Logging.AddDebug();
#endif
            return builder.Build();
        }
    }
}