using GestaoEstoque.API.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace GestaoEstoque.API.Models
{
    public class Produto : IProduto
    {
        [Required]
        public int ID { get; set; }
        [Required]
        [StringLength(100,MinimumLength = 1)]
        public string Nome { get; set; }
        [StringLength(254, MinimumLength = 6)]
        public string Descricao { get; set; }
        [Required]
        [LongValidator]
        public decimal Preco { get; set; }
        [Required]
        [IntegerValidator]
        public int IdFornecedor { get; set; }
        [Required]
        [IntegerValidator]
        public int QuantidadeEmEstoque { get; set; }
    }
}
