using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TesteHub.Models;

namespace TesteHub.DAO
{
    public class PessoaDAO
    {

        private SqlConnection conexao;

        public PessoaDAO()
        {
            this.conexao = new SqlConnection(DadosConexao.StringConexao());
            this.conexao.Open();
        }

        public void Dispose()
        {
            this.conexao.Close();
        }

        internal void Adicionar(Pessoa p)
        {
            try
            {
                var insertCmd = conexao.CreateCommand();

                insertCmd.CommandText = "INSERT INTO Pessoas (Nome, Cpf_Cnpj, NomeFantasia, DataNascimento, TipoPessoa) " +
                                        "VALUES (@nome, @cpf_Cnpj, @nomeFantasia, @dataNascimento,  @tipoPessoa)";

                var paramSituacao = new SqlParameter("dataNascimento", p.DataNascimento);
                insertCmd.Parameters.Add(paramSituacao);

                var paramDataCriacao = new SqlParameter("nome", p.Nome);
                insertCmd.Parameters.Add(paramDataCriacao);

                var paramIdCtPai = new SqlParameter("cpf_Cnpj", p.Cpf_Cnpj);
                insertCmd.Parameters.Add(paramIdCtPai);

                var paramSaldo = new SqlParameter("nomeFantasia", p.NomeFantasia);
                insertCmd.Parameters.Add(paramSaldo);           

                var paramIdPessoa = new SqlParameter("tipoPessoa", p.TipoPessoa);
                insertCmd.Parameters.Add(paramIdPessoa);

                insertCmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                throw new SystemException(e.Message, e);
            }
        }

        internal int Atualizar(Pessoa p)
        {
            try
            {
                var updateCmd = conexao.CreateCommand();
                updateCmd.CommandText = "UPDATE Pessoas SET Nome = @nome, Cpf_Cnpj = @cpf_Cnpj, NomeFantasia = @nomeFantasia, DataNascimento = @dataNascimento,  TipoPessoa = @tipoPessoa WHERE Id = @id";

                var paramId = new SqlParameter("id", p.Id);
                updateCmd.Parameters.Add(paramId);

                var paramNome = new SqlParameter("nome", p.Nome);
                updateCmd.Parameters.Add(paramNome);

                var paramCpfCnpj = new SqlParameter("cpf_Cnpj", p.Cpf_Cnpj);
                updateCmd.Parameters.Add(paramCpfCnpj);

                var paramNomeFantasia = new SqlParameter("nomeFantasia", p.NomeFantasia);
                updateCmd.Parameters.Add(paramNomeFantasia);

                var paramDataNascimento = new SqlParameter("dataNascimento", p.DataNascimento);
                updateCmd.Parameters.Add(paramDataNascimento);
                
                var paramTipoPessoa = new SqlParameter("tipoPessoa", p.TipoPessoa);
                updateCmd.Parameters.Add(paramTipoPessoa);

                return updateCmd.ExecuteNonQuery();

            }
            catch (SqlException e)
            {
                throw new SystemException(e.Message, e);
            }
        }

        internal void Remover(int id)
        {
            try
            {
                var deleteCmd = conexao.CreateCommand();
                deleteCmd.CommandText = "DELETE FROM Pessoas WHERE Id = @id";

                var paramId = new SqlParameter("id", id);
                deleteCmd.Parameters.Add(paramId);

                deleteCmd.ExecuteNonQuery();

            }
            catch (SqlException e)
            {
                throw new SystemException(e.Message, e);
            }
        }

        internal Pessoa ListarPorId(int id)
        {
            var p = new Pessoa();

            var selectCmd = conexao.CreateCommand();
            selectCmd.CommandText = "SELECT Id, Nome, Cpf_Cnpj, NomeFantasia, DataNascimento, TipoPessoa FROM Pessoas Where Id = " + id;

            var resultado = selectCmd.ExecuteReader();
            while (resultado.Read())
            {

                p.Id = Convert.ToInt32(resultado["Id"]);
                p.Nome = Convert.ToString(resultado["Nome"]);
                p.Cpf_Cnpj = Convert.ToString(resultado["Cpf_Cnpj"]);
                p.NomeFantasia = Convert.ToString(resultado["NomeFantasia"]);
                p.DataNascimento = Convert.ToString(resultado["DataNascimento"]);
                p.TipoPessoa = Convert.ToString(resultado["TipoPessoa"]);

            }
            resultado.Close();
            return p;

        }

        internal IList<Pessoa> ListarTodas()
        {
            var lista = new List<Pessoa>();

            var selectCmd = conexao.CreateCommand();
            selectCmd.CommandText = "SELECT Id, Nome, Cpf_Cnpj, NomeFantasia, DataNascimento, TipoPessoa FROM Pessoas";
            

            var resultado = selectCmd.ExecuteReader();
            while (resultado.Read())
            {
                Pessoa p = new Pessoa();
                p.Id = Convert.ToInt32(resultado["Id"]);
                p.Nome = Convert.ToString(resultado["Nome"]);
                p.Cpf_Cnpj = Convert.ToString(resultado["Cpf_Cnpj"]);
                p.NomeFantasia = Convert.ToString(resultado["NomeFantasia"]);
                p.DataNascimento = Convert.ToString(resultado["DataNascimento"]);
                p.TipoPessoa = Convert.ToString(resultado["TipoPessoa"]);

                lista.Add(p);
            }
            resultado.Close();

            return lista;

        }
    }
}