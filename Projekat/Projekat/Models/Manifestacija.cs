using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
    public class Manifestacija
    {
      
        [Required]
        [Display(Name = "naziv")]
        [DataType(DataType.Text)]

        private string naziv;

        public string Naziv
        {
            get { return naziv; }
            set { naziv = value; }
        }

        private eTipManifestacije tipManifestacije;

        public eTipManifestacije TipManifestacije
        {
            get { return tipManifestacije; }
            set { tipManifestacije = value; }
        }
        private int br_mesta;

        public int Br_mesta
        {
            get { return br_mesta; }
            set { br_mesta = value; }
        }
        private DateTime date;

        public DateTime Date
        {
            get { return date; }
            set { date = value; }
        }

        private int cena_REGULAR;

        public int Cena_REGULAR
        {
            get { return cena_REGULAR; }
            set { cena_REGULAR = value; }
        }

        private bool satus;

        public bool Status
        {
            get { return satus; }
            set { satus = value; }
        }

        private MestoOdrzavanja mestoOdrzavanja;

        private Korisnik organizator;

        public Korisnik Organizator
        {
            get { return organizator; }
            set { organizator = value; }
        }


        private string image_path;

        public string Image_path
        {
            get { return image_path; }
            set { image_path = value; }
        }

        private bool odobreno;

        public bool Odobreno
        {
            get { return odobreno; }
            set { odobreno = value; }
        }


        private double prosecnaOcena;

        public double ProsecnaOcena
        {
            get { return prosecnaOcena; }
            set { prosecnaOcena = value; }
        }


        public MestoOdrzavanja MestoOdrzavanja { get => mestoOdrzavanja; set => mestoOdrzavanja = value; }

        public Manifestacija(string naziv, eTipManifestacije tipManifestacije, int br_mesta, DateTime date, int cena_REGULAR,bool status, string ulica, int broj, string grad, int postanski_Broj, string image_path,string organisator_name,bool odobreno,double prosek)
        {
            MestoOdrzavanja = new MestoOdrzavanja();
            Organizator = new Korisnik();

            this.naziv = naziv;
            this.tipManifestacije = tipManifestacije;
            this.br_mesta = br_mesta;
            this.date = date;
            this.cena_REGULAR = cena_REGULAR;
            this.satus = status;

            MestoOdrzavanja.Ulica = ulica;
            MestoOdrzavanja.Broj = broj;
            MestoOdrzavanja.Grad = grad;
            MestoOdrzavanja.Postanski_Broj = postanski_Broj;

            Organizator.Username = organisator_name;
            
            this.image_path = image_path;
            this.odobreno = odobreno;
            ProsecnaOcena = prosek;
        }

        public Manifestacija()
        {
            
        }


        //  Image newImage = Image.FromFile("SampImag.jpg");

    }
}