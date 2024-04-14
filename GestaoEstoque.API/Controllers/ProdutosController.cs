using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestaoEstoque.API.Data;
using GestaoEstoque.API.Models;
using Microsoft.AspNetCore.Authorization;
using NetDevPack.Identity.Authorization;

namespace GestaoEstoque.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : MainController
    {
        private readonly EstoqueContextDB _context;

        public ProdutosController(EstoqueContextDB context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Produto>>> GetProdutos()
        {
            if (_context.Produtos == null)
            {
                return NotFound();
            }
            return await _context.Produtos.ToListAsync();
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Produto>> GetProduto(int id)
        {
            if (_context.Produtos == null)
            {
                return NotFound();
            }
            var produto = await _context.Produtos.AsNoTracking<Produto>().FirstOrDefaultAsync(p => p.ID == id);

            if (produto == null)
            {
                return NotFound();
            }

            return produto;
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutProduto(int id, Produto produto)
        {
            if (id != produto.ID)
            {
                return BadRequest();
            }

            var fornecedor = await _context.Fornecedores.AsNoTracking<Fornecedor>().FirstOrDefaultAsync(f => f.ID == produto.IdFornecedor);

            if (fornecedor == null) return BadRequest();

            if (produto.QuantidadeEmEstoque < 0) return BadRequest();

            if (produto.Preco <= 0) return BadRequest();

            if (produto.Nome.Length <= 1) return BadRequest();

            _context.Entry(produto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProdutoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Produto>> PostProduto(Produto produto)
        {
            if (_context.Produtos == null)
            {
                return Problem("Entidade nula, favor contatar o desenvolvedor.");
            }

            var fornecedor = await _context.Fornecedores.AsNoTracking<Fornecedor>().FirstOrDefaultAsync(f => f.ID == produto.IdFornecedor);

            if (fornecedor == null) { return BadRequest(); }

            if (produto.QuantidadeEmEstoque < 0) return BadRequest();

            if (produto.Preco <= 0) return BadRequest();

            if (produto.Nome.Length <= 1) return BadRequest();

            _context.Produtos.Add(produto);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetProduto", new { id = produto.ID }, produto);
        }

        [HttpDelete("{id}")]
        [CustomAuthorize("Produto", "Gerente")]
        public async Task<IActionResult> DeleteProduto(int id)
        {
            if (_context.Produtos == null)
            {
                return NotFound();
            }

            var produto = await _context.Produtos.AsNoTracking<Produto>().FirstOrDefaultAsync(p => p.ID == id);

            if (produto == null)
            {
                return NotFound();
            }

            _context.Produtos.Remove(produto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProdutoExists(int id)
        {
            return (_context.Produtos?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
