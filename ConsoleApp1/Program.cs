using Bakset.validators;
using Basket.repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket
{
    class Program
    {
        static void Main(string[] args)
        {

            ClientDataBaseRepository repository = new ClientDataBaseRepository(ClientValidator.GetInstance());
            Client client = new Client("tiganul", "1234");
            Client nukkClient = repository.FindOne("123");
            Client mustBeNullClient = repository.Save(client);
            Client clientOne = repository.FindOne(client.id);
            repository.Delete(clientOne.id);
            IEnumerable<Client> rezC = repository.FindAll();


            EchipaDataBaseRepository repositoryE = new EchipaDataBaseRepository(EchipaValidator.GetInstance());
            Echipa echipa = new Echipa("tiganul", "1234");
            Echipa nukkEchipa = repositoryE.FindOne("123");
            Echipa mustBeNullEchipa = repositoryE.Save(echipa);
            Echipa echipaOne = repositoryE.FindOne(echipa.id);
            repositoryE.Delete(echipaOne.id);
            IEnumerable<Echipa> rezE = repositoryE.FindAll();


            BiletDataBaseRepository repositoryB = new BiletDataBaseRepository(BiletValidator.GetInstance());
            Bilet bilet = new Bilet("tiganul", "numele clientului", (float)123.123, "id meci", "id client");
            Bilet nukkBilet = repositoryB.FindOne("123");
            Bilet mustBeNullBilet = repositoryB.Save(bilet);
            Bilet biletOne = repositoryB.FindOne(bilet.id);
            repositoryB.Delete(biletOne.id);
            IEnumerable<Bilet> rezB = repositoryB.FindAll();


            MeciDataBaseRepository repositoryM = new MeciDataBaseRepository(MeciValidator.GetInstance());
            Meci meci = new Meci("id", "home", "away", DateTime.Now, 16);
            Meci nukkMeci = repositoryM.FindOne("123");
            Meci mustBeNullMeci = repositoryM.Save(meci);
            Meci meciOne = repositoryM.FindOne(meci.id);
            repositoryM.Delete(biletOne.id);
            IEnumerable<Meci> rezM = repositoryM.FindAll();


            // The code provided will print ‘Hello World’ to the console.
            // Press Ctrl+F5 (or go to Debug > Start Without Debugging) to run your app.
            Console.WriteLine("Hello World!");
            Console.ReadKey();

            // Go to http://aka.ms/dotnet-get-started-console to continue learning how to build a console app! 
            
        }
    }
}


