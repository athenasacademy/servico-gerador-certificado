using AthenasAcademy.GeradorCertificado.Models;

namespace AthenasAcademy.GeradorCertificado.Services.Interfaces
{
    /// <summary>
    /// Interface que define o método para a geração de um código QR.
    /// </summary>
    public interface IQRCodeService
    {
        /// <summary>
        /// Gera um código QR com base nas informações fornecidas.
        /// </summary>
        /// <param name="nomeArquivoQRCode">O nome do arquivo QR Code a ser gerado.</param>
        /// <param name="matricula">O número de matrícula a ser incluído no código QR.</param>
        /// <param name="codigoCurso">O código do curso a ser incluído no código QR.</param>
        /// <param name="caminhoSaida">O caminho de saída onde o arquivo QR Code será gerado.</param>
        /// <returns>Um objeto QrCodeDetalhesModel que contém informações sobre o arquivo QR Code gerado.</returns>
        QrCodeDetalhesModel GerarQRCode(string nomeArquivoQRCode, int matricula, int codigoCurso, string caminhoSaida);
    }
}
