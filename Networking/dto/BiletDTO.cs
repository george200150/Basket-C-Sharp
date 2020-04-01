using System;


namespace Networking.dto
{
    [Serializable()]
    public class BiletDTO
    {
        public String id { get; }
        public String numeClient { get; }
        public float pret { get; }
        public String idMeci { get; }
        public String idClient { get; }

        public BiletDTO(String id, String numeClient, float pret, String idMeci, String idClient)
        {
            this.id = id;
            this.numeClient = numeClient;
            this.pret = pret;
            this.idMeci = idMeci;
            this.idClient = idClient;
        }
    }
}
