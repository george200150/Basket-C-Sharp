using System.Collections.Generic;
using System.Data;

using Model.domain;
using Model.validators;

namespace Persistence.repos
{
    public class ClientDataBaseRepository : ICrudRepository<string, Client>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IValidator<Client> validator;
        IDbConnection con;

        public ClientDataBaseRepository(IValidator<Client> validator)
        {
            logger.Info("entry constructor");
            this.validator = validator;
            this.con = ADOInvariant.GetConnection();
            logger.Info("successful constructor exit");
        }

        public Client findClientByCredentials(string username, string password)
        {
            logger.Debug("entry find");
            if (username == null || password == null)
            {
                logger.Error("null id exception");
                throw new RepositoryException("ID-UL SI PAROLA NU POT FI NULL");
            }
            using (var comm = con.CreateCommand())
            {
                logger.Info("entry query");
                comm.CommandText = "SELECT id,password FROM \"Clienti\" WHERE id=@id AND password=@password";
                IDbDataParameter paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = username;
                comm.Parameters.Add(paramId);
                IDbDataParameter paramPassword = comm.CreateParameter();
                paramPassword.ParameterName = "@password";
                paramPassword.Value = password;
                comm.Parameters.Add(paramPassword);

                using (var dataR = comm.ExecuteReader())
                {
                    if (dataR.Read())
                    {
                        string id = dataR.GetString(0);

                        Client client = new Client(id, password);
                        logger.InfoFormat("Exiting findOne with value {0}", client);
                        return client;
                    }
                }
            }
            logger.InfoFormat("Exiting findOne with value {0}", null);
            return null;
        }


        public Client Delete(string id)
        {
            logger.Info("entry delete");

            if (id == null)
            {
                logger.Error("null id exception");
                throw new RepositoryException("ID-ul nu poate fi NULL!");
            }

            Client client = FindOne(id);
            logger.Info("found data");
            if (client != null) // this is a drawback... however... not gonna change it any sooner
            {
                using (var comm = con.CreateCommand())
                {
                    comm.CommandText = "DELETE FROM \"Clienti\" WHERE id=@id";
                    IDbDataParameter paramId = comm.CreateParameter();
                    paramId.ParameterName = "@id";
                    paramId.Value = id;
                    comm.Parameters.Add(paramId);
                    logger.Info("entry query");
                    var dataR = comm.ExecuteNonQuery();
                    if (dataR == 0)
                        throw new RepositoryException("No task deleted!");
                }
            }
            logger.InfoFormat("successful query with value {0}", client);
            return client;
        }


        public IEnumerable<Client> FindAll()
        {
            logger.Info("entry findAll");
            IList<Client> clientList = new List<Client>();
            using (var comm = con.CreateCommand())
            {
                logger.Info("entry query");
                comm.CommandText = "SELECT id, password FROM \"Clienti\"";

                using (var dataR = comm.ExecuteReader())
                {
                    while (dataR.Read())
                    {
                        string id = dataR.GetString(0);
                        string password = dataR.GetString(1);

                        Client client = new Client(id, password);
                        clientList.Add(client);
                    }
                } // TODO: ??? throw new IllegalArgumentException("Error: Could not connect to the database"); ???
                logger.Info("successful query");
            }
            logger.InfoFormat("successful exit with value {0}", clientList);
            return clientList;
        }


        public Client FindOne(string id)
        {
            if (id == null)
            {
                logger.Error("null id exception");
                throw new RepositoryException("ID-ul NU POATE FI NULL");
            }
            logger.InfoFormat("Entering findOne with value {0}", id);

            using (var comm = con.CreateCommand())
            {
                logger.Info("entry query");
                comm.CommandText = "SELECT id,password FROM \"Clienti\" WHERE id=@id";
                IDbDataParameter paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = id;
                comm.Parameters.Add(paramId);

                using (var dataR = comm.ExecuteReader())
                {
                    if (dataR.Read())
                    {
                        // string id = dataR.GetString(0);
                        string password = dataR.GetString(1);

                        Client client = new Client(id, password);
                        logger.InfoFormat("Exiting findOne with value {0}", client);
                        return client;
                    }
                }
            }
            logger.InfoFormat("Exiting findOne with value {0}", null);
            return null;
        }


        public Client Save(Client entity)
        {
            logger.Info("entry save");
            if (entity == null)
            {
                logger.Error("null id exception");
                throw new RepositoryException("ENTITATEA NU POATE FI NULL");
            }

            validator.Validate(entity);
            logger.Info("validated data");
            if (FindOne(entity.id) != null)
            {
                logger.Error("duplicate found exception");
                throw new ValidationException("DUPLICAT GASIT!");
            }

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "INSERT INTO \"Clienti\" VALUES (@id, @password)";
                var paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = entity.id;
                comm.Parameters.Add(paramId);

                var paramPasswd = comm.CreateParameter();
                paramPasswd.ParameterName = "@password";
                paramPasswd.Value = entity.password;
                comm.Parameters.Add(paramPasswd);

                var result = comm.ExecuteNonQuery();
                if (result == 0)
                {
                    logger.Error("sql error - Niciun client adaugat!");
                    throw new RepositoryException("Niciun client adaugat!");
                }
            }
            logger.Info("successful query", null);
            return null;
        }


        public Client Update(Client entity)
        {
            logger.Info("entry update");
            if (entity == null)
            {
                logger.Error("null entity exception");
                throw new RepositoryException("Entitatea nu poate fi NULL!");
            }
            validator.Validate(entity);
            logger.Info("validated data");

            if (FindOne(entity.id) != null)
            {
                Client old = FindOne(entity.id);
                logger.Info("found data");

                using (var comm = con.CreateCommand())
                {
                    comm.CommandText = "UPDATE \"Clienti\" SET password=@password WHERE id=@id";
                    var paramId = comm.CreateParameter();
                    paramId.ParameterName = "@id";
                    paramId.Value = entity.id;
                    comm.Parameters.Add(paramId);

                    var paramPasswd = comm.CreateParameter();
                    paramPasswd.ParameterName = "@password";
                    paramPasswd.Value = entity.password;
                    comm.Parameters.Add(paramPasswd);

                    var result = comm.ExecuteNonQuery();
                    if (result == 0)
                    {
                        logger.Error("sql error - Niciun client adaugat!");
                        throw new RepositoryException("Niciun client adaugat!");
                    }

                    logger.InfoFormat("successful query with value {0}", old);
                    return old;
                }
            }
            logger.InfoFormat("not found data. unsuccessful update with {0}", null);
            return null;
        }
    }
}
