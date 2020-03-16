using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket
{
    class Bilet : Entity<string>
    {
        public string numeClient { get; set; }
        public float pret { get; set; }
        public string idMeci { get; set; }
        public string idClient { get; set; }

        public Bilet(String id, String numeClient, float pret, String idMeci)
        {
            base.id = id;
            this.numeClient = numeClient;
            this.pret = pret;
            this.idMeci = idMeci;
            this.idClient = null;
        }

        public Bilet(String id, String numeClient, float pret, String idMeci, String idClient)
        {
            base.id = id;
            this.numeClient = numeClient;
            this.pret = pret;
            this.idMeci = idMeci;
            this.idClient = idClient;
        }

        public override string ToString()
        {
            return "Bilet{" +
                "numeClient='" + numeClient + '\'' +
                ", pret=" + pret +
                ", idMeci='" + idMeci + '\'' +
                ", idClient='" + idClient + '\'' +
                '}';
        }
    }
}

