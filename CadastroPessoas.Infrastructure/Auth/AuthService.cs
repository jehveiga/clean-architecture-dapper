using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CadastroPessoas.Infrastructure.Auth
{
    public class AuthService(IConfiguration configuration) : IAuthService
    {
        // Método que será usado para Criptografar a senha
        public string ComputeSha256Hash(string password)
        {
            using SHA256 sha256Hash = SHA256.Create();
            // ComputeHash - retorna byte array
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

            // Converte byte array para string
            StringBuilder builder = new();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2")); // x2 faz com que seja convertido em representação hexadecimal
            }

            return builder.ToString();
        }

        // Método que efetua a criação do token
        public string GenerateJwtToken(string email, string role)
        {
            // Emissor do serviço do token
            string issuer = configuration.GetSection("JwtOptions:Issuer").Value ?? string.Empty;
            // Publico do serviço do token
            string audience = configuration.GetSection("JwtOptions:Audience").Value ?? string.Empty;
            // A Chave do serviço do token
            string key = configuration.GetSection("JwtOptions:SecurityKey").Value ?? string.Empty;

            #region Chave/Criptografia

            // Obtendo a chave e convertendo em formato simétrico
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(key));
            // Obtendo a assinatura da chave simetrica criada no método acima, adicionando a criptografia 'HmacSha256',
            // Criando a credencial de assinatura
            SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);

            #endregion Chave/Criptografia

            #region Claims de usuário

            List<Claim> claims =
            [
                new(ClaimTypes.Name, email),
                new(ClaimTypes.Email, email),
                new(ClaimTypes.Role, role)
            ];

            #endregion Claims de usuário

            #region Configuração/Geração do Token

            JwtSecurityToken token = new(
                issuer: issuer,
                audience: audience,
                expires: DateTime.Now.AddHours(8),
                signingCredentials: credentials,
                claims: claims);

            JwtSecurityTokenHandler tokenHandler = new();
            string stringToken = tokenHandler.WriteToken(token);

            #endregion Configuração/Geração do Token

            return stringToken;
        }
    }
}
