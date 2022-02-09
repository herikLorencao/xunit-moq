using System;
using Alura.CoisasAFazer.Core.Models;
using Alura.CoisasAFazer.Infrastructure;
using Alura.CoisasAFazer.Services.Handlers;
using Alura.CoisasAFazer.WebApp.Controllers;
using Alura.CoisasAFazer.WebApp.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Alura.CoisasAFazer.Teste
{
    public class TarefasControllerEndpointCadastraTarefa
    {
        [Fact]
        public void DadaTarefaComInformacoesValidasRetornarOk()
        {
            const int idCategoria = 1;
            var mockRepo = new Mock<IRepositorioTarefas>();
            var mockLogger = new Mock<ILogger<CadastraTarefaHandler>>();

            mockRepo.Setup(r => r.ObtemCategoriaPorId(idCategoria))
                .Returns(new Categoria(idCategoria, "Teste"));

            var tarefasController = new TarefasController(mockRepo.Object, mockLogger.Object);

            var model = new CadastraTarefaVM();
            model.IdCategoria = idCategoria;
            model.Titulo = "Estudar xUnit";
            model.Prazo = DateTime.Now;

            var retorno = tarefasController.EndpointCadastraTarefa(model);
            Assert.IsType<OkResult>(retorno);
        }

        [Fact]
        public void QuandoExcecaoLancadaDeveRetornarStatusErro()
        {
            const int idCategoria = 1;
            var mockRepo = new Mock<IRepositorioTarefas>();
            var mockLogger = new Mock<ILogger<CadastraTarefaHandler>>();

            mockRepo.Setup(r => r.ObtemCategoriaPorId(idCategoria))
                .Returns(new Categoria(idCategoria, "Estudo"));
            mockRepo.Setup(r => r.IncluirTarefas(It.IsAny<Tarefa[]>()))
                .Throws(new Exception());

            var tarefasController = new TarefasController(mockRepo.Object, mockLogger.Object);
            var model = new CadastraTarefaVM();
            model.IdCategoria = idCategoria;
            model.Titulo = "Estudo xUnit e Moq";
            model.Prazo = DateTime.Now;

            var retorno = tarefasController.EndpointCadastraTarefa(model);
            var statusRetorno = (retorno as StatusCodeResult)?.StatusCode;
            
            Assert.IsType<StatusCodeResult>(retorno);
            Assert.Equal(StatusCodes.Status500InternalServerError, statusRetorno);
        }
    }
}