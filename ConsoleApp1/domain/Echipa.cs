using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket
{
    class Echipa : Entity<string>
    {
        public string nume { get; set; }

        public Echipa(String id, string nume)
        {
            base.id = id;
            this.nume = nume;
        }

        public override string ToString()
        {
            return "Echipa{" +
                "id='" + base.id + '\'' +
                "nume='" + nume + '\'' +
                '}';
        }

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (!(obj is Echipa)) return false;
            Echipa echipa = (Echipa)obj;
            return Object.Equals(this.id, echipa.id) &&
                Object.Equals(this.nume, echipa.nume);
        }

        public override int GetHashCode()
        {
            return -1482261198 + EqualityComparer<string>.Default.GetHashCode(nume);
        }

    }
}
