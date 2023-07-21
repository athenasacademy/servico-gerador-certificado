
using PdfSharp.Charting;
using YamlDotNet.Serialization;

namespace AthenasAcademy.GeradorCertificado.Models
{
    public class SecretConfig
    {
        [YamlMember(Alias = "AwsAccessKey")]
        public string AwsAccessKey { get; set; }

        [YamlMember(Alias = "AwsSecretKey")]
        public string AwsSecretKey { get; set; }

        [YamlMember(Alias = "AwsBucketBase")]
        public string AwsBucketBase { get; set; }
    }
}