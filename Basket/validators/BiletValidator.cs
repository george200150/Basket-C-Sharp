using Basket;
using Basket.domain;
using Basket.validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bakset.validators
{
    class BiletValidator : AbstractValidator<Bilet>
    {
        private BiletValidator()
        {
        }

        public void Validate(Bilet entity)
        {
            string exceptions = "";
            if (entity.id.Equals(""))
            {
                exceptions += "Id-ul nu poate fi vid!\n";
            }
            if (entity.idMeci.Equals(""))
            {
                exceptions += "Id-ul meciului nu poate fi vid!\n";
            }
            if (entity.pret < 0)
            {
                exceptions += "Pretul biletului nu poate fi negativ!";
            }

            if (exceptions.Length > 0)
            {
                throw new ValidationException(exceptions);
            }
        }

        public static AbstractValidator<Bilet> GetInstance()
        {
            return AbstractValidator<Bilet>.GetInstance();
        }

    }
}

