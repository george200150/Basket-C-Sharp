using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Persistence.repos;
using Model.domain;
using System.Threading;


namespace Server
{
    public class ServicesImpl : IServices
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private ClientDataBaseRepository userRepository;
        private MeciDataBaseRepository meciRepository;
        private BiletDataBaseRepository biletRepository;
        private readonly IDictionary<String, IObserver> loggedClients;

        public ServicesImpl(ClientDataBaseRepository uRepo, MeciDataBaseRepository mRepo, BiletDataBaseRepository bRepo)
        {
            userRepository = uRepo;
            meciRepository = mRepo;
            biletRepository = bRepo;
            loggedClients = new Dictionary<String, IObserver>();
        }


        private void notifyTicketsBought(Meci meci)
        {
            IEnumerable<Client> users = userRepository.FindAll();
            logger.Debug("FULL SERVER: login RECEIVED !OBSERVER! COMMAND @" + DateTime.Now);

            foreach (Client us in users)
            {
                if (loggedClients.ContainsKey(us.id))
                {
                    IObserver chatClient = loggedClients[us.id];
                    Task.Run(() => chatClient.notifyTicketsSold(meci));
                }
            }
        }


        public void login(Client user, IObserver client)
        {
            logger.Debug("FULL SERVER: login RECEIVED COMMAND @" + DateTime.Now);
            Client userR = userRepository.findClientByCredentials(user.id, user.password);
            if (userR != null)
            {
                if (loggedClients.ContainsKey(user.id))
                    throw new ServicesException("User already logged in.");
                loggedClients[user.id] = client;
            }
            else
                throw new ServicesException("Authentication failed.");
            logger.Debug("FULL SERVER: login SENT RETURN TO SERVER PROXY @" + DateTime.Now);
        }



        public void logout(Client user, IObserver client)
        {
            logger.Debug("FULL SERVER: logout RECEIVED COMMAND @" + DateTime.Now);
            IObserver localClient = loggedClients[user.id];
            if (localClient == null)
                throw new ServicesException("User " + user.id + " is not logged in.");
            loggedClients.Remove(user.id);
            logger.Debug("FULL SERVER: logout SENT RETURN TO SERVER PROXY @" + DateTime.Now);
        }

        public Meci[] findAllMeciWithTickets()
        {
            logger.Debug("FULL SERVER: findAllMeciWithTickets RECEIVED COMMAND @" + DateTime.Now);
            Console.WriteLine("_DEBUG: FIND MATCHES WITH TICKETS");
            IEnumerable<Meci> res = meciRepository.FindAll().Where(x => x.numarBileteDisponibile > 0);
            Meci[] meciuri = res.ToArray();
            logger.Debug("FULL SERVER: findAllMeciWithTickets SENT RETURN TO SERVER PROXY @" + DateTime.Now);
            return meciuri;
        }

        public Meci[] findAllMeci()
        {
            logger.Debug("FULL SERVER: findAllMeci RECEIVED COMMAND @" + DateTime.Now);
            Console.WriteLine("_DEBUG: FIND MATCHES");
            IEnumerable<Meci> fromDBresult = meciRepository.FindAll();
            Meci[] meciuri = fromDBresult.ToArray();
            logger.Debug("FULL SERVER: findAllMeci SENT RETURN TO SERVER PROXY @" + DateTime.Now);
            return meciuri;
        }


        public void ticketsSold(Meci meci, Client loggedInClient)
        {
            logger.Debug("FULL SERVER: ticketsSold RECEIVED COMMAND @" + DateTime.Now);
            Meci ret = meciRepository.Update(meci);
            if (ret == null)
                throw new ServicesException("MECI NOT FOUND IN DB TO BE UPDATED!");
            int delta = ret.numarBileteDisponibile - meci.numarBileteDisponibile; // number of tickets bought
            Bilet[] goodTickets = biletRepository.FindAll().Where(x => x.idClient == null).ToArray();

            for (int i = 0; i < delta; i++)
            {
                Bilet bilet = goodTickets[i];
                bilet.idClient = loggedInClient.id;
                bilet.numeClient = loggedInClient.nume;
                Bilet res = biletRepository.Update(bilet);
                if (res == null)
                    throw new ServicesException("BILET NOT FOUNT IN DB TO BE UPDATED!");
            }
            Task.Run(() =>
            {// hopefully not a deadlock (has enough time to finish)
                try
                {
                    Thread.Sleep(1000);
                    logger.Debug("FULL SERVER: ticketsSold SENT OBSERVER COMMAND TO notifyTicketsBought @" + DateTime.Now);
                    notifyTicketsBought(meci);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            });
        }
    }
}
