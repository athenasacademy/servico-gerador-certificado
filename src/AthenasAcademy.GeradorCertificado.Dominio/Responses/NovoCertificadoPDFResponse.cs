using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AthenasAcademy.GeradorCertificado.Dominio.Responses
{
    public class NovoCertificadoPDFResponse
    {
        public string Arquivo { get; set; }

        public string CertificadoPDF { get; set; }

        public string CertificadoBase64 { get; set; }

        public byte[] CertificadoBytes { get; set; }
    }
}
