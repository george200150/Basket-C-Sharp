using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.domain
{
    class Meci : Entity<string>
    {
        public string home { get; set; }
        public string away { get; set; }
        public DateTime date { get; set; }
        public TipMeci tip { get; set; }
        public int numarBileteDisponibile { get; set; }

        public Meci(String id, string home, string away, DateTime date, TipMeci tip, int numarBileteDisponibile)
        {
            base.id = id;
            this.home = home;
            this.away = away;
            this.date = date;
            this.tip = tip;
            this.numarBileteDisponibile = numarBileteDisponibile;
        }

        public override string ToString()
        {
            return "Meci{" +
                "id='" + base.id + '\'' +
                "home=" + home +
                ", away=" + away +
                ", date=" + date +
                ", tip=" + tip +
                ", numarBileteDisponibile=" + numarBileteDisponibile +
                '}';
        }
    }
}

