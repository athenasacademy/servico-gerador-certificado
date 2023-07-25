using AthenasAcademy.GeradorCertificado.Dominio.Requests;
using AthenasAcademy.GeradorCertificado.Services;
using AthenasAcademy.GeradorCertificado.Services.Interfaces;
using System.Threading.Tasks;
using System.Web.Http;

namespace AthenasAcademy.GeradorCertificado.API.Controllers
{
    /// <summary>
    /// Controlador responsável pelas operações relacionadas a certificados.
    /// </summary>
    public class CertificadoController : ApiController
    {
        private readonly IGeradorCertificadoService _certificadoService;
        private readonly ITokenService _tokenService;

        /// <summary>
        /// Cria uma nova instância do controlador CertificadoController.
        /// </summary>
        public CertificadoController()
        {
            _certificadoService = GeradorCertificadoService.Instancia;
            _tokenService = TokenService.Instancia;
        }

        /// <summary>
        /// Gera um novo certificado com base nas informações fornecidas.
        /// </summary>
        /// <param name="request">Objeto contendo as informações necessárias para gerar o certificado.</param>
        /// <param name="token">Token de autenticação.</param>
        /// <returns>Uma ação HTTP que representa o resultado da operação.</returns>
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
