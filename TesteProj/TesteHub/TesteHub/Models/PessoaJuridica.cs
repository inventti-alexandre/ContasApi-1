using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TesteHub.Models
{
    public class PessoaJuridica: Pessoa
    {
        public PessoaJuridica() { }

        public PessoaJuridica(Pessoa p)
        {
            this.Id = p.Id;
            this.Nome = p.Nome;
            this.Cpf_Cnpj = p.Cpf_Cnpj;
            this.NomeFantasia = p.NomeFantasia;
            this.DataNascimento = "";
            this.TipoPessoa = p.TipoPessoa;            
        }
        public override string ValidaCampos()
        {
            string retorno = "";
            retorno = base.ValidaCampos();

            if (!ValidaDocumento.IsCnpj(this.Cpf_Cnpj))
            {
                retorno += "Campo 'Cpf_Cnpj' não é um CNPJ válido para Pessoa Juridia";
            }
            return retorno;
        }
    }
}