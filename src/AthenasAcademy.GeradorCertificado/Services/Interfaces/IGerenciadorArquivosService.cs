using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AthenasAcademy.GeradorCertificado.Services.Interfaces
{
    public interface IGerenciadorArquivosService
    {
        void LimparCaminhoBase();
        
        void LimparCaminhoBase(string[] caminhosArquivos);

        string ObterTamanhoArquivo(string caminhoArquivo);

        byte[] RecuperarBytesDaImagem(Bitmap bitmap);

        string RecuperarCaminhoArquivoModelo();

        string RecuperarCaminhoArquivo(string nomeArquivo, bool exception);

        string RecuperarCaminhoBase();

        string ConverterParaBase64(string caminhoArquivo);
    }
}
