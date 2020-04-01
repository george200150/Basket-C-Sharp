using System;


namespace Networking.dto
{
    [Serializable()]
    public class UserDTO
    {
        public String id { get; }
        public String passwd { get; }
        public String nume { get; }

        public UserDTO(String id, String passwd, String nume)
        {
            this.id = id;
            this.passwd = passwd;
            this.nume = nume;
        }

        public override String ToString()
        {
            return "UserDTO[" + id + ' ' + passwd + "]";
        }
    }
}
