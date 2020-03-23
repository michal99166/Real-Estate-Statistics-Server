using System;
using RESS.Shared.Exceptions.Middlewares;

namespace RESS.Shared.Exceptions.Mappers
{
    public interface IExceptionCompositionRoot
    {
        ExceptionResponse Map(Exception exception);
    }
}