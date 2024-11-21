// Runtime/Core/Exceptions.cs

using System;

namespace Core.Singleton
{
    public class SingletonException : Exception
    {
        public SingletonException(string message) : base(message) { }
        public SingletonException(string message, Exception inner) : base(message, inner) { }
    }

    public class SingletonNotInitializedException : SingletonException
    {
        public SingletonNotInitializedException(Type type) 
            : base($"Singleton of type {type.Name} is not initialized") { }
    }

    public class SingletonDestroyedException : SingletonException
    {
        public SingletonDestroyedException(Type type) 
            : base($"Singleton of type {type.Name} was destroyed") { }
    }

    public class SingletonInitializationTimeoutException : SingletonException
    {
        public SingletonInitializationTimeoutException(Type type, float timeout) 
            : base($"Initialization of {type.Name} timed out after {timeout} seconds") { }
    }

    public class SingletonDataException : SingletonException
    {
        public SingletonDataException(string message) : base(message) { }
        public SingletonDataException(string message, Exception inner) : base(message, inner) { }
    }
}
