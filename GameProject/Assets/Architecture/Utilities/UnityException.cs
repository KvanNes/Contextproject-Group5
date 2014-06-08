using System.Runtime.Serialization;
using System;

namespace Utilities
{
    [Serializable]
    public class UnityException : Exception
    {
        // Constructors
        public UnityException(string message)
            : base(message)
        {
        }

        // Ensure Exception is Serializable
        protected UnityException(SerializationInfo info, StreamingContext ctxt)
            : base(info, ctxt)
        {
        }
    }
}