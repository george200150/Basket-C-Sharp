using Model.domain;



namespace Model.validators
{
    public class MeciValidator : AbstractValidator<Meci>
    {
        private MeciValidator()
        {
        }

        public override void Validate(Meci entity)
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

        public new static AbstractValidator<Meci> GetInstance() // made it new **
        {
            return AbstractValidator<Meci>.GetInstance();
        }

    }
}