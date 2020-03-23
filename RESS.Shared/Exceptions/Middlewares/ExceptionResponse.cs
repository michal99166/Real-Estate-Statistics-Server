using System.Net;

namespace RESS.Shared.Exceptions.Middlewares
{
    public class ExceptionResponse
    {
        public HttpStatusCode HttpStatus { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
    }
}