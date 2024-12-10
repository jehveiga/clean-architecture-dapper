using CadastroPessoas.Domain.Entities;
using CadastroPessoas.Infrastructure.Map;

namespace CadastroPessoas.Infrastructure.Queries
{
    public static class UsuarioQueries
    {
        public static QueryModel GetByIdQuery(int id)
        {
            string table = ContextTablesMapping.UsuariosTable;
            string query = @$"
            SELECT
                [UsuarioID] AS Id,
                [Login] AS Login,
                [Tipo] AS Tipo,
                [Ativo] AS Status,
                [DataCadastro] AS DataCadastro
            FROM 
                {table}
            WHERE
                [UsuarioID] = @Id
            ";

            var parameters = new
            {
                Id = id
            };

            return new QueryModel(query, parameters);
        }

        public static QueryModel GetUserByEmailAndPasswordQuery(string email, string senhaHash)
        {
            string table = ContextTablesMapping.UsuariosTable;
            string query = @$"
            SELECT
                [Login] AS Login,
                [Tipo] AS Tipo
            FROM 
                {table}
            WHERE
                [Login] = @Email AND
                [SenhaHash] = @SenhaHash
            ";

            var parameters = new
            {
                Email = email,
                SenhaHash = senhaHash
            };

            return new QueryModel(query, parameters);
        }

        public static QueryModel CriaQuery(Usuario usuario)
        {
            string table = ContextTablesMapping.UsuariosTable;
            string query = $@"
                INSERT INTO {table}
                (
                    [Login],
                    [SenhaHash],
                    [Tipo],
                    [Ativo],
                    [DataCadastro]
                )
                OUTPUT 
                    inserted.UsuarioID
                VALUES
                (   
                    @Login,
                    @SenhaHash,
                    @Tipo,
                    @Ativo,
                    @DataCadastro
                )
            ";

            var parameters = new
            {
                usuario.Login,
                usuario.SenhaHash,
                usuario.Tipo,
                Ativo = usuario.Status,
                usuario.DataCadastro

            };

            return new QueryModel(query, parameters);
        }
    }
}
