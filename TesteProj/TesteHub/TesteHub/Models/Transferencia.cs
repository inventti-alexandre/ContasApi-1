using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TesteHub.Models
{
    public class Transferencia: Transacao
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public int IdContaOrigem { get; set; }

        public Transferencia() { }
    }
}