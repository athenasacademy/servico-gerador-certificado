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
        Task<CertificadoResponse> ObterCertificado(CertificadoRequest request);
        Task<NovoCertificadoResponse> GerarCertificado(NovoCertificadoRequest request);
    }
}
