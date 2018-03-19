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
    public class PessoaController : ApiController
    {

        public HttpResponseMessage Post([FromBody]Pessoa p)
        {
            var pessoa = new Pessoa();
            PessoaDAO dao = new PessoaDAO();

            if (p.TipoPessoa == "PF")
            {
                pessoa = new PessoaFisica(p);
            }
            else
            {
                pessoa = new PessoaJuridica(p);
            }

            string valida = pessoa.ValidaCampos();
            if (valida != "")
            {
                ModelState.AddModelError("Pessoa", valida);
            }

            if (ModelState.IsValid)
            {
                dao.Adicionar(pessoa);
                return Request.CreateResponse(HttpStatusCode.Created, p);

                /*string location = Url.Link("DefaultApi", new { controller = "Pessoa", id = p.Id });
                response.Headers.Location = new Uri(location);
                return response;*/
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
                PessoaDAO dao = new PessoaDAO();
                Pessoa pessoa = dao.ListarPorId(id);

                if (pessoa.Id != 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, pessoa);
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

        [Route("api/Pessoa/All")]
        public HttpResponseMessage GetAll()
        {

            try
            {
                PessoaDAO dao = new PessoaDAO();
                IList<Pessoa> pessoas = dao.ListarTodas();
                return Request.CreateResponse(HttpStatusCode.OK, pessoas);
            }
            catch (KeyNotFoundException)
            {
                string mensagem = string.Format("Não foi encontrado nenhum dado! ");
                HttpError erro = new HttpError(mensagem);
                return Request.CreateResponse(HttpStatusCode.NotFound, erro);
            }

        }

        public HttpResponseMessage Delete([FromUri] int id)
        {
            PessoaDAO dao = new PessoaDAO();
            dao.Remover(id);
            return Request.CreateResponse(HttpStatusCode.OK, "Pessoa excluida!");
        }

        public HttpResponseMessage Put([FromBody] Pessoa p)
        {

            var pessoa = new Pessoa();
            PessoaDAO dao = new PessoaDAO();
            string data = p.DataNascimento;

            if (p.TipoPessoa == "PF")
            {
                pessoa = new PessoaFisica(p);
            }
            else
            {
                pessoa = new PessoaJuridica(p);
            }

            string valida = pessoa.ValidaCampos();
            if (valida != "")
            {
                ModelState.AddModelError("Pessoa", valida);
            }

            if (ModelState.IsValid)
            {
                var i = dao.Atualizar(pessoa);                
                if (i > 0)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Pessoa " + p.Id + " - " + p.Nome +  " Alterada ");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "Nenhuma linha foi alterada!");
                }
            }
            else
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState );
            }

        }
    }
}
