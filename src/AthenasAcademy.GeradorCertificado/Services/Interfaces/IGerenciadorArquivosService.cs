using System.Drawing;
using System.Threading.Tasks;

namespace AthenasAcademy.GeradorCertificado.Services.Interfaces
{
    /// <summary>
    /// Interface que define os métodos para o gerenciamento de arquivos.
    /// </summary>
    public interface IGerenciadorArquivosService
    {
        /// <summary>
        /// Limpa o caminho base de arquivos.
        /// </summary>
        void LimparCaminhoBase();

        /// <summary>
        /// Limpa os caminhos de arquivos especificados.
        /// </summary>
        /// <param name="caminhosArquivos">Os caminhos dos arquivos a serem excluídos.</param>
        void LimparCaminhoBase(string[] caminhosArquivos);

        /// <summary>
        /// Obtém o tamanho do arquivo no caminho especificado.
        /// </summary>
        /// <param name="caminhoArquivo">O caminho do arquivo.</param>
        /// <returns>O tamanho do arquivo em bytes.</returns>
        string ObterTamanhoArquivo(string caminhoArquivo);

        /// <summary>
        /// Recupera os bytes de uma imagem representada como um Bitmap.
        /// </summary>
        /// <param name="bitmap">O Bitmap da imagem.</param>
        /// <returns>Os bytes da imagem.</returns>
        byte[] RecuperarBytesDaImagem(Bitmap bitmap);

        /// <summary>
        /// Recupera o caminho do arquivo do modelo utilizado.
        /// </summary>
        /// <returns>O caminho do arquivo do modelo.</returns>
        string RecuperarCaminhoArquivoModelo();

        /// <summary>
        /// Gera o caminho do arquivo com o nome especificado.
        /// </summary>
        /// <param name="nomeArquivo">O nome do arquivo.</param>
        /// <param name="exception">Define se deve gerar uma exceção caso o caminho não exista.</param>
        /// <returns>O caminho do arquivo gerado.</returns>
        string GerarCaminhoArquivo(string nomeArquivo, bool exception);

        /// <summary>
        /// Recupera o caminho base de arquivos.
        /// </summary>
        /// <returns>O caminho base de arquivos.</returns>
        string RecuperarCaminhoBase();

        /// <summary>
        /// Converte o arquivo no caminho especificado para uma representação em Base64.
        /// </summary>
        /// <param name="caminhoArquivo">O caminho do arquivo.</param>
        /// <returns>A representação em Base64 do arquivo.</returns>
        string ConverterParaBase64(string caminhoArquivo);

        /// <summary>
        /// Recupera os bytes do arquivo no caminho especificado de forma assíncrona.
        /// </summary>
        /// <param name="caminhoArquivo">O caminho do arquivo.</param>
        /// <returns>Uma tarefa que representa a operação assíncrona. Os bytes do arquivo são retornados como resultado da tarefa.</returns>
        Task<byte[]> RecuperarBytesArquivoAsync(string caminhoArquivo);
    }
}
