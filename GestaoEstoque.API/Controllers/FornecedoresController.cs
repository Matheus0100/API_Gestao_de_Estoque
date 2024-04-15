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
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FornecedoresController : MainController
    {
        private readonly EstoqueContextDB _context;

        public FornecedoresController(EstoqueContextDB context)
        {
            _context = context;
        }
        
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Fornecedor>>> GetFornecedores()
        {
            if (_context.Fornecedores == null)
            {
                return NotFound();
            }
            return await _context.Fornecedores.ToListAsync();
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Fornecedor>> GetFornecedor(int id)
        {
            if (_context.Fornecedores == null)
            {
                return NotFound();
            }
            var fornecedor = await _context.Fornecedores.AsNoTracking<Fornecedor>().FirstOrDefaultAsync(f => f.ID == id);

            if (fornecedor == null)
            {
                return NotFound();
            }

            return fornecedor;
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutFornecedor(int id, Fornecedor fornecedor)
        {
            if (id != fornecedor.ID)
            {
                return BadRequest();
            }

            _context.Entry(fornecedor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FornecedorExists(id))
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
        public async Task<ActionResult<Fornecedor>> PostFornecedor(Fornecedor fornecedor)
        {
            if (_context.Fornecedores == null)
            {
                return Problem("Erro: Conjunto de entidades nulo, favor contatar o desenvolvedor.");
            }

            _context.Fornecedores.Add(fornecedor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFornecedor", new { id = fornecedor.ID }, fornecedor);
        }

        [HttpDelete("{id}")]
        [CustomAuthorize("Produto", "Gerente")]
        public async Task<IActionResult> DeleteFornecedor(int id)
        {
            if (_context.Fornecedores == null)
            {
                return NotFound();
            }
            var fornecedor = await _context.Fornecedores.AsNoTracking<Fornecedor>().FirstOrDefaultAsync(f => f.ID == id);
            if (fornecedor == null)
            {
                return NotFound();
            }

            _context.Fornecedores.Remove(fornecedor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FornecedorExists(int id)
        {
            return (_context.Fornecedores?.Any(e => e.ID == id)).GetValueOrDefault();
        }
    }
}
