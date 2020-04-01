using Model.domain;


namespace Model.validators
{
    public class BiletValidator : AbstractValidator<Bilet>
    {
        private BiletValidator()
        {
        }

        public override void Validate(Bilet entity)
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

        public new static AbstractValidator<Bilet> GetInstance() // made it new **
        {
            return AbstractValidator<Bilet>.GetInstance();
        }

    }
}

