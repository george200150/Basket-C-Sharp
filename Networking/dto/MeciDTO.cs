using System;
using Model.domain;


namespace Networking.dto
{
    [Serializable()]
    public class MeciDTO
    {
        public String id { get; }
        public String home { get; }
        public String away { get; }
        public DateTime date { get; }
        public TipMeci tip { get; }
        public int numarBilete { get; }

        public MeciDTO(String id, String home, String away, DateTime date, TipMeci tip, int numarBilete)
        {
            this.id = id;
            this.home = home;
            this.away = away;
            this.date = date;
            this.tip = tip;
            this.numarBilete = numarBilete;
        }
    }
}
