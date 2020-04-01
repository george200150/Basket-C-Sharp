using System;
using System.Collections.Generic;
using System.Data;

using Model.domain;
using Model.validators;


namespace Persistence.repos
{
    public class MeciDataBaseRepository : ICrudRepository<string, Meci>
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private IValidator<Meci> validator;
        IDbConnection con;

        public MeciDataBaseRepository(IValidator<Meci> validator)
        {
            logger.Info("entry constructor");
            this.validator = validator;
            this.con = ADOInvariant.GetConnection();
            logger.Info("successful constructor exit");
        }

        public Meci Delete(string id)
        {
            logger.Info("entry delete");

            if (id == null)
            {
                logger.Error("null id exception");
                throw new RepositoryException("ID-ul nu poate fi NULL!");
            }

            Meci meci = FindOne(id);
            logger.Info("found data");
            if (meci != null) // this is a drawback... however... not gonna change it any sooner
            {
                using (var comm = con.CreateCommand())
                {
                    comm.CommandText = "DELETE FROM \"Meciuri\" WHERE id=@id";
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
            logger.InfoFormat("successful query with value {0}", meci);
            return meci;
        }

        public IEnumerable<Meci> FindAll()
        {
            logger.Info("entry findAll");
            IList<Meci> meciList = new List<Meci>();
            using (var comm = con.CreateCommand())
            {
                logger.Info("entry query");
                comm.CommandText = "SELECT id, home, away, \"dataCalendar\", tip, \"numarBilete\" FROM \"Meciuri\"";

                using (var dataR = comm.ExecuteReader())
                {
                    while (dataR.Read())
                    {
                        string id = dataR.GetString(0);
                        string home = dataR.GetString(1);
                        string away = dataR.GetString(2);
                        DateTime date = dataR.GetDateTime(3);
                        TipMeci tip = (TipMeci)dataR.GetInt32(4);
                        int numarBileteDisponibile = dataR.GetInt32(5);

                        Meci meci = new Meci(id, home, away, date, tip, numarBileteDisponibile);
                        meciList.Add(meci);
                    }
                } // TODO: ??? throw new IllegalArgumentException("Error: Could not connect to the database"); ???
                logger.Info("successful query");
            }
            logger.InfoFormat("successful exit with value {0}", meciList);
            return meciList;
        }


        public Meci FindOne(string id)
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
                comm.CommandText = "SELECT id, home, away, \"dataCalendar\", tip, \"numarBilete\" FROM \"Meciuri\" WHERE id=@id";
                IDbDataParameter paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = id;
                comm.Parameters.Add(paramId);

                using (var dataR = comm.ExecuteReader())
                {
                    if (dataR.Read())
                    {
                        // string id = dataR.GetString(0);
                        string home = dataR.GetString(1);
                        string away = dataR.GetString(2);
                        DateTime date = dataR.GetDateTime(3);
                        TipMeci tip = (TipMeci)dataR.GetInt32(4);
                        int numarBileteDisponibile = dataR.GetInt32(5);
                        Meci meci = new Meci(id, home, away, date, tip, numarBileteDisponibile);
                        logger.InfoFormat("Exiting findOne with value {0}", meci);
                        return meci;
                    }
                }
            }
            logger.InfoFormat("Exiting findOne with value {0}", null);
            return null;
        }

        public Meci Save(Meci entity)
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
                comm.CommandText = "INSERT INTO \"Meciuri\" VALUES (@id, @home, @away, @dataCalendar, @tip, @numarBilete)";
                var paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = entity.id;
                comm.Parameters.Add(paramId);

                var paramHome = comm.CreateParameter();
                paramHome.ParameterName = "@home";
                paramHome.Value = entity.home;
                comm.Parameters.Add(paramHome);

                var paramAway = comm.CreateParameter();
                paramAway.ParameterName = "@away";
                paramAway.Value = entity.away;
                comm.Parameters.Add(paramAway);

                var paramDate = comm.CreateParameter();
                paramDate.ParameterName = "@dataCalendar";
                paramDate.Value = entity.date;
                comm.Parameters.Add(paramDate);

                var paramTip = comm.CreateParameter();
                paramTip.ParameterName = "@tip";
                paramTip.Value = (int)entity.tip;
                comm.Parameters.Add(paramTip);

                var paramNumarBileteDisponibile = comm.CreateParameter();
                paramNumarBileteDisponibile.ParameterName = "@numarBilete";
                paramNumarBileteDisponibile.Value = entity.numarBileteDisponibile;
                comm.Parameters.Add(paramNumarBileteDisponibile);

                var result = comm.ExecuteNonQuery();
                if (result == 0)
                {
                    logger.Error("sql error - Niciun meci adaugat!");
                    throw new RepositoryException("Niciun meci adaugat!");
                }
            }
            logger.Info("successful query", null);
            return null;
        }


        public Meci Update(Meci entity)
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
                Meci old = FindOne(entity.id);
                logger.Info("found data");

                using (var comm = con.CreateCommand())
                {
                    comm.CommandText = "UPDATE \"Meciuri\" SET home=@home, away=@away, \"dataCalendar\"=@dataCalendar, tip=@tip, \"numarBilete\"=@numarBilete WHERE id=@id";
                    var paramId = comm.CreateParameter();
                    paramId.ParameterName = "@id";
                    paramId.Value = entity.id;
                    comm.Parameters.Add(paramId);

                    var paramHome = comm.CreateParameter();
                    paramHome.ParameterName = "@home";
                    paramHome.Value = entity.home;
                    comm.Parameters.Add(paramHome);

                    var paramAway = comm.CreateParameter();
                    paramAway.ParameterName = "@away";
                    paramAway.Value = entity.away;
                    comm.Parameters.Add(paramAway);

                    var paramDate = comm.CreateParameter();
                    paramDate.ParameterName = "@dataCalendar";
                    paramDate.Value = entity.date;
                    comm.Parameters.Add(paramDate);

                    var paramTip = comm.CreateParameter();
                    paramTip.ParameterName = "@tip";
                    paramTip.Value = (int) entity.tip;
                    comm.Parameters.Add(paramTip);

                    var paramNumarBileteDisponibile = comm.CreateParameter();
                    paramNumarBileteDisponibile.ParameterName = "@numarBilete";
                    paramNumarBileteDisponibile.Value = entity.numarBileteDisponibile;
                    comm.Parameters.Add(paramNumarBileteDisponibile);

                    var result = comm.ExecuteNonQuery();
                    if (result == 0)
                    {
                        logger.Error("sql error - Niciun meci adaugat!");
                        throw new RepositoryException("Niciun meci adaugat!");
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
