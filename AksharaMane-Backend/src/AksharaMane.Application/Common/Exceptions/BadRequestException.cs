using System;

namespace AksharaMane.Application.Common.Exceptions;

public class BadRequestException : System.Exception
{
    public BadRequestException(string message)
        : base(message)
    {
    }
}