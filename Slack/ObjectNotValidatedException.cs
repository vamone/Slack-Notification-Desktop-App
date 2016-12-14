using System;

namespace Slack
{
    public class ObjectNotValidatedException : Exception
    {
        public ObjectNotValidatedException(Type objectType) : base(FormatException(objectType))
        {
        }

        private static string FormatException(Type objectType)
        {
            return objectType.Name;
        }
    }
}