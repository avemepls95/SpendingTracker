﻿using System.Runtime.Serialization;
using FluentValidation;
using FluentValidation.Results;

namespace SpendingTracker.GenericSubDomain.Validation;

public class FluentValidationException : ValidationException
{
    public FluentValidationException(string message)
        : base(message)
    {
    }

    public FluentValidationException(string message, IEnumerable<ValidationFailure> errors)
        : base(message, errors)
    {
    }

    public FluentValidationException(string message, IEnumerable<ValidationFailure> errors, bool appendDefaultMessage)
        : base(message, errors, appendDefaultMessage)
    {
    }

    public FluentValidationException(IEnumerable<ValidationFailure> errors)
        : base(errors)
    {
    }

    public FluentValidationException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}