using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TesteHub.Models
{
    public class PessoaFisica : Pessoa
    {
        public PessoaFisica(){}

        public PessoaFisica(Pessoa p)
        {
            this.Id = p.Id;
            this.Nome = p.Nome;
            this.Cpf_Cnpj = p.Cpf_Cnpj;
            this.NomeFantasia = "";
            this.DataNascimento = p.DataNascimento;
            this.TipoPessoa = p.TipoPessoa;
        }

        public override string ValidaCampos()
        {
            string retorno = "";
            retorno = base.ValidaCampos();

            if (!ValidaDocumento.IsCpf(this.Cpf_Cnpj))
            {
                retorno += "Campo 'Cpf_Cnpj' não é um CPF válido para Pessoa Fisica";
            }

            if(!ValidaData.IsDate(this.DataNascimento))
            {
                retorno += "Campo 'DataNascimento' não é uma data valida! " + this.DataNascimento + " cc " ;
            }
            return retorno;
        }
    }
}