using System;
using System.Net;

namespace AthenasAcademy.GeradorCertificado.Exceptions
{
    public class APICustomException : Exception
    {
        public APICustomException(string message, Exception innerException) : base(message, innerException)
        {

        }

        public APICustomException(string message) : base(message)
        {

        }
    }
}