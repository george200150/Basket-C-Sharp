using Basket.domain;
using Basket.repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.services
{
    class ClientService
    {
        ClientDataBaseRepository clientRepository;
        public ClientService(ClientDataBaseRepository clientRepository)
        {
            this.clientRepository = clientRepository;
        }

        public Client Delete(string id)
        {
            return this.clientRepository.Delete(id);
        }

        public IEnumerable<Client> FindAll()
        {
            return this.clientRepository.FindAll();
        }

        public Client FindOne(string id)
        {
            return this.clientRepository.FindOne(id);
        }

        public Client Save(Client entity)
        {
            return this.clientRepository.Save(entity);
        }

        public Client Update(Client entity)
        {
            return this.clientRepository.Update(entity);
        }
    }
}
