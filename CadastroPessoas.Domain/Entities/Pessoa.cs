namespace CadastroPessoas.Domain.Entities
{
    public class Pessoa
    {
        public Pessoa(string nome, int idade)
        {
            Nome = nome;
            Idade = idade;
        }

        public Pessoa(string nome, int idade, DateTime dataDeCriacao)
        {
            Nome = nome;
            Idade = idade;
            DataDeCriacao = dataDeCriacao;
        }

        public Pessoa(int id, string nome, int idade, DateTime dataDeCriacao, DateTime dataDeAlteracao)
        {
            Id = id;
            Nome = nome;
            Idade = idade;
            DataDeCriacao = dataDeCriacao;
            DataDeAlteracao = dataDeAlteracao;
        }

        public void InserirDataAlteracao()
        {
            DataDeAlteracao = DateTime.Now;
        }

        public int Id { get; private set; }
        public string Nome { get; private set; }
        public int Idade { get; private set; }
        public DateTime DataDeCriacao { get; private set; }
        public DateTime DataDeAlteracao { get; private set; }

    }
}
