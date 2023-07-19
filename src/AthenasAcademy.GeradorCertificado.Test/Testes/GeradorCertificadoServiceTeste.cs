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
    [TestClass]
    public class GeradorCertificadoServiceTeste
    {
        //[Fact]
        [TestMethod]
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
                                                .ComRecuperarCaminhoBaseOK()
                                                .ComObterTamanhoArquivoOK()
                                                .ComGerarCaminhoArquivoOK()
                                                .InstanciarServico();

            // Act
            NovoCertificadoRequest request = fixture.NovoCertificadoRequestMOCK();
            NovoCertificadoPDFResponse response = await service.GerarCertificadoPDF(request);


            IEnumerable<ICall> callsQRCodeService = fixture.GetQRCodeServiceCalls()
                .Where(x => x.GetMethodInfo().Name == "GerarQRCode");

            IEnumerable<ICall> callsPNGService = fixture.GetPNGServiceCalls()
                .Where(x => x.GetMethodInfo().Name == "GerarPNG");

            bool callsNecessariosEmAwsS3Repository = fixture.GetAwsS3RepositoryCalls().Select(x => x.GetMethodInfo().Name)
                .All(name =>
                    name == "EnviarPDFAsync" ||
                    name == "GerarURIAsync");

            bool callsNecessariosEmArquivoService = fixture.GetGerenciadorArquivosServiceCalls()
                .Select(x => x.GetMethodInfo().Name)
                .All(name =>
                    name == "LimparCaminhoBase" ||
                    name == "RecuperarCaminhoBase" ||
                    name == "GerarCaminhoArquivo" ||
                    name == "RecuperarCaminhoArquivoModelo" ||
                    name == "GerarCaminhoArquivo" ||
                    name == "ConverterParaBase64" ||
                    name == "ObterTamanhoArquivo" ||
                    name == "RecuperarBytesArquivoAsync");

            // Assert
            Assert.IsTrue(callsNecessariosEmArquivoService);
            Assert.IsTrue(callsNecessariosEmAwsS3Repository);
            Assert.AreEqual(1, callsQRCodeService.Count());
            Assert.AreEqual(1, callsPNGService.Count());
            Assert.AreNotEqual(null, response);
        }
    }
}
