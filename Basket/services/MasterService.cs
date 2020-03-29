using Basket.domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.services
{
    class MasterService
    {
        ClientService clientService;
        EchipaService echipaService;
        BiletService biletService;
        MeciService meciService;

        public MasterService(BiletService biletService, ClientService clientService, EchipaService echipaService, MeciService meciService)
        {
            this.biletService = biletService;
            this.clientService = clientService;
            this.echipaService = echipaService;
            this.meciService = meciService;
        }

        public Client FindClientByCredentials(String usr, String psswd)
        {
            Client client = FindOneClient(usr);
            if (client != null && client.password.Equals(psswd))
                return client;
            return null;
        }

        public Bilet FindOneBilet(String id)
        {
            return biletService.FindOne(id);
        }
        public IEnumerable<Bilet> FindAllBilet()
        {
            return biletService.FindAll();
        }
        public Bilet SaveBilet(Bilet entity)
        {
            return biletService.Save(entity);
        }
        public Bilet DeleteBilet(String id)
        {
            return biletService.Delete(id);
        }
        public Bilet UpdateBilet(Bilet newEntity)
        {
            return biletService.Update(newEntity);
        }


        public Client FindOneClient(String id)
        {
            return clientService.FindOne(id);
        }
        public IEnumerable<Client> FindAllClient()
        {
            return clientService.FindAll();
        }
        public Client SaveClient(Client entity)
        {
            return clientService.Save(entity);
        }
        public Client DeleteClient(String id)
        {
            return clientService.Delete(id);
        }
        public Client UpdateClient(Client newEntity)
        {
            return clientService.Update(newEntity);
        }



        public Echipa FindOneEchipa(String id)
        {
            return echipaService.FindOne(id);
        }
        public IEnumerable<Echipa> FindAllEchipa()
        {
            return echipaService.FindAll();
        }
        public Echipa SaveEchipa(Echipa entity)
        {
            return echipaService.Save(entity);
        }
        public Echipa DeleteEchipa(String id)
        {
            return echipaService.Delete(id);
        }
        public Echipa UpdateEchipa(Echipa newEntity)
        {
            return echipaService.Update(newEntity);
        }



        public Meci FindOneMeci(String id)
        {
            return meciService.FindOne(id);
        }
        public IEnumerable<Meci> FindAllMeci()
        {
            return meciService.FindAll();
        }
        public Meci SaveMeci(Meci entity)
        {
            return meciService.Save(entity);
        }
        public Meci DeleteMeci(String id)
        {
            return meciService.Delete(id);
        }
        public Meci UpdateMeci(Meci newEntity)
        {
            return meciService.Update(newEntity);
        }
    }
}

