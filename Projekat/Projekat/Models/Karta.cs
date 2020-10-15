using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
    public class Karta
    {
        
        private int brojac = 0;

        private string id;

        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        private Manifestacija manifestacija;

        public Manifestacija Manifestacija
        {
            get { return manifestacija; }
            set { manifestacija = value; }
        }

        private DateTime date;

        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        private int cena;

        public int Cena
        {
            get { return cena; }
            set { cena = value; }
        }

        private Korisnik kupac;

        public Korisnik Kupac
        {
            get { return kupac; }
            set { kupac = value; }
        }

        private bool status;

        public bool Status
        {
            get { return status; }
            set { status = value; }
        }
        private int naplacena_cena;

        public int Naplacena_cena
        {
            get { return naplacena_cena; }
            set { naplacena_cena = value; }
        }


        private eTipKarte tipaKarte;

        public eTipKarte TipaKarte
        {
            get { return tipaKarte; }
            set { tipaKarte = value; }
        }
        private int br_karata;

        public int Br_karata
        {
            get { return br_karata; }
            set { br_karata = value; }
        }

        private DateTime loggTime;

        public DateTime LoggTime
        {
            get { return loggTime; }
            set { loggTime = value; }
        }

        
        public Karta(string id,string naziv,DateTime datum, int cena, string username, bool status,  eTipKarte tipKarte)
        {

             this.ID=id;

            manifestacija = new Manifestacija();
            kupac = new Korisnik();

            manifestacija.Naziv = naziv;
            manifestacija.Date = datum;

            kupac.Username = username;

            Naplacena_cena=cena;
           
            Status = status;
            TipaKarte = tipKarte;

        }
        //Logger
        public Karta(string id, string naziv, DateTime datum, int cena, string username, bool status, eTipKarte tipKarte,DateTime time)
        {

            this.ID = id;

            manifestacija = new Manifestacija();
            kupac = new Korisnik();

            manifestacija.Naziv = naziv;
            manifestacija.Date = datum;

            kupac.Username = username;

            Naplacena_cena = cena;

            Status = status;
            TipaKarte = tipKarte;
            LoggTime = time;

        }

        public Karta()
        {
        }

     

        public Karta(int id,Korisnik korisnik, Manifestacija manifestacija, eTipKarte tipKarte, int cena_REG,int br_karata)
        {
            CommonMethods cm = new CommonMethods();
            this.kupac = korisnik;
            this.manifestacija = manifestacija;
            ID = id.ToString();
            //ID = (++brojac).ToString();
            Br_karata = br_karata;
            Naplacena_cena = br_karata*cm.GetCenaKarte(tipKarte, cena_REG);//da li je vip,fan pit,regular

        }

    }
}