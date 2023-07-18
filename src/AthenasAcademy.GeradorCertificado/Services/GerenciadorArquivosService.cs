using AthenasAcademy.GeradorCertificado.Services.Interfaces;
using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;
using System.Web;

namespace AthenasAcademy.GeradorCertificado.Services
{
    public class GerenciadorArquivosService : IGerenciadorArquivosService
    {
        private string CAMINHO_BASE_ARQUIVO = HttpContext.Current.Server.MapPath("~/Files/");
        private string CAMINHO_BASE_ARQUIVO_MODELO = HttpContext.Current.Server.MapPath("~/Files/Model/") + "ATHENASACADEMY_horizontal_v2.png";
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
            if (Directory.Exists(CAMINHO_BASE_ARQUIVO))
                return CAMINHO_BASE_ARQUIVO;

            throw new IOException(string.Format("Dados base para emissão do certificado não foram encontrados na convensão '{0}'", CAMINHO_BASE_ARQUIVO));
        }

        public string GerarCaminhoArquivo(string nomeArquivo, bool exception)
        {
            if (Directory.Exists(CAMINHO_BASE_ARQUIVO))
                return Path.Combine(CAMINHO_BASE_ARQUIVO, nomeArquivo);

            if (exception)
                throw new IOException(string.Format("Dados base para emissão do certificado não foram encontrados na convensão '{0}'", CAMINHO_BASE_ARQUIVO));

            return string.Empty;
        }

        public string RecuperarCaminhoArquivoModelo()
        {
            if (File.Exists(CAMINHO_BASE_ARQUIVO_MODELO))
                return CAMINHO_BASE_ARQUIVO_MODELO;

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

        public async Task<byte[]> RecuperarBytesArquivoAsync(string caminhoArquivo)
        {
            try
            {
                using (FileStream fileStream = new FileStream(caminhoArquivo, FileMode.Open, FileAccess.Read))
                {
                    byte[] conteudoArquivo = new byte[fileStream.Length];
                    await fileStream.ReadAsync(conteudoArquivo, 0, (int)fileStream.Length);
                    return conteudoArquivo;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao ler o arquivo de forma assíncrona: " + ex.Message);
                return null;
            }
        }
    }
}