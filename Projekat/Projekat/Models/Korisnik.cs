using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
    public class Korisnik
    {
       
        public string Username { get; set; }

        private string password;

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        private string name;

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        private string lastName;

        public string LastName
        {
            get { return lastName; }
            set { lastName = value; }
        }

        private string gender;

        public string Gender
        {
            get { return gender; }
            set { gender = value; }
        }
        private string dateOfBirth;

        public string DateOfBirth { get => dateOfBirth; set => dateOfBirth = value; }


        private eUloga uloga;

        public eUloga Uloga
        {
            get { return uloga; }
            set { uloga = value; }
        }


        List<Karta> SveKarteBezObziraNaStatus ;

        List<Manifestacija> manifestacije;

        private int br_bodova;

        public int Br_Bodova
        {
            get { return br_bodova; }
            set { br_bodova = value; }
        }

        public TipKorisnika TipKorisnika { get => tipKorisnika; set => tipKorisnika = value; }

        TipKorisnika tipKorisnika;

     

        public Korisnik(string username, string password, string name, string lastName, string gender, string dateOfBirth, eUloga uloga, int br_bodova,eTipKorisnika tipKorisnika)
        {
           

            Username = username;
            this.password = password;
            this.name = name;
            this.lastName = lastName;
            this.gender = gender;
            DateOfBirth = dateOfBirth;
            this.uloga = uloga;
            this.br_bodova = br_bodova;
            this.tipKorisnika = new TipKorisnika(tipKorisnika);
           
        }

        public Korisnik(string username)
        {
            Username = username;
        }

        public Korisnik()
        {
        }
    }
}