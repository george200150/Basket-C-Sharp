using System;

namespace Model.domain
{
    public class Client : Entity<string>
    {
        public string password { get; set; }
        public String nume { get; set; }

        public Client(String id, String password)
        {
            base.id = id;
            this.password = password;
        }

        public Client(String id, String password, String nume)
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
