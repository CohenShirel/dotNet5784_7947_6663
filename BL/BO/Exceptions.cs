using BO;
namespace BO;

public class Exceptions
{
    [Serializable]
    public class BlDoesNotExistException : Exception
    {
        public BlDoesNotExistException(string? message) : base(message) { }
        public BlDoesNotExistException(string message, Exception innerException)
                    : base(message, innerException) { }
    }

    [Serializable]
    public class BlNullPropertyException : Exception
    {
        public BlNullPropertyException(string? message) : base(message) { }
    }
    [Serializable]
    public class BlNotCorrectDetailsException : Exception
    {
        public BlNotCorrectDetailsException(string? message) : base(message) { }
    }
    [Serializable]
    public class BlException: Exception
    {
        public BlException(string? message) : base(message) { }
        public BlException(string message, Exception innerException)
                    : base(message, innerException) { }
    }
    [Serializable]
    public class BlAlreadyExistsException : Exception
    {
        public BlAlreadyExistsException(string? message) : base(message) { }
        public BlAlreadyExistsException(string message, Exception innerException)
                    : base(message, innerException) { }
    }

    [Serializable]
    public class BlDeletionImpossibleException : Exception
    {
        public BlDeletionImpossibleException(string? message) : base(message) { }
        public BlDeletionImpossibleException(string message, Exception innerException)
                    : base(message, innerException) { }
    }

}
