using CadastroPessoas.Domain.Entities;

namespace CadastroPessoas.Application.Dtos.v1.ViewModels;

public class CreatedViewModel
{
    public CreatedViewModel(int id, string nome, int idade, string dataDeCriacao)
    {
        Id = id;
        Nome = nome;
        Idade = idade;
        DataDeCriacao = dataDeCriacao;
    }

    public int Id { get; set; }
    public string Nome { get; set; }
    public int Idade { get; set; }
    public string DataDeCriacao { get; set; }
    public static CreatedViewModel FromEntity(Pessoa pessoa)
    {
        return new CreatedViewModel(pessoa.Id, pessoa.Nome, pessoa.Idade, pessoa.DataDeCriacao.ToString("dd/MM/yyyy"));
    }

    public static CreatedViewModel FromEntity(Pessoa pessoa, int id)
    {
        return new CreatedViewModel(id, pessoa.Nome, pessoa.Idade, pessoa.DataDeCriacao.ToString("dd/MM/yyyy"));
    }
}
