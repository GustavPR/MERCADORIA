using MauiApp1.Models;
using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace MauiApp1.Views
{
    public partial class NovoProduto : ContentPage
    {
        // Construtor da classe
        public NovoProduto()
        {
            InitializeComponent();
        }

        // Manipulador do evento Clicked do ToolbarItem
        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            await ToolbarItem_ClickedAsync(sender, e); // Chama o método assíncrono
        }

        // Método assíncrono que manipula o clique do item da barra de ferramentas
        private async Task ToolbarItem_ClickedAsync(object sender, EventArgs e)
        {
            try
            {
                // Validação para garantir que os campos não estão vazios
                if (string.IsNullOrEmpty(txt_descricao.Text) ||
                    string.IsNullOrEmpty(txt_quantidade.Text) ||
                    string.IsNullOrEmpty(txt_preco.Text) ||
                    categoriaPicker.SelectedItem == null)
                {
                    await DisplayAlert("Erro", "Todos os campos são obrigatórios, incluindo a categoria.", "OK");
                    return;
                }

                // Tenta converter os valores de quantidade e preço para double
                if (!double.TryParse(txt_quantidade.Text, out double quantidade) ||
                    !double.TryParse(txt_preco.Text, out double preco))
                {
                    await DisplayAlert("Erro", "Quantidade e preço devem ser números válidos.", "OK");
                    return;
                }

                // Captura a categoria selecionada
                string categoria = categoriaPicker.SelectedItem.ToString();

                // Criação do objeto Produto
                Produto p = new Produto
                {
                    Descricao = txt_descricao.Text,
                    Quantidade = quantidade,
                    Preco = preco,
                    Categoria = categoria // Atribuindo a categoria ao produto
                };

                // Inserir o produto no banco de dados de forma assíncrona
                await App.DB.Insert(p);

                // Exibe um alerta de sucesso
                await DisplayAlert("Sucesso!", "Produto inserido com sucesso.", "OK");

                // Navega de volta para a página anterior
                await Navigation.PopAsync();

                // Atualizar a lista na página ListaProduto
                // Encontrar a página ListaProduto na pilha de navegação e chamar o método para atualizar a lista
                var listaProdutoPage = Navigation.NavigationStack.OfType<ListaProduto>().FirstOrDefault();
                listaProdutoPage?.AtualizarListaProdutos(); // Chama o método para atualizar a lista na página ListaProduto
            }
            catch (Exception ex)
            {
                // Em caso de erro, exibe a mensagem de erro
                await DisplayAlert("Ops", ex.Message, "OK");
            }
        }
    }
}
