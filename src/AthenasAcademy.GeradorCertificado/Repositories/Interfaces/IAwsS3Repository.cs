using AthenasAcademy.GeradorCertificado.Models;
using System.Threading.Tasks;

namespace AthenasAcademy.GeradorCertificado.Repositories.Interfaces
{
    public interface IAwsS3Repository
    {
        Task<string> EnviarPDFAsync(PDFDetalhesModel pdfDetalhes, string bucket);

        Task<string> EnviarPNGAsync(PNGDetalhesModel pdfDetalhes, string bucket);

        Task<string> GerarURIAsync(string objeto, string bucket);
    }
}
