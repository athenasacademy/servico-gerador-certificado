using AthenasAcademy.GeradorCertificado.Dominio.Requests;
using AthenasAcademy.GeradorCertificado.Dominio.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AthenasAcademy.GeradorCertificado.Services.Interfaces
{
    public interface IGeradorCertificadoService
    {
        Task<NovoCertificadoPDFResponse> GerarCertificadoPDF (NovoCertificadoRequest request);
    }
}
