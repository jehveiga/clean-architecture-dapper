using CadastroPessoas.Domain.Entities;
using CadastroPessoas.Infrastructure.Map;

namespace CadastroPessoas.Infrastructure.Queries
{
    public static class PessoaQueries
    {
        public static QueryModel GetPessoasQuery()
        {
            string table = ContextTablesMapping.PessoasTable;
            string query = @$"
            SELECT
                [ID] AS Id,
                [CL_NOME] AS Nome,
                [CL_IDADE] AS Idade,
                [CL_CREATEDON] AS DataDeCriacao,
                [CL_UPDATEDON] AS DataDeAlteracao
            FROM 
                {table}
            ";

            var parameters = new { };

            return new QueryModel(query, parameters);
        }

        public static QueryModel GetPessoasByIdQuery(int id)
        {
            string table = ContextTablesMapping.PessoasTable;
            string query = @$"
            SELECT
                [ID] AS Id,
                [CL_NOME] AS Nome,
                [CL_IDADE] AS Idade,
                [CL_CREATEDON] AS DataDeCriacao,
                [CL_UPDATEDON] AS DataDeAlteracao
            FROM 
                {table}
            WHERE
                [ID] = @Id
            ";

            var parameters = new
            {
                Id = id
            };

            return new QueryModel(query, parameters);
        }

        public static QueryModel CriaPessoaQuery(Pessoa pessoaInput)
        {
            string table = ContextTablesMapping.PessoasTable;
            string query = $@"
                INSERT INTO {table}
                (
                    [CL_NOME],
                    [CL_IDADE],
                    [CL_CREATEDON]
                )
                OUTPUT 
                    inserted.ID
                VALUES
                (   
                    @Nome,
                    @Idade,
                    @DataDeCriacao
                )
            ";

            var parameters = new
            {
                pessoaInput.Nome,
                pessoaInput.Idade,
                pessoaInput.DataDeCriacao
            };

            return new QueryModel(query, parameters);
        }

        public static QueryModel AlteraPessoaQuery(Pessoa pessoa, int id)
        {
            string table = ContextTablesMapping.PessoasTable;
            string query = $@"
                UPDATE 
                    {table}
                SET
                    [CL_NOME] = @Nome,
                    [CL_IDADE] = @Idade,
                    [CL_UPDATEDON] = @DataDeAlteracao
                WHERE 
                    ID = @Id
            ";

            var parameters = new
            {
                Id = id,
                pessoa.Nome,
                pessoa.Idade,
                pessoa.DataDeAlteracao
            };

            return new QueryModel(query, parameters);
        }

        public static QueryModel ExcluirPessoaQuery(int id)
        {
            string table = ContextTablesMapping.PessoasTable;
            string query = $@"
                DELETE FROM 
                    {table}
                WHERE
                (   
                    ID = @Id
                )
            ";

            var parameters = new
            {
                Id = id
            };

            return new QueryModel(query, parameters);
        }
    }
}
