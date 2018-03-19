using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TesteHub.Models
{
    public abstract class Transacao
    {
        [Required]
        public int IdContaDestino { get; set; }
        [Required]
        public double Valor { get; set; }
        [Required]
        public string DataTransacao { get; set; }
        public bool Estornada { get; set; }

        public string ValidaCampoDt()
        {
            string retorno = "";
            if(!ValidaData.IsDate(this.DataTransacao))
            {
                retorno = "Campo 'DataTransacao' não é uma data valida !";
            }
            return retorno;
        }

    }
}