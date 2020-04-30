using System;

namespace Model.domain
{
    public class Client : Entity<string>
    {
        public string password { get; set; }
        public string nume { get; set; }
        public string host { get; set; }
        public int port { get; set; }

        public Client(string id, string password)
        {
            base.id = id;
            this.password = password;
        }

        public Client(string id, string password, string nume, string host, int port)
        {
            base.id = id;
            this.password = password;
            this.nume = nume;
            this.host = host;
            this.port = port;
        }

        public Client(string id, string password, string nume)
        {
            base.id = id;
            this.password = password;
            this.nume = nume;
        }

        public override string ToString()
        {
            return "Client{" +
            "id='" + base.id + '\'' +
            '}';
        }
    }
}
