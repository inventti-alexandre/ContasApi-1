using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TesteHub.Models;

namespace TesteHub.DAO
{
    public class ContaDAO: IDisposable
    {
        private SqlConnection conexao;

        public ContaDAO()
        {            
            this.conexao = new SqlConnection(DadosConexao.StringConexao());
            this.conexao.Open();
        }

        public void Dispose()
        {
            this.conexao.Close();
        }

        internal void Adicionar(Conta c)
        {
            try
            {
                var idNovo = BuscaProximo();
                
                var insertCmd = conexao.CreateCommand();
                insertCmd.CommandText = "INSERT INTO Contas (Id, Nome, DataCriacao, IdCtPai, Saldo, Situacao, IdPessoa, ContaMatriz) " +
                                        "VALUES (@id, @nome, @dataCriacao, @idCtPai, @saldo, @situacao, @idPessoa, @contaMatriz)";

                var paramId = new SqlParameter("id", idNovo);
                insertCmd.Parameters.Add(paramId);

                var paramNome = new SqlParameter("nome", c.Nome);
                insertCmd.Parameters.Add(paramNome);

                var paramDataCriacao = new SqlParameter("dataCriacao", DateTime.Parse(c.DataCriacao));
                insertCmd.Parameters.Add(paramDataCriacao);

                if (c.ContaMatriz == true)
                {
                    c.IdCtPai = idNovo;
                }

                var paramIdCtPai = new SqlParameter("idCtPai", c.IdCtPai);
                insertCmd.Parameters.Add(paramIdCtPai);


                var paramSaldo = new SqlParameter("saldo", c.Saldo);
                insertCmd.Parameters.Add(paramSaldo);

                var paramSituacao = new SqlParameter("situacao", c.Situacao);
                insertCmd.Parameters.Add(paramSituacao);

                var paramIdPessoa = new SqlParameter("idPessoa", c.IdPessoa);
                insertCmd.Parameters.Add(paramIdPessoa);

                var paramContaMatriz = new SqlParameter("contaMatriz", c.ContaMatriz);
                insertCmd.Parameters.Add(paramContaMatriz);

                insertCmd.ExecuteNonQuery();

                c.Id = idNovo;
                AtualizaArvore(c);
            }
            catch (SqlException e)
            {
                throw new SystemException(e.Message, e);
            }
        }

        internal int Atualizar(Conta c)
        {
            try
            {
                var updateCmd = conexao.CreateCommand();
                updateCmd.CommandText = "UPDATE Contas SET Nome = @nome, IdCtPai = @idCtPai, Situacao = @situacao, IdPessoa = @idPessoa, ContaMatriz = @contaMatriz WHERE Id = @id";

                var paramNome = new SqlParameter("nome", c.Nome);
                updateCmd.Parameters.Add(paramNome);

                var paramIdCtPai = new SqlParameter("idCtPai", c.IdCtPai);
                updateCmd.Parameters.Add(paramIdCtPai);

                var paramSituacao = new SqlParameter("situacao", c.Situacao);
                updateCmd.Parameters.Add(paramSituacao);

                var paramIdPessoa = new SqlParameter("idPessoa", c.IdPessoa);
                updateCmd.Parameters.Add(paramIdPessoa);

                var paramContaMatriz = new SqlParameter("contaMatriz", c.ContaMatriz);
                updateCmd.Parameters.Add(paramContaMatriz);

                var paramId = new SqlParameter("id", c.Id);
                updateCmd.Parameters.Add(paramId);

                var retorno = updateCmd.ExecuteNonQuery();

                AtualizaArvore(c);
                return retorno;

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
                deleteCmd.CommandText = "DELETE FROM Contas WHERE Id = @id";

                var paramId = new SqlParameter("id", id);
                deleteCmd.Parameters.Add(paramId);

                deleteCmd.ExecuteNonQuery();
                
            }
            catch (SqlException e)
            {
                throw new SystemException(e.Message, e);
            }
        }

        internal Conta ListarPorId(int id)
        {
            string strConsulta = "SELECT Id,Nome,DataCriacao,IdCtPai,Saldo,Situacao,IdPessoa,ContaMatriz " +
                            "FROM Contas Where Id = " + id;

            return RetornoLista(strConsulta).First();
        }

        internal IList<Conta> ListarTodas()
        {
            string strConsulta = "SELECT Id,Nome,DataCriacao,IdCtPai,Saldo,Situacao,IdPessoa,ContaMatriz " +
                            "FROM Contas ";
            return RetornoLista(strConsulta);

        }

        internal IList<Conta> ListarPorTipo(bool tipoMatriz)
        {
            var selectCmd = conexao.CreateCommand();
            string strConsulta = "SELECT Id, Nome, DataCriacao, IdCtPai, Saldo, Situacao, IdPessoa, ContaMatriz " +
                                "FROM Contas Where where ContaMatriz = " + Convert.ToByte(tipoMatriz);

            return RetornoLista(strConsulta);
        }

