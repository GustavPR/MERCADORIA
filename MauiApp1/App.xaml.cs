using MauiApp1.Helpers; // Certifique-se de que o namespace está correto

namespace MauiApp1;

public partial class App : Application
{
    static SQLiteDatabaseHelper _db;

    public static SQLiteDatabaseHelper DB
    {
        get
        {
            if (_db == null)
            {
                // Defina o caminho do banco de dados
                string path = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "banco_sqlite_compras.db3"
                );

                // Crie a instância do SQLiteDatabaseHelper com o caminho correto
                _db = new SQLiteDatabaseHelper(path);
            }
            return _db;
        }
    }

    public App()
    {
        InitializeComponent();

        // Define a MainPage como uma NavigationPage com ListaProduto como página inicial
        MainPage = new NavigationPage(new Views.ListaProduto());
    }
}
