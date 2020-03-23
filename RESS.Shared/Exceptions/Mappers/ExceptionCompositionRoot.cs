﻿using System;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using RESS.Shared.Exceptions.Middlewares;

namespace RESS.Shared.Exceptions.Mappers
{
    public class ExceptionCompositionRoot : IExceptionCompositionRoot
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public ExceptionCompositionRoot(IServiceScopeFactory serviceScopeFactory)
            => _serviceScopeFactory = serviceScopeFactory;

        public ExceptionResponse Map(Exception exception)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var mappers = scope.ServiceProvider.GetServices<IExceptionToResponseMapper>().ToList();
            var nonDefaultMappers = mappers.Where(m => !(m is DefaultExceptionToResponseMapper));

            var result = nonDefaultMappers
                .Select(m => m.Map(exception))
                .SingleOrDefault(r => r is { });

            if (result is { })
            {
                return result;
            }

            var defaultMapper = mappers.SingleOrDefault(m => m is DefaultExceptionToResponseMapper);
            return defaultMapper.Map(exception);
        }
    }
}