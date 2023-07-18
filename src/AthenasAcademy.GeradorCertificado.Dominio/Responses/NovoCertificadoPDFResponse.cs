using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AthenasAcademy.GeradorCertificado.Dominio.Responses
{
    public class NovoCertificadoPDFResponse
    {
        public string NomeArquivo { get; set; }

        public string CaminhoArquivo { get; set; }

        public byte[] PDFArquivo { get; set; }

        public string UriDownload { get; set; }
    }
}
