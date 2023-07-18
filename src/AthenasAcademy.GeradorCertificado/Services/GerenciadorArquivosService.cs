using AthenasAcademy.GeradorCertificado.Services.Interfaces;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace AthenasAcademy.GeradorCertificado.Services
{
    public class GerenciadorArquivosService : IGerenciadorArquivosService
    {
        private const string CAMINHO_BASE_ARQUIVO = "certificado";
        private const string CAMINHO_BASE_ARQUIVO_MODELO = "certificado:modelo:ATHENASACADEMY_horizontal_v2.png";
        private static GerenciadorArquivosService instancia;

        public static GerenciadorArquivosService Instancia
        {
            get {
                if (instancia is null)
                    instancia = new GerenciadorArquivosService();

                return instancia; 
            }
        }

        public void LimparCaminhoBase()
        {
            string caminhoSrc = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\"));
            string caminhoBase = Path.Combine(caminhoSrc, CAMINHO_BASE_ARQUIVO);

            string[] arquivos = Directory.GetFiles(caminhoBase);

            foreach (string arquivo in arquivos)
            {
                File.Delete(arquivo);
            }
        }

        public void LimparCaminhoBase(string[] caminhosArquivos)
        {
            if (caminhosArquivos == null || caminhosArquivos.Length == 0)
                return;

            foreach (var arquivo in caminhosArquivos)
            {
                if (File.Exists(arquivo))
                    File.Delete(arquivo);
            }
        }

        public string ObterTamanhoArquivo(string caminhoArquivo) => new FileInfo(caminhoArquivo).Length.ToString();

        public string RecuperarCaminhoBase()
        {
            string caminhoSrc = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\"));
            string caminhoBase = Path.Combine(caminhoSrc, CAMINHO_BASE_ARQUIVO);

            if (Directory.Exists(caminhoBase))
                return caminhoBase;

            throw new IOException(string.Format("Dados base para emissão do certificado não foram encontrados na convensão '{0}'", CAMINHO_BASE_ARQUIVO));
        }

        public string RecuperarCaminhoArquivo(string nomeArquivo, bool exception)
        {
            string caminhoSrc = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\"));
            string caminhoBase = Path.Combine(caminhoSrc, CAMINHO_BASE_ARQUIVO);

            if (Directory.Exists(caminhoBase))
                return Path.Combine(caminhoBase, nomeArquivo);

            if (exception)
                throw new IOException(string.Format("Dados base para emissão do certificado não foram encontrados na convensão '{0}'", CAMINHO_BASE_ARQUIVO));

            return string.Empty;
        }

        public string RecuperarCaminhoArquivoModelo()
        {
            string caminhoSrc = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\"));
            string caminhoBase = Path.Combine(caminhoSrc, CAMINHO_BASE_ARQUIVO_MODELO.Replace(':', Path.DirectorySeparatorChar));

            if (File.Exists(caminhoBase))
                return caminhoBase;

            throw new IOException(string.Format("Dados base para emissão do certificado não foram encontrados na convensão '{0}'", CAMINHO_BASE_ARQUIVO_MODELO));
        }

        public byte[] RecuperarBytesDaImagem(Bitmap bitmap)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                bitmap.Save(stream, ImageFormat.Png);
                return stream.ToArray();
            }
        }

        public string ConverterParaBase64(string caminhoArquivo) => Convert.ToBase64String(File.ReadAllBytes(caminhoArquivo));

    }
}