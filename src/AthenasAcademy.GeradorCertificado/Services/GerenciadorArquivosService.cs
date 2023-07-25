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
        #region Dependencias
        private string CAMINHO_BASE_ARQUIVO;
        private string CAMINHO_BASE_ARQUIVO_MODELO;
        private static GerenciadorArquivosService instancia;
        #endregion

        #region Construtores
        public static GerenciadorArquivosService Instancia
        {
            get
            {
                if (instancia is null)
                    instancia = new GerenciadorArquivosService(
                        HttpContext.Current.Server.MapPath("~/Files/"),
                        HttpContext.Current.Server.MapPath("~/Files/Model/") + "ATHENASACADEMY_horizontal_v2.png");

                return instancia;
            }
        }

        public GerenciadorArquivosService(string caminhoBaseArquivo, string caminhoBaseArquivoModelo)
        {
            CAMINHO_BASE_ARQUIVO = caminhoBaseArquivo;
            CAMINHO_BASE_ARQUIVO_MODELO = caminhoBaseArquivoModelo;
        }
        #endregion

        #region Métodos Públicos
        public void LimparCaminhoBase()
        {
            try
            {
                string caminhoSrc = Path.GetFullPath(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\"));
                string caminhoBase = Path.Combine(caminhoSrc, CAMINHO_BASE_ARQUIVO);

                string[] arquivos = Directory.GetFiles(caminhoBase);

                foreach (string arquivo in arquivos)
                {
                    try
                    {
                        using (var stream = new FileStream(arquivo, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                        {
                            File.Delete(arquivo);
                            Console.WriteLine($"Arquivo {arquivo} excluído com sucesso.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Erro ao excluir o arquivo {arquivo}: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao limpar o caminho base: " + ex.Message, ex);
            }
        }

        public void LimparCaminhoBase(string[] caminhosArquivos)
        {
            try
            {
                if (caminhosArquivos == null || caminhosArquivos.Length == 0)
                    return;

                foreach (var arquivo in caminhosArquivos)
                {
                    if (File.Exists(arquivo))
                        File.Delete(arquivo);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao limpar o caminho base: " + ex.Message, ex);
            }
        }

        public string ObterTamanhoArquivo(string caminhoArquivo)
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

        public string RecuperarCaminhoBase()
        {
            try
            {
                if (Directory.Exists(CAMINHO_BASE_ARQUIVO))
                    return CAMINHO_BASE_ARQUIVO;

                throw new IOException(string.Format("Dados base para emissão do certificado não foram encontrados na convensão '{0}'", CAMINHO_BASE_ARQUIVO));
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao recuperar o caminho base: " + ex.Message, ex);
            }
        }

        public string GerarCaminhoArquivo(string nomeArquivo, bool exception)
        {
            try
            {
                if (Directory.Exists(CAMINHO_BASE_ARQUIVO))
                    return Path.Combine(CAMINHO_BASE_ARQUIVO, nomeArquivo);

                if (exception)
                    throw new IOException(string.Format("Dados base para emissão do certificado não foram encontrados na convensão '{0}'", CAMINHO_BASE_ARQUIVO));

                return string.Empty;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao gerar o caminho do arquivo: " + ex.Message, ex);
            }
        }

        public string RecuperarCaminhoArquivoModelo()
        {
            try
            {
                if (File.Exists(CAMINHO_BASE_ARQUIVO_MODELO))
                    return CAMINHO_BASE_ARQUIVO_MODELO;

                throw new IOException(string.Format("Dados base para emissão do certificado não foram encontrados na convensão '{0}'", CAMINHO_BASE_ARQUIVO_MODELO));
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao recuperar o caminho do arquivo modelo: " + ex.Message, ex);
            }
        }

        public byte[] RecuperarBytesDaImagem(Bitmap bitmap)
        {
            try
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    bitmap.Save(stream, ImageFormat.Png);
                    return stream.ToArray();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao recuperar os bytes da imagem: " + ex.Message, ex);
            }
        }

        public string ConverterParaBase64(string caminhoArquivo)
        {
            try
            {
                return Convert.ToBase64String(File.ReadAllBytes(caminhoArquivo));
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao converter o arquivo para Base64: " + ex.Message, ex);
            }
        }

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
                throw new Exception("Erro ao ler o arquivo de forma assíncrona: " + ex.Message, ex);
            }
        }
        #endregion
    }
}