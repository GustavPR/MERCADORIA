using Microsoft.Extensions.Logging;
using MauiApp1.Helpers;
using SQLite;

namespace MauiApp1;

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

	 string path = Path.Combine(FileSystem.AppDataDirectory, "database.db3");

        // Registra o SQLiteDatabaseHelper como um serviço singleton
        builder.Services.AddSingleton<SQLiteDatabaseHelper>(s => new SQLiteDatabaseHelper(path));

        // Constrói e retorna a instância do MauiApp
        return builder.Build();
	}
}
