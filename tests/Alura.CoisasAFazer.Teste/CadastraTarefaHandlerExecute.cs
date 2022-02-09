using System;
using System.Linq;
using Alura.CoisasAFazer.Core.Commands;
using Alura.CoisasAFazer.Core.Models;
using Alura.CoisasAFazer.Infrastructure;
using Alura.CoisasAFazer.Services.Handlers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Alura.CoisasAFazer.Teste
{
    public class CadastraTarefaHandlerExecute
    {
        [Fact]
        public void DadaTarefaComInfoValidaDeveIncluirNoDB()
        {
            var comando = new CadastraTarefa("Estudar xUnit", new Categoria("Estudo"), DateTime.Now);

            var mockLogger = new Mock<ILogger<CadastraTarefaHandler>>();
            var options = new DbContextOptionsBuilder<DbTarefasContext>()
                .UseInMemoryDatabase("DbTarefasContext")
                .Options;
            var context = new DbTarefasContext(options);
            var repositorio = new RepositorioTarefa(context);
            var handler = new CadastraTarefaHandler(repositorio, mockLogger.Object);

            handler.Execute(comando);

            var tarefa = repositorio.ObtemTarefas(tarefa => tarefa.Titulo == "Estudar xUnit")
                .FirstOrDefault();

            Assert.NotNull(tarefa);
        }

        [Fact]
        public void QuandoExceptionForLancadaResultadoIsSucessDeveSerFalse()
        {
            var comando = new CadastraTarefa("Estudar xUnit", new Categoria("Estudo"), DateTime.Now);
            var mockLogger = new Mock<ILogger<CadastraTarefaHandler>>();
            var mock = new Mock<IRepositorioTarefas>();
            mock.Setup(r => r.IncluirTarefas(It.IsAny<Tarefa[]>()))
                .Throws(new Exception("Houve um erro ao cadastrar as tarefas"));
            var repo = mock.Object;

            var handler = new CadastraTarefaHandler(repo, mockLogger.Object);

            var commandResult = handler.Execute(comando);
            Assert.False(commandResult.IsSuccess);
        }

        [Fact]
        public void QuandoExceptionForLancadaDevoLogarMensagemExcecao()
        {
            var mensagemExcecao = "Houve um erro ao cadastrar as tarefas";
            var excecaoEsperada = new Exception(mensagemExcecao);

            var comando = new CadastraTarefa("Estudar xUnit", new Categoria("Estudo"), DateTime.Now);
            var mock = new Mock<IRepositorioTarefas>();
            mock.Setup(r => r.IncluirTarefas(It.IsAny<Tarefa[]>()))
                .Throws(excecaoEsperada);
            var repo = mock.Object;
            var mockLogger = new Mock<ILogger<CadastraTarefaHandler>>();

            var handler = new CadastraTarefaHandler(repo, mockLogger.Object);

            var commandResult = handler.Execute(comando);

            // O Moq não consegue utilizar métodos de extensão como o LogError, por exemplo 
            mockLogger.Verify(
                l => l.Log(LogLevel.Error, It.IsAny<EventId>(), It.IsAny<object>(),
                    excecaoEsperada, It.IsAny<Func<object, Exception, string>>()), Times.Once());
        }

        delegate void CapturaMensagemLog(LogLevel level, EventId eventId, object state, Exception exception,
            Func<object, Exception, string> func);

        [Fact]
        public void QuandoExceptionForLancadaLogarMensagem()
        {
            var tituloTarefa = "Estudar xUnit e Moq";

            var comando = new CadastraTarefa(tituloTarefa, new Categoria("Estudo"), DateTime.Now);
            var mock = new Mock<IRepositorioTarefas>();
            var mockLogger = new Mock<ILogger<CadastraTarefaHandler>>();
            var handler = new CadastraTarefaHandler(mock.Object, mockLogger.Object);

            var logLevel = LogLevel.Error;
            var logMsg = string.Empty;

            CapturaMensagemLog captura = (level, eventId, state, exception, func) =>
            {
                logLevel = level;
                logMsg = func(state, exception);
            };

            mockLogger.Setup(
                l => l.Log(
                    It.IsAny<LogLevel>(),
                    It.IsAny<EventId>(),
                    It.IsAny<object>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<object,
                        Exception, string>>()
                )).Callback(captura);
            
            handler.Execute(comando);

            Assert.Contains(tituloTarefa, logMsg);
            Assert.Equal(LogLevel.Debug, logLevel);
        }
    }
}