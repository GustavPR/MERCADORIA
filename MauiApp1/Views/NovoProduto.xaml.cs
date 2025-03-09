using MauiApp1.Models;
using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
//aaaaaagit log
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
        // Esse é um método não assíncrono
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
                    string.IsNullOrEmpty(txt_preco.Text))
                {
                    await DisplayAlert("Erro", "Todos os campos são obrigatórios.", "OK");
                    return;
                }

                // Tenta converter os valores de quantidade e preço para double
                if (!double.TryParse(txt_quantidade.Text, out double quantidade) ||
                    !double.TryParse(txt_preco.Text, out double preco))
                {
                    await DisplayAlert("Erro", "Quantidade e preço devem ser números válidos.", "OK");
                    return;
                }

                // Criação do objeto Produto
                Produto p = new Produto
                {
                    Descricao = txt_descricao.Text,
                    Quantidade = quantidade,
                    Preco = preco
                };

                // Inserir o produto no banco de dados de forma assíncrona
                await App.DB.Insert(p);

                // Exibe um alerta de sucesso
                await DisplayAlert("Sucesso!", "Produto inserido com sucesso.", "OK");
            }
            catch (Exception ex)
            {
                // Em caso de erro, exibe a mensagem de erro
                await DisplayAlert("Ops",ex.Message, "OK");
            }
        }
    }
}


