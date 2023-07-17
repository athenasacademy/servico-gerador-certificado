using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AthenasAcademy.GeradorCertificado.Models
{
    public class PNGDetalhesModel
    {
        public string PNGBase64 { get; set; }

        public string NomeArquivo { get; set; }

        public string CaminhoArquivo { get; set; }

        public string TamanhoArquivo { get; set; }
    }
}