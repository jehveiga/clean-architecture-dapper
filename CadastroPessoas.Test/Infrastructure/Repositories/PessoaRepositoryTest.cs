using CadastroPessoas.Domain.Entities;
using CadastroPessoas.Domain.Interfaces.Repositories;
using Moq;

namespace CadastroPessoas.Test.Infrastructure.Repositories;

[TestClass]
public class PessoaRepositoryTest
{
    [TestMethod]
    [TestCategory("Infrastructure")]
    public void Deveria_Retornar_Uma_Lista_De_Pessoas_Com_Itens()
    {
        // Arrange
        IEnumerable<Pessoa> mockPessoaList =
        [
            new Pessoa(1, "Maria", 36, DateTime.Now, DateTime.Now),
            new Pessoa(2, "Diego", 27, DateTime.Now, DateTime.Now),
            new Pessoa(3, "Victor", 89, DateTime.Now, DateTime.Now),
        ];

        Mock<IPessoaRepository> mockPessoaRepository = new();

        // Act
        mockPessoaRepository.Setup(p => p.GetPessoasAsync())
                            .Returns(() => Task.Run(() => mockPessoaList));
        bool result = mockPessoaRepository.Object.GetPessoasAsync().Result.Any();

        // Assert
        Assert.IsTrue(result);
    }
}