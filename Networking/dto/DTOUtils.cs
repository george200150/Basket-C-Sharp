using System;
using Model.domain;


namespace Networking.dto
{
    public class DTOUtils
    {
        public static Client getFromDTO(UserDTO usdto)
        {
            String id = usdto.id;
            String pass = usdto.passwd;
            String nume = usdto.nume;
            Client client = new Client(id, pass, nume);
            return client;
        }
        public static UserDTO getDTO(Client user)
        {
            String id = user.id;
            String pass = user.password;
            String nume = user.nume;
            return new UserDTO(id, pass, nume);
        }

        public static UserDTO[] getDTO(Client[] users)
        {
            UserDTO[] frDTO = new UserDTO[users.Length];
            for (int i = 0; i < users.Length; i++)
                frDTO[i] = getDTO(users[i]);
            return frDTO;
        }

        public static Client[] getFromDTO(UserDTO[] users)
        {
            Client[] friends = new Client[users.Length];
            for (int i = 0; i < users.Length; i++)
            {
                friends[i] = getFromDTO(users[i]);
            }
            return friends;
        }


        public static Meci getFromDTO(MeciDTO meciDTO)
        {
            String id = meciDTO.id;
            String home = meciDTO.home;
            String away = meciDTO.away;
            DateTime date = meciDTO.date;
            TipMeci tip = meciDTO.tip;
            int numarBileteDisponibile = meciDTO.numarBilete;
            return new Meci(id, home, away, date, tip, numarBileteDisponibile);
        }
        public static Meci[] getFromDTO(MeciDTO[] meciDTOs)
        {
            Meci[] meciuri = new Meci[meciDTOs.Length];
            for (int i = 0; i < meciDTOs.Length; i++)
            {
                meciuri[i] = getFromDTO(meciDTOs[i]);
            }
            return meciuri;
        }
        public static MeciDTO getDTO(Meci meci)
        {
            String id = meci.id;
            String home = meci.home;
            String away = meci.away;
            DateTime date = meci.date;
            TipMeci tip = meci.tip;
            int numarBileteDisponibile = meci.numarBileteDisponibile;
            return new MeciDTO(id, home, away, date, tip, numarBileteDisponibile);
        }

        public static MeciDTO[] getDTO(Meci[] meciuri)
        {
            MeciDTO[] meciuriDTO = new MeciDTO[meciuri.Length];
            for (int i = 0; i < meciuri.Length; i++)
            {
                meciuriDTO[i] = getDTO(meciuri[i]);
            }
            return meciuriDTO;
        }



        public static Bilet[] getFromDTO(BiletDTO[] bileteDTOs)
        {
            Bilet[] bilete = new Bilet[bileteDTOs.Length];
            for (int i = 0; i < bileteDTOs.Length; i++)
            {
                bilete[i] = getFromDTO(bileteDTOs[i]);
            }
            return bilete;
        }
        public static BiletDTO[] getDTO(Bilet[] bilete)
        {
            BiletDTO[] biletdtos = new BiletDTO[bilete.Length];
            for (int i = 0; i < bilete.Length; i++)
            {
                biletdtos[i] = getDTO(bilete[i]);
            }
            return biletdtos;
        }
        public static BiletDTO getDTO(Bilet bilet)
        {
            String id = bilet.id;
            String numeClient = bilet.numeClient;
            float pret = bilet.pret;
            String idMeci = bilet.idMeci;
            String idClient = bilet.idClient;

            return new BiletDTO(id, numeClient, pret, idMeci, idClient);
        }
        public static Bilet getFromDTO(BiletDTO biletDTO)
        {
            String id = biletDTO.id;
            String numeClient = biletDTO.numeClient;
            float pret = biletDTO.pret;
            String idMeci = biletDTO.idMeci;
            String idClient = biletDTO.idClient;

            return new Bilet(id, numeClient, pret, idMeci, idClient);
        }
    }
}
