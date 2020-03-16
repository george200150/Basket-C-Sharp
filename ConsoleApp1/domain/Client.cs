using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basket
{
    class Client : Entity<string>
    {
        public string password { get; set; }

        public Client(String id, String password)
        {
            base.id = id;
            this.password = password;
        }

        public override string ToString()
        {
            return "Client{" +
            "id='" + base.id + '\'' +
            '}';
        }
    }
}
