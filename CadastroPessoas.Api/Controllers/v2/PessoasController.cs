using Asp.Versioning;
using CadastroPessoas.Api.Mappings.v2;
using CadastroPessoas.Api.Util;
using CadastroPessoas.Application.Dtos.v2.InputModels;
using CadastroPessoas.Application.Dtos.v2.ViewModels;
using CadastroPessoas.Domain.Entities;
using CadastroPessoas.Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace CadastroPessoas.Api.Controllers.v2
{
    [ApiController]
    [ApiVersion("2")]
    [Route("v{version:apiVersion}/pessoas/")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class PessoasController(IPessoaRepository pessoaRepository) : ControllerBase
    {
        [HttpGet]
        [Authorize(Roles = "General, Soldado")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(IEnumerable<CreatedViewModel>))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status403Forbidden)]
        [SwaggerOperation(Summary = "Obter todos as pessoas", Description = "Retorna uma lista com todos as pessoas")]
        public async Task<ActionResult<IEnumerable<CreatedViewModel>>> Get()
        {
            IEnumerable<Pessoa> pessoasEntity = await pessoaRepository.GetPessoasAsync();
            IEnumerable<CreatedViewModel> pessoaViewModel = pessoasEntity.ConvertPessoaParaDto();

            return Ok(pessoaViewModel);
        }

        [HttpGet("{id:int}")]
        [Authorize(Roles = "General, Soldado")]
        [SwaggerOperation(Summary = "Obter pessoa por id", Description = "Retorna uma pessoa conforme id informado no path")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(CreatedViewModel))]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<CreatedViewModel>> GetById([SwaggerParameter("O identificador único da pessoa.", Required = true)] int id)
        {
            Pessoa? pessoa = await pessoaRepository.GetPessoaById(id);

            if (pessoa is null)
            {
                Log.Information("Not Found. - Controller: {Controller} - Action: {Metodo} - PessoaId: {Id}", nameof(PessoasController), nameof(GetById), id);
                return NotFound();
            }

            CreatedViewModel pessoaViewModel = CreatedViewModel.FromEntity(pessoa);

            return Ok(pessoaViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "General")]
        [SwaggerOperation(Summary = "Criar nova pessoa", Description = "Valida e cria nova pessoa no banco de dados.")]
        [SwaggerResponse(StatusCodes.Status400BadRequest, Type = typeof(ErrorResponse))]
        [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(CreatedViewModel))]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Post(InseriPessoaInputModel pessoaInputModel)
        {
            Pessoa pessoa = pessoaInputModel.ToEntity();
            int result = await pessoaRepository.InsertPessoaAsync(pessoa);

            CreatedViewModel pessoaViewModel = CreatedViewModel.FromEntity(pessoa, result);
            return result <= 0 ? BadRequest() : CreatedAtAction(nameof(GetById), new { id = result }, pessoaViewModel);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = "General")]
        [SwaggerOperation(Summary = "Altera pessoa por id", Description = "Altera registro da pessoa no banco de daddos.")]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Put(
            [SwaggerParameter("O identificador único da pessoa.", Required = true)] int id,
            AlteraPessoaInputModel pessoaInputModel)
        {
            Pessoa pessoa = pessoaInputModel.ToEntity();

            int lineResult = await pessoaRepository.UpdatePessoaAsync(pessoa, id);

            return lineResult <= 0 ? NotFound() : Ok();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "General")]
        [SwaggerOperation(Summary = "Deletar pessoa por id", Description = "Remove registro da pessoa do id informado")]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        [SwaggerResponse(StatusCodes.Status200OK)]
        [SwaggerResponse(StatusCodes.Status401Unauthorized)]
        [SwaggerResponse(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> Delete([SwaggerParameter("O identificador único da pessoa.", Required = true)] int id)
        {
            int lineResult = await pessoaRepository.DeletePessoaAsync(id);

            return lineResult <= 0 ? NotFound() : Ok();
        }
    }
}
