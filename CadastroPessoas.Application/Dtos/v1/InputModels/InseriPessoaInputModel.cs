using CadastroPessoas.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CadastroPessoas.Application.Dtos.v1.InputModels;

public class InseriPessoaInputModel(string nome, int idade)
{
    /// <summary>
    /// Nome da  pessoa
    /// </summary>
    /// <example>Barbara Finance</example>
    [Required(ErrorMessage = "O campo nome é requerido")]
    public string Nome { get; set; } = nome;

    /// <summary>
    /// Idade da  pessoa
    /// </summary>
    /// <example>35</example>
    [Required(ErrorMessage = "O campo idade é requerido")]
    public int Idade { get; set; } = idade;

    [JsonIgnore]
    public DateTime DataDeCriacao { get; set; } = DateTime.Now;

    public Pessoa ToEntity()
    {
        Pessoa pessoa = new(Nome, Idade, DataDeCriacao);
        return pessoa;
    }
}
