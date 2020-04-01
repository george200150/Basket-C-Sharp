using System;


namespace Model.validators
{
    public class ValidationException : ApplicationException
    {
        public ValidationException() { }
        public ValidationException(String mess) : base(mess) { }
        public ValidationException(String mess, Exception e) : base(mess, e) { }
    }
}
