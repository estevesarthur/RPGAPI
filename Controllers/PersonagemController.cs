using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using RpgApi.Models;
using System.Linq;
using RpgApi.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;


namespace RpgApi.Controllers
{   
    [Authorize(Roles = "Jogador, Admin")]
    [ApiController]
    [Route("[controller]")]
    public class PersonagemController : ControllerBase
    {
       private readonly DataContext _context;
       private readonly IHttpContextAccessor _httpContextAccessor;
       public PersonagemController(DataContext context, IHttpContextAccessor httpContextAccessor)
       {
           _context = context;
           _httpContextAccessor = httpContextAccessor;
       }

       private int ObterUsuarioId()
       {
           return int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
       }
       private string ObterPerfilUsuario(){
           return _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Role);
       }

       [HttpPost]
       public async Task<IActionResult> AddPersonagemAsync(Personagem novoPersonagem)
       {
            novoPersonagem.Usuario = await _context.Usuarios.FirstOrDefaultAsync(uBusca => uBusca.Id == ObterUsuarioId());

            await _context.Personagens.AddAsync(novoPersonagem);
            await _context.SaveChangesAsync();
            List<Personagem> listPersonagens = await _context.Personagens.ToListAsync();
            return Ok(listPersonagens);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetSingleAsync(int id)
        { //método assincrono para chamar
            Personagem p = await _context.Personagens
            .Include(us => us.Usuario)
            .Include(ar => ar.Arma)
            .Include(ph => ph.PersonagemHabilidades).ThenInclude(h => h.Habilidade)
            .FirstOrDefaultAsync(b => b.Id == id);
            return Ok(p);
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            List<Personagem> listPersonagens = new List<Personagem>();

            if(ObterPerfilUsuario() == "Admin")
                listPersonagens = await _context.Personagens
                //.Include(u => u.usuario)
                .ToListAsync();

            else
                listPersonagens = await _context.Personagens
                    .Where(p => p.Usuario.Id == ObterUsuarioId()).ToListAsync();

            return Ok(listPersonagens);
        }

        [HttpPut]
        public async Task<IActionResult> UpdatePersonagemAsync(Personagem p)
        {
            p.Usuario = await _context.Usuarios.FirstOrDefaultAsync(uBusca => uBusca.Id == ObterUsuarioId());
            _context.Personagens.Update(p);
            await _context.SaveChangesAsync();
            return Ok(p);
        }

        [HttpGet("GetByUser")]

        public async Task<IActionResult> GetByUserAsync()
        {
            int id = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);

            List<Personagem> personagens = await _context.Personagens.Where(c => c.Usuario.Id == id).ToListAsync();
            return Ok(personagens);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            Personagem pRemover = await _context.Personagens.FirstOrDefaultAsync(p => p.Id == id);

            _context.Personagens.Remove(pRemover);
           await _context.SaveChangesAsync();

            List<Personagem> listPersonagens = await _context.Personagens.ToListAsync();

            return Ok(listPersonagens);
        }
    }
}