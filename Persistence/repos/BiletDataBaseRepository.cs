using System;
using System.Collections.Generic;
using System.Data;

using Model.domain;
using Model.validators;

namespace Persistence.repos
{
    public class BiletDataBaseRepository : ICrudRepository<string, Bilet>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IValidator<Bilet> validator;
        IDbConnection con;

        public BiletDataBaseRepository(IValidator<Bilet> validator)
        {
            logger.Info("entry constructor");
            this.validator = validator;
            this.con = ADOInvariant.GetConnection();
            logger.Info("successful constructor exit");
        }


        public Bilet Delete(string id)
        {
            logger.Info("entry delete");

            if (id == null)
            {
                logger.Error("null id exception");
                throw new RepositoryException("ID-ul nu poate fi NULL!");
            }

            Bilet bilet = FindOne(id);
            logger.Info("found data");
            if (bilet != null) // this is a drawback... however... not gonna change it any sooner
            {
                using (var comm = con.CreateCommand())
                {
                    comm.CommandText = "DELETE FROM \"Bilete\" WHERE id=@id";
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
            logger.InfoFormat("successful query with value {0}", bilet);
            return bilet;
        }

        public IEnumerable<Bilet> FindAll()
        {
            logger.Info("entry findAll");
            IList<Bilet> biletList = new List<Bilet>();
            using (var comm = con.CreateCommand())
            {
                logger.Info("entry query");
                comm.CommandText = "SELECT id, \"numeClient\", pret, \"idMeci\" FROM \"Bilete\""; //TODO: CANNOT READ NULL COLUMN  //TODO: CANNOT READ NULL COLUMN  //TODO: CANNOT READ NULL COLUMN

                using (var dataR = comm.ExecuteReader())
                {
                    while (dataR.Read())
                    {
                        string id = dataR.GetString(0);
                        string numeClient = null;
                        var value = dataR.GetValue(1);
                        if (value != DBNull.Value)
                            numeClient = (string)value;
                        //string numeClient = dataR.GetString(1); //TODO: CANNOT READ NULL COLUMN  //TODO: CANNOT READ NULL COLUMN  //TODO: CANNOT READ NULL COLUMN
                        float pret = dataR.GetFloat(2);
                        string idMeci = dataR.GetString(3);

                        Bilet bilet = new Bilet(id, numeClient, pret, idMeci);
                        biletList.Add(bilet);
                    }
                } // TODO: ??? throw new IllegalArgumentException("Error: Could not connect to the database"); ???
                logger.Info("successful query");
            }
            logger.InfoFormat("successful exit with value {0}", biletList);
            return biletList;
        }

        public Bilet FindOne(string id)
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
                comm.CommandText = "SELECT id, \"numeClient\", pret, \"idMeci\" FROM \"Bilete\" WHERE id=@id";
                IDbDataParameter paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = id;
                comm.Parameters.Add(paramId);

                using (var dataR = comm.ExecuteReader())
                {
                    if (dataR.Read())
                    {
                        //string id = dataR.GetString(0);
                        string numeClient = null;
                        var value = dataR.GetValue(1);
                        if (value != DBNull.Value)
                            numeClient = (string)value;
                        float pret = dataR.GetFloat(2);
                        string idMeci = dataR.GetString(3);

                        Bilet bilet = new Bilet(id, numeClient, pret, idMeci);
                        logger.InfoFormat("Exiting findOne with value {0}", bilet);
                        return bilet;
                    }
                }
            }
            logger.InfoFormat("Exiting findOne with value {0}", null);
            return null;
        }

        public Bilet Save(Bilet entity)
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
                comm.CommandText = "INSERT INTO \"Bilete\" VALUES (@id, @numeClient, @pret, @idMeci)";
                var paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = entity.id;
                comm.Parameters.Add(paramId);

                var paramNumeClient = comm.CreateParameter();
                paramNumeClient.ParameterName = "@numeClient";
                paramNumeClient.Value = entity.numeClient;
                comm.Parameters.Add(paramNumeClient);

                var paramPret = comm.CreateParameter();
                paramPret.ParameterName = "@pret";
                paramPret.Value = entity.pret;
                comm.Parameters.Add(paramPret);

                var paramIdMeci = comm.CreateParameter();
                paramIdMeci.ParameterName = "@idMeci";
                paramIdMeci.Value = entity.idMeci;
                comm.Parameters.Add(paramIdMeci);

                var result = comm.ExecuteNonQuery();
                if (result == 0)
                {
                    logger.Error("sql error - Niciun bilet adaugat!");
                    throw new RepositoryException("Niciun bilet adaugat!");
                }
            }
            logger.Info("successful query", null);
            return null;
        }

        public Bilet Update(Bilet entity)
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
                Bilet old = FindOne(entity.id);
                logger.Info("found data");

                using (var comm = con.CreateCommand())
                {
                    comm.CommandText = "UPDATE \"Bilete\" SET \"numeClient\"=@numeClient, pret=@pret, \"idMeci\"=@idMeci WHERE id=@id";
                    var paramId = comm.CreateParameter();
                    paramId.ParameterName = "@id";
                    paramId.Value = entity.id;
                    comm.Parameters.Add(paramId);

                    var paramNumeClient = comm.CreateParameter();
                    paramNumeClient.ParameterName = "@numeClient";
                    paramNumeClient.Value = entity.numeClient;
                    comm.Parameters.Add(paramNumeClient);

                    var paramPret = comm.CreateParameter();
                    paramPret.ParameterName = "@pret";
                    paramPret.Value = entity.pret;
                    comm.Parameters.Add(paramPret);

                    var paramIdMeci = comm.CreateParameter();
                    paramIdMeci.ParameterName = "@idMeci";
                    paramIdMeci.Value = entity.idMeci;
                    comm.Parameters.Add(paramIdMeci);

                    var result = comm.ExecuteNonQuery();
                    if (result == 0)
                    {
                        logger.Error("sql error - Niciun bilet adaugat!");
                        throw new RepositoryException("Niciun bilet adaugat!");
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
