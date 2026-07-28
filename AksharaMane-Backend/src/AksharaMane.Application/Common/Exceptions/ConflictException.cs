using System;

namespace AksharaMane.Application.Common.Exceptions;

public class ConflictException : System.Exception
{
    public ConflictException(string message)
        : base(message)
    {
    }
}