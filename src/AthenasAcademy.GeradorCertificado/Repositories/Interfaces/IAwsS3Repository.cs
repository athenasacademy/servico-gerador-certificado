using AthenasAcademy.GeradorCertificado.Models;
using System.Threading.Tasks;

namespace AthenasAcademy.GeradorCertificado.Repositories.Interfaces
{
    /// <summary>
    /// Interface que define os métodos para o repositório de interação com o serviço AWS S3.
    /// </summary>
    public interface IAwsS3Repository
    {
        /// <summary>
        /// Envia um arquivo PDF para o bucket especificado no serviço AWS S3.
        /// </summary>
        /// <param name="pdfDetalhes">As informações detalhadas do arquivo PDF a ser enviado.</param>
        /// <param name="bucket">O nome do bucket no serviço AWS S3.</param>
        /// <returns>A URL do objeto enviado.</returns>
        Task<string> EnviarPDFAsync(PDFDetalhesModel pdfDetalhes, string bucket);

        /// <summary>
        /// Envia um arquivo PNG para o bucket especificado no serviço AWS S3.
        /// </summary>
        /// <param name="pngDetalhes">As informações detalhadas do arquivo PNG a ser enviado.</param>
        /// <param name="bucket">O nome do bucket no serviço AWS S3.</param>
        /// <returns>A URL do objeto enviado.</returns>
        Task<string> EnviarPNGAsync(PNGDetalhesModel pngDetalhes, string bucket);

        /// <summary>
        /// Gera uma URL de acesso público para o objeto especificado no serviço AWS S3.
        /// </summary>
        /// <param name="objeto">O nome do objeto no serviço AWS S3.</param>
        /// <param name="bucket">O nome do bucket no serviço AWS S3.</param>
        /// <returns>A URL de acesso público para o objeto.</returns>
        Task<string> GerarURIAsync(string objeto, string bucket);
    }
}
