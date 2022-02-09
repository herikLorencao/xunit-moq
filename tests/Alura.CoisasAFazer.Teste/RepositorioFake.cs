﻿using System;
using System.Collections.Generic;
using System.Linq;
using Alura.CoisasAFazer.Core.Models;
using Alura.CoisasAFazer.Infrastructure;

namespace Alura.CoisasAFazer.Teste
{
    public class RepositorioFake : IRepositorioTarefas
    {
        private List<Tarefa> _tarefas = new List<Tarefa>();
        
        public void IncluirTarefas(params Tarefa[] tarefas)
        {
            _tarefas.AddRange(tarefas);
        }

        public void AtualizarTarefas(params Tarefa[] tarefas)
        {
            throw new NotImplementedException();
        }

        public void ExcluirTarefas(params Tarefa[] tarefas)
        {
            throw new NotImplementedException();
        }

        public Categoria ObtemCategoriaPorId(int id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Tarefa> ObtemTarefas(Func<Tarefa, bool> filtro)
        {
            return _tarefas.Where(filtro);
        }
    }
}