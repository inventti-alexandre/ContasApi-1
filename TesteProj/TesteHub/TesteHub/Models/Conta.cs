using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using TesteHub.DAO;

namespace TesteHub.Models
{
    public class Conta
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(150, MinimumLength = 1, ErrorMessage = "Campo Nome não pode ser maior que 150 caracter ou menor que 1 caracter!")]
        public string Nome { get; set; }
        public string DataCriacao { get; set; }
        [Required]
        public double Saldo { get; set; }
        [Required]
        public string Situacao { get; set; }
        [Required]
        public int IdCtPai { get; set; }
        [Required]
        public bool ContaMatriz { get; set; }
        [Required]
        public int IdPessoa { get; set; }
        //public Pessoa PessoaConta { get; set; }

        public Conta()
        {
            //PessoaConta = new Pessoa();
        }
    }

}