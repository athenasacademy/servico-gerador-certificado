using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AthenasAcademy.GeradorCertificado.Dominio.Responses
{
    public class NovoCertificadoResponse
    {
        public int Matricula { get; set; }

        public int CodigoCurso { get; set; }

        public string CertificadoPDF { get; set; }
        
        public string CertificadoPNG { get; set; }
    }
}
