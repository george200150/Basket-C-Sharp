using System;

using Services;
using Networking.dto;
using Networking.protocols;
using System.Net.Sockets;
using Model.domain;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Networking.network
{
    public class ServicesRpcProxy : IServices
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private String host;
        private int port;
        private bool isInitialized = false;

        private IObserver client;

        private IFormatter formatter;
        private NetworkStream stream;
        private TcpClient connection;
        private EventWaitHandle _waitHandle;

        private Queue<Response> qresponses;
        private volatile bool finished;
        public ServicesRpcProxy(String host, int port)
        {
            this.host = host;
            this.port = port;
            qresponses = new Queue<Response>();
            logger.Debug("CLIENT SIDE PROXY INIT @" + DateTime.Now);
        }

        public void login(Client user, IObserver client)
        {
            if (!isInitialized)
            {
                initializeConnection();
                this.isInitialized = true;
            }
            UserDTO udto = DTOUtils.getDTO(user);
            Request req = new Request.Builder().Type(RequestType.LOGIN).Data(udto).Build();
            sendRequest(req);
            logger.Debug("PROXY CLIENT: login SENT REQUEST @" + DateTime.Now);
            Response response = readResponse();
            logger.Debug("PROXY CLIENT: login RECEIVED RESPONSE @" + DateTime.Now);
            if (response.Type() == ResponseType.OK)
            {
                this.client = client;
                logger.Debug("PROXY CLIENT: SUCCESSFUL login @" + DateTime.Now);
                return;
            }
            if (response.Type() == ResponseType.ERROR)
            {
                String err = response.Data().ToString();
                closeConnection();
                logger.Debug("PROXY CLIENT: FAILED login @" + DateTime.Now);
                throw new ServicesException(err);
            }
        }


        public void logout(Client user, IObserver client)
        {
            UserDTO udto = DTOUtils.getDTO(user);
            Request req = new Request.Builder().Type(RequestType.LOGOUT).Data(udto).Build();
            sendRequest(req);
            logger.Debug("PROXY CLIENT: logout SENT REQUEST @" + DateTime.Now);
            Response response = readResponse();
            logger.Debug("PROXY CLIENT: logout RECEIVED RESPONSE @" + DateTime.Now);
            closeConnection();
            this.isInitialized = false;
            logger.Debug("PROXY CLIENT: SUCCESSFUL logout @" + DateTime.Now);
            if (response.Type() == ResponseType.ERROR)
            {
                String err = response.Data().ToString();
                logger.Debug("PROXY CLIENT: FAILED logout @" + DateTime.Now);
                throw new ServicesException(err);

            }
        }

        public Meci[] findAllMeciWithTickets()
        {
            Request req = new Request.Builder().Type(RequestType.GET_MATCHES_W_TICKETS).Build();
            sendRequest(req);
            logger.Debug("PROXY CLIENT: findAllMeciWithTickets SENT REQUEST @" + DateTime.Now);
            Response response = readResponse();
            logger.Debug("PROXY CLIENT: findAllMeciWithTickets RECEIVED RESPONSE @" + DateTime.Now);
            if (response.Type() == ResponseType.ERROR)
            {
                String err = response.Data().ToString();
                logger.Debug("PROXY CLIENT: FAILED findAllMeciWithTickets @" + DateTime.Now);
                throw new ServicesException(err);
            }
            MeciDTO[] meciDTOS = (MeciDTO[])response.Data();
            Meci[] meciuri = DTOUtils.getFromDTO(meciDTOS);
            logger.Debug("PROXY CLIENT: SUCCESSFUL findAllMeciWithTickets @" + DateTime.Now);
            return meciuri;
        }


        public Meci[] findAllMeci()
        {
            if (!isInitialized)
            {
                initializeConnection();
                this.isInitialized = true;
            }
            Request req = new Request.Builder().Type(RequestType.GET_MATCHES).Build();
            sendRequest(req);
            logger.Debug("PROXY CLIENT: findAllMeci SENT REQUEST @" + DateTime.Now);
            Response response = readResponse();
            logger.Debug("PROXY CLIENT: findAllMeci RECEIVED RESPONSE @" + DateTime.Now);
            if (response.Type() == ResponseType.ERROR)
            {
                String err = response.Data().ToString();
                logger.Debug("PROXY CLIENT: FAILED findAllMeci @" + DateTime.Now);
                throw new ServicesException(err);
            }
            MeciDTO[] meciDTOS = (MeciDTO[])response.Data();
            Meci[] meciuri = DTOUtils.getFromDTO(meciDTOS);
            logger.Debug("PROXY CLIENT: SUCCESSFUL findAllMeci @" + DateTime.Now);
            return meciuri;
        }


        public void ticketsSold(Meci meci, Client loggedInClient)
        {
            MeciDTO meciDTO = DTOUtils.getDTO(meci);
            UserDTO userDTO = DTOUtils.getDTO(loggedInClient);
            Object[]
            sendData = new Object[2];
            sendData[0] = meciDTO;
            sendData[1] = userDTO;
            Request req = new Request.Builder().Type(RequestType.TICKETS_SOLD).Data(sendData).Build();
            sendRequest(req);
            logger.Debug("PROXY CLIENT: ticketsSold SENT REQUEST @" + DateTime.Now + " WARNING: THIS FUNCTION HAS NO RESPONSE!!!");
            Response response = readResponse();
            if (response.Type() == ResponseType.ERROR)
            {
                String err = response.Data().ToString();
                throw new ServicesException(err);
            }
            logger.Debug("PROXY CLIENT: SUCCESSFUL ticketsSold @" + DateTime.Now + " WARNING: THIS FUNCTION HAS NO RESPONSE!!!");
        }


        private void closeConnection()
        {
            finished = true;
            try
            {
                stream.Close();
                //output.close();
                connection.Close();
                _waitHandle.Close();
                client = null;
                logger.Debug("PROXY CLIENT: SUCCESSFUL closeConnection @" + DateTime.Now);
            }
            catch (IOException e)
            {
                logger.Debug("PROXY CLIENT: FAILED closeConnection @" + DateTime.Now);
                Console.WriteLine(e.StackTrace);
            }

        }


        private void sendRequest(Request request)
        {

            if (!isInitialized)
            {
                initializeConnection();
                this.isInitialized = true;
            }

            logger.DebugFormat("NETWORKING FROM CLIENT PROXY TO SERVER: INITIALIZING sendRequest @" + DateTime.Now, request);
            try
            {
                logger.Debug("E S T E   P O S I B I L   S A     F I U   B L O C A T     D E     S C R I E R E     !!! - output.writeObject(request);");
                formatter.Serialize(stream, request);
                logger.DebugFormat("--- scriere: {}", request);
                stream.Flush();
                logger.DebugFormat("NETWORKING FROM CLIENT PROXY TO SERVER: SUCESSFUL sendRequest @" + DateTime.Now, request);
            }
            catch (IOException e)
            {
                logger.Debug("NETWORKING FROM CLIENT PROXY TO SERVER: FAILED sendRequest @" + DateTime.Now);
                throw new ServicesException("Error sending object " + e);
            }
        }

        private Response readResponse()
        {
            Response response = null;
            logger.Debug("NETWORKING FROM CLIENT PROXY TO SERVER: INITIALIZING readResponse @" + DateTime.Now);
            try
            {
                logger.Debug("E S T E   P O S I B I L   S A     F I U   B L O C A T     D E     C I T I R E     !!! - response=qresponses.take();");
                _waitHandle.WaitOne();
                lock (qresponses)
                {
                    //Monitor.Wait(responses); 
                    response = qresponses.Dequeue();
                }
                logger.DebugFormat("--- citire: {}", response);
                logger.DebugFormat("NETWORKING FROM CLIENT PROXY TO SERVER: SUCESSFUL qresponses.take readResponse {} @" + DateTime.Now, response);
            }
            catch (ThreadInterruptedException e)
            {
                logger.Debug("NETWORKING FROM CLIENT PROXY TO SERVER: FAILED readResponse @" + DateTime.Now);
                Console.WriteLine(e.StackTrace);
            }
            return response;
        }


        private void initializeConnection()
        {
            if (!this.isInitialized)
            {
                try
                {
                    logger.Debug("PROXY CLIENT: INITIALIZING initializeConnection @" + DateTime.Now);
                    connection = new TcpClient(host, port);
                    stream = connection.GetStream();
                    formatter = new BinaryFormatter();
                    finished = false;
                    _waitHandle = new AutoResetEvent(false);
                    startReader();
                    logger.Debug("PROXY CLIENT: SUCCESSFUL initializeConnection @" + DateTime.Now);
                }
                catch (IOException e)
                {
                    logger.Debug("PROXY CLIENT: FAILED initializeConnection @" + DateTime.Now);
                    Console.WriteLine(e.StackTrace);
                }
            }
        }
        private void startReader()
        {
            Thread tw = new Thread(run);
            tw.Start();
        }


        private void handleUpdate(Response response)
        {

            if (response.Type() == ResponseType.UPDATE_TICKETS_IN_CLIENT_MATCH)
            { // this is the handler for the TICKETS_SOLD request

                Meci meci = DTOUtils.getFromDTO((MeciDTO)response.Data());
                Console.WriteLine("pe biletul care a fost cumparat a fost scris numele clientului. pentru meciul " + meci);
                try
                {
                    client.notifyTicketsSold(meci);
                }
                catch (ServicesException e)
                {
                    Console.WriteLine(e.StackTrace);
                }
            }
        }


        private bool isUpdate(Response response)
        {
            return response.Type() == ResponseType.UPDATE_TICKETS_IN_CLIENT_MATCH;
        }


        public virtual void run()
        {
            while (!finished)
            {
                try
                {
                    object response = formatter.Deserialize(stream);
                    logger.DebugFormat("PROXY CLIENT: READING DATA FROM THE SERVER @" + DateTime.Now, response);
                    Console.WriteLine("response received " + response);
                    if (isUpdate((Response)response))
                    {
                        logger.DebugFormat("PROXY CLIENT: Debug isUpdate((Response)response) == true @" + DateTime.Now, response);
                        handleUpdate((Response)response);
                    }
                    else
                    {
                        {
                            logger.DebugFormat("PROXY CLIENT: Debug qresponses.put((Response)response) @" + DateTime.Now, response);

                            qresponses.Enqueue((Response)response);

                        }
                        _waitHandle.Set();
                    }
                }
                catch (IOException e)
                {
                    logger.Debug("PROXY CLIENT: FAILED run (because other end of socket disconnected from server) : IOException {} @" + DateTime.Now, e);
                    Console.WriteLine("Reading error " + e);
                }
            }
        }
    }
}

