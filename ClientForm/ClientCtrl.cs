using Model.domain;
using Services;
using System;


namespace ClientForm
{
    class ClientCtrl : IObserver
    {
        public event EventHandler<UserEventArgs> updateEvent; //ctrl calls it when it has received an update (manually defined custom delegate)
        private readonly IServices server;


        public ClientCtrl(IServices server)
        {
            this.server = server;
        }


        public Meci[] getAllMeciuri()
        {
            return this.server.findAllMeci();
        }


        public Meci[] getAllMeciWithTickets()
        {
            return this.server.findAllMeciWithTickets();
        }


        public void ticketsSold(Meci selectedMatch, Client currentUser)
        {
            this.server.ticketsSold(selectedMatch, currentUser);
        }


        public void logout(Client usr)
        {
            Console.WriteLine("Ctrl logout");
            server.logout(usr, this);
        }


        protected virtual void OnUserEvent(UserEventArgs e)
        {
            if (updateEvent == null) return;
            updateEvent(this, e);
            Console.WriteLine("Update Event called");
        }


        public void notifyTicketsSold(Meci meci) // CALLED BY SERVICE (this is a server random update) to refresh GUI
        {
            Console.WriteLine("Meci updated" + meci);
            UserEventArgs userArgs = new UserEventArgs(UpdateType.TICKETS_SOLD, meci);
            OnUserEvent(userArgs);
        }

    }
}
