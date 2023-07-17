using AthenasAcademy.GeradorCertificado.Models;
using QRCoder;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace AthenasAcademy.GeradorCertificado.Services
{
    public static class QRCodeService
    {
        public static QrCodeDetalhesModel Gerar(string texto, string arquivoSaida, string caminhoSaida = "")
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(texto, QRCodeGenerator.ECCLevel.Q);

            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(10);

            QrCodeDetalhesModel qrCodeDetalhes = new QrCodeDetalhesModel();

            if (!string.IsNullOrEmpty(caminhoSaida))
            {
                string path = Path.Combine(Path.Combine(Path.GetTempPath(), caminhoSaida), arquivoSaida);

                // Redimensiona a imagem para 150x150 pixels antes de salvar
                using (Bitmap qrCodeResized = new Bitmap(qrCodeImage, new Size(150, 150)))
                {
                    qrCodeResized.Save(path, ImageFormat.Png);
                }

                qrCodeDetalhes.NomeArquivo = Path.GetFileName(path);
                qrCodeDetalhes.CaminhoArquivo = path;
                qrCodeDetalhes.TamanhoArquivo = new FileInfo(path).Length.ToString();
            }

            // Converte a imagem para base64 e preenche o PNGBase64 no modelo
            using (MemoryStream memoryStream = new MemoryStream())
            {
                qrCodeImage.Save(memoryStream, ImageFormat.Png);
                qrCodeDetalhes.PNGBase64 = Convert.ToBase64String(memoryStream.ToArray());
            }

            return qrCodeDetalhes;
        }
    }
}