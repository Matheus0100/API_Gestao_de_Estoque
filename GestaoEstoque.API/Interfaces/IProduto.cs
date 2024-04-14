using Microsoft.Build.Framework;

namespace GestaoEstoque.API.Interfaces
{
    public interface IProduto
    {
        [Required]
        int ID { get; set; }
        [Required]
        string Nome { get; set; }
        string Descricao { get; set; }
        [Required]
        decimal Preco { get; set; }
        [Required]
        int IdFornecedor { get; set; }
        [Required]
        int QuantidadeEmEstoque { get; set; }
    }
}
