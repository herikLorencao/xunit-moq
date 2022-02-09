using System;
using System.Linq;
using Alura.CoisasAFazer.Core.Commands;
using Alura.CoisasAFazer.Core.Models;
using Alura.CoisasAFazer.Infrastructure;
using Alura.CoisasAFazer.Services.Handlers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Xunit;

namespace Alura.CoisasAFazer.Teste
{
    public class CadastraTarefaHandlerExecute
    {
        [Fact]
        public void DadaTarefaComInfoValidaDeveIncluirNoDB()
        {
            var comando = new CadastraTarefa("Estudar xUnit", new Categoria("Estudo"), DateTime.Now);

            
            var options = new DbContextOptionsBuilder<DbTarefasContext>()
                .UseInMemoryDatabase("DbTarefasContext")
                .Options;
            var context = new DbTarefasContext(options);
            var repositorio = new RepositorioTarefa(context);
            var handler = new CadastraTarefaHandler(repositorio);
            
            handler.Execute(comando);

            var tarefa = repositorio.ObtemTarefas(tarefa => tarefa.Titulo == "Estudar xUnit")
                .FirstOrDefault();
            
            Assert.NotNull(tarefa);
        }
    }
}