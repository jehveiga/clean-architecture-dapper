using CadastroPessoas.Domain.Entities;

namespace CadastroPessoas.Domain.Interfaces.Repositories;

public interface IPessoaRepository
{
    Task<IEnumerable<Pessoa>> GetPessoasAsync();
    Task<Pessoa?> GetPessoaById(int id);
    Task<int> InsertPessoaAsync(Pessoa pessoa);
    Task<int> DeletePessoaAsync(int id);
    Task<int> UpdatePessoaAsync(Pessoa pessoa, int id);
}
