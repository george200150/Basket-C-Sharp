using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thrift.Server;
using Thrift.Transport;

namespace ClientForm
{
    class MessageServer
    {
        public MessageServer(int port)
        {
            Task.Run(() => start(port));
        }

        public void start(int port)
        {
            try
            {
                MessageHandler handler = new MessageHandler();
                MessageService.Processor processor = new MessageService.Processor(handler);
                TServerTransport serverTransport = new TServerSocket(port);
                TServer server = new TSimpleServer(processor, serverTransport);

                Console.WriteLine("Starting the server...");
                server.Serve();
            }
            catch (Exception x)
            {
                Console.WriteLine(x.StackTrace);
            }
        }
    }
}
