using Basket.domain;
using Basket.repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.services
{
    class EchipaService
    {
        EchipaDataBaseRepository echipaRepository;
        public EchipaService(EchipaDataBaseRepository echipaRepository)
        {
            this.echipaRepository = echipaRepository;
        }

        public Echipa Delete(string id)
        {
            return this.echipaRepository.Delete(id);
        }

        public IEnumerable<Echipa> FindAll()
        {
            return this.echipaRepository.FindAll();
        }

        public Echipa FindOne(string id)
        {
            return this.echipaRepository.FindOne(id);
        }

        public Echipa Save(Echipa entity)
        {
            return this.echipaRepository.Save(entity);
        }

        public Echipa Update(Echipa entity)
        {
            return this.echipaRepository.Update(entity);
        }
    }
}
