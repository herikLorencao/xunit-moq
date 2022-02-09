using System;
using System.Collections.Generic;
using System.Linq;
using Alura.CoisasAFazer.Core.Commands;
using Alura.CoisasAFazer.Core.Models;
using Alura.CoisasAFazer.Infrastructure;
using Alura.CoisasAFazer.Services.Handlers;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Alura.CoisasAFazer.Teste
{
    public class GerenciaPrazoDasTarefasHandlerExecute
    {
        [Fact]
        public void QuandoTarefasEstiveremAtrasadasDeveMudarStatus()
        {
            var compCategoria = new Categoria(1, "Compras");
            var casaCategoria = new Categoria(2, "Casa");
            var trabCategoria = new Categoria(3, "Trabalho");
            var saudCategoria = new Categoria(4, "Saúde");
            var higiCategoria = new Categoria(5, "Higiene");

            var tarefas = new List<Tarefa>
            {
                // atrasadas a partir de 1/1/2019
                new Tarefa(1, "Tirar lixo", casaCategoria, new DateTime(2018, 12, 31), null, StatusTarefa.Criada),
                new Tarefa(4, "Fazer almoço", casaCategoria, new DateTime(2017, 12, 31), null, StatusTarefa.Criada),
                new Tarefa(9, "Ir à Academia", saudCategoria, new DateTime(2018, 12, 31), null, StatusTarefa.Criada),
                new Tarefa(7, "Concluir o relatório", trabCategoria, new DateTime(2018, 5, 7), null,
                    StatusTarefa.Criada),
                new Tarefa(10, "Beber água", saudCategoria, new DateTime(2018, 12, 31), null, StatusTarefa.Pendente),
                // dentro do prazo 1/1/2019
                new Tarefa(8, "Comparecer a reunião", trabCategoria, new DateTime(2018, 11, 12), new DateTime(2018, 11, 12),
                    StatusTarefa.Concluida),
                new Tarefa(2, "Arrumar cama", casaCategoria, new DateTime(2019, 4, 5), null, StatusTarefa.Criada),
                new Tarefa(3, "Escovar dentes", higiCategoria, new DateTime(2019, 1, 2), null, StatusTarefa.Criada),
                new Tarefa(5, "Comprar presentes da Ana", compCategoria, new DateTime(2019, 10, 8), null,
                    StatusTarefa.Criada),
                new Tarefa(6, "Comprar ração", compCategoria, new DateTime(2019, 11, 20), null, StatusTarefa.Criada),
            };
            
            var options = new DbContextOptionsBuilder<DbTarefasContext>()
                .UseInMemoryDatabase("DbTarefasContext")
                .Options;
            var context = new DbTarefasContext(options);
            var repositorio = new RepositorioTarefa(context);
            
            repositorio.IncluirTarefas(tarefas.ToArray());

            var comando = new GerenciaPrazoDasTarefas(new DateTime(2019, 1, 1));
            var handler = new GerenciaPrazoDasTarefasHandler(repositorio);
            
            handler.Execute(comando);

            var tarefasEmAtraso = repositorio.ObtemTarefas(t => t.Status == StatusTarefa.EmAtraso);
            Assert.Equal(5, tarefasEmAtraso.Count());
        }
    }
}