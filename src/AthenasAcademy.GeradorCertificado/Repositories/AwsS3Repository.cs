using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using AthenasAcademy.GeradorCertificado.Models;
using AthenasAcademy.GeradorCertificado.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace AthenasAcademy.GeradorCertificado.Repositories
{
    public class AwsS3Repository : IAwsS3Repository
    {
        private const string ACCESS_KEY = "AKIAS2MQFVFOCOZNQYVH";
        private const string SECRET_KEY = "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";
        private const string BUCKET_BASE = "academy-academy";
        private static AwsS3Repository instancia;
        
        public static AwsS3Repository Instancia
        {
            get
            {
                if (instancia is null)
                    instancia = new AwsS3Repository();

                return instancia;
            }
        }

        public async Task<string> EnviarPDFAsync(PDFDetalhesModel pdfDetalhes, string bucket)
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

                    return  request.Key;
                }
            }
        }

        public async Task<string> EnviarPNGAsync(PNGDetalhesModel pdfDetalhes, string bucket)
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
                throw new Exception($"Erro geral: {ex.Message}");
            }
        }

        private AmazonS3Client GetClient()
        {
            BasicAWSCredentials credentials = new BasicAWSCredentials(ACCESS_KEY, SECRET_KEY);

            AmazonS3Config config = new AmazonS3Config
            {
                RegionEndpoint = RegionEndpoint.USWest2 // us-west-2 oregon
            };

            return new AmazonS3Client(credentials, config);
        }
    }
}