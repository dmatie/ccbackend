namespace Afdb.ClientConnection.Domain.Events;

public sealed class SelectedProjectCreatedEvent(string sapCode, string projectTitle)
{
    public string SapCode { get; } = sapCode;
    public string ProjectTitle { get; } = projectTitle;
}