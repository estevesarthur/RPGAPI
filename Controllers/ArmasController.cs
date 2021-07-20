using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RpgApi.Data;
using RpgApi.Models;

namespace RpgApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class ArmaController : ControllerBase
    {
        private readonly DataContext _context;
        
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ArmaController(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private int ObterUsuarioId()
        {
            return int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        [HttpPost]
        public async Task<IActionResult> AddArmasAsync(Arma novaArma)
        {
            //Buscar personagem de acordo com a claim do token
            Personagem personagem = await _context.Personagens
                .FirstOrDefaultAsync(p => p.Id == novaArma.PersonagemId);

            //Se não achar ninguém com o Id correspondente
            if (personagem == null)
                return BadRequest("Não existe personagem com Id informado.");
            
            Arma arma = await _context.Armas
                .FirstOrDefaultAsync(a => a.PersonagemId == novaArma.PersonagemId);

            if(arma != null)
                return BadRequest("O Personagem informado já contém uma arma.");

            await _context.Armas.AddAsync(novaArma);
            await _context.SaveChangesAsync();

            List<Arma> listArmas = await _context.Armas
                .Where(p => p.PersonagemId == novaArma.PersonagemId)//encontrar arma pelo personagem
                .ToListAsync();

            return Ok(listArmas);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingleAsync(int id)
        { //método assincrono para chamar
            Arma a = await _context.Armas.FirstOrDefaultAsync(b => b.Id == id);
            return Ok(a);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            List<Arma> listArmas = await _context.Armas.ToListAsync();
            return Ok(listArmas);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateArmaAsync(Arma a)
        {
            _context.Armas.Update(a);
            await _context.SaveChangesAsync();
            return Ok(a);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            Arma aRemover = await _context.Armas.FirstOrDefaultAsync(a => a.Id == id);

            _context.Armas.Remove(aRemover);
            await _context.SaveChangesAsync();

            List<Arma> listArmas = await _context.Armas.ToListAsync();

            return Ok(listArmas);
        }

        [HttpGet("Sorteio")]
        public IActionResult Sorteio()
        {
            int numero = new Random().Next(10);

            return Ok(numero);
        }
    }
}