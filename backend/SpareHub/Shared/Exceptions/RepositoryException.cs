namespace Shared.Exceptions;

public class RepositoryException(string message, Exception innerException) : Exception(message, innerException);