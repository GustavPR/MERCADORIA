using System;
using System.Collections.Generic;
using System.Linq;
using MauiApp1.Models;
using SQLite;
using System.Collections.ObjectModel;

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

        // Variável para armazenar a categoria selecionada
        private string categoriaSelecionada;

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await AtualizarListaProdutos();
        }

        public async Task AtualizarListaProdutos()
        {
            try
            {
                List<Produto> tmp = await App.DB.GetAll();

                if (!string.IsNullOrEmpty(categoriaSelecionada) && categoriaSelecionada != "Todos os Produtos")
                {
                    tmp = tmp.Where(p => p.Categoria == categoriaSelecionada).ToList();
                }

                lista.Clear();
                foreach (var item in tmp)
                {
                    lista.Add(item);
                }

                var totalGasto = tmp.Sum(p => p.Total);
                totalGastoLabel.Text = $"Total Gasto: {totalGasto:C}";
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }

        private async void CategoriaPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            categoriaSelecionada = categoriaPicker.SelectedItem?.ToString();
            await AtualizarListaProdutos();
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Navigation.PushAsync(new NovoProduto());
            }
            catch (Exception ex)
            {
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }

        private async void ToolbarItem_Clicked1(object sender, EventArgs e)
        {
            try
            {
                double soma = lista.Sum(i => i.Total);
                string msg = $"O total é {soma:C}";
                await DisplayAlert("Total dos Produtos", msg, "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }

        private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                string q = e.NewTextValue;
                lst_produtos.IsRefreshing = true;

                List<Produto> tmp = string.IsNullOrWhiteSpace(q)
                    ? await App.DB.GetAll()
                    : await App.DB.Search(q);

                lista.Clear();
                foreach (var item in tmp)
                {
                    lista.Add(item);
                }

                var totalGasto = tmp.Sum(p => p.Total);
                totalGastoLabel.Text = $"Total Gasto: {totalGasto:C}";
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
            finally
            {
                lst_produtos.IsRefreshing = false;
            }
        }

        private async void lst_produtos_Refreshing(object sender, EventArgs e)
        {
            try
            {
                await AtualizarListaProdutos();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
            finally
            {
                lst_produtos.IsRefreshing = false;
            }
        }

        private async void lst_produtos_SelectedItem(object sender, SelectedItemChangedEventArgs e)
        {
            try
            {
                var produtoSelecionado = e.SelectedItem as Produto;
                if (produtoSelecionado != null)
                {
                    await DisplayAlert("Produto Selecionado", $"Descrição: {produtoSelecionado.Descricao}\nPreço: {produtoSelecionado.Preco:C}", "OK");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "OK");
            }
        }

        // Método que será chamado quando o MenuItem for clicado
        private async void MenuItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Obter o produto selecionado através do BindingContext
                var menuItem = sender as MenuItem;
                var produto = menuItem?.BindingContext as Produto;

                if (produto != null)
                {
                    // Remover o produto do banco de dados
                    var result = await App.DB.Delete(produto.Id);

                    if (result > 0)
                    {
                        // Remover o produto da ObservableCollection (UI)
                        lista.Remove(produto);

                        // Exibir uma mensagem de sucesso
                        await DisplayAlert("Sucesso", "Produto removido com sucesso.", "OK");
                    }
                    else
                    {
                        await DisplayAlert("Erro", "Falha ao remover o produto.", "OK");
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
