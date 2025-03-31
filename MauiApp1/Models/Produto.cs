using SQLite;

namespace MauiApp1.Models
{
    public class Produto
    {
        String _descricao;
        String _categoria;

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Descricao
        {
            get => _descricao;
            set
            {
                if (value == null)
                {
                    throw new Exception("Por favor, preencha a descrição");
                }
                _descricao = value;
            }
        }

        // Novo campo Categoria
        public string Categoria
        {
            get => _categoria;
            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new Exception("Por favor, preencha a categoria");
                }
                _categoria = value;
            }
        }

        public double Quantidade { get; set; }
        public double Preco { get; set; }
        public double Total { get => Quantidade * Preco; }
    }
}
