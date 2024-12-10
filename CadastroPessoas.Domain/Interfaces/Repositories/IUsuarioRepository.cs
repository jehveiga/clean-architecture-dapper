using CadastroPessoas.Domain.Entities;

namespace CadastroPessoas.Domain.Interfaces.Repositories;

public interface IUsuarioRepository
{
    Task<Usuario?> GetUsuarioByIdAsync(int id);
    Task<Usuario?> GetUsuarioByEmailAndSenhaHashAsync(string email, string senhaHash);
    Task<int> InsertUsuarioAsync(Usuario usuario);
}
