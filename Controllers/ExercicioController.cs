using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using RpgApi.Models;
using RpgApi.Models.Enuns;
using System.Linq;

namespace RpgApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExercicioController : ControllerBase
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

        //Exercícios Lista de Personagens

        [HttpGet("GetByClasse/{classeId}")] //Letra A
        public IActionResult GetByClasse(int classeId)
        {
            List<Personagem> listInOrder = personagens.FindAll(p => (int)p.Classe == classeId);
            return Ok(listInOrder);
        }

        [HttpGet("GetByName/{name}")] // Letra B
        public IActionResult GetByName(string name)
        {
            /* Poderia ter sido feito assim também:
            Personagem p = personagens.Find(p => p Nome ==nome);
            if(p == null)
                return NotFound("NOT FOUND ANY CARACTER WITH THIS NAME, PLEASE TRY AGAIN.")
            else
                retunr Ok(p);*/

            List<Personagem> listInOrder = personagens.FindAll(p => p.Nome==name);
            if (listInOrder.Count == 0)
                return NotFound("NOT FOUND ANY CARACTER WITH THIS NAME, PLEASE TRY AGAIN.");
                
            else
                return Ok(listInOrder);
        }
        
        [HttpPost("Validacao")] // Letra C
        public IActionResult Validacao(Personagem NovoPersonagem)
        {
            personagens.Add(NovoPersonagem);
            if(NovoPersonagem.Defesa < 10 || NovoPersonagem.Inteligencia > 30 ){
                return BadRequest("Defesa não pode ser menor que 10 ou a Inteligência não pode ser maior que 30.");
            }else{            //Vi um modelo em que "personagens.Add(NovoPersonagem);" fica antes de return Ok(personagens);
            return Ok(personagens);
            }
        }

        [HttpPost("ValidacaoMago")]// Letra D
        public IActionResult ValidacaoMago(Personagem NovoPersonagem)
        {
            personagens.Add(NovoPersonagem);
            if(NovoPersonagem.Inteligencia < 35 && NovoPersonagem.Classe == ClasseEnum.Mago){
                return BadRequest("A Classe de Magos não pode ter Inteligência menor que 35.");
            }else{           //Vi um modelo em que "personagens.Add(NovoPersonagem);" fica antes de return Ok(personagens);
            return Ok(personagens);
            }
        }

        [HttpGet("GetClerigoMago")] //Letra E
        public IActionResult GetClerigoMago()
        {// tem que testar pegando clerigo e mago em vez de != ClasseEnum.Cavaleiro tentar com == ClasseEnum.Mago && ==ClasseEnum.Clerigo
            List<Personagem> lista = personagens.FindAll(p => p.Classe != ClasseEnum.Cavaleiro).OrderByDescending(p => p.PontosVida).ThenBy(p => p.Forca).ToList(); 
            return Ok(lista);
        }

        [HttpGet("GetStatistics")] //Letra F
        public IActionResult GetStatistics()
        {
            return Ok("Quantidade de personagens: " + personagens.Count + "\n" + "A soma total da inteligência de todos os personagens: " + personagens.Sum(p => p.Inteligencia));

        }
    }
}