using CadastroPessoas.Domain.Entities;

namespace CadastroPessoas.Test.Domain.Entities
{
    [TestClass]
    public class PessoaTests
    {
        [TestMethod]
        [TestCategory("Domain")]
        [DataRow("Vanderley", 35)]
        [DataRow("Daniel", 25)]
        [DataRow("Diego", 45)]
        public void Deveria_Passar_Se_Pessoa_Propriedades_Estao_Prenchidas(string name,
                                                                           int age)
        {
            // Arrange
            Pessoa pessoa = new(name, age, DateTime.Now);

            // Act

            // Assert
            Assert.IsNotNull(pessoa);

            Assert.IsNotNull(pessoa.Nome);
            Assert.IsNotNull(pessoa.Idade);
            Assert.IsNotNull(pessoa.DataDeCriacao);

            Assert.IsTrue(pessoa.Idade > 0);
        }

        [TestMethod]
        [TestCategory("Domain")]
        public void Deveria_Passar_Se_DataAlteracao_Esta_Prenchida()
        {
            // Arrange
            Pessoa pessoa = new(nome: "Vanderley",
                                idade: 35,
                                dataDeCriacao: new DateTime(1994, 7, 5, 16, 23, 42, DateTimeKind.Local));

            // Act
            pessoa.InserirDataAlteracao();

            // Assert
            Assert.IsNotNull(pessoa);

            Assert.IsNotNull(pessoa.Nome);
            Assert.IsNotNull(pessoa.Idade);
            Assert.IsNotNull(pessoa.DataDeCriacao);
            Assert.IsNotNull(pessoa.DataDeAlteracao);

            Assert.IsTrue(pessoa.Idade > 0);

        }
    }
}