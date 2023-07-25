using System.Threading.Tasks;

namespace AthenasAcademy.GeradorCertificado.Services
{
    /// <summary>
    /// Interface que define os métodos para validação de tokens de autenticação.
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Valida o token de autenticação.
        /// </summary>
        /// <param name="token">O token de autenticação a ser validado.</param>
        /// <returns>Um valor booleano indicando se o token é válido (true) ou inválido (false).</returns>
        Task<bool> ValidarToken(string token);
    }
}
