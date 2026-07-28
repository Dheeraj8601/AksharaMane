using System;

namespace AksharaMane.Application.Common.Exceptions;

public class NotFoundException : System.Exception
{
    public NotFoundException(string message)
        : base(message)
    {
    }
}