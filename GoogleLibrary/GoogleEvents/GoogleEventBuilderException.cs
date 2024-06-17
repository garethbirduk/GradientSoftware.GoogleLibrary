using System.Runtime.Serialization;

namespace GoogleLibrary.GoogleEvents
{
    [Serializable]
    internal class GoogleEventBuilderException : Exception
    {
        public GoogleEventBuilderException()
        {
        }

        public GoogleEventBuilderException(string? message) : base(message)
        {
        }

        public GoogleEventBuilderException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected GoogleEventBuilderException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}