using AthenasAcademy.GeradorCertificado.Models;

namespace AthenasAcademy.GeradorCertificado.Services.Interfaces
{
    public interface IPNGService
    {
        PNGDetalhesModel GerarPNG(string nomeArquivoPNG, string caminhoArquivoQrCode, string textoCertificado, string caminhoArquivo, string caminhoArquivoModelo);
    }
}
