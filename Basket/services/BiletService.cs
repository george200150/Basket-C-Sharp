using Basket.domain;
using Basket.repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.services
{
    class BiletService
    {
        BiletDataBaseRepository biletRepository;
        public BiletService(BiletDataBaseRepository biletRepository)
        {
            this.biletRepository = biletRepository;
        }

        public Bilet Delete(string id)
        {
            return this.biletRepository.Delete(id);
        }

        public IEnumerable<Bilet> FindAll()
        {
            return this.biletRepository.FindAll();
        }

        public Bilet FindOne(string id)
        {
            return this.biletRepository.FindOne(id);
        }

        public Bilet Save(Bilet entity)
        {
            return this.biletRepository.Save(entity);
        }

        public Bilet Update(Bilet entity)
        {
            return this.biletRepository.Update(entity);
        }
    }
}
