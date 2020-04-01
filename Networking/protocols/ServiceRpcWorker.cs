using Model.domain;
using Networking.dto;
using Services;
using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;

namespace Networking.protocols
{

    public class ClientRpcWorker : IObserver //, Runnable
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IServices server;

        private TcpClient connection;
        private NetworkStream stream;
        private IFormatter formatter;

        private volatile bool connected;
        public ClientRpcWorker(IServices server, TcpClient connection)
        {
            this.server = server;
            this.connection = connection;
            try
            {
                stream = connection.GetStream();
                formatter = new BinaryFormatter();
                connected = true;
                logger.Debug("PROXY SERVER: SUCCESSFUL ClientRpcWorker @" + DateTime.Now);
            }
            catch (IOException e)
            {
                Console.WriteLine(e.StackTrace);
                logger.Debug("PROXY SERVER: FAILED ClientRpcWorker @" + DateTime.Now);
            }
        }


        public virtual void run()
        { // aici intra din proxy, de la sendRequest()
            while (connected)
            {
                try
                {
                    logger.Debug("E S T E   P O S I B I L   S A     F I U   B L O C A T     D E     C I T I R E     !!! - Object request=input.readObject();");
                    object request = formatter.Deserialize(stream);

                    logger.DebugFormat("--- citire: {}", request);
                    logger.Debug("PROXY SERVER: run RECEIVED REQUEST @" + DateTime.Now);
                    object response = handleRequest((Request)request);
                    if (response != null)
                    {
                        sendResponse((Response)response);
                        logger.Debug("PROXY SERVER: run SENT RESPONSE @" + DateTime.Now);
                    }
                }
                catch (IOException e) {
                    Console.WriteLine(e.StackTrace);
                }
                try
                {
                    Thread.Sleep(250);
                }
                catch (ThreadInterruptedException e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }
            try
            {
                logger.Debug("PROXY SERVER: run : BEGIN SHUTDOWN @" + DateTime.Now);
                stream.Close();
                connection.Close();
                logger.Debug("PROXY SERVER: run : COMPLETED SHUTDOWN @" + DateTime.Now);
            }
            catch (IOException e)
            {
                logger.Debug("PROXY SERVER: run : IOException @" + DateTime.Now);
                Console.WriteLine("Error " + e);
            }
        }


        public void notifyTicketsSold(Meci meci) {
            MeciDTO mecidto = DTOUtils.getDTO(meci);
            Response resp = new Response.Builder().Type(ResponseType.UPDATE_TICKETS_IN_CLIENT_MATCH).Data(mecidto).Build();
            logger.Debug("PROXY SERVER: ticketsSold BUILT RESPONSE @" + DateTime.Now);
            Console.WriteLine("Tickets sold for match " + meci);
            try
            {
                sendResponse(resp);
                logger.DebugFormat("PROXY SERVER: ticketsSold SENT RESPONSE @" + DateTime.Now, resp);
            }
            catch (IOException e)
            {
                Console.WriteLine(e.StackTrace);
            }
        }

        private static Response okResponse = new Response.Builder().Type(ResponseType.OK).Build();


        private Response handleRequest(Request request)
        {
            Response response = null;
            if (request.Type() == RequestType.LOGIN)
            {
                logger.Debug("PROXY SERVER: handleRequest RECEIVED REQUEST type==RequestType.LOGIN @" + DateTime.Now);
                Console.WriteLine("Login request ..." + request.Type());
                UserDTO udto = (UserDTO)request.Data();
                Client user = DTOUtils.getFromDTO(udto);
                try
                {
                    server.login(user, this);
                    logger.Debug("PROXY SERVER: handleRequest SENT COMMAND TO SERVER server.login @" + DateTime.Now);
                    return okResponse;
                }
                catch (ServicesException e)
                {
                    connected = false;
                    logger.Debug("PROXY SERVER: FAILED handleRequest type==RequestType.LOGIN @" + DateTime.Now);
                    return new Response.Builder().Type(ResponseType.ERROR).Data(e.Message).Build();
                }
            }
            if (request.Type() == RequestType.LOGOUT)
            {
                logger.Debug("PROXY SERVER: handleRequest RECEIVED REQUEST type==RequestType.LOGOUT @" + DateTime.Now);
                Console.WriteLine("Logout request");
                UserDTO udto = (UserDTO)request.Data();
                Client user = DTOUtils.getFromDTO(udto);
                try
                {
                    server.logout(user, this);
                    connected = false;
                    logger.Debug("PROXY SERVER: handleRequest SENT COMMAND TO SERVER server.logout @" + DateTime.Now);
                    return okResponse;

                }
                catch (ServicesException e)
                {
                    logger.Debug("PROXY SERVER: FAILED handleRequest type==RequestType.LOGOUT @" + DateTime.Now);
                    return new Response.Builder().Type(ResponseType.ERROR).Data(e.Message).Build();
                }
            }

            if (request.Type() == RequestType.GET_MATCHES)
            {
                logger.Debug("PROXY SERVER: handleRequest RECEIVED REQUEST type==RequestType.GET_MATCHES @" + DateTime.Now);
                Console.WriteLine("Get Matches request");
                try
                {
                    Meci[] meciuri = server.findAllMeci();
                    MeciDTO[] mecidtos = DTOUtils.getDTO(meciuri);
                    logger.Debug("PROXY SERVER: handleRequest SENT COMMAND TO SERVER server.findAllMeci @" + DateTime.Now);
                    return new Response.Builder().Type(ResponseType.GET_MATCHES).Data(mecidtos).Build(); // TODO: this is the response of the GET_MATCHES request

                }
                catch (ServicesException e)
                {
                    logger.Debug("PROXY SERVER: FAILED handleRequest type==RequestType.GET_MATCHES @" + DateTime.Now);
                    return new Response.Builder().Type(ResponseType.ERROR).Data(e.Message).Build();
                }
            }


            if (request.Type() == RequestType.GET_MATCHES_W_TICKETS)
            {
                logger.Debug("PROXY SERVER: handleRequest RECEIVED REQUEST type==RequestType.GET_MATCHES_W_TICKETS @" + DateTime.Now);
                Console.WriteLine("Get Matches request");
                try
                {
                    Meci[] meciuri = server.findAllMeciWithTickets();
                    MeciDTO[] mecidtos = DTOUtils.getDTO(meciuri);
                    logger.Debug("PROXY SERVER: handleRequest SENT COMMAND TO SERVER server.findAllMeci @" + DateTime.Now);
                    return new Response.Builder().Type(ResponseType.GET_MATCHES_W_TICKETS).Data(mecidtos).Build(); // TODO: this is the response of the GET_MATCHES_W_TICKETS request

                }
                catch (ServicesException e)
                {
                    logger.Debug("PROXY SERVER: FAILED handleRequest type==RequestType.GET_MATCHES_W_TICKETS @" + DateTime.Now);
                    return new Response.Builder().Type(ResponseType.ERROR).Data(e.Message).Build();
                }
            }


            if (request.Type() == RequestType.TICKETS_SOLD)
            {
                logger.Debug("PROXY SERVER: handleRequest RECEIVED REQUEST type==RequestType.TICKETS_SOLD @" + DateTime.Now);
                Console.WriteLine("TICKETS_SOLD update meci request");
                Object[] data = (Object[])request.Data();
                MeciDTO meciDTO = (MeciDTO)data[0];
                UserDTO client = (UserDTO)data[1];
                Meci meci = DTOUtils.getFromDTO(meciDTO);
                Client loggedInClient = DTOUtils.getFromDTO(client);
                try
                {
                    server.ticketsSold(meci, loggedInClient); // this is the response of the TICKETS_SOLD request
                    logger.Debug("PROXY SERVER: handleRequest SENT COMMAND TO SERVER server.ticketsSold @" + DateTime.Now);

                    return new Response.Builder().Type(ResponseType.TICKETS_SOLD).Data(meciDTO).Build();

                }
                catch (ServicesException e)
                {
                    logger.Debug("PROXY SERVER: FAILED handleRequest type==RequestType.TICKETS_SOLD @" + DateTime.Now);
                    return new Response.Builder().Type(ResponseType.ERROR).Data(e.Message).Build();
                }
            }

            logger.DebugFormat("PROXY SERVER: RETURN RESPONSE handleRequest @" + DateTime.Now + " WARNING: should never reach this point: request type not found !!!", response);
            return response;
        }


        private void sendResponse(Response response)
        {
            Console.WriteLine("sending response " + response);
            logger.Debug("E S T E   P O S I B I L   S A     F I U   B L O C A T     D E     S C R I E R E     !!! - output.writeObject(response);");
            formatter.Serialize(stream, response);
            logger.DebugFormat("--- scriere: {}", response);
            stream.Flush();
            logger.DebugFormat("PROXY SERVER: SUCCESSFUL sendResponse @" + DateTime.Now, response);
        }
    }
}
