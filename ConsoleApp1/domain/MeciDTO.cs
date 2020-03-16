using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket.domain
{
    class MeciDTO
    {
        public string id { get; set; }
        public Echipa home { get; set; }
        public Echipa away { get; set; }
        public DateTime date { get; set; }
        public TipMeci tip { get; set; }
        public int numarBilete { get; set; }

        public string homeString { get; set; }
        public string awayString { get; set; }
        public string numarBileteSauSoldOut { get; set; }


        public MeciDTO(String id, Echipa home, Echipa away, DateTime date, TipMeci tip, int numarBilete)
        {
            this.id = id;
            this.home = home;
            this.away = away;
            this.date = date;
            this.tip = tip;
            this.numarBilete = numarBilete;

            this.homeString = home.ToString();
            this.awayString = away.ToString();
            if (numarBilete == 0)
                this.numarBileteSauSoldOut = "SOLD OUT";
            else
                this.numarBileteSauSoldOut = numarBilete + "";
        }
    }
}
