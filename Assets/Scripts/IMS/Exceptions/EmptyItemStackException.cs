using System;

namespace IMS.Exceptions
{
    public class EmptyItemStackException : Exception
    {
        public EmptyItemStackException() : base("The stack supplied was empty!") {}
    }
}