using System;
using RESS.Shared.Exceptions.Middlewares;

namespace RESS.Shared.Exceptions.Mappers
{
    public interface IExceptionToResponseMapper
    {
        ExceptionResponse Map(Exception exception);
    }
}