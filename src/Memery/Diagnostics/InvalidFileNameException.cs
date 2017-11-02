namespace Memery.Diagnostics
{
    [System.Serializable]
    public class InvalidFileNameException : System.Exception
    {
        public InvalidFileNameException() { }
        public InvalidFileNameException(string message) : base(message) { }
        public InvalidFileNameException(string message, System.Exception inner) : base(message, inner) { }
        protected InvalidFileNameException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}