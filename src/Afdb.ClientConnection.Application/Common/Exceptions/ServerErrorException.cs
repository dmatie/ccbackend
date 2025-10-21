﻿namespace Afdb.ClientConnection.Application.Common.Exceptions;

public class ServerErrorException : Exception
{
    public ServerErrorException(string name, object key)
           : base($"Entity \"{name}\" ({key}) was not found.")
    {
    }

    public ServerErrorException(string message)
        : base(message)
    {
    }

    public ServerErrorException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