        private IList<Conta> RetornoLista(string str)
        {
            var lista = new List<Conta>();
            var selectCmd = conexao.CreateCommand();
            selectCmd.CommandText = str;

            var resultado = selectCmd.ExecuteReader();

            while (resultado.Read())
            {
                Conta c = new Conta();
                c.Id = Convert.ToInt32(resultado["Id"]);
                c.Nome = Convert.ToString(resultado["Nome"]);
                c.DataCriacao = Convert.ToString(resultado["DataCriacao"]);
                c.IdCtPai = Convert.ToInt32(resultado["IdCtPai"]);
                c.Saldo = Convert.ToDouble(resultado["Saldo"]);
                c.Situacao = Convert.ToString(resultado["Situacao"]);
                c.IdPessoa = Convert.ToInt32(resultado["IdPessoa"]);
                c.ContaMatriz = Convert.ToBoolean(resultado["ContaMatriz"]);
                lista.Add(c);
            }
            resultado.Close();
            return lista;
        }


        private void AtualizaArvore(Conta c)
        {
            var arvore = new List<ArvoreContas>();

            var deleteCmd = conexao.CreateCommand();
            deleteCmd.CommandText = "Delete from ArvoreConta where IdOrigem = @idDel or IdPaiOrigem  = @idDel or  IdDestino = @idDel or IdPaiDestino = @idDel ";
            var paramIdDel = new SqlParameter("idDel", c.Id);
            deleteCmd.Parameters.Add(paramIdDel);

            deleteCmd.ExecuteNonQuery();

            if (c.Situacao == "Ativa")
            {

                var selectCmd = conexao.CreateCommand();

                if (c.ContaMatriz == false)
                {
                    selectCmd.CommandText = "SELECT distinct IdOrigem, IdPaiOrigem  FROM ArvoreConta Where IdPaiOrigem = @idPai OR IdPaiDestino = @idPai ";

                    var paramIdPai = new SqlParameter("idPai", c.IdCtPai);
                    selectCmd.Parameters.Add(paramIdPai);

                    var resultado = selectCmd.ExecuteReader();
                    int idPaiPai = 0;
                    bool noNovo = false;
                    if(!resultado.HasRows)
                    {
                        resultado.Close();
                        selectCmd.CommandText = "SELECT distinct IdOrigem, IdPaiOrigem  FROM ArvoreConta Where idDestino = @idPai  and IdOrigem = IdPaiDestino ";
                        resultado = selectCmd.ExecuteReader();
                        noNovo = true;
                    }
                    
                    while (resultado.Read())
                    {
                        var a = new ArvoreContas();
                        a.IdOrigem = Convert.ToInt32(resultado["IdOrigem"]);
                        a.IdPaiOrigem = Convert.ToInt32(resultado["IdPaiOrigem"]);
                        //a.IdDestino = Convert.ToInt32(resultado["IdDestino"]);
                        //a.IdPaiDestino = Convert.ToInt32(resultado["IdPaiDestino"]);
                        idPaiPai = a.IdOrigem;
                        arvore.Add(a);
                    }
                    resultado.Close();
                    selectCmd.CommandText = "";

                    if (noNovo)
                    {
                        selectCmd.CommandText += "insert into ArvoreConta(IdOrigem, IdPaiOrigem, IdDestino, IdPaiDestino) values (" + c.IdCtPai + ", " + idPaiPai + ", " + c.Id + ", " + c.IdCtPai + ")  ";
                        selectCmd.CommandText += "insert into ArvoreConta(IdOrigem, IdPaiOrigem, IdDestino, IdPaiDestino) values (" + c.Id + ", " + c.IdCtPai + ", " + c.IdCtPai + ", " + idPaiPai + ")  ";

                    }

                    foreach (var a in arvore)
                    {
                        selectCmd.CommandText += "insert into ArvoreConta(IdOrigem, IdPaiOrigem, IdDestino, IdPaiDestino) values (" + a.IdOrigem + ", " + a.IdPaiOrigem + ", " + c.Id + ", " + c.IdCtPai + ")  ";
                        selectCmd.CommandText += "insert into ArvoreConta(IdOrigem, IdPaiOrigem, IdDestino, IdPaiDestino) values (" + c.Id + ", " + c.IdCtPai + ", " + a.IdOrigem + ", " + a.IdPaiOrigem + ")  ";
                    }

                }else
                {
                    selectCmd.CommandText = "insert into ArvoreConta(IdOrigem, IdPaiOrigem, IdDestino, IdPaiDestino) values (@idMat,@idMat,@idMat,@idMat)";
                    var paramIdMatriz = new SqlParameter("idMat", c.Id);
                    selectCmd.Parameters.Add(paramIdMatriz);
                }
                selectCmd.ExecuteNonQuery();
            }

        }

        private int BuscaProximo()
        {
           var selectCmd = conexao.CreateCommand();
            selectCmd.CommandText = "select max(id) + 1 as Proximo  from contas "; ;

            int result = 1;

            var resultado = selectCmd.ExecuteReader();
            while (resultado.Read())
            {
                if(resultado["Proximo"] != DBNull.Value)
                    result = Convert.ToInt32(resultado["Proximo"]);
            }
            resultado.Close();

            return result;

        }


    }
}