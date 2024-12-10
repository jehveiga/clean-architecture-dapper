namespace CadastroPessoas.Application.Dtos.v2.ViewModels
{
    public record LoginUserViewModel(bool Authenticated, string Email, string Token);
}
