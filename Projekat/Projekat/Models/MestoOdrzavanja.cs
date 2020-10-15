using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
    public class MestoOdrzavanja
    {
        private string adresa;

        public string Adresa
        {
            get { return adresa; }
            set { adresa = value; }
        }


        private string ulica;

        public string Ulica
        {
            get { return ulica; }
            set { ulica = value; }
        }
        private int broj;

        public int Broj
        {
            get { return broj; }
            set { broj = value; }
        }

        private string grad;

        public string Grad
        {
            get { return grad; }
            set { grad = value; }
        }

        private int postanski_Broj;

        public int Postanski_Broj
        {
            get { return postanski_Broj; }
            set { postanski_Broj = value; }
        }


        public override string ToString()
        {

            return adresa;
        }

        public MestoOdrzavanja(string ulica, int broj, string grad, int postanski_Broj)
        {
            this.ulica = ulica;
            this.broj = broj;
            this.grad = grad;
            this.postanski_Broj = postanski_Broj;
            adresa = String.Format(ulica + "|" + broj + "|"  + grad + "|" + postanski_Broj);

        }

        public MestoOdrzavanja()
        {
        }
    }
}