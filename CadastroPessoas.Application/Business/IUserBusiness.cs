using CadastroPessoas.Application.Dtos.v2.InputModels;
using CadastroPessoas.Application.Dtos.v2.ViewModels;

namespace CadastroPessoas.Application.Business
{
    public interface IUserBusiness
    {
        Task<UsuarioViewModel?> GetById(int id);
        Task<LoginUserViewModel?> ProcessaLoginUsuario(LoginUserInputModel login);
        Task<int> Create(InseriUsuarioInputModel usuarioModel);
    }
}
