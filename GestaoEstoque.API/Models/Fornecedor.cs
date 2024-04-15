using GestaoEstoque.API.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestaoEstoque.API.Models
{
    public class Fornecedor : IFornecedor
    {
        public int ID { get; set; }
        [Required]
        [StringLength(200)]
        public string Nome { get; set; }
        [Required]
        [StringLength(14)]
        public string Documento { get; set; }
        [Required]
        public bool Ativo { get; set; }
    }
}
