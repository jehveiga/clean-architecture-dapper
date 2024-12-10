using CadastroPessoas.Application.Dtos.v2.InputModels;
using CadastroPessoas.Application.Dtos.v2.ViewModels;
using CadastroPessoas.Domain.Entities;
using CadastroPessoas.Domain.Interfaces.Repositories;
using CadastroPessoas.Infrastructure.Auth;

namespace CadastroPessoas.Application.Business
{
    public class UserBusiness(IUsuarioRepository usuarioRepository,
                              IAuthService authService) : IUserBusiness
    {
        public async Task<int> Create(InseriUsuarioInputModel usuarioModel)
        {
            string passwordHash = authService.ComputeSha256Hash(usuarioModel.Password);
            Usuario usuario = new(usuarioModel.Login, passwordHash, usuarioModel.Tipo);
            int idUsuario = await usuarioRepository.InsertUsuarioAsync(usuario);

            return idUsuario;
        }

        public async Task<UsuarioViewModel?> GetById(int id)
        {
            Usuario? usuario = await usuarioRepository.GetUsuarioByIdAsync(id);

            if (usuario == null)
                return default;

            UsuarioViewModel usuarioViewModel = UsuarioViewModel.FromEntity(usuario);

            return usuarioViewModel;

        }

        public async Task<LoginUserViewModel?> ProcessaLoginUsuario(LoginUserInputModel login)
        {
            // Utilizar o mesmo algoritmo para criar o hash dessa senha
            string passswordHash = authService.ComputeSha256Hash(login.Password);

            // Buscar no meu banco de dados um User que tenha meu e-mail e minha senha em formato hash
            Usuario? usuario = await usuarioRepository.GetUsuarioByEmailAndSenhaHashAsync(login.Email, passswordHash);

            if (usuario == null)
                return default;

            // Se existir, gero o token usando os dados do usuário
            string token = authService.GenerateJwtToken(usuario.Login, usuario.Tipo); // Obtendo o token gerado pelo método generate usando email e Role passada por parametro

            return new LoginUserViewModel(true, usuario.Login, token);
        }
    }
}
