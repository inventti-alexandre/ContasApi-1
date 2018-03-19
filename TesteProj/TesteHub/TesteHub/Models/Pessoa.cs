using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TesteHub.DAO;

namespace TesteHub.Models
{
    public  class Pessoa
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(150)]
        public string Nome { get; set; }
        [Required]
        [MaxLength(18)]
        public string Cpf_Cnpj { get; set; }
        public string DataNascimento { get; set; }
        [MaxLength(150)]
        public string NomeFantasia { get; set; }
        [Required]
        [MaxLength(2)]
        public string TipoPessoa { get; set; }

        public Pessoa(){}

        public virtual string ValidaCampos()
        {
            string retorno = "";

            if(this.Id < 0)
            {
                retorno = "Campo 'Id' não pode ser menor que 1! /n";
            }

            if(this.Nome == "")
            {
                retorno += "Campo 'Nome' não informado! /n";
            }

            if (this.TipoPessoa != "PF" && this.TipoPessoa != "PJ")
            {
                retorno += "Campo 'TipoPessoa' Inválido! /n";
            }

            return retorno;
        }

    }


}