using AthenasAcademy.GeradorCertificado.Models;

namespace AthenasAcademy.GeradorCertificado.Services.Interfaces
{
    public interface IQRCodeService
    {
        QrCodeDetalhesModel GerarQRCode(string nomeArquivoQRCode, int matricula, int codigoCurso, string caminhoSaida);
    }
}
