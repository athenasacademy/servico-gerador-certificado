using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AthenasAcademy.GeradorCertificado.Dominio.Requests
{
    public class CertificadoRequest
    {
        public string NomeAluno { get; set; }

        public int Matricula { get; set; }

        public string NomeCurso { get; set; }

        public int CodigoCurso{ get; set; }
    }
}
