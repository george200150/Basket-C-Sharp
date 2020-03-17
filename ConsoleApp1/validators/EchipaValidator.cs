using Basket;
using Basket.validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakset.validators
{
    class EchipaValidator : AbstractValidator<Echipa>
    {
        private EchipaValidator()
        {
        }

        public void Validate(Echipa entity)
        {
            string exceptions = "";
            if (entity.nume.Equals(""))
            {
                exceptions += "Numele nu poate fi gol\n";
            }
        }

        public static AbstractValidator<Echipa> GetInstance()
        {
            return AbstractValidator<Echipa>.GetInstance();
        }

    }
}