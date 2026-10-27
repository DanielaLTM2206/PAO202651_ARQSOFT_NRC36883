using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using UniversalConverter.Client.Services;
using UniversalConverter.Client.ViewModels;
using UniversalConverter.Client.Views;

namespace UniversalConverter.Client
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

#if DEBUG
            // builder.Logging.AddDebug();
#endif

            // Configuracion desde recurso incrustado appsettings.json
            try
            {
                var appSettingsFileName = "appsettings.json";
                var assembly = Assembly.GetExecutingAssembly();
                var resourceName = $"{assembly.GetName().Name}.{appSettingsFileName}";
                using var stream = assembly.GetManifestResourceStream(resourceName);
                if (stream != null)
                {
                    var config = new ConfigurationBuilder().AddJsonStream(stream).Build();
                    builder.Configuration.AddConfiguration(config);
                }
            }
            catch
            {
                // Fallback seguro si no encuentra el recurso
            }

            // Inyeccion de Dependencias - Servicios REST
            builder.Services.AddSingleton<IConversionService, RestConversionService>();
            builder.Services.AddSingleton<RestConversionService>(sp => (RestConversionService)sp.GetRequiredService<IConversionService>());

            // ViewModels
            builder.Services.AddSingleton<LoginViewModel>();
            builder.Services.AddSingleton<MainViewModel>();

            // Pages
            builder.Services.AddSingleton<LoginPage>();
            builder.Services.AddSingleton<MainPage>();

            return builder.Build();
        }
    }
}
