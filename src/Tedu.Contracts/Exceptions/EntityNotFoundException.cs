using System.Runtime.Serialization;

namespace Tedu.Infrastructure.Repositories
{
    [Serializable]
    internal class EntityNotFoundException : Exception
    {

        public EntityNotFoundException() : base("Entity was not found.")
        {
        }

        public EntityNotFoundException(Exception? innerException) : base("Entity was not found.", innerException)
        {
        }

        protected EntityNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}