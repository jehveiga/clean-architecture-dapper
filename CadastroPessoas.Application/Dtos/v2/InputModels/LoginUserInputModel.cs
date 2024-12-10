using System.ComponentModel.DataAnnotations;

namespace CadastroPessoas.Application.Dtos.v2.InputModels
{
    public record LoginUserInputModel(
        [Required]
        [EmailAddress] string Email,
        [Required] string Password);
}
