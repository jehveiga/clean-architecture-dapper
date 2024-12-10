using CadastroPessoas.Domain.Entities;
using CadastroPessoas.Domain.Interfaces.Repositories;
using CadastroPessoas.Infrastructure.Factory;
using CadastroPessoas.Infrastructure.Queries;
using Dapper;
using System.Data;

namespace CadastroPessoas.Infrastructure.Repositories
{
    public class UsuarioRepository(SqlFactory sqlFactory) : IUsuarioRepository
    {
        private readonly IDbConnection _connection = sqlFactory.CreateConnection();
        public async Task<Usuario?> GetUsuarioByIdAsync(int id)
        {
            QueryModel query = UsuarioQueries.GetByIdQuery(id);

            Usuario? usuarioDb = await _connection.QueryFirstOrDefaultAsync<Usuario>(query.Query, query.Parameters);

            return usuarioDb;
        }

        public async Task<Usuario?> GetUsuarioByEmailAndSenhaHashAsync(string email, string senhaHash)
        {
            QueryModel query = UsuarioQueries.GetUserByEmailAndPasswordQuery(email, senhaHash);

            Usuario? usuarioDb = await _connection.QueryFirstOrDefaultAsync<Usuario>(query.Query, query.Parameters);

            return usuarioDb;
        }

        public async Task<int> InsertUsuarioAsync(Usuario usuario)
        {
            QueryModel query = UsuarioQueries.CriaQuery(usuario);
            int result = await _connection.QueryFirstOrDefaultAsync<int>(query.Query, query.Parameters);

            return result;
        }
    }
}
