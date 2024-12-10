using CadastroPessoas.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace CadastroPessoas.Application.Dtos.v2.InputModels
{
    public record InseriUsuarioInputModel(

        [Required]
        [EmailAddress]
        string Login,
        [Required]
        string Password,
        [Required]
        [AllowedValues(["General", "Capitão", "Sargento", "Soldado"], ErrorMessage = "Inserir os valores permitidos: \"General\", \"Capitão\", \"Sargento\", \"Soldado\"")]
        string Tipo)

    {
        public Usuario ToEntity()
        {
            Usuario usuario = new(Login, Password, Tipo);
            return usuario;
        }
    }
}
