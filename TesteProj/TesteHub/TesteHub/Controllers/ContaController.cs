using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TesteHub.DAO;
using TesteHub.Models;

namespace TesteHub.Controllers
{
    public class ContaController : ApiController
    {

        public HttpResponseMessage Get(int id)
        {
            try
            {
                ContaDAO dao = new ContaDAO();
                Conta contas = dao.ListarPorId(id);
                return Request.CreateResponse(HttpStatusCode.OK, contas);
            }
            catch (KeyNotFoundException)
            {
                string mensagem = string.Format("Conta não encontrada!");
                HttpError erro = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.NotFound, erro);
            }

        }

        [Route("api/Conta/All") ]
        public HttpResponseMessage GetAll()
        {
            try
            {
                ContaDAO dao = new ContaDAO();
                IList<Conta> contas = dao.ListarTodas();
                return Request.CreateResponse(HttpStatusCode.OK, contas);
            }
            catch (KeyNotFoundException)
            {
                string mensagem = string.Format("Conta não encontrada! ");
                HttpError erro = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.NotFound, erro);
            }

        }

        /*/Lista todas contas com seus respectiva Pessoa
        [Route("api/Conta/Simples/All")]
        public HttpResponseMessage GetAllPessoa()
        {
            try
            {
                ContaDAO dao = new ContaDAO();
                IList<Conta> contas = dao.ListarTodas(true);
                return Request.CreateResponse(HttpStatusCode.OK, contas);
            }
            catch (KeyNotFoundException)
            {
                string mensagem = string.Format("Conta não encontrada! ");
                HttpError erro = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.NotFound, erro);
            }

        }


        [Route("api/Conta/{id}")]
        public HttpResponseMessage GetPessoa(int id)
        {
            try
            {
                ContaDAO dao = new ContaDAO();
                Conta contas = dao.ListarPorId(id, true);
                return Request.CreateResponse(HttpStatusCode.OK, contas);
            }
            catch (KeyNotFoundException)
            {
                string mensagem = string.Format("Conta não encontrada! ");
                HttpError erro = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.NotFound, erro);
            }

        }*/

        [Route("api/Conta/Matriz")]
        public HttpResponseMessage GetMatriz()
        {
            try
            {
                ContaDAO dao = new ContaDAO();
                IList<Conta> contas = dao.ListarPorTipo(true); // true para matriz
                return Request.CreateResponse(HttpStatusCode.OK, contas);
            }
            catch (KeyNotFoundException)
            {
                string mensagem = string.Format("Conta não encontrada! ");
                HttpError erro = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.NotFound, erro);
            }

        }

        [Route("api/Conta/Filial")]
        public HttpResponseMessage GetFilial()
        {
            try
            {
                ContaDAO dao = new ContaDAO();
                IList<Conta> contas = dao.ListarPorTipo(false);// false para filial
                return Request.CreateResponse(HttpStatusCode.OK, contas);
            }
            catch (KeyNotFoundException)
            {
                string mensagem = string.Format("Conta não encontrada! ");
                HttpError erro = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.NotFound, erro);
            }

        }


        public HttpResponseMessage Post([FromBody]Conta c)
        {
            if (ModelState.IsValid)
            {
                ContaDAO dao = new ContaDAO();
                dao.Adicionar(c);

                HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.Created);
                string location = Url.Link("DefaultApi", new { controller = "conta", id = c.Id });
                response.Headers.Location = new Uri(location);
                return response;
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

        }

        public HttpResponseMessage Delete([FromUri] int id)
        {
            ContaDAO dao = new ContaDAO();
            dao.Remover(id);
            return Request.CreateResponse(HttpStatusCode.OK, "Conta excluida!");
        }

        public HttpResponseMessage Put([FromBody] Conta c)
        {
            ContaDAO dao = new ContaDAO();
            var i = dao.Atualizar(c);
            if (i > 0)
            {
                return Request.CreateResponse(HttpStatusCode.OK, i);
            }
            else
            { 
                return Request.CreateResponse(HttpStatusCode.OK, "Nenhuma linha foi alterada!");
            }
        }

    }

}
