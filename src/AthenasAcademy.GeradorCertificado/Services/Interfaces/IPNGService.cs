using AthenasAcademy.GeradorCertificado.Models;
using System.Threading.Tasks;

namespace AthenasAcademy.GeradorCertificado.Services.Interfaces
{
    public interface IPNGService
    {
        Task<PNGDetalhesModel> GerarPNG(string nomeArquivoPNG, string caminhoArquivoQrCode, string textoCertificado, string caminhoArquivo, string caminhoArquivoModelo, bool armazenarNoBucket = true);
    }
}
