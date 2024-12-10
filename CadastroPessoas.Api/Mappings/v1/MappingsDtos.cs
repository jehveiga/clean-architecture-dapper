using CadastroPessoas.Application.Dtos.v1.ViewModels;
using CadastroPessoas.Domain.Entities;

namespace CadastroPessoas.Api.Mappings.v1
{
    public static class MappingsDtos
    {
        public static IEnumerable<CreatedViewModel> ConvertPessoaParaDto(
            this IEnumerable<Pessoa> pessoas)
        {
            List<CreatedViewModel> pessoasDto = (from pessoa in pessoas
                                                 select new CreatedViewModel(
                                    pessoa.Id,
                                    pessoa.Nome,
                                    pessoa.Idade,
                                    pessoa.DataDeCriacao.ToString("dd/MM/yyyy")
                                    )).ToList();

            return pessoasDto;
        }
    }
}
