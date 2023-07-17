using AthenasAcademy.GeradorCertificado.Dominio.Requests;
using AthenasAcademy.GeradorCertificado.Dominio.Responses;
using AthenasAcademy.GeradorCertificado.Models;
using AthenasAcademy.GeradorCertificado.Services.Interfaces;
using System;
using System.Drawing.Imaging;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace AthenasAcademy.GeradorCertificado.Services
{
    public class GeradorCertificadoService : IGeradorCertificadoService
    {
        private const string CAMINHO_BASE_ARQUIVO = "certificado";
        private const string CAMINHO_BASE_ARQUIVO_MODELO = "certificado:modelo:ATHENASACADEMY_horizontal.png";

        #region Contrutores
        private static GeradorCertificadoService instancia;

        public static GeradorCertificadoService Instancia
        {
            get {
                if (instancia is null)
                    instancia = new GeradorCertificadoService();

                return instancia; 
            }
        }

        private GeradorCertificadoService()
        {
            string caminhoBaseArquivo = Path.Combine(Path.GetTempPath(), CAMINHO_BASE_ARQUIVO);

            if (!Directory.Exists(caminhoBaseArquivo))
                Directory.CreateDirectory(caminhoBaseArquivo);
        }
        #endregion

        #region Métodos públicos
        public Task<CertificadoResponse> ObterCertificado(CertificadoRequest request)
        {
            // Implementação para obter um certificado com base nos dados fornecidos
            throw new NotImplementedException();
        }

        public async Task<NovoCertificadoResponse> GerarCertificado(NovoCertificadoRequest request)
        {
            string nomeArquivoPNG = "CERTIFICADO_" + request.Matricula.ToString("D10") + ".png";
            string nomeArquivoQRCode = "QRCODE_" + request.Matricula.ToString("D10") + ".png";
            string nomeArquivoPDF = "CERTIFICADO_" + request.Matricula.ToString("D10") + ".pdf";

            string textoCertificado = GerarTexto(request);

            QrCodeDetalhesModel qrCodeDetalhesModel = GerarQRCode(nomeArquivoQRCode, request.Matricula, request.CodigoCurso);
            PNGDetalhesModel pngDetalhesModel = GerarPNG(nomeArquivoPNG, qrCodeDetalhesModel.CaminhoArquivo, textoCertificado);
            PDFDetalhesModel pdfDetalhesModel = GerarPDF(nomeArquivoPDF, pngDetalhesModel.CaminhoArquivo);

            GerarNovoCertificadoModel novoCertificado = new GerarNovoCertificadoModel()
            {
                QrCode = qrCodeDetalhesModel,
                PNG = pngDetalhesModel,
                PDF = pdfDetalhesModel,
            };
            
            return await Task.FromResult(new NovoCertificadoResponse());
        }
        #endregion

        #region Métodos Privados
        private string FormatarDetahesAluno(string nomeAluno)
        {
            TextInfo textInfo = new CultureInfo("en-US", false).TextInfo;
            nomeAluno = textInfo.ToTitleCase(nomeAluno.ToLower());

            string[] sNomes = nomeAluno.Split(' ');

            if (sNomes.Count() == 0)
                return sNomes[0];

            for (int i = 0; i < sNomes.Count(); i++)
            {
                sNomes[i] = RemoverParticulas(sNomes[i]);
            }

            for (int i = 0; i < sNomes.Count() - 1; i++)
            {
                if (i > 0)
                    sNomes[i] = AbreviarSobrenome(sNomes[i]);
            }

            sNomes = sNomes.Where(s => !string.IsNullOrEmpty(s)).ToArray();

            return string.Join(" ", sNomes);
        }

        private string FormatarDetahesMatriculaAluno(int matriculaAluno)
        {
            return string.Format("Matrícula: {0}", matriculaAluno.ToString("D10"));
        }

        public string RemoverParticulas(string sobrenome)
        {
            string[] particulas = { "De", "Dos", "Da", "Das" };

            foreach (string particula in particulas)
            {
                if (sobrenome.Equals(particula, StringComparison.OrdinalIgnoreCase))
                    sobrenome = string.Empty;
            }

            return sobrenome;
        }

        public string AbreviarSobrenome(string sobrenome)
        {
            if (sobrenome.Length > 0)
                sobrenome = $"{sobrenome[0]}.";

            return sobrenome;
        }

        private string FormatarDetahesCurso(string nomeCurso, int codCurso, DateTime dataConclusao, int cargaHoraria)
        {
            nomeCurso = nomeCurso.ToUpper();
            string cargaHorariaCurso = cargaHoraria.ToString() + " horas";
            string codigoCurso = codCurso.ToString("D10");

            if (nomeCurso.Length < 25)
            {
                return string.Format(
                    "concluiu em {0} o curso '{1}' código: {2} com carga horária total de {3}",
                    dataConclusao.ToString("dd/MM/yyyy"),
                    nomeCurso,
                    codigoCurso,
                    cargaHorariaCurso);
            }

            else if (nomeCurso.Length > 25 && nomeCurso.Length < 50)
            {
                return string.Format(
                    "concluiu em {0} o curso '{1}' código: {2}{3} com carga horária total de {4}",
                    dataConclusao.ToString("dd/MM/yyyy"),
                    nomeCurso,
                    codigoCurso,
                    Environment.NewLine,
                    cargaHorariaCurso);
            }

            else
            {
                return string.Format(
                    "concluiu em {0} o curso '{1}'{2} código: {3} com carga horária total de {4}",
                    dataConclusao.ToString("dd/MM/yyyy"),
                    nomeCurso,
                    Environment.NewLine,
                    codigoCurso,
                    cargaHorariaCurso);
            }
        }

        private string FormatarDetahesAproveitamentoCurso(int taxaAproveitamento) 
        {
            return string.Format("ministrado pela Athenas Academy, onde obteve um aproveitamento de {0}%", taxaAproveitamento.ToString());
        }

        private string GerarTexto(NovoCertificadoRequest request)
        {
            string msgAluno = FormatarDetahesAluno(request.NomeAluno);
            string msgMatricula = FormatarDetahesMatriculaAluno(request.Matricula);
            string msgCurso = FormatarDetahesCurso(request.NomeCurso, request.CodigoCurso, request.DataConclusao, request.CargaHoraria);
            string msgAproveitamento = FormatarDetahesAproveitamentoCurso(request.Aproveitamento);

            StringBuilder sb = new StringBuilder();

            sb.AppendLine(msgAluno);
            sb.AppendLine();
            sb.AppendLine(msgMatricula);
            sb.AppendLine();
            sb.AppendLine(msgCurso);
            sb.AppendLine();
            sb.AppendLine(msgAproveitamento);

            return sb.ToString();
        }

        private string RecuperarCaminhoArquivo(string nomeArquivoPNG) => Path.Combine(Path.Combine(Path.GetTempPath(), CAMINHO_BASE_ARQUIVO), nomeArquivoPNG);

        private string RecuperarCaminhoArquivoModelo() => Path.Combine(Path.GetTempPath(), CAMINHO_BASE_ARQUIVO_MODELO.Replace(':', Path.DirectorySeparatorChar));

        private byte[] RecuperarBytesDaImagem(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Png);
                return stream.ToArray();
            }
        }

        private QrCodeDetalhesModel GerarQRCode(string nomeArquivoQRCode, int matricula, int codigoCurso)
        {
            UriBuilder uriBuilder = new UriBuilder("https://athenas-academy.tech");
            uriBuilder.Path = "/validar/" + matricula.ToString("D10") + codigoCurso.ToString("D10");

            string textoCode = uriBuilder.Uri.ToString();

            return QRCodeService.Gerar(textoCode, nomeArquivoQRCode, CAMINHO_BASE_ARQUIVO);
        }

        private PNGDetalhesModel GerarPNG(string nomeArquivoPNG, string caminhoArquivoQrCode, string textoCertificado)
        {
            string caminhoArquivo = RecuperarCaminhoArquivo(nomeArquivoPNG);
            string caminhoArquivoModelo = RecuperarCaminhoArquivoModelo();

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

                        byte[] bytesImagem = RecuperarBytesDaImagem(novaImagem);

                        return new PNGDetalhesModel
                        {
                            PNGBase64 = Convert.ToBase64String(bytesImagem),
                            NomeArquivo = Path.GetFileName(caminhoArquivo),
                            CaminhoArquivo = caminhoArquivo,
                            TamanhoArquivo = new FileInfo(caminhoArquivo).Length.ToString()
                        };
                    }
                }
            }
        }

        private PDFDetalhesModel GerarPDF(string nomeArquivoPDF, string caminhoArquivo)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
