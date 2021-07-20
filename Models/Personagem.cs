using System.Collections.Generic;
using System.Text.Json.Serialization;
using RpgApi.Models.Enuns;

namespace RpgApi.Models
{
    public class Personagem
    {
        public int Id { get; set; }
        public string Nome { get; set; } = "Frodo";
        public int PontosVida { get; set; } = 100;
        public int Forca { get; set; } = 10;
        public int Defesa { get; set; } = 10;
        public int Inteligencia { get; set; } = 10;
        public ClasseEnum Classe { get; set; } = ClasseEnum.Cavaleiro;
        public byte[] FotoPersonagem { get; set; }
        public int Disputas { get; set; }
        public int Vitorias { get; set; }
        public int Derrotas { get; set; }

        [JsonIgnore]   //serve para nao ocorrer o looping entre arma e personagem
        public Usuario Usuario { get; set; }

        [JsonIgnore]
        public Arma Arma { get; set; }

        public List<PersonagemHabilidade> PersonagemHabilidades { get; set; }

    }

}