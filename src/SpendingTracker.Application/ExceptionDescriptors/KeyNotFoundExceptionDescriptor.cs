﻿using System.Net;
using SpendingTracker.Application.Middleware.ExceptionHandling;
using SpendingTracker.GenericSubDomain.Validation;

namespace SpendingTracker.Application.ExceptionDescriptors
{
    internal sealed class KeyNotFoundExceptionDescriptor : IExceptionDescriptor
    {
        public bool CanHandle(Exception ex)
        {
            return ex is KeyNotFoundException;
        }

        public HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public ErrorProperty[] Handle(Exception ex)
        {
            var errors = new[]
            {
                ErrorProperty.FromCode(ValidationErrorCodeEnum.KeyNotFound)
            };

            return errors;
        }
    }
}