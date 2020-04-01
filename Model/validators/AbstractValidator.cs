

namespace Model.validators
{
    public class AbstractValidator<E> : IValidator<E>
    {
        protected static AbstractValidator<E> instance = null;

        public virtual void Validate(E entity)
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
