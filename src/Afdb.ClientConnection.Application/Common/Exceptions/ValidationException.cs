using FluentValidation.Results;

namespace Afdb.ClientConnection.Application.Common.Exceptions;

public class ValidationException : Exception
{
    public ValidationException()
        : base("One or more validation failures have occurred.")
    {
        Errors = new List<ValidationFailure>();
    }

    public ValidationException(IEnumerable<ValidationFailure> failures)
        : this()
    {
        Errors = failures.ToList();
    }

    public IList<ValidationFailure> Errors { get; }
}