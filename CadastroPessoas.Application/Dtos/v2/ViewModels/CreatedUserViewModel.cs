using CadastroPessoas.Domain.Entities;

namespace CadastroPessoas.Application.Dtos.v2.ViewModels
{
    public record CreatedUserViewModel(int Id,
        string Login,
        string Tipo,
        string Status,
        DateTime DataCadastro)
    {
        public static CreatedUserViewModel FromEntity(Usuario usuario)
        {
            return new(usuario.Id,
                usuario.Login,
                usuario.Tipo,
                usuario.Status.ToString(),
                usuario.DataCadastro);
        }
    }
}
