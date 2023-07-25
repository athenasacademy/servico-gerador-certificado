using AthenasAcademy.GeradorCertificado.Models;
using AthenasAcademy.GeradorCertificado.Services.Interfaces;
using System;
using System.Drawing.Imaging;
using System.Drawing;
using System.IO;
using AthenasAcademy.GeradorCertificado.Repositories;
using System.Threading.Tasks;

namespace AthenasAcademy.GeradorCertificado.Services
{
    public class PNGService : IPNGService
    {
        #region Construtores
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
        #endregion

        #region Métodos Públicos
        public async Task<PNGDetalhesModel> GerarPNG(string nomeArquivoPNG, string caminhoArquivoQrCode, string textoCertificado, string caminhoArquivo, string caminhoArquivoModelo, bool armazenarNoBucket = true)
        {
            try
            {
                PNGDetalhesModel png = Gerar(nomeArquivoPNG, caminhoArquivoQrCode, textoCertificado, caminhoArquivo, caminhoArquivoModelo);

                if (armazenarNoBucket)
                    await AwsS3Repository.Instancia.EnviarPNGAsync(png, "certificados/PNG");

                return png;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar o PNG: " + ex.Message, ex);
            }
        }
        #endregion

        #region Métodos Privados
        private PNGDetalhesModel Gerar(string nomeArquivoPNG, string caminhoArquivoQrCode, string textoCertificado, string caminhoArquivo, string caminhoArquivoModelo)
        {
            try
            {
                using (Bitmap originalImagem = new Bitmap(caminhoArquivoModelo))
                {
                    using (Bitmap novaImagem = CloneImagem(originalImagem))
                    {
                        using (Graphics graphics = Graphics.FromImage(novaImagem))
                        {
                            DesenharTextoCertificado(graphics, textoCertificado, novaImagem);

                            if (!string.IsNullOrEmpty(caminhoArquivoQrCode) && File.Exists(caminhoArquivoQrCode))
                            {
                                using (Bitmap pngImagem = new Bitmap(caminhoArquivoQrCode))
                                {
                                    DesenharImagemPNG(graphics, pngImagem, novaImagem);
                                }
                            }

                            SalvarImagem(caminhoArquivo, novaImagem);

                            byte[] bytesImagem = RecuperarBytesDaImagem(novaImagem);

                            return new PNGDetalhesModel
                            {
                                PNGBase64 = Convert.ToBase64String(bytesImagem),
                                NomeArquivo = Path.GetFileName(caminhoArquivo),
                                CaminhoArquivo = caminhoArquivo,
                                TamanhoArquivo = ObterTamanhoArquivo(caminhoArquivo)
                            };
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar a imagem PNG: " + ex.Message, ex);
            }
        }

        private Bitmap CloneImagem(Bitmap originalImagem)
        {
            try
            {
                return originalImagem.Clone(new System.Drawing.Rectangle(0, 0, originalImagem.Width, originalImagem.Height), PixelFormat.Format32bppArgb);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao clonar a imagem: " + ex.Message, ex);
            }
        }

        private void DesenharTextoCertificado(Graphics graphics, string textoCertificado, Bitmap novaImagem)
        {
            try
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
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao desenhar o texto do certificado: " + ex.Message, ex);
            }
        }

        private void DesenharImagemPNG(Graphics graphics, Bitmap pngImagem, Bitmap novaImagem)
        {
            try
            {
                int pngX = novaImagem.Width - pngImagem.Width - 292;
                int pngY = novaImagem.Height - pngImagem.Height - 37;
                graphics.DrawImage(pngImagem, new System.Drawing.Rectangle(pngX, pngY, pngImagem.Width, pngImagem.Height));
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao desenhar a imagem PNG: " + ex.Message, ex);
            }
        }

        private void SalvarImagem(string caminhoArquivo, Bitmap novaImagem)
        {
            try
            {
                using (FileStream fs = new FileStream(caminhoArquivo, FileMode.Create))
                {
                    novaImagem.Save(fs, ImageFormat.Png);
                    fs.Close();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao salvar a imagem: " + ex.Message, ex);
            }
        }

        private byte[] RecuperarBytesDaImagem(Bitmap bitmap)
        {
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    bitmap.Save(stream, ImageFormat.Png);
                    stream.Close();
                    return stream.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao recuperar os bytes da imagem: " + ex.Message, ex);
            }
        }

        private string ObterTamanhoArquivo(string caminhoArquivo)
        {
            try
            {
                return new FileInfo(caminhoArquivo).Length.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao obter o tamanho do arquivo: " + ex.Message, ex);
            }
        }
        #endregion
    }
}