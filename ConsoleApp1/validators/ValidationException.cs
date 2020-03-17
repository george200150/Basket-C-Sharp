using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Bakset.validators
{
    public class ValidationException : ApplicationException
    {
        public ValidationException() { }
        public ValidationException(String mess) : base(mess) { }
        public ValidationException(String mess, Exception e) : base(mess, e) { }
    }
}
