﻿namespace Afdb.ClientConnection.Domain.Enums;
public enum RequestStatus
{
    Pending = 1,
    Approved = 2,
    Rejected = 3,
    Completed = 10,
    Cancelled = 20
}