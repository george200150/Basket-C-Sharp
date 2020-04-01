using System.Collections.Generic;

using Model.domain;

namespace Persistence.repos
{
    public interface ICrudRepository<ID, E> where E : Entity<ID>
    {
        E FindOne(ID id);
        IEnumerable<E> FindAll();
        E Save(E entity);
        E Delete(ID id);
        E Update(E entity);
    }
}
