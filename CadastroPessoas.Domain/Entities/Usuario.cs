using CadastroPessoas.Domain.Enums;

namespace CadastroPessoas.Domain.Entities
{
    public class Usuario
    {
        public Usuario(string login, string tipo)
        {
            Login = login;
            Tipo = tipo;
        }

        public Usuario(string login, string senhaHash, string tipo)
        {
            Login = login;
            SenhaHash = senhaHash;
            Tipo = tipo;
        }

        public Usuario(int id, string login, string tipo, bool status, DateTime dataCadastro)
        {
            Id = id;
            Login = login;
            Tipo = tipo;
            Status = ConverteStatusToEnum(status);
            DataCadastro = dataCadastro;
        }

        public int Id { get; private set; }

        public string Login { get; private set; }

        public string SenhaHash { get; private set; } = null!;

        public string Tipo { get; private set; } // Tipos: General, Capitão, Sargento, Soldado

        public EStatusUsuario Status { get; private set; } = EStatusUsuario.Ativo;

        public DateTime DataCadastro { get; set; } = DateTime.Now;

        private static EStatusUsuario ConverteStatusToEnum(bool status)
        {
            return status
                ? EStatusUsuario.Ativo
                : EStatusUsuario.Desativado;
        }
    }
}
