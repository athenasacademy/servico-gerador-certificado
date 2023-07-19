using AthenasAcademy.GeradorCertificado.Dominio.Requests;
using AthenasAcademy.GeradorCertificado.Dominio.Responses;
using AthenasAcademy.GeradorCertificado.Models;
using AthenasAcademy.GeradorCertificado.Repositories.Interfaces;
using AthenasAcademy.GeradorCertificado.Services;
using AthenasAcademy.GeradorCertificado.Services.Interfaces;
using AutoFixture;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.IO;

namespace AthenasAcademy.GeradorCertificado.Test.Fixtures
{
    public class GeradorCertificadoServiceFixture
    {
        #region Dependencias
        private Fixture _fixture;

        private IGerenciadorArquivosService gerenciadorArquivosService;
        private IQRCodeService qrCodeService;
        private IPNGService pngService;
        private IAwsS3Repository awsS3Repository;
        #endregion

        #region Construtores
        public GeradorCertificadoServiceFixture()
        {
            _fixture = new Fixture();

            gerenciadorArquivosService = Substitute.For<IGerenciadorArquivosService>();
            qrCodeService = Substitute.For<IQRCodeService>();
            pngService = Substitute.For<IPNGService>();
            awsS3Repository = Substitute.For<IAwsS3Repository>();
        }

        public IGeradorCertificadoService InstanciarServico()
        {
            return new GeradorCertificadoService(
                gerenciadorArquivosService,
                qrCodeService,
                pngService,
                awsS3Repository
                );
        }
        #endregion

        #region Calls
        public IEnumerable<NSubstitute.Core.ICall> GetGerenciadorArquivosServiceCalls()
        {
            return gerenciadorArquivosService.ReceivedCalls();
        }

        public IEnumerable<NSubstitute.Core.ICall> GetQRCodeServiceCalls()
        {
            return qrCodeService.ReceivedCalls();
        }

        public IEnumerable<NSubstitute.Core.ICall> GetPNGServiceCalls()
        {
            return pngService.ReceivedCalls();
        }

        public IEnumerable<NSubstitute.Core.ICall> GetAwsS3RepositoryCalls()
        {
            return awsS3Repository.ReceivedCalls();
        }
        #endregion

        #region Aranges
        public GeradorCertificadoServiceFixture ComGerarQRCodeOK()
        {
            qrCodeService.GerarQRCode(
                Arg.Any<string>(),
                Arg.Any<int>(),
                Arg.Any<int>(),
                Arg.Any<string>()
                ).
                ReturnsForAnyArgs(
                _fixture.Create<QrCodeDetalhesModel>()
                );

            return this;
        }

        public GeradorCertificadoServiceFixture ComGerarPNGOK()
        {
            string pngModelTesteNomeArquivo = "CERTIFICADO_TESTE_0000000003.png";
            string pngModelTesteCaminhoArquivo = Path.GetFullPath(
                Path.Combine(
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\.."),
                    Path.Combine("Files", pngModelTesteNomeArquivo)
                )); 

            PNGDetalhesModel pngModel = _fixture.Create<PNGDetalhesModel>();

            pngModel.NomeArquivo = pngModelTesteNomeArquivo;
            pngModel.CaminhoArquivo = pngModelTesteCaminhoArquivo;

            pngService.GerarPNG(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<string>()
                ).
                ReturnsForAnyArgs(pngModel);

            return this;
        }

        public GeradorCertificadoServiceFixture ComEnviarPDFAsyncOK()
        {
            awsS3Repository.EnviarPDFAsync(
                Arg.Any<PDFDetalhesModel>(),
                Arg.Any<string>()
                ).
                ReturnsForAnyArgs(
                _fixture.Create<string>()
                );

            return this;
        }

        public GeradorCertificadoServiceFixture ComRecuperarBytesArquivoAsyncOK()
        {
            gerenciadorArquivosService.RecuperarBytesArquivoAsync(
                Arg.Any<string>()
                ).
                ReturnsForAnyArgs(
                _fixture.Create<byte[]>()
                );

            return this;
        }

        public GeradorCertificadoServiceFixture ComGerarURIAsyncOK()
        {
            awsS3Repository.GerarURIAsync(
                Arg.Any<string>(),
                Arg.Any<string>()
                ).
                ReturnsForAnyArgs(
                _fixture.Create<string>()
                );

            return this;
        }

        public GeradorCertificadoServiceFixture ComRecuperarCaminhoBaseOK()
        {
            gerenciadorArquivosService.RecuperarCaminhoBase().
                Returns(Path.Combine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\.."), "Files"));

            return this;
        }

        public GeradorCertificadoServiceFixture ComObterTamanhoArquivoOK()
        {
            gerenciadorArquivosService.ObterTamanhoArquivo(
                Arg.Any<string>()
                ).
                ReturnsForAnyArgs(
                _fixture.Create<string>()
                );

            return this;
        }

        public GeradorCertificadoServiceFixture ComGerarCaminhoArquivoOK()
        {
            string pdfModelTesteNomeArquivo = "CERTIFICADO_TESTE_0000000003.pdf";
            string pdfModelTesteCaminhoArquivo = Path.GetFullPath(
                Path.Combine(
                    Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\.."),
                    Path.Combine("Files", pdfModelTesteNomeArquivo)
                ));

            gerenciadorArquivosService.GerarCaminhoArquivo(
                Arg.Any<string>(),false).
                Returns(pdfModelTesteCaminhoArquivo);

            return this;
        }
        #endregion

        #region Mocks
        public NovoCertificadoRequest NovoCertificadoRequestMOCK()
        {
            return _fixture.Create<NovoCertificadoRequest>();
        }
        #endregion
    }
}