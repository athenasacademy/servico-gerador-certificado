using System.Threading.Tasks;

namespace AthenasAcademy.GeradorCertificado.Services
{
    public interface ITokenService
    {
        Task<bool> ValidarToken(string token);
    }
}
