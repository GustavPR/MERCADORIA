using System.Collections.ObjectModel;
using MauiApp1.Models;
using MauiApp1.Views;

namespace MauiApp1.Views
{
    public partial class ListaProduto : ContentPage
    {
        ObservableCollection<Produto> lista = new ObservableCollection<Produto>();

        public ListaProduto()
        {
            InitializeComponent();
            lst_produtos.ItemsSource = lista;
        }

        protected async override void OnAppearing()
        {
            try
            {
                List<Produto> tmp = await App.DB.GetAll();
                lista.Clear();
                foreach (var item in tmp)
                {
                    lista.Add(item);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }

        private void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                Navigation.PushAsync(new NovoProduto());
            }
            catch (Exception ex)
            {
                DisplayAlert("Ops", ex.Message, "OK");
            }
        }

        private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string q = e.NewTextValue;

                // Evita buscar se o texto estiver vazio
                if (string.IsNullOrWhiteSpace(q))
                {
                    List<Produto> tmp = await App.DB.GetAll();
                    lista.Clear();
                    foreach (var item in tmp)
                    {
                        lista.Add(item);
                    }
                }
                else
                {
                    List<Produto> tmp = await App.DB.Search(q);
                    lista.Clear();
                    foreach (var item in tmp)
                    {
                        lista.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }

        private void ToolbarItem_Clicked1(object sender, EventArgs e)
        {
            double soma = lista.Sum(i => i.Total);
            string msg = $"O total é {soma:C}";
            DisplayAlert("Total dos Produtos", msg, "OK");
        }

        private async void MenuItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                MenuItem selecionado = sender as MenuItem;

                Produto p = selecionado.BindingContext as Produto;
                bool confirm = await DisplayAlert("Tem Certeza?", $"Remover o Produto {p.Descricao}?", "Sim", "Não");
                if (confirm)
                {
                    await App.DB.Delete(p.Id);
                    lista.Remove(p);
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }

       private async void lst_produtos_SelectedItem(object sender, SelectedItemChangedEventArgs e)
{
    try
    {
        // Verifica se um item foi selecionado
        if (e.SelectedItem is Produto produtoSelecionado)
        {
            // Cria uma instância da página EditarProduto
            var editarProdutoPage = new EditarProduto();

            // Define o BindingContext com o produto selecionado
            editarProdutoPage.BindingContext = produtoSelecionado;

            // Navega para a página EditarProduto
            await Navigation.PushAsync(editarProdutoPage);
        }
    }
    catch (Exception ex)
    {
        // Exibe uma mensagem de erro
        await DisplayAlert("Erro", ex.Message, "OK");
    }
    finally
    {
        // Deseleciona o item para permitir uma nova seleção no futuro
        lst_produtos.SelectedItem = null;
    }
}
    }
}