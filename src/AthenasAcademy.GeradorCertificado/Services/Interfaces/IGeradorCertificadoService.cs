using AthenasAcademy.GeradorCertificado.Dominio.Requests;
using AthenasAcademy.GeradorCertificado.Dominio.Responses;
using System.Threading.Tasks;

namespace AthenasAcademy.GeradorCertificado.Services.Interfaces
{
    public interface IGeradorCertificadoService
    {
        Task<NovoCertificadoPDFResponse> GerarCertificadoPDF(NovoCertificadoRequest request);
    }
}
