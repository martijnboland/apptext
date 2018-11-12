using System;

namespace AppText.Core.Storage
{
    public class VersionException : Exception
    {
        public VersionException(string message) : base(message)
        {
        }
    }
}
