

namespace Model.validators
{
    public interface IValidator<E>
    {
        void Validate(E entity);
    }
}
