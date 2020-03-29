using Bakset.validators;
using Basket.domain;
using Basket.loggers;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.repositories
{
    class EchipaDataBaseRepository : ICrudRepository<string, Echipa>
    {
        private IValidator<Echipa> validator;
        IDbConnection con;

        public EchipaDataBaseRepository(IValidator<Echipa> validator)
        {
            Log.logger.Info("entry constructor");
            this.validator = validator;
            this.con = ADOInvariant.GetConnection();
            Log.logger.Info("successful constructor exit");
        }


        public Echipa Delete(string id)
        {
            Log.logger.Info("entry delete");

            if (id == null)
            {
                Log.logger.Error("null id exception");
                throw new RepositoryException("ID-ul nu poate fi NULL!");
            }

            Echipa echipa = FindOne(id);
            Log.logger.Info("found data");
            if (echipa != null) // this is a drawback... however... not gonna change it any sooner
            {
                using (var comm = con.CreateCommand())
                {
                    comm.CommandText = "DELETE FROM \"Echipe\" WHERE id=@id";
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
            Log.logger.InfoFormat("successful query with value {0}", echipa);
            return echipa;
        }

        public IEnumerable<Echipa> FindAll()
        {
            Log.logger.Info("entry findAll");
            IList<Echipa> echipaList = new List<Echipa>();
            using (var comm = con.CreateCommand())
            {
                Log.logger.Info("entry query");
                comm.CommandText = "SELECT id, nume FROM \"Echipe\"";

                using (var dataR = comm.ExecuteReader())
                {
                    while (dataR.Read())
                    {
                        string id = dataR.GetString(0);
                        string nume = dataR.GetString(1);

                        Echipa echipa = new Echipa(id, nume);
                        echipaList.Add(echipa);
                    }
                } // TODO: ??? throw new IllegalArgumentException("Error: Could not connect to the database"); ???
                Log.logger.Info("successful query");
            }
            Log.logger.InfoFormat("successful exit with value {0}", echipaList);
            return echipaList;
        }


        public Echipa FindOne(string id)
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
                comm.CommandText = "SELECT id,nume FROM \"Echipe\" WHERE id=@id";
                IDbDataParameter paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = id;
                comm.Parameters.Add(paramId);

                using (var dataR = comm.ExecuteReader())
                {
                    if (dataR.Read())
                    {
                        // string id = dataR.GetString(0);
                        string nume = dataR.GetString(1);

                        Echipa echipa = new Echipa(id, nume);
                        Log.logger.InfoFormat("Exiting findOne with value {0}", echipa);
                        return echipa;
                    }
                }
            }
            Log.logger.InfoFormat("Exiting findOne with value {0}", null);
            return null;
        }


        public Echipa Save(Echipa entity)
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
                comm.CommandText = "INSERT INTO \"Echipe\" VALUES (@id, @nume)";
                var paramId = comm.CreateParameter();
                paramId.ParameterName = "@id";
                paramId.Value = entity.id;
                comm.Parameters.Add(paramId);

                var paramNume = comm.CreateParameter();
                paramNume.ParameterName = "@nume";
                paramNume.Value = entity.nume;
                comm.Parameters.Add(paramNume);

                var result = comm.ExecuteNonQuery();
                if (result == 0)
                {
                    Log.logger.Error("sql error - Nicio echipa adaugata!");
                    throw new RepositoryException("Nicio echipa adaugata!");
                }
            }
            Log.logger.Info("successful query", null);
            return null;
        }


        public Echipa Update(Echipa entity)
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
                Echipa old = FindOne(entity.id);
                Log.logger.Info("found data");

                using (var comm = con.CreateCommand())
                {
                    comm.CommandText = "UPDATE \"Echipe\" SET nume=@nume WHERE id=@id";
                    var paramId = comm.CreateParameter();
                    paramId.ParameterName = "@id";
                    paramId.Value = entity.id;
                    comm.Parameters.Add(paramId);

                    var paramNume = comm.CreateParameter();
                    paramNume.ParameterName = "@nume";
                    paramNume.Value = entity.nume;
                    comm.Parameters.Add(paramNume);

                    var result = comm.ExecuteNonQuery();
                    if (result == 0)
                    {
                        Log.logger.Error("sql error - Nicio echipa adaugata!");
                        throw new RepositoryException("Nicio echipa adaugata!");
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

