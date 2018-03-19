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
    public class AporteController : ApiController
    {
        public HttpResponseMessage Post([FromBody]Aporte aporte)
        {
            AporteDAO dao = new AporteDAO();

            string valida = aporte.ValidaCampoDt();
            valida += dao.Valida(aporte);

            if(valida != "")
            {
                ModelState.AddModelError("Aporte", valida);
            }

            if (ModelState.IsValid)
            {
                dao.Adicionar(aporte);
                return Request.CreateResponse(HttpStatusCode.Created, aporte);
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

        }

        [Route("api/Aporte/{idAlpha}")]
        public HttpResponseMessage Get(string idAlpha)
        {
            try
            {
                AporteDAO dao = new AporteDAO();
                Aporte aporte = dao.ListarPorId(idAlpha);

                if (aporte.IdAlpha != "")
                {
                    return Request.CreateResponse(HttpStatusCode.OK, aporte);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Pessoa não encontrada!");
                }
            }
            catch (KeyNotFoundException)
            {
                string mensagem = string.Format("Pessoa não encontrada!");
                HttpError erro = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.NotFound, erro);
            }

        }

        [Route("api/Aporte/All")]
        public HttpResponseMessage GetAll()
        {

            try
            {
                AporteDAO dao = new AporteDAO();
                IList<Aporte> aportes = dao.ListarTodas();
                return Request.CreateResponse(HttpStatusCode.OK, aportes);
            }
            catch (KeyNotFoundException)
            {
                string mensagem = string.Format("Não foi encontrado nenhum dado! ");
                HttpError erro = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.NotFound, erro);
            }

        }

        public HttpResponseMessage Delete([FromUri] Aporte a)
        {
            AporteDAO dao = new AporteDAO();
            dao.Remover(a);
            return Request.CreateResponse(HttpStatusCode.OK, "Aporte Excluido!");
        }

        public HttpResponseMessage Put([FromBody] Aporte a)
        {

            AporteDAO dao = new AporteDAO();

            string valida = a.ValidaCampoDt();

            if (valida != "")
            {
                ModelState.AddModelError("Aporte", valida);
            }

            if (ModelState.IsValid)
            {
                var i = dao.Atualizar(a);
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
