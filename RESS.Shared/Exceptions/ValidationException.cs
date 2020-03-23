﻿using System;
using System.Collections.Generic;

namespace RESS.Shared.Exceptions
{
    public sealed class ValidationException : Exception
    {
        public IEnumerable<string> Errors { get; }

        public ValidationException(IEnumerable<string> errors)
            => Errors = errors;
    }
}