using Microsoft.AspNetCore.Mvc;
using Alura.CoisasAFazer.WebApp.Models;
using Alura.CoisasAFazer.Core.Commands;
using Alura.CoisasAFazer.Infrastructure;
using Alura.CoisasAFazer.Services.Handlers;
using Microsoft.Extensions.Logging;

namespace Alura.CoisasAFazer.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TarefasController : ControllerBase
    {
        [HttpPost]
        public IActionResult EndpointCadastraTarefa(CadastraTarefaVM model)
        {
            var context = new DbTarefasContext();
            var repo = new RepositorioTarefa(context);
            
            var cmdObtemCateg = new ObtemCategoriaPorId(model.IdCategoria);
            var categoria = new ObtemCategoriaPorIdHandler(repo).Execute(cmdObtemCateg);
            if (categoria == null)
            {
                return NotFound("Categoria não encontrada");
            }

            var comando = new CadastraTarefa(model.Titulo, categoria, model.Prazo);
            var logger = new LoggerFactory().CreateLogger<CadastraTarefaHandler>();
            var handler = new CadastraTarefaHandler(repo, logger);
            handler.Execute(comando);
            return Ok();
        }
    }
}