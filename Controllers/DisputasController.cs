using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RpgApi.Data;
using RpgApi.Models;

namespace RpgApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DisputasController : ControllerBase
    {
        private readonly DataContext _context;
        public DisputasController(DataContext context)
        {
            _context = context;
        }

        [HttpPost("Arma")] //ATAQUE COM ARMA
        public async Task<IActionResult> AtaqueComArma(Disputa d)
        {
            Personagem atacante = await _context.Personagens
                .Include(p => p.Arma)
                .FirstOrDefaultAsync(p => p.Id == d.AtacanteId);

            Personagem oponente = await _context.Personagens
                .FirstOrDefaultAsync(p => p.Id == d.OponenteId);

            int dano = atacante.Arma.Dano + (new Random().Next(atacante.Forca));
            dano = dano - new Random().Next(oponente.Defesa);

            if (dano > 0)
                oponente.PontosVida = oponente.PontosVida - (int)dano;
            if (oponente.PontosVida <= 0)
                d.Narracao = $"{oponente.Nome} foi derrotado!";

            _context.Personagens.Update(oponente);
            await _context.SaveChangesAsync();

            StringBuilder dados = new StringBuilder(); //classe para concatenar
            dados.AppendFormat("   Atacante: {0}", atacante.Nome); //dentro do zero, aparecera o nome
            dados.AppendFormat("   Oponente: {0}", oponente.Nome);
            dados.AppendFormat("   Pontos de vida do atacante: {0}", atacante.PontosVida);
            dados.AppendFormat("   Pontos de vida do oponente: {0}", oponente.PontosVida);
            dados.AppendFormat("   Dano: {0}", dano);

            d.Narracao += dados.ToString(); //pegar toda essa frase acima dentro do banco

            _context.Disputas.Add(d);
            _context.SaveChanges();

            return Ok(d);

            /*Lógica do ataque com Arma - Exemplo: 
            Dano do ataque = (30) + 
            Força do ataque Sorteado para personagem de Força máxima 40 = (20)  
            Dano do ataque = (30) + (20) = 50

            Defesa do oponente = (60)
            Valor sorteado da defesa do oponente (30)
            Dano do ataque (50) - Defesa do oponente (30) --> 50 - 30 = 20
            20 - Valor que o oponente vai perder em pontos de vida
            100 (se o oponente tiver 100 no total) --> 100 - 20 = 80*/
        }

        [HttpPost("Habilidade")] //ATAQUE COM HABILIDADES
        public async Task<IActionResult> AtaqueComHabilidade(Disputa d)
        {
            Personagem atacante = await _context.Personagens
                .Include(p => p.PersonagemHabilidades).ThenInclude(ph => ph.Habilidade) //habilidade pelo personagem -> atacante
                .FirstOrDefaultAsync(p => p.Id == d.AtacanteId);

            Personagem oponente = await _context.Personagens  //personagem qualquer   
                .FirstOrDefaultAsync(p => p.Id == d.OponenteId);

            PersonagemHabilidade ph = await _context.PersonagemHabilidades //buscar dados da PH atraves da id da habilidade passado no post
                .Include(p => p.Habilidade)
                .FirstOrDefaultAsync(phBusca => phBusca.HabilidadeId == d.HabilidadeId);

            if (ph == null)
                d.Narracao = $"{atacante.Nome} não possui esta habilidade.";
            else
            {
                int dano = ph.Habilidade.Dano + (new Random().Next(atacante.Inteligencia));
                dano = dano - new Random().Next(oponente.Defesa);

                if (dano > 0)
                    oponente.PontosVida = oponente.PontosVida - (int)dano;
                if (oponente.PontosVida <= 0)
                    d.Narracao += $"{oponente.Nome} foi derrotado!";

                _context.Personagens.Update(oponente);
                await _context.SaveChangesAsync();

                StringBuilder dados = new StringBuilder(); //classe para concatenar
                dados.AppendFormat("   Atacante: {0}", atacante.Nome); //dentro do zero, aparecera o nome
                dados.AppendFormat("   Oponente: {0}", oponente.Nome);
                dados.AppendFormat("   Pontos de vida do atacante: {0}", atacante.PontosVida);
                dados.AppendFormat("   Pontos de vida do oponente: {0}", oponente.PontosVida);
                dados.AppendFormat("   Dano: {0}", dano);

                d.Narracao += dados.ToString(); //pegar toda essa frase acima dentro do banco

                _context.Disputas.Add(d);
                _context.SaveChanges();
            }

            return Ok(d);

        }

        [HttpGet("PersonagemRandom")]
        public async Task<IActionResult> Sorteio()
        {
            List<Personagem> personagens = await _context.Personagens.ToListAsync();

            //sorteio com a qnt de personagem
            int sorteio = new Random().Next(personagens.Count);

            //buscar na lista pelo indice sorteado(nao é o ID)
            Personagem p = personagens[sorteio];

            string msg = string.Format("Nº SORTEADO {0}. PERSONAGEM: {1}", sorteio, p.Nome);

            return Ok(msg);
        }

        [HttpPost("DisputaEmGrupo")]
        public async Task<IActionResult> DisputaEmGrupo(Disputa d)
        {
            //Toda codificação das etapas das etapas seguintes sequencialmente aqui
            //Antes do return
            List<Personagem> personagens =
                await _context.Personagens
                .Include(p => p.Arma)
                .Include(p => p.PersonagemHabilidades).ThenInclude(ph => ph.Habilidade)
                .Where(p => d.ListaIdPersonagens.Contains(p.Id)).ToListAsync();

            bool derrotado = false;
            while (!derrotado)
            {
                foreach (Personagem atacante in personagens)
                {
                    List<Personagem> oponentes = personagens.Where(p => p.Id != atacante.Id).ToList();
                    Personagem oponente = oponentes[new Random().Next(oponentes.Count)];

                    int dano = 0;
                    string ataqueUsado = string.Empty;
                    bool ataqueUsaArma = new Random().Next(2) == 0;

                    if (ataqueUsaArma)
                    {
                        dano = atacante.Arma.Dano + (new Random().Next(atacante.Forca));
                        dano = dano - new Random().Next(oponente.Defesa);
                        ataqueUsado = atacante.Arma.Nome;

                        if (dano > 0)
                            oponente.PontosVida = oponente.PontosVida - (int)dano;
                    }
                    else
                    {
                        int sorteioHabilidadeId = new Random().Next(atacante.PersonagemHabilidades.Count);
                        Habilidade habilidadeEscolhida = atacante.PersonagemHabilidades[sorteioHabilidadeId].Habilidade;
                        ataqueUsado = habilidadeEscolhida.Nome;

                        dano = habilidadeEscolhida.Dano + (new Random().Next(atacante.Inteligencia));
                        dano = dano - new Random().Next(oponente.Defesa);

                        if (dano > 0)
                            oponente.PontosVida = oponente.PontosVida - (int)dano;
                    }

                    string resultado = string.Format("{0} atacou {1} usando {2} como dano {3}.", atacante.Nome, oponente.Nome, ataqueUsado, dano);
                    d.Resultados.Add(resultado);

                    if (oponente.PontosVida <= 0)
                    {
                        derrotado = true;
                        atacante.Vitorias++;
                        oponente.Derrotas++;
                        d.Resultados.Add($"{oponente.Nome} foi derrotado!");
                        d.Resultados.Add($"{atacante.Nome} ganhou com {atacante.PontosVida} restantes!");
                        break;
                    }
                }

                personagens.ForEach(p =>
                {
                    p.Disputas++;
                    p.PontosVida = 100;
                });

                _context.Personagens.UpdateRange(personagens);
                await _context.SaveChangesAsync();
            }
            return Ok(d);
        }

    }
}