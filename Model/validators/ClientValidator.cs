using Model.domain;


namespace Model.validators
{
    public class ClientValidator : AbstractValidator<Client>
    {
        private ClientValidator()
        {
        }

        public override void Validate(Client entity)
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

        public new static AbstractValidator<Client> GetInstance() // made it new **
        {
            return AbstractValidator<Client>.GetInstance();
        }

    }
}