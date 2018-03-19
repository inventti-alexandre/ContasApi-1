using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TesteHub.Models;

namespace TesteHub.DAO
{
    public class TransferenciaDAO
    {
        private SqlConnection conexao;

        public TransferenciaDAO()
        {
            this.conexao = new SqlConnection(DadosConexao.StringConexao());
            this.conexao.Open();
        }

        public void Dispose()
        {
            this.conexao.Close();
        }

        internal void Adicionar(Transferencia t)
        {

            string strTransferencia;
            string strDestino;
            string strOrigem;
            string strComando;
            try
            {

            strTransferencia = " INSERT INTO Transferencias (IdContaOrigem, IdContaDestino, Valor, DataTransacao,Estornada) " +
                                    "VALUES (@idContaOrigem, @idContaDestino, @valor, @dataTransacao, 0) ";

            strDestino = " UPDATE Contas SET Saldo = Saldo + @valor WHERE Id = @IdContaDestino ";

            strOrigem =  " UPDATE Contas SET Saldo = Saldo - @valor WHERE Id = @idContaOrigem ";

            strComando = String.Format("{0}  {1}  {2} ",strTransferencia,strDestino,strOrigem) ;

            var insertCmd = conexao.CreateCommand();
            insertCmd.CommandText = strComando;

            var paramidContaOrigem = new SqlParameter("idContaOrigem", t.IdContaOrigem);
            insertCmd.Parameters.Add(paramidContaOrigem);

            var paramIdContaDestino = new SqlParameter("idContaDestino", t.IdContaDestino);
            insertCmd.Parameters.Add(paramIdContaDestino);

            var paramValor = new SqlParameter("valor", t.Valor);
            insertCmd.Parameters.Add(paramValor);

            var paramDataTransacao = new SqlParameter("dataTransacao", t.DataTransacao);
            insertCmd.Parameters.Add(paramDataTransacao);

            insertCmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                throw new SystemException(e.Message, e);
            }
        }

        internal int Atualizar(Transferencia t)
        {

            string strTransferencia;
            string strDestino;
            string strOrigem;
            string strComando;

            try
            {
            strTransferencia = " update Transferencias set Estornada = @estorno where Id = @id ";
            strDestino = " UPDATE Contas SET Saldo = Saldo - @valor WHERE Id = @IdContaDestino ";
            strOrigem = " UPDATE Contas SET Saldo = Saldo + @valor WHERE Id = @idContaOrigem ";

            strComando = String.Format("{0}  {1}  {2} ", strTransferencia, strDestino, strOrigem);

            var insertCmd = conexao.CreateCommand();
            insertCmd.CommandText = strComando;

            var paramId = new SqlParameter("id", t.Id);
            insertCmd.Parameters.Add(paramId);

            var paramIdContaOrigem = new SqlParameter("idContaOrigem", t.IdContaOrigem);
            insertCmd.Parameters.Add(paramIdContaOrigem);

            var paramIdContaDestino = new SqlParameter("idContaDestino", t.IdContaDestino);
            insertCmd.Parameters.Add(paramIdContaDestino);

            var paramEstorno = new SqlParameter("estorno", t.Estornada);
            insertCmd.Parameters.Add(paramEstorno);

            var paramValor = new SqlParameter("valor", t.Valor);
            insertCmd.Parameters.Add(paramValor);

                var i =  insertCmd.ExecuteNonQuery();
                return i;
            }
            catch (SqlException e)
            {
                throw new SystemException(e.Message, e);
            }
        }

        internal void Remover(Transferencia t)
        {
            //Implementado apenas por padrão, pois o ideal é não excluir uma transação
            if (this.Atualizar(t) > 0)
            { 
                try
                {
                    var deleteCmd = conexao.CreateCommand();
                    deleteCmd.CommandText = "DELETE FROM Contas WHERE Id = @id";

                    var paramId = new SqlParameter("id", t.Id);
                    deleteCmd.Parameters.Add(paramId);

                    deleteCmd.ExecuteNonQuery();

                }
                catch (SqlException e)
                {
                    throw new SystemException(e.Message, e);
                }
            }
        }

        internal Transferencia ListarPorId(int id)
        {
            var t = new Transferencia();

            var selectCmd = conexao.CreateCommand();
            selectCmd.CommandText = "SELECT Id, IdContaOrigem, IdContaDestino, Valor, DataTransacao,Estornada FROM Transferencias Where Id = " + id;

            var resultado = selectCmd.ExecuteReader();
            while (resultado.Read())
            {

                t.Id = Convert.ToInt32(resultado["Id"]);
                t.IdContaOrigem = Convert.ToInt32(resultado["IdContaOrigem"]);
                t.IdContaDestino = Convert.ToInt32(resultado["IdContaDestino"]);
                t.Valor = Convert.ToDouble(resultado["Valor"]);
                t.DataTransacao = Convert.ToString(resultado["DataTransacao"]);
                t.Estornada = Convert.ToBoolean(resultado["Estornada"]);

            }
            resultado.Close();
            return t;

        }

        internal IList<Transferencia> ListarTodas()
        {
            var strComando = "SELECT Id, IdContaOrigem, IdContaDestino, Valor, DataTransacao,Estornada FROM Transferencias";
            return RetornaLista(strComando);

        }

        internal IList<Transferencia> ListarPorConta(int id, string campo)
        {

            var strComando = "SELECT Id, IdContaOrigem, IdContaDestino, Valor, DataTransacao,Estornada FROM Transferencias where ";
            strComando += campo + " = " + id;
            return RetornaLista(strComando);
        }

        private IList<Transferencia> RetornaLista(string strComando)
        {
            var lista = new List<Transferencia>();

            var selectCmd = conexao.CreateCommand();
            selectCmd.CommandText = strComando;

            var resultado = selectCmd.ExecuteReader();
            while (resultado.Read())
            {
                var t = new Transferencia();

                t.Id = Convert.ToInt32(resultado["Id"]);
                t.IdContaOrigem = Convert.ToInt32(resultado["IdContaOrigem"]);
                t.IdContaDestino = Convert.ToInt32(resultado["IdContaDestino"]);
                t.Valor = Convert.ToDouble(resultado["Valor"]);
                t.DataTransacao = Convert.ToString(resultado["DataTransacao"]);
                t.Estornada = Convert.ToBoolean(resultado["Estornada"]);

                lista.Add(t);
            }
            resultado.Close();

            return lista;
        }
        internal string Valida(Transferencia t)
        {
            string retorno = "";

            var selectCmd = conexao.CreateCommand();
            selectCmd.CommandText = "SELECT 'x' FROM ArvoreConta Where IdOrigem = @idOrigem  and IdDestino = @idDestino and IdOrigem <> IdPaiOrigem ";

            var paramIdDestino = new SqlParameter("idDestino", t.IdContaDestino);
            selectCmd.Parameters.Add(paramIdDestino);

            var paramIdContaOrigem = new SqlParameter("idOrigem", t.IdContaOrigem);
            selectCmd.Parameters.Add(paramIdContaOrigem);

            var resultado = selectCmd.ExecuteReader();
            if(!resultado.HasRows)
            {
                retorno = "Não é possivel fazer transferência entre as contas! ";
            }
            resultado.Close();
            return retorno;
        }

    }
}