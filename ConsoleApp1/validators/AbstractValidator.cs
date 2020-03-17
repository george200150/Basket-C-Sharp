using Bakset.validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.validators
{
    class AbstractValidator<E> : IValidator<E>
    {
        protected static AbstractValidator<E> instance = null;

        public void Validate(E entity)
        {
        }

        public static AbstractValidator<E> GetInstance()
        {
            if (instance == null)
            {
                instance = new AbstractValidator<E>();
            }
            return instance;
        }
    }
}
