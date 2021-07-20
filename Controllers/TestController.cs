using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RpgApi.Models;
using RpgApi.Models.Enuns;
using System.Linq;

namespace RpgApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {
        private static List<Personagem> personagens = new List<Personagem>
        {

            new Personagem() { Id = 1, }, //Frodo Cavaleiro
            new Personagem() { Id = 2, Nome = "Sam", PontosVida=100, Forca=22, Defesa=25, Inteligencia=30, Classe=ClasseEnum.Cavaleiro},
            new Personagem() { Id = 3, Nome = "Galadriel", PontosVida=100, Forca=18, Defesa=21, Inteligencia=35, Classe=ClasseEnum.Clerigo},
            new Personagem() { Id = 4, Nome = "Gandalf", PontosVida=100, Forca=18, Defesa=18, Inteligencia=37, Classe=ClasseEnum.Mago},
            new Personagem() { Id = 5, Nome = "Hobbit", PontosVida=100, Forca=20, Defesa=17, Inteligencia=31, Classe=ClasseEnum.Cavaleiro},
            new Personagem() { Id = 6, Nome = "Celeborn", PontosVida=100, Forca=21, Defesa=13, Inteligencia=34, Classe=ClasseEnum.Clerigo},
            new Personagem() { Id = 7, Nome = "Radagast", PontosVida=100, Forca=25, Defesa=11, Inteligencia=35, Classe=ClasseEnum.Mago}
        };

        [HttpGet("GetAll")] // direcionar a rota
        public IActionResult Get()
        {
            return Ok(personagens);
        }
        public IActionResult GetSingle()
        {
            return Ok(personagens[0]);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            Personagem pEncontrado = personagens.FirstOrDefault(pe => pe.Id == id);
            return Ok(pEncontrado);
            // retorna personagem por Id
        }

        [HttpGet("GetCombinacao")] //Achar personagem por força menor que 20 e inteligencia maior que 30, tipo filtro
        public IActionResult GetCombinacao(int id)
        {
            List<Personagem> listaEncontrada =
                personagens.FindAll(p => p.Forca < 20 && p.Inteligencia > 30);
            return Ok(listaEncontrada);
        }

        [HttpGet("GetOrdenado")]
        public IActionResult GetOrdem()
        {
            List<Personagem> listaOrdenada = personagens.OrderBy(p => p.Nome).ToList();
            return Ok(listaOrdenada);
        }

        [HttpGet("GetContagem")]
        public IActionResult GetQuantidade()
        {
            return Ok("Quantidade de personagens: " + personagens.Count);
        }

        [HttpGet("GetSomaForca")]
        public IActionResult GetSomaForca()
        {
            return Ok("A soma total da força de todos os personagens: " + personagens.Sum(p => p.Forca));
        }

        [HttpGet("GetWithoutKnight")]
        public IActionResult GetAllWithoutKnight()
        {
            List<Personagem> listFind = personagens.FindAll(p => p.Classe != ClasseEnum.Cavaleiro);
            return Ok(listFind);
        }

        [HttpGet("GetByApproximateName/{name}")]
        public IActionResult GetByApproximateName(string name)
        {
            List<Personagem> listFind = personagens.FindAll(p => p.Nome.Contains(name));//Contains distingue maiuscula e minuscula
            return Ok(listFind);
        }

        [HttpGet("GetMageDelete")]
        public IActionResult GetMageDelete()
        {
            personagens.RemoveAll(p => p.Classe == ClasseEnum.Mago);
            return Ok(personagens);
        }

        [HttpGet("GetByIntelligence/{value}")]
        public IActionResult GetByIntelligence(int value)
        {
            List<Personagem> listFind = personagens.FindAll(p => p.Inteligencia >= value);

            if (listFind.Count == 0)
                return BadRequest("Nenhuma personagem encontrado");
            else
                return Ok(listFind);
        }



        [HttpPost]
        public IActionResult AddPersonagem(Personagem NovoPersonagem)
        {
            if (NovoPersonagem.Inteligencia == 0)
                return BadRequest("Inteligência não pode ser igual a ZERO.");

            personagens.Add(NovoPersonagem);
            return Ok(personagens);
        }
        [HttpPut]
        public IActionResult UpdatePersonagem(Personagem p)
        {
            Personagem personagemAlterado = personagens.Find(pers => pers.Id == p.Id);
            personagemAlterado.Nome = p.Nome;
            personagemAlterado.PontosVida = p.PontosVida;
            personagemAlterado.Forca = p.Forca;
            personagemAlterado.Defesa = p.Defesa;
            personagemAlterado.Inteligencia = p.Inteligencia;
            personagemAlterado.Classe = p.Classe;

            return Ok(personagens);
        }
        
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            personagens.RemoveAll(pers => pers.Id == id);
            return Ok(personagens);
        }

    }
}