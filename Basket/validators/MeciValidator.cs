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
    class MeciValidator : AbstractValidator<Meci>
    {
        private MeciValidator()
        {
        }

        public void Validate(Meci entity)
        {
            string exceptions = "";
            if (entity.id.Equals(""))
            {
                exceptions += "Id-ul nu poate fi vid!\n";
            }
            if (entity.home.Equals(""))
            {
                exceptions += "Echipa 1 nu poate fi vida\n";
            }
            if (entity.away.Equals(""))
            {
                exceptions += "Echipa 2 nu poate fi vida\n";
            }
            //if (entity.tip == null)
            //{
            //    exceptions += "tipul meciului trebuie specificat!\n";
            //}
            if (entity.numarBileteDisponibile < 0)
            {
                exceptions += "Numarul de bilete trebuie sa fie pozitiv!";
            }
        }

        public static AbstractValidator<Meci> GetInstance()
        {
            return AbstractValidator<Meci>.GetInstance();
        }

    }
}