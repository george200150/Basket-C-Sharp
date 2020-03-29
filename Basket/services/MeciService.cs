using Basket.domain;
using Basket.repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.services
{
    class MeciService
    {
        MeciDataBaseRepository meciRepository;
        public MeciService(MeciDataBaseRepository meciRepository)
        {
            this.meciRepository = meciRepository;
        }

        public Meci Delete(string id)
        {
            return this.meciRepository.Delete(id);
        }

        public IEnumerable<Meci> FindAll()
        {
            return this.meciRepository.FindAll();
        }

        public Meci FindOne(string id)
        {
            return this.meciRepository.FindOne(id);
        }

        public Meci Save(Meci entity)
        {
            return this.meciRepository.Save(entity);
        }

        public Meci Update(Meci entity)
        {
            return this.meciRepository.Update(entity);
        }
    }
}
