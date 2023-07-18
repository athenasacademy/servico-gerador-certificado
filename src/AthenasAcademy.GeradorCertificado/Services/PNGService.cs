using AthenasAcademy.GeradorCertificado.Models;
using AthenasAcademy.GeradorCertificado.Services.Interfaces;
using System;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;

namespace AthenasAcademy.GeradorCertificado.Services
{
    public class PNGService : IPNGService
    {
        private static PNGService instancia;

        public static PNGService Instancia
        {
            get
            {
                if (instancia is null)
                    instancia = new PNGService();

                return instancia;
            }
        }

        public PNGDetalhesModel GerarPNG(string nomeArquivoPNG, string caminhoArquivoQrCode, string textoCertificado, string caminhoArquivo, string caminhoArquivoModelo)
        {
            return Gerar(nomeArquivoPNG, caminhoArquivoQrCode, textoCertificado, caminhoArquivo, caminhoArquivoModelo);
        }

        private PNGDetalhesModel Gerar(string nomeArquivoPNG, string caminhoArquivoQrCode, string textoCertificado, string caminhoArquivo, string caminhoArquivoModelo)
        {
            using (Bitmap originalImagem = new Bitmap(caminhoArquivoModelo))
            {
                using (Bitmap novaImagem = originalImagem.Clone(new System.Drawing.Rectangle(0, 0, originalImagem.Width, originalImagem.Height), PixelFormat.Format32bppArgb))
                {
                    using (Graphics graphics = Graphics.FromImage(novaImagem))
                    {
                        string[] linhasTexto = textoCertificado.Split('\n');

                        for (int i = 0; i < linhasTexto.Length; i++)
                        {
                            var textFont = new System.Drawing.Font("Arial", i == 0 ? 24 : 14, FontStyle.Regular);
                            Color textColor = Color.Black;

                            float x = (novaImagem.Width - graphics.MeasureString(linhasTexto[i], textFont).Width) / 2;
                            float y = ((novaImagem.Height - graphics.MeasureString(linhasTexto[i], textFont).Height) / 2) - 30;

                            float posLine = y + i == 0 ? 0 : (i * 20);

                            graphics.DrawString(linhasTexto[i], textFont, new SolidBrush(textColor), x, y + posLine);
                        }

                        // Carrega a imagem PNG para inserir no canto inferior direito
                        if (!string.IsNullOrEmpty(caminhoArquivoQrCode) && File.Exists(caminhoArquivoQrCode))
                        {
                            using (Bitmap pngImagem = new Bitmap(caminhoArquivoQrCode))
                            {
                                int pngX = novaImagem.Width - pngImagem.Width - 292;
                                int pngY = novaImagem.Height - pngImagem.Height - 37;
                                graphics.DrawImage(pngImagem, new System.Drawing.Rectangle(pngX, pngY, pngImagem.Width, pngImagem.Height));
                            }
                        }

                        using (FileStream fs = new FileStream(caminhoArquivo, FileMode.Create))
                        {
                            novaImagem.Save(fs, ImageFormat.Png);
                        }

                        byte[] bytesImagem = GerenciadorArquivosService.Instancia.RecuperarBytesDaImagem(novaImagem);

                        return new PNGDetalhesModel
                        {
                            PNGBase64 = Convert.ToBase64String(bytesImagem),
                            NomeArquivo = Path.GetFileName(caminhoArquivo),
                            CaminhoArquivo = caminhoArquivo,
                            TamanhoArquivo = GerenciadorArquivosService.Instancia.ObterTamanhoArquivo(caminhoArquivo)
                        };
                    }
                }
            }
        }
    }
}