using Bakset.validators;
using Basket.repositories;
using Basket.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Basket
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            ClientDataBaseRepository repositoryC = new ClientDataBaseRepository(ClientValidator.GetInstance());
            EchipaDataBaseRepository repositoryE = new EchipaDataBaseRepository(EchipaValidator.GetInstance());
            BiletDataBaseRepository repositoryB = new BiletDataBaseRepository(BiletValidator.GetInstance());
            MeciDataBaseRepository repositoryM = new MeciDataBaseRepository(MeciValidator.GetInstance());

            ClientService serviceC = new ClientService(repositoryC);
            EchipaService serviceE = new EchipaService(repositoryE);
            BiletService serviceB = new BiletService(repositoryB);
            MeciService serviceM = new MeciService(repositoryM);
            MasterService masterService = new MasterService(serviceB, serviceC, serviceE, serviceM);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new LoginPage(masterService));
        }
    }
}
