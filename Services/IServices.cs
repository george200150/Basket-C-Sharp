using Model.domain;


namespace Services
{
    public interface IServices
    {
        void login(Client user, IObserver client);
        void logout(Client user, IObserver client);

        Meci[] findAllMeciWithTickets();
        Meci[] findAllMeci();
        void ticketsSold(Meci meci, Client loggedInClient);
    }
}
