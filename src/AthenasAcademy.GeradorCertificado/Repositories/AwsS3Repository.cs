using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using AthenasAcademy.GeradorCertificado.Models;
using AthenasAcademy.GeradorCertificado.Repositories.Interfaces;
using PdfSharp.Charting;
using System;
using System.Threading.Tasks;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization;
using System.IO;
using System.Web.Hosting;

namespace AthenasAcademy.GeradorCertificado.Repositories
{
    public class AwsS3Repository : IAwsS3Repository
    {

        #region Dependencias

        private static SecretConfig credenciais = ObterSecrets();
        private static string ACCESS_KEY = credenciais.AwsAccessKey;
        private static string SECRET_KEY = credenciais.AwsSecretKey;
        private static string BUCKET_BASE = credenciais.AwsBucketBase;

        private static AwsS3Repository instancia;
        #endregion

        #region Construtores
        public static AwsS3Repository Instancia
        {
            get
            {
                if (instancia is null)
                    instancia = new AwsS3Repository();

                return instancia;
            }
        }
        #endregion

        #region Métodos Públicos
        public async Task<string> EnviarPDFAsync(PDFDetalhesModel pdfDetalhes, string bucket)
        {
            try
            {
                using (AmazonS3Client client = GetClient())
                {
                    using (TransferUtility utility = new TransferUtility(client))
                    {
                        TransferUtilityUploadRequest request = new TransferUtilityUploadRequest();

                        request.BucketName = BUCKET_BASE;
                        request.FilePath = pdfDetalhes.CaminhoArquivo;
                        request.Key = $"{bucket}/{pdfDetalhes.NomeArquivo}";

                        await utility.UploadAsync(request);

                        return request.Key;
                    }
                }
            }
            catch (AmazonS3Exception ex)
            {
                throw new Exception($"Erro ao enviar o arquivo PDF para o Amazon S3: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro geral ao enviar o arquivo PDF para o Amazon S3: {ex.Message}");
            }
        }


        public async Task<string> EnviarPNGAsync(PNGDetalhesModel pdfDetalhes, string bucket)
        {
            try
            {
                using (AmazonS3Client client = GetClient())
                {
                    using (TransferUtility utility = new TransferUtility(client))
                    {
                        TransferUtilityUploadRequest request = new TransferUtilityUploadRequest();

                        request.BucketName = BUCKET_BASE;
                        request.FilePath = pdfDetalhes.CaminhoArquivo;
                        request.Key = $"{bucket}/{pdfDetalhes.NomeArquivo}";

                        await utility.UploadAsync(request);

                        return request.Key;
                    }
                }
            }
            catch (AmazonS3Exception ex)
            {
                throw new Exception($"Erro ao enviar o arquivo PNG para o Amazon S3: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro geral ao enviar o arquivo PNG para o Amazon S3: {ex.Message}");
            }
        }


        public async Task<string> GerarURIAsync(string objeto, string bucket)
        {
            try
            {
                RegionEndpoint regionEndpoint = RegionEndpoint.USWest2;

                using (AmazonS3Client client = GetClient())
                {
                    GetPreSignedUrlRequest request = new GetPreSignedUrlRequest
                    {
                        BucketName = BUCKET_BASE + "@/" + bucket,
                        Key = objeto,
                        Expires = DateTime.UtcNow.AddHours(1)
                    };

                    return await Task.FromResult(client.GetPreSignedURL(request));
                }
            }
            catch (AmazonS3Exception ex)
            {
                throw new Exception($"Erro ao gerar o link de download: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro geral ao gerar a URL de download: {ex.Message}");
            }
        }
        #endregion

        #region Métodos Privados
        private AmazonS3Client GetClient()
        {
            try
            {
                BasicAWSCredentials credentials = new BasicAWSCredentials(ACCESS_KEY, SECRET_KEY);

                AmazonS3Config config = new AmazonS3Config
                {
                    RegionEndpoint = RegionEndpoint.USWest2 // us-west-2 oregon
                };

                return new AmazonS3Client(credentials, config);
            }
            catch (AmazonS3Exception ex)
            {
                throw new Exception($"Erro ao criar o cliente Amazon S3: {ex.Message}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Erro geral ao criar o cliente Amazon S3: {ex.Message}");
            }
        }
        #endregion

        #region ConfiguracaoSecrets
        private static SecretConfig ObterSecrets()
        {
            var yamlPath = HostingEnvironment.MapPath(@"~/Config/secrets.yaml");

            var yamlContent = File.ReadAllText(yamlPath);

            var deserializer = new DeserializerBuilder()
                .WithNamingConvention(PascalCaseNamingConvention.Instance)
                .Build();

            return deserializer.Deserialize<SecretConfig>(yamlContent);
        }
        #endregion
    }
}