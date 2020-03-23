﻿using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using RESS.Shared.Exceptions.Mappers;

namespace RESS.Shared.Exceptions.Middlewares
{
    internal sealed class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IExceptionCompositionRoot _exceptionCompositionRoot;

        public ExceptionMiddleware(RequestDelegate next, IExceptionCompositionRoot exceptionCompositionRoot)
        {
            _next = next;
            _exceptionCompositionRoot = exceptionCompositionRoot;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                var responseData = _exceptionCompositionRoot.Map(ex);

                if (responseData is { })
                {
                    var json = JsonConvert.SerializeObject(new { responseData.Code, Message = responseData.Message });
                    httpContext.Response.StatusCode = (int)responseData.HttpStatus;
                    httpContext.Response.Headers.Add("content-type", "application/json");
                    await httpContext.Response.WriteAsync(json);
                }
                else
                {
                    throw;
                }
            }
        }
    }
}