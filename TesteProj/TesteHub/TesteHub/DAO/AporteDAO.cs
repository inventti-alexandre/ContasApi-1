using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using TesteHub.Models;

namespace TesteHub.DAO
{
    public class AporteDAO
    {
        private SqlConnection conexao;

        public AporteDAO()
        {
            this.conexao = new SqlConnection(DadosConexao.StringConexao());
            this.conexao.Open();
        }

        public void Dispose()
        {
            this.conexao.Close();
        }

        internal void Adicionar(Aporte a)
        {
            string strTransferencia;
            string strDestino;
            string strComando;
            try
            {
            strTransferencia = "INSERT INTO Aportes (IdAlpha, IdContaDestino, Valor, DataTransacao, Estornada) " +
                                    "VALUES (@idAlpha, @idContaDestino, @valor, @dataTransacao, 0)";

            strDestino = "UPDATE Contas SET Saldo = Saldo + @valor WHERE Id = @IdContaDestino";

            strComando = String.Format("{0} {1}  ", strTransferencia, strDestino);

            var insertCmd = conexao.CreateCommand();
            insertCmd.CommandText = strComando;

            var paramIdAlpha = new SqlParameter("idAlpha", a.IdAlpha);
            insertCmd.Parameters.Add(paramIdAlpha);

            var paramIdContaDestino = new SqlParameter("idContaDestino", a.IdContaDestino);
            insertCmd.Parameters.Add(paramIdContaDestino);

            var paramValor = new SqlParameter("valor", a.Valor);
            insertCmd.Parameters.Add(paramValor);

            var paramDataTransacao = new SqlParameter("dataTransacao", a.DataTransacao);
            insertCmd.Parameters.Add(paramDataTransacao);

            insertCmd.ExecuteNonQuery();
            }
            catch (SqlException e)
            {
                throw new SystemException(e.Message, e);
            }
        }



        internal int Atualizar(Aporte a)
        {
            string strAporte;
            string strDestino;
            string strComando;

            try
            {

                strAporte = "update Aportes set Estornada = 1 where IdAlpha =  @iAlpha ";
                strDestino = "UPDATE Contas SET Saldo = Saldo - @valor WHERE Id = @IdContaDestino";

                strComando = String.Format("{0}  {1} ", strAporte, strDestino);

                var insertCmd = conexao.CreateCommand();
                insertCmd.CommandText = strComando;

                var paramIdAlpha = new SqlParameter("idAlpha", a.IdAlpha);
                insertCmd.Parameters.Add(paramIdAlpha);

                var paramIdContaDestino = new SqlParameter("idContaDestino", a.IdContaDestino);
                insertCmd.Parameters.Add(paramIdContaDestino);

                var paramValor = new SqlParameter("valor", a.Valor);
                insertCmd.Parameters.Add(paramValor);

                var i = insertCmd.ExecuteNonQuery();
                return i;
            }
            catch (SqlException e)
            {
                throw new SystemException(e.Message, e);
            }
        }

        internal void Remover(Aporte a)
        {
            //Implementado apenas por padrão, pois o ideal é não excluir uma transação
            if (this.Atualizar(a) > 0)
            {
                try
                {
                    var deleteCmd = conexao.CreateCommand();
                    deleteCmd.CommandText = "DELETE FROM Aportes WHERE IdAlpha = @idAlpha";

                    var paramId = new SqlParameter("idAlpha", a.IdAlpha);
                    deleteCmd.Parameters.Add(paramId);

                    deleteCmd.ExecuteNonQuery();

                }
                catch (SqlException e)
                {
                    throw new SystemException(e.Message, e);
                }
            }
        }

        internal Aporte ListarPorId(string idAlpha)
        {
            var a = new Aporte();

            var selectCmd = conexao.CreateCommand();
            selectCmd.CommandText = "SELECT IdAlpha, IdContaDestino, Valor, DataTransacao, Estornada FROM Aportes Where IdAlpha = @idAlpha ";

            var paramId = new SqlParameter("idAlpha", idAlpha);
            selectCmd.Parameters.Add(paramId);

            var resultado = selectCmd.ExecuteReader();
            while (resultado.Read())
            {

                a.IdAlpha = Convert.ToString(resultado["IdAlpha"]);
                a.IdContaDestino = Convert.ToInt32(resultado["IdContaDestino"]);
                a.Valor = Convert.ToDouble(resultado["Valor"]);
                a.DataTransacao = Convert.ToString(resultado["DataTransacao"]);
                a.Estornada = Convert.ToBoolean(resultado["Estornada"]);

            }
            resultado.Close();
            return a;

        }

        internal IList<Aporte> ListarTodas()
        {
            var strComando = "SELECT IdAlpha, IdContaDestino, Valor, DataTransacao, Estornada FROM Aportes";
            return RetornaLista(strComando);

        }

        internal IList<Aporte> ListarPorConta(int IdContaDestino)
        {

            var strComando = "SELECT IdAlpha, IdContaDestino, Valor, DataTransacao, Estornada FROM Aportes where IdContaDestino = " + IdContaDestino;
            return RetornaLista(strComando);
        }

        private IList<Aporte> RetornaLista(string strComando)
        {
            var lista = new List<Aporte>();

            var selectCmd = conexao.CreateCommand();
            selectCmd.CommandText = strComando;

            var resultado = selectCmd.ExecuteReader();
            while (resultado.Read())
            {
                var a = new Aporte();

                a.IdAlpha = Convert.ToString(resultado["IdAlpha"]);
                a.IdContaDestino = Convert.ToInt32(resultado["IdContaDestino"]);
                a.Valor = Convert.ToDouble(resultado["Valor"]);
                a.DataTransacao = Convert.ToString(resultado["DataTransacao"]);
                a.Estornada = Convert.ToBoolean(resultado["Estornada"]);

                lista.Add(a);
            }
            resultado.Close();

            return lista;
        }

        internal string Valida(Aporte aporte)
        {
            string retorno;

            var selectCmd = conexao.CreateCommand();
            selectCmd.CommandText = "SELECT Situacao, ContaMatriz FROM Contas Where Id = @idContaDestino ";

            var paramId = new SqlParameter("idContaDestino", aporte.IdContaDestino);
            selectCmd.Parameters.Add(paramId);

            retorno = "";

            var resultado = selectCmd.ExecuteReader();
            while (resultado.Read())
            {
                if(Convert.ToString(resultado["Situacao"]) != "Ativa")
                {
                    retorno = "Conta esta em situação " + Convert.ToString(resultado["Situacao"]);
                }

                if(!Convert.ToBoolean(resultado["ContaMatriz"]))
                {
                    retorno += "Conta Selecionada não é Matriz" ;
                }
            }
            resultado.Close();
            return retorno;

        }
    }
}