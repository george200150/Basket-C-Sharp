using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakset.validators
{
    interface IValidator<E>
    {
        void Validate(E entity); // nu exista un "THROWS" in C#
    }
}
