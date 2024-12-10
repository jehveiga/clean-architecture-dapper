using CadastroPessoas.Domain.Entities;
using CadastroPessoas.Domain.Interfaces.Repositories;
using CadastroPessoas.Infrastructure.Factory;
using CadastroPessoas.Infrastructure.Queries;
using Dapper;
using System.Data;

namespace CadastroPessoas.Infrastructure.Repositories
{
    public class PessoaRepository(SqlFactory sqlFactory) : IPessoaRepository
    {
        private readonly IDbConnection _connection = sqlFactory.CreateConnection();
        public async Task<IEnumerable<Pessoa>> GetPessoasAsync()
        {
            QueryModel query = PessoaQueries.GetPessoasQuery();

            IEnumerable<Pessoa> pessoas = await _connection.QueryAsync<Pessoa>(query.Query, query.Parameters) ?? Enumerable.Empty<Pessoa>();

            return pessoas;
        }

        public async Task<Pessoa?> GetPessoaById(int id)
        {
            QueryModel query = PessoaQueries.GetPessoasByIdQuery(id);

            Pessoa? pessoaDb = await _connection.QueryFirstOrDefaultAsync<Pessoa>(query.Query, query.Parameters);

            return pessoaDb;
        }

        public async Task<int> InsertPessoaAsync(Pessoa pessoa)
        {
            QueryModel query = PessoaQueries.CriaPessoaQuery(pessoa);
            int result = await _connection.QueryFirstOrDefaultAsync<int>(query.Query, query.Parameters);

            return result;
        }

        public async Task<int> UpdatePessoaAsync(Pessoa pessoa, int id)
        {
            QueryModel query = PessoaQueries.AlteraPessoaQuery(pessoa, id);
            return await _connection.ExecuteAsync(query.Query, query.Parameters);
        }

        public async Task<int> DeletePessoaAsync(int id)
        {
            QueryModel query = PessoaQueries.ExcluirPessoaQuery(id);
            return await _connection.ExecuteAsync(query.Query, query.Parameters);
        }
    }
}
