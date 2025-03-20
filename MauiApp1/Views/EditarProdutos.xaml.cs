using System;
using System.Threading.Tasks;
using MauiApp1.Models;
using Microsoft.Maui.Controls;

namespace MauiApp1.Views
{
    public partial class EditarProduto : ContentPage
    {
        public EditarProduto()
        {
            InitializeComponent();
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                // Verifica se o BindingContext é um Produto
                if (BindingContext is not Produto produto_anexado)
                {
                    await DisplayAlert("Erro", "Produto não encontrado.", "OK");
                    return;
                }

                // Validação dos campos
                if (string.IsNullOrWhiteSpace(txt_descricao.Text))
                {
                    await DisplayAlert("Erro", "A descrição é obrigatória.", "OK");
                    return;
                }

                if (!double.TryParse(txt_quantidade.Text, out double quantidade))
                {
                    await DisplayAlert("Erro", "Quantidade inválida.", "OK");
                    return;
                }

                if (!double.TryParse(txt_preco.Text, out double preco))
                {
                    await DisplayAlert("Erro", "Preço inválido.", "OK");
                    return;
                }

                // Atualiza as propriedades do produto com os valores da interface
                produto_anexado.Descricao = txt_descricao.Text;
                produto_anexado.Quantidade = quantidade;
                produto_anexado.Preco = preco;

                // Atualiza o produto no banco de dados
                await App.DB.Update(produto_anexado);

                // Exibe uma mensagem de sucesso
                await DisplayAlert("Sucesso!", "Produto atualizado com sucesso.", "OK");

                // Navega de volta para a página anterior
                await Navigation.PopAsync();
            }
            catch (Exception ex)
            {
                // Exibe uma mensagem de erro
                await DisplayAlert("Erro", $"Não foi possível atualizar o produto: {ex.Message}", "OK");
            }
        }
    }
}