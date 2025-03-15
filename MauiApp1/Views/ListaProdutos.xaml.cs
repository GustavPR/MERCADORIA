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
                lista.Clear();

                List<Produto> tmp = await App.DB.Search(q);
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
                var menuItem = sender as MenuItem;
                var produto = menuItem?.CommandParameter as Produto;

                if (produto != null)
                {
                    bool confirm = await DisplayAlert("Confirmar", $"Deseja remover {produto.Descricao}?", "Sim", "Não");
                    if (confirm)
                    {
                        await App.DB.Delete(produto.Id);
                        lista.Remove(produto);
                    }
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }
    }
}