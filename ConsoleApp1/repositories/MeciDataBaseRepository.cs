using Bakset.validators;
using Basket.loggers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.repositories
{
    class MeciDataBaseRepository : ICrudRepository<string, Meci>
    {
        private IValidator<Meci> validator;
        IDbConnection con;

        public MeciDataBaseRepository(IValidator<Meci> validator)
        {
            Log.logger.Info("entry constructor");
            this.validator = validator;
            this.con = ADOInvariant.GetConnection();
            Log.logger.Info("successful constructor exit");
        }

        public Meci Delete(string id)
        {
            Log.logger.Info("entry delete");

            if (id == null)
            {
                Log.logger.Error("null id exception");
                throw new RepositoryException("ID-ul nu poate fi NULL!");
            }

            Meci meci = FindOne(id);
            Log.logger.Info("found data");
            if (meci != null) // this is a drawback... however... not gonna change it any sooner
            {
                using (var comm = con.CreateCommand())
                {
                    comm.CommandText = "DELETE FROM \"Meciuri\" WHERE id=@id";
                    IDbDataParameter paramId = comm.CreateParameter();
                    paramId.ParameterName = "@id";
                    paramId.Value = id;
                    comm.Parameters.Add(paramId);
                    Log.logger.Info("entry query");
                    var dataR = comm.ExecuteNonQuery();
                    if (dataR == 0)
                        throw new RepositoryException("No task deleted!");
                }
            }
            Log.logger.InfoFormat("successful query with value {0}", meci);
            return meci;
        }

        public IEnumerable<Meci> FindAll()
        {
            Log.logger.Info("entry findAll");
            IList<Meci> meciList = new List<Meci>();
            using (var comm = con.CreateCommand())
            {
                Log.logger.Info("entry query");
                comm.CommandText = "SELECT id, home, away, date, \"numarBileteDisponibile\" FROM \"Meciuri\"";

                using (var dataR = comm.ExecuteReader())
                {
                    while (dataR.Read())
                    {
                        string id = dataR.GetString(0);
                        string home = dataR.GetString(1);
                        string away = dataR.GetString(2);
                        DateTime date = dataR.GetDateTime(3);
                        int numarBileteDisponibile = dataR.GetInt32(4);

                        Meci meci = new Meci(id, home, away, date, numarBileteDisponibile);
                        meciList.Add(meci);
                    }
                } // TODO: ??? throw new IllegalArgumentException("Error: Could not connect to the database"); ???
                Log.logger.Info("successful query");
            }
            Log.logger.InfoFormat("successful exit with value {0}", meciList);
            return meciList;
        }


        public Meci FindOne(string id)
        {
            if (id == null)
            {
                Log.logger.Error("null id exception");
                throw new RepositoryException("ID-ul NU POATE FI NULL");
            }
            Log.logger.InfoFormat("Entering findOne with value {0}", id);

            using (var comm = con.CreateCommand())
            {
                Log.logger.Info("entry query");
                comm.CommandText = "SELECT id, home, away, date, \"numarBileteDisponibile\" FROM \"Meciuri\" WHERE id=@id";
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
                        int numarBileteDisponibile = dataR.GetInt32(4);

                        Meci meci = new Meci(id, home, away, date, numarBileteDisponibile);
                        Log.logger.InfoFormat("Exiting findOne with value {0}", meci);
                        return meci;
                    }
                }
            }
            Log.logger.InfoFormat("Exiting findOne with value {0}", null);
            return null;
        }

        public Meci Save(Meci entity)
        {
            Log.logger.Info("entry save");
            if (entity == null)
            {
                Log.logger.Error("null id exception");
                throw new RepositoryException("ENTITATEA NU POATE FI NULL");
            }

            validator.Validate(entity);
            Log.logger.Info("validated data");
            if (FindOne(entity.id) != null)
            {
                Log.logger.Error("duplicate found exception");
                throw new ValidationException("DUPLICAT GASIT!");
            }

            using (var comm = con.CreateCommand())
            {
                comm.CommandText = "INSERT INTO \"Meciuri\" VALUES (@id, @home, @away, @date, @numarBileteDisponibile)";
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
                paramDate.ParameterName = "@date";
                paramDate.Value = entity.date;
                comm.Parameters.Add(paramDate);

                var paramNumarBileteDisponibile = comm.CreateParameter();
                paramNumarBileteDisponibile.ParameterName = "@numarBileteDisponibile";
                paramNumarBileteDisponibile.Value = entity.numarBileteDisponibile;
                comm.Parameters.Add(paramNumarBileteDisponibile);

                var result = comm.ExecuteNonQuery();
                if (result == 0)
                {
                    Log.logger.Error("sql error - Niciun meci adaugat!");
                    throw new RepositoryException("Niciun meci adaugat!");
                }
            }
            Log.logger.Info("successful query", null);
            return null;
        }


        public Meci Update(Meci entity)
        {
            Log.logger.Info("entry update");
            if (entity == null)
            {
                Log.logger.Error("null entity exception");
                throw new RepositoryException("Entitatea nu poate fi NULL!");
            }
            validator.Validate(entity);
            Log.logger.Info("validated data");

            if (FindOne(entity.id) != null)
            {
                Meci old = FindOne(entity.id);
                Log.logger.Info("found data");

                using (var comm = con.CreateCommand())
                {
                    comm.CommandText = "UPDATE \"Meciuri\" SET home=@home, away=@away, date=@date, \"numarBileteDisponibile\"=@numarBileteDisponibile WHERE id=@id";
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
                    paramDate.ParameterName = "@date";
                    paramDate.Value = entity.date;
                    comm.Parameters.Add(paramDate);

                    var paramNumarBileteDisponibile = comm.CreateParameter();
                    paramNumarBileteDisponibile.ParameterName = "@numarBileteDisponibile";
                    paramNumarBileteDisponibile.Value = entity.numarBileteDisponibile;
                    comm.Parameters.Add(paramNumarBileteDisponibile);

                    var result = comm.ExecuteNonQuery();
                    if (result == 0)
                    {
                        Log.logger.Error("sql error - Niciun meci adaugat!");
                        throw new RepositoryException("Niciun meci adaugat!");
                    }

                    Log.logger.InfoFormat("successful query with value {0}", old);
                    return old;
                }
            }
            Log.logger.InfoFormat("not found data. unsuccessful update with {0}", null);
            return null;
        }

    }
}
