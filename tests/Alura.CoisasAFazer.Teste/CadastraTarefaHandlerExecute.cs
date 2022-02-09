using System;
using System.Linq;
using Alura.CoisasAFazer.Core.Commands;
using Alura.CoisasAFazer.Core.Models;
using Alura.CoisasAFazer.Services.Handlers;
using Xunit;

namespace Alura.CoisasAFazer.Teste
{
    public class CadastraTarefaHandlerExecute
    {
        [Fact]
        public void DadaTarefaComInfoValidaDeveIncluirNoDB()
        {
            var comando = new CadastraTarefa("Estudar xUnit", new Categoria("Estudo"), DateTime.Now);
            var repositorio = new RepositorioFake();
            
            var handler = new CadastraTarefaHandler(repositorio);
            
            handler.Execute(comando);

            var tarefa = repositorio.ObtemTarefas(tarefa => tarefa.Titulo == "Estudar xUnit")
                .FirstOrDefault();
            
            Assert.NotNull(tarefa);
        }
    }
}