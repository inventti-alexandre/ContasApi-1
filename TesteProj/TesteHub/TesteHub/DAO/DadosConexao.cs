using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TesteHub.DAO
{
    public static class DadosConexao
    {

        public static string StringConexao()
        {
            string servidor = "local\\SQLEXPRESS"; //Caminho do servidor do Banco de dados
            string database = "ControleCtDB"; //Nome da Base, não precisa alterar pois já esta com esse nome no script de criação
            string usuario = "sa";  //Usuario da basde de dados
            string senha = ""; // Senha
            
            return string.Format("Server={0};Database={1};User Id={2};Password={3}", servidor, database, usuario, senha);
        }
    }

}