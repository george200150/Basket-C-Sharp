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
    class ClientValidator : AbstractValidator<Client>
    {
        private ClientValidator()
        {
        }

        public void Validate(Client entity)
        {
            string exceptions = "";
            if (entity.id.Equals(""))
            {
                exceptions += "Id-ul nu poate fi vid!";
            }

            if (exceptions.Length > 0)
            {
                throw new ValidationException(exceptions);
            }
        }

        public static AbstractValidator<Client> GetInstance()
        {
            return AbstractValidator<Client>.GetInstance();
        }

    }
}