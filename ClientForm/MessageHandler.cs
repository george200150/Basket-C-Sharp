using System.Diagnostics;


namespace ClientForm
{
    class MessageHandler : MessageService.Iface
    {
        public string sendMessage(string message)
        {
            Debug.WriteLine("I received : " + message);
            MainPage.needsUpdate = true;
            return "good";
        }
    }
}
