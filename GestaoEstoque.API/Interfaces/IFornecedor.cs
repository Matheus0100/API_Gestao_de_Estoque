using Microsoft.Build.Framework;

namespace GestaoEstoque.API.Interfaces
{
    public interface IFornecedor
    {
        [Required]
        int ID { get; set; }
        [Required]
        string Nome { get; set; }
        [Required]
        string Documento { get; set; }
        [Required]
        bool Ativo { get; set; }
    }
}
