using AthenasAcademy.GeradorCertificado.Dominio.Requests;
using AthenasAcademy.GeradorCertificado.Dominio.Responses;
using AthenasAcademy.GeradorCertificado.Services.Interfaces;
using AthenasAcademy.GeradorCertificado.Test.Fixtures;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute.Core;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace AthenasAcademy.GeradorCertificado.Test.Testes
{
    public class GeradorCertificadoServiceTeste
    {
        [Fact]
        public async Task DeveGerarPDF()
        {
            // Arange
            GeradorCertificadoServiceFixture fixture = new GeradorCertificadoServiceFixture();
            IGeradorCertificadoService service = fixture
                                                .ComGerarQRCodeOK()
                                                .ComGerarPNGOK()
                                                .ComEnviarPDFAsyncOK()
                                                .ComRecuperarBytesArquivoAsyncOK()
                                                .ComGerarURIAsyncOK()
                                                .InstanciarServico();

            // Act
            NovoCertificadoRequest request = fixture.NovoCertificadoRequestMOCK();
            NovoCertificadoPDFResponse response = await service.GerarCertificadoPDF(request);

            IEnumerable<ICall> callsArquivoService = fixture.GetGerenciadorArquivosServiceCalls()
                .Where(x => 
                       x.GetMethodInfo().Name == "LimparCaminhoBase" && 
                       x.GetMethodInfo().Name == "RecuperarBytesArquivoAsync");

            IEnumerable<ICall> callsQRCodeService = fixture.GetQRCodeServiceCalls()
                .Where(x => x.GetMethodInfo().Name == "GerarQRCode");

            IEnumerable<ICall> callsPNGService = fixture.GetPNGServiceCalls()
                .Where(x => x.GetMethodInfo().Name == "TryGetValue");

            IEnumerable<ICall> callsAwsS3Repository = fixture.GetAwsS3RepositoryCalls()
                .Where(x => 
                       x.GetMethodInfo().Name == "EnviarPDFAsync" &&
                       x.GetMethodInfo().Name == "GerarURIAsync");

            // Assert
            Assert.Equals(2, callsArquivoService.Count());
            Assert.Equals(1, callsQRCodeService.Count());
            Assert.Equals(1, callsPNGService.Count());
            Assert.Equals(2, callsAwsS3Repository.Count());
            Assert.IsNotNull(response);
        }
    }
}
