using Model.validators;
using Networking.protocols;
using Networking.utils;
using Persistence.repos;
using Services;
using System;
using System.Net.Sockets;
using System.Threading;


namespace Server
{
    public class StartRpcServer
    {
        private static int defaultPort = 55555;

        public static void Main(string[] args)
        {


            ClientDataBaseRepository userRepo = new ClientDataBaseRepository(ClientValidator.GetInstance());
            MeciDataBaseRepository meciRepo = new MeciDataBaseRepository(MeciValidator.GetInstance());
            BiletDataBaseRepository biletRepo = new BiletDataBaseRepository(BiletValidator.GetInstance());
            IServices serviceImpl = new ServicesImpl(userRepo, meciRepo, biletRepo);


            Console.WriteLine("Starting server on port: " + defaultPort);
            AbstractServer server = new ChatRpcConcurrentServer("127.0.0.1", defaultPort, serviceImpl);

            server.Start();
            Console.WriteLine("Server started ...");
            //Console.WriteLine("Press <enter> to exit...");
            Console.ReadLine();


        }


        public class ChatRpcConcurrentServer : ConcurrentServer
        {
            private IServices server;
            private ClientRpcWorker worker;
            public ChatRpcConcurrentServer(string host, int port, IServices server) : base(host, port)
            {
                this.server = server;
                Console.WriteLine("SerialChatServer...");
            }
            protected override Thread createWorker(TcpClient client)
            {
                worker = new ClientRpcWorker(server, client);
                return new Thread(new ThreadStart(worker.run));
            }
        }
    }
}
