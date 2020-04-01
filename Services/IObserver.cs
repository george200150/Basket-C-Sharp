using Model.domain;


namespace Services
{
    public interface IObserver
    {
        void notifyTicketsSold(Meci meci);
    }

}
