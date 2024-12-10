using CadastroPessoas.Domain.Entities;

namespace CadastroPessoas.Application.Dtos.v2.InputModels;

public class AlteraPessoaInputModel(string nome, int idade)
{
    /// <summary>
    /// Nome da  pessoa
    /// </summary>
    /// <example>Barbara Finance</example>
    public string Nome { get; set; } = nome;

    /// <summary>
    /// Idade da  pessoa
    /// </summary>
    /// <example>35</example>
    public int Idade { get; set; } = idade;
    public Pessoa ToEntity()
    {
        Pessoa pessoa = new(Nome, Idade);
        pessoa.InserirDataAlteracao();
        return pessoa;
    }
}
