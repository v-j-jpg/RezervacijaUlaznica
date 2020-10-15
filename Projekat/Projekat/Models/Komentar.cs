using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
    public class Komentar
    {
        private string id;

        public string ID
        {
            get { return id; }
            set { id = value; }
        }


        private Korisnik korisnik;

        public Korisnik Korisnik
        {
            get { return korisnik; }
            set { korisnik = value; }
        }

        private Manifestacija manifestacija;

        public Manifestacija Manifestacija
        {
            get { return manifestacija; }
            set { manifestacija = value; }
        }

        private string textComment;

        public string TextComment
        {
            get { return textComment; }
            set { textComment = value; }
        }

        private int ocena;
       

        public int Ocena
        {
            get { return ocena; }
            set { ocena = value; }
        }

        private eStanjeKomentara staus;

        public eStanjeKomentara Status
        {
            get { return staus; }
            set { staus = value; }
        }



        public Komentar(string id, string naziv_manif, string username, string textComment, int ocena, eStanjeKomentara status)
        {
            manifestacija = new Manifestacija();
            korisnik = new Korisnik();

            this.id = id;

            manifestacija.Naziv = naziv_manif;
            korisnik.Username = username;

            this.textComment = textComment;
            this.ocena = ocena;
            Status = status;
        }

        public override string ToString()
        {
            return korisnik + " " + manifestacija + " " + textComment + " " + ocena;
        }
    }
}