using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DevSteamAPI.Data;
using DevSteamAPI.Models;

namespace DevSteamAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItensCarrinhoController : ControllerBase
    {
        private readonly DevSteamAPIContext _context;

        public ItensCarrinhoController(DevSteamAPIContext context)
        {
            _context = context;
        }

        // GET: api/ItensCarrinho
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ItemCarrinho>>> GetItensCarrinho()
        {
            return await _context.ItemCarrinho.ToListAsync();
        }

        // GET: api/ItensCarrinho/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ItemCarrinho>> GetItemCarrinho(Guid id)
        {
            var itemCarrinho = await _context.ItemCarrinho.FindAsync(id);

            if (itemCarrinho == null)
            {
                return NotFound();
            }

            return itemCarrinho;
        }

        // POST: api/ItensCarrinho
        [HttpPost]
        public async Task<ActionResult<ItemCarrinho>> PostItemCarrinho(ItemCarrinho itemCarrinho)
        {
            itemCarrinho.Total = itemCarrinho.Quantidade * itemCarrinho.Valor;
            _context.ItemCarrinho.Add(itemCarrinho);
            await _context.SaveChangesAsync();

            await AtualizarTotalCarrinho(itemCarrinho.CarrinhoId);

            return CreatedAtAction("GetItemCarrinho", new { id = itemCarrinho.ItemCarrinhoId }, itemCarrinho);
        }

        // DELETE: api/ItensCarrinho/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItemCarrinho(Guid id)
        {
            var itemCarrinho = await _context.ItemCarrinho.FindAsync(id);
            if (itemCarrinho == null)
            {
                return NotFound();
            }

            _context.ItemCarrinho.Remove(itemCarrinho);
            await _context.SaveChangesAsync();

            await AtualizarTotalCarrinho(itemCarrinho.CarrinhoId);

            return NoContent();
        }

        private async Task AtualizarTotalCarrinho(Guid carrinhoId)
        {
            var carrinho = await _context.Carrinho.FindAsync(carrinhoId);
            if (carrinho != null)
            {
                carrinho.Total = await _context.ItemCarrinho
                    .Where(i => i.CarrinhoId == carrinhoId)
                    .SumAsync(i => i.Total);
                await _context.SaveChangesAsync();
            }
        }

        private bool ItemCarrinhoExists(Guid id)
        {
            return _context.ItemCarrinho.Any(e => e.ItemCarrinhoId == id);
        }
    }
}