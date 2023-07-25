using AthenasAcademy.GeradorCertificado.Exceptions;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AthenasAcademy.GeradorCertificado.Services
{
    public class TokenService : ITokenService
    {
        private const string KEY_REQUEST = "2387fasdfhoa48qyhgnqa;r/zsd-40";

        #region Construtores
        private static TokenService instancia;

        public static TokenService Instancia
        {
            get
            {
                if (instancia is null)
                    instancia = new TokenService();

                return instancia;
            }
        }
        #endregion

        public Task<bool> ValidarToken(string token)
        {
            return Task.FromResult(
                ValidarHashDeSenhaSHA256(token));
        }

        private string GerarHashDeSenhaSHA256(string token)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] passwordBytes = Encoding.UTF8.GetBytes(token);
                byte[] hashedBytes = sha256.ComputeHash(passwordBytes);

                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashedBytes)
                {
                    sb.Append(b.ToString("x2"));
                }

                return sb.ToString();
            }
        }

        private bool ValidarHashDeSenhaSHA256(string tokenRequest, bool lancarException = false)
        {
            string tokenHashCalculada = GerarHashDeSenhaSHA256(KEY_REQUEST);
            bool validacao = tokenRequest == tokenHashCalculada;

            if (!validacao && lancarException)
                throw new APICustomException(message: "Autenticação Inválido!");

            return validacao;
        }

    }
}