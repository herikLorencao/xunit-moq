using Alura.CoisasAFazer.Core.Commands;
using Alura.CoisasAFazer.Infrastructure;
using Alura.CoisasAFazer.Services.Handlers;
using Moq;
using Xunit;

namespace Alura.CoisasAFazer.Teste
{
    public class ObtemCategoriaPorIdExecute
    {
        [Fact]
        public void QuandoIdExistenteDeveChamarObtemCategoriaUmaVez()
        {
            var mock = new Mock<IRepositorioTarefas>();
            var repo = mock.Object;
            
            const int idCategoria = 20;
            var comando = new ObtemCategoriaPorId(idCategoria);
            var handler = new ObtemCategoriaPorIdHandler(repo);

            handler.Execute(comando);
            
            mock.Verify(r => r.ObtemCategoriaPorId(idCategoria), Times.Once());
        }
    }
}