using AthenasAcademy.GeradorCertificado.Dominio.Requests;
using AthenasAcademy.GeradorCertificado.Services;
using AthenasAcademy.GeradorCertificado.Services.Interfaces;
using System.Threading.Tasks;
using System.Web.Http;

namespace AthenasAcademy.GeradorCertificado.API.Controllers
{
    public class CertificadoController : ApiController
    {
        private readonly IGeradorCertificadoService _certificadoService;

        public CertificadoController()
        {
            _certificadoService = GeradorCertificadoService.Instancia;
        }

        [HttpPost]
        [Route("gerar-certificado")]
        public async Task<IHttpActionResult> Gerar([FromBody] NovoCertificadoRequest request)
        {
            return Ok(await _certificadoService.GerarCertificadoPDF(request));
        }
    }
}
