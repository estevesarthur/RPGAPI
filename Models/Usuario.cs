using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace RpgApi.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        [NotMapped]
        public string PasswordString { get; set; }
        public byte[] Foto { get; set; }
        public string Perfil { get; set; }
        public List<Personagem> Personagens { get; set; }
    }
}