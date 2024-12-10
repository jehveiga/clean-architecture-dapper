using CadastroPessoas.Domain.Entities;

namespace CadastroPessoas.Application.Dtos.v2.ViewModels
{
    public record UsuarioViewModel(
        int Id,
        string Login,
        string Tipo,
        string Status,
        DateTime DataCadastro
        )
    {
        public static UsuarioViewModel FromEntity(Usuario usuario)
        {
            return new UsuarioViewModel(usuario.Id,
                usuario.Login,
                usuario.Tipo,
                usuario.Status.ToString(),
                usuario.DataCadastro);
        }
    }

}
