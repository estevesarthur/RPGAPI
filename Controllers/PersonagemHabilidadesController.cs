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
    public class PersonagemHabilidadesController : ControllerBase
    {
        //Codificação dos métodos dentro do corpo da controller.
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PersonagemHabilidadesController(DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        [HttpPost]
        public async Task<IActionResult> AddPersonagemHabilidadeAsync(PersonagemHabilidade novoPersonagemHabilidade)
        {
            Personagem personagem = await _context.Personagens
                .Include(p => p.Arma)
                .Include(p => p.PersonagemHabilidades).ThenInclude(ps => ps.Habilidade)
                .FirstOrDefaultAsync(p => p.Id == novoPersonagemHabilidade.PersonagemId);

            if (personagem == null)
                return BadRequest("Personagem não encontrado para o Id informado.");

            Habilidade habilidade = await _context.Habilidades
                .FirstOrDefaultAsync(s => s.Id == novoPersonagemHabilidade.HabilidadeId);

            if (habilidade == null)
                return BadRequest("Habilidade não localizada.");

            PersonagemHabilidade ph = new PersonagemHabilidade();
            ph.Personagem = personagem;
            ph.Habilidade = habilidade;

            await _context.PersonagemHabilidades.AddAsync(ph);
            await _context.SaveChangesAsync();

            return Ok(ph);
        }

        [HttpGet("{id}")]

        public async Task<IActionResult> GetSingleAsync2(int id)
        {
            Personagem p = await _context.Personagens
            .Include(usuario => usuario.Usuario)
            .Include(arma => arma.Arma)
            .Include(ph => ph.PersonagemHabilidades).ThenInclude(h => h.Habilidade)
            .FirstOrDefaultAsync(b => b.Id == id);
            return Ok(p);
        }

        [HttpPost("DeletePersonagemHabilidade")]

        public async Task<IActionResult> DeletePersonagemHabilidadeAsync(PersonagemHabilidade ph)
        {
            PersonagemHabilidade phRemover = await _context.PersonagemHabilidades
            .FirstOrDefaultAsync(phBusca => phBusca.PersonagemId == ph.PersonagemId && phBusca.HabilidadeId == ph.HabilidadeId);

            _context.PersonagemHabilidades.Remove(phRemover);
            await _context.SaveChangesAsync();

            return Ok(phRemover);
            //poderia ser return Ok("Dados excluídos com sucesso!")
        }
    }

}