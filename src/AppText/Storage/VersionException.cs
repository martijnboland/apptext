using System;

namespace AppText.Storage
{
    public class VersionException : Exception
    {
        public VersionException(string message) : base(message)
        {
        }
    }
}
