﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Net;
using RESS.Shared.Exceptions.Middlewares;

namespace RESS.Shared.Exceptions.Mappers
{
    public class DefaultExceptionToResponseMapper : IExceptionToResponseMapper
    {
        public ExceptionResponse Map(Exception exception)
            => exception switch
            {
                DomainException ex => new ExceptionResponse
                {
                    HttpStatus = HttpStatusCode.BadRequest,
                    Code = ex.ErrorCode,
                    Message = ex.Message
                },
                AppException ex => new ExceptionResponse
                {
                    HttpStatus = HttpStatusCode.BadRequest,
                    Code = ex.ErrorCode,
                    Message = ex.Message
                },
                ValidationException ex => new ExceptionResponse
                {
                    HttpStatus = HttpStatusCode.BadRequest,
                    Code = "validation_failed",
                    Message = string.Join(Environment.NewLine, ex.Errors)
                },
                NotFoundException ex => new ExceptionResponse
                {
                    HttpStatus = HttpStatusCode.NotFound,
                    Code = "resource_not_found",
                    Message = "Requested resource was not found"
                },
                _ => new ExceptionResponse
                {
                    HttpStatus = HttpStatusCode.BadRequest,
                    Code = "error",
                    Message = exception.Message
                }
            };
    }
}