using AthenasAcademy.GeradorCertificado.Models;
using System.Threading.Tasks;

namespace AthenasAcademy.GeradorCertificado.Services.Interfaces
{
    /// <summary>
    /// Interface que define os métodos para a geração de arquivos PNG.
    /// </summary>
    public interface IPNGService
    {
        /// <summary>
        /// Gera um arquivo PNG com base nas informações fornecidas.
        /// </summary>
        /// <param name="nomeArquivoPNG">O nome do arquivo PNG a ser gerado.</param>
        /// <param name="caminhoArquivoQrCode">O caminho do arquivo QR Code a ser utilizado.</param>
        /// <param name="textoCertificado">O texto do certificado a ser incluído no PNG.</param>
        /// <param name="caminhoArquivo">O caminho do arquivo PNG a ser gerado.</param>
        /// <param name="caminhoArquivoModelo">O caminho do arquivo de modelo a ser utilizado.</param>
        /// <param name="armazenarNoBucket">Indica se o arquivo PNG gerado deve ser armazenado no bucket (padrão é true).</param>
        /// <returns>Um objeto PNGDetalhesModel que contém informações sobre o arquivo PNG gerado.</returns>
        Task<PNGDetalhesModel> GerarPNG(string nomeArquivoPNG, string caminhoArquivoQrCode, string textoCertificado, string caminhoArquivo, string caminhoArquivoModelo, bool armazenarNoBucket = true);
    }
}
