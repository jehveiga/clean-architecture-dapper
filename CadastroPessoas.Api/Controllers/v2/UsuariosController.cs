using Asp.Versioning;
using CadastroPessoas.Application.Business;
using CadastroPessoas.Application.Dtos.v2.InputModels;
using CadastroPessoas.Application.Dtos.v2.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Net.Mime;

namespace CadastroPessoas.Api.Controllers.v2
{
    [ApiController]
    [ApiVersion("2")]
    [Route("v{version:apiVersion}/usuarios/")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    public class UsuariosController(IUserBusiness userBusiness) : ControllerBase
    {
        [HttpGet("{id:int}")]
        [SwaggerOperation(Summary = "Obter usuario por id", Description = "Retorna um usuário conforme id informado no path")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(UsuarioViewModel))]
        [SwaggerResponse(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<UsuarioViewModel>> GetById(int id)
        {
            UsuarioViewModel? usuario = await userBusiness.GetById(id);

            return usuario == null ? NotFound() : Ok(usuario);
        }

        // v2/usuarios
        [HttpPost]
        [SwaggerOperation(Summary = "Cria um novo usuário", Description = "Valida e cria um novo usuário no banco de dados.")]
        [SwaggerResponse(StatusCodes.Status201Created, Type = typeof(CreatedUsuarioViewModel))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PostAsync(InseriUsuarioInputModel usuarioInputModel)
        {
            int idUsuario = await userBusiness.Create(usuarioInputModel);

            if (idUsuario == 0)
                return BadRequest();

            CreatedUsuarioViewModel usuarioViewModel = new(usuarioInputModel.Login);

            return CreatedAtAction(nameof(GetById), new { id = idUsuario }, usuarioViewModel);
        }

        // v2/usuarios/login
        [HttpPost("login")]
        [SwaggerOperation(Summary = "Efetua um  login", Description = "Retorna o token de autenticação.")]
        [SwaggerResponse(StatusCodes.Status200OK, Type = typeof(CreatedUsuarioViewModel))]
        [SwaggerResponse(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<LoginUserViewModel>> Login(LoginUserInputModel login)
        {
            LoginUserViewModel? loginViewModel = await userBusiness.ProcessaLoginUsuario(login);

            return loginViewModel is null ? BadRequest() : Ok(loginViewModel);
        }

    }
}
