using Microsoft.Extensions.Logging;
using Distribuidora_La_Central.Shared.Services;
using Distribuidora_La_Central.Services;

namespace Distribuidora_La_Central;

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
            });

        // Add device-specific services used by the Distribuidora_La_Central.Shared project
        builder.Services.AddSingleton<IFormFactor, FormFactor>();

        builder.Services.AddMauiBlazorWebView();

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif
        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7263") });
        return builder.Build();
    }
}
