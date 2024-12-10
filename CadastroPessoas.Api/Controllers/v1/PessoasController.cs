using Asp.Versioning;
using CadastroPessoas.Api.Mappings.v1;
using CadastroPessoas.Api.Util;
using CadastroPessoas.Application.Dtos.v1.InputModels;
using CadastroPessoas.Application.Dtos.v1.ViewModels;
using CadastroPessoas.Domain.Entities;
using CadastroPessoas.Domain.Interfaces.Repositories;
using CadastroPessoas.Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace CadastroPessoas.Api.Controllers.v1
{
    [ApiController]
    [ApiVersion("1", Deprecated = true)]
    [Route("v{version:apiVersion}/pessoas/")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class PessoasController(IPessoaRepository pessoaRepository) : ControllerBase
    {
        [HttpGet]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<CreatedViewModel>))]
        [SwaggerOperation(Summary = "Obter todos as pessoas", Description = "Retorna uma lista com todos as pessoas")]
        public async Task<ActionResult<IEnumerable<CreatedViewModel>>> Get()
        {
            IEnumerable<Pessoa> pessoasEntity = await pessoaRepository.GetPessoasAsync();
            IEnumerable<CreatedViewModel> pessoaViewModel = pessoasEntity.ConvertPessoaParaDto();

            return Ok(pessoaViewModel);
        }

        [Authorize]
        [HttpGet("{id:int}")]
        [SwaggerOperation(Summary = "Obter pessoa por id", Description = "Retorna uma pessoa conforme id informado no path")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(CreatedViewModel))]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CreatedViewModel>> GetById([SwaggerParameter("O identificador único da pessoa.", Required = true)] int id,
                                                                  IDistributedCache cache)
        {
            Pessoa? pessoa = await cache.GetAsync($"pessoas-{id}",
                async token =>
                {
                    Pessoa? pessoa = await pessoaRepository.GetPessoaById(id);

                    return pessoa;
                },
                CacheOptions.DefaultExpiration);

            if (pessoa is null)
                return NotFound();

            CreatedViewModel pessoaViewModel = CreatedViewModel.FromEntity(pessoa);

            return Ok(pessoaViewModel);
        }

        [HttpPost]
        [SwaggerOperation(Summary = "Criar nova pessoa", Description = "Valida e cria nova pessoa no banco de dados.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(CreatedViewModel))]
        public async Task<IActionResult> Post(InseriPessoaInputModel pessoaInputModel)
        {
            Pessoa pessoa = pessoaInputModel.ToEntity();

            int result = await pessoaRepository.InsertPessoaAsync(pessoa);

            CreatedViewModel pessoaViewModel = CreatedViewModel.FromEntity(pessoa, result);
            return result <= 0 ? BadRequest() : CreatedAtAction(nameof(GetById), new { id = result }, pessoaViewModel);
        }

        [HttpPut("{id:int}")]
        [SwaggerOperation(Summary = "Altera pessoa por id", Description = "Altera registro da pessoa no banco de daddos.")]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status200OK)]
        public async Task<IActionResult> Put(
            [SwaggerParameter("O identificador único da pessoa.", Required = true)] int id,
            AlteraPessoaInputModel pessoaInputModel,
            IDistributedCache cache)
        {
            Pessoa pessoa = pessoaInputModel.ToEntity();

            int lineResult = await pessoaRepository.UpdatePessoaAsync(pessoa, id);

            await cache.RemoveAsync($"products-{id}");

            return lineResult <= 0 ? NotFound() : Ok();
        }

        [HttpDelete("{id:int}")]
        [SwaggerOperation(Summary = "Deletar pessoa por id", Description = "Remove registro da pessoa do id informado")]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status200OK)]
        public async Task<IActionResult> Delete([SwaggerParameter("O identificador único da pessoa.", Required = true)] int id,
                                                IDistributedCache cache)
        {
            int lineResult = await pessoaRepository.DeletePessoaAsync(id);

            await cache.RemoveAsync($"products-{id}");

            return lineResult <= 0 ? NotFound() : Ok();
        }
    }
}
