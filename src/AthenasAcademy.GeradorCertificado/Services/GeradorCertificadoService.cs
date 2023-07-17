using AthenasAcademy.GeradorCertificado.Dominio.Requests;
using AthenasAcademy.GeradorCertificado.Dominio.Responses;
using AthenasAcademy.GeradorCertificado.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace AthenasAcademy.GeradorCertificado.Services
{
    public class GeradorCertificadoService : IGeradorCertificadoService
    {
        private static GeradorCertificadoService instancia;

        public static GeradorCertificadoService Instancia
        {
            get {
                if (instancia is null)
                    instancia = new GeradorCertificadoService();

                return instancia; 
            }
        }

        public Task<CertificadoResponse> ObterCertificado(CertificadoRequest request)
        {
            // Implementação para obter um certificado com base nos dados fornecidos
            throw new NotImplementedException();
        }

        public Task<NovoCertificadoResponse> GerarCertificado(NovoCertificadoRequest request)
        {
            // Implementação para gerar um novo certificado com base nos dados fornecidos
            throw new NotImplementedException();
        }
    }
}
