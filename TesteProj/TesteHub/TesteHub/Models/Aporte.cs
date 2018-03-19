using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TesteHub.Models
{
    public class Aporte: Transacao
    {
        [Required]
        public string IdAlpha { get; set; }
        public Aporte() { }
    }
}