using System.Runtime.Serialization;

namespace GoogleLibrary.GoogleEventBuilders
{
    [Serializable]
    internal class GoogleEventBuilderException : Exception
    {
        protected GoogleEventBuilderException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public GoogleEventBuilderException()
        {
        }

        public GoogleEventBuilderException(string? message) : base(message)
        {
        }

        public GoogleEventBuilderException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}