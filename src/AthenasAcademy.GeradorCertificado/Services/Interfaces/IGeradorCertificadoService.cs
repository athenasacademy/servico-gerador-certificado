using AthenasAcademy.GeradorCertificado.Dominio.Requests;
using AthenasAcademy.GeradorCertificado.Dominio.Responses;
using System.Threading.Tasks;

namespace AthenasAcademy.GeradorCertificado.Services.Interfaces
{
    /// <summary>
    /// Interface que define os métodos para o serviço de geração de certificados em PDF.
    /// </summary>
    public interface IGeradorCertificadoService
    {
        /// <summary>
        /// Gera um novo certificado em formato PDF com base nas informações fornecidas.
        /// </summary>
        /// <param name="request">As informações necessárias para a geração do certificado.</param>
        /// <returns>Um objeto do tipo <see cref="NovoCertificadoPDFResponse"/> contendo os dados do certificado gerado.</returns>
        Task<NovoCertificadoPDFResponse> GerarCertificadoPDF(NovoCertificadoRequest request);
    }
}
