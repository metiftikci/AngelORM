using System;

namespace AngelORM
{
    public class AngelORMException : Exception
    {
        public AngelORMException()
        {

        }

        public AngelORMException(string message)
            : base(message)
        {

        }

        public AngelORMException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}
