using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AthenasAcademy.GeradorCertificado.Models
{
    public class GerarNovoCertificadoModel
    {
        public QrCodeDetalhesModel QrCode { get; set; }

        public PDFDetalhesModel PDF { get; set; }

        public PNGDetalhesModel PNG { get; set; }
    }
}