using System;


namespace Services
{
    public class ServicesException : Exception
    {
        public ServicesException() : base() { }

        public ServicesException(String msg) : base(msg) { }

        public ServicesException(String msg, Exception ex) : base(msg, ex) { }

    }
}
