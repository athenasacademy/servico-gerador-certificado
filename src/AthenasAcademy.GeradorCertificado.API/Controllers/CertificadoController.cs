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
        private readonly ITokenService _tokenService;

        public CertificadoController()
        {
            _certificadoService = GeradorCertificadoService.Instancia;
            _tokenService = TokenService.Instancia;
        }

        [HttpPost]
        [Route("gerar-certificado")]
        public async Task<IHttpActionResult> Gerar([FromBody] NovoCertificadoRequest request, [FromUri] string token)
        {
            if (!await _tokenService.ValidarToken(token))
                return Unauthorized();

            Dominio.Responses.NovoCertificadoPDFResponse response = await _certificadoService.GerarCertificadoPDF(request);

            if (response is null)
                return InternalServerError();

            return Ok(response);
        }

    }
}
