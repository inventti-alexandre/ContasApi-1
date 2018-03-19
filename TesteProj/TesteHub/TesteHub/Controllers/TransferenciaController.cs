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
    public class TransferenciaController : ApiController
    {
        public HttpResponseMessage Post([FromBody]Transferencia transferencia)
        {
            TransferenciaDAO dao = new TransferenciaDAO();
            string valida = transferencia.ValidaCampoDt();
            valida += dao.Valida(transferencia);

            if (valida != "")
            {
                ModelState.AddModelError("Transferencia", valida);
            }

            if (ModelState.IsValid)
            {
                dao.Adicionar(transferencia);
                return Request.CreateResponse(HttpStatusCode.Created, transferencia);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

        }

        public HttpResponseMessage Get(int id)
        {
            try
            {
                TransferenciaDAO dao = new TransferenciaDAO();
                Transferencia transferencia = dao.ListarPorId(id);

                if (transferencia.Id > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, transferencia);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Transferencia não encontrada!");
                }
            }
            catch (KeyNotFoundException)
            {
                string mensagem = string.Format("Transferencia não encontrada!");
                HttpError erro = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.NotFound, erro);
            }

        }

        [Route("api/Transferencia/All")]
        public HttpResponseMessage GetAll()
        {
            try
            {
                TransferenciaDAO dao = new TransferenciaDAO();
                IList<Transferencia> transferencia = dao.ListarTodas();
                return Request.CreateResponse(HttpStatusCode.OK, transferencia);
            }
            catch (KeyNotFoundException)
            {
                string mensagem = string.Format("Não foi encontrado nenhum dado! ");
                HttpError erro = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.NotFound, erro);
            }
        }

        [Route("api/Transferencia/ContaOrigem/{id}")]
        public HttpResponseMessage GetCtOrigem(int id)
        {
            try
            {
                TransferenciaDAO dao = new TransferenciaDAO();
                IList<Transferencia> transferencia = dao.ListarPorConta(id, "IdContaOrigem");
                return Request.CreateResponse(HttpStatusCode.OK, transferencia);
            }
            catch (KeyNotFoundException)
            {
                string mensagem = string.Format("Não foi encontrado nenhum dado! ");
                HttpError erro = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.NotFound, erro);
            }
        }
        [Route("api/Transferencia/ContaDestino/{id}")]
        public HttpResponseMessage GetCtDestino(int id)
        {
            try
            {
                TransferenciaDAO dao = new TransferenciaDAO();
                IList<Transferencia> transferencia = dao.ListarPorConta(id, "IdContaDestino");
                return Request.CreateResponse(HttpStatusCode.OK, transferencia);
            }
            catch (KeyNotFoundException)
            {
                string mensagem = string.Format("Não foi encontrado nenhum dado! ");
                HttpError erro = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.NotFound, erro);
            }
        }

        public HttpResponseMessage Delete([FromUri] Transferencia t)
        {
            TransferenciaDAO dao = new TransferenciaDAO();
            dao.Remover(t);
            return Request.CreateResponse(HttpStatusCode.OK, "Transferencia Excluida!");
        }

        public HttpResponseMessage Put([FromBody] Transferencia t)
        {

            TransferenciaDAO dao = new TransferenciaDAO();            
            string valida = t.ValidaCampoDt();

            if (valida != "")
            {
                ModelState.AddModelError("Transferencia", valida);
            }

            if (ModelState.IsValid)
            {
                var i = dao.Atualizar(t);
                if (i > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, i);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Nenhuma linha foi alterada!");
                }
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

        }

    }
}
