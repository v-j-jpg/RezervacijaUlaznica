using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace Projekat.Models
{
    public class Baza
    {
        #region User
        public static Dictionary<string, Korisnik> ReadUsers(string path)
        {
            Dictionary<string, Korisnik> korisnici = new Dictionary<string, Korisnik>();

            path = HostingEnvironment.MapPath(path);

            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);

            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] tokens = line.Split('|');
                //  Korisnik k1 = new Korisnik((Korisnik_Uloga)Enum.Parse(typeof(Korisnik_Uloga), tokens[0]), tokens[1], tokens[2], tokens[3], tokens[4],tokens[5],tokens[6],);

                Korisnik k1 = new Korisnik(
                    tokens[0],//username
                    tokens[1],//pass
                    tokens[2],//name
                    tokens[3],//lastname
                    tokens[4],//gender
                    tokens[5],//date
                    (eUloga)Enum.Parse(typeof(eUloga), tokens[6]),//Uloga
                    Int32.Parse(tokens[7]),//br_bodova
                    (eTipKorisnika)Enum.Parse(typeof(eTipKorisnika), tokens[8])//tip korisnika
                    );
                korisnici.Add(k1.Username, k1);
            }
            sr.Close();
            stream.Close();

            return korisnici;
        }

        public static void SaveUser(Korisnik korisnik)
        {
            // save user in file users.txt

            string k = korisnik.Username + "|" + korisnik.Password + "|" + korisnik.Name + "|" +
                korisnik.LastName + "|" + korisnik.Gender + "|" + korisnik.DateOfBirth + "|" + korisnik.Uloga +
                "|" + korisnik.Br_Bodova + "|"+ korisnik.TipKorisnika.ImeTipaKorisnika + Environment.NewLine;

            string path = HostingEnvironment.MapPath("~/App_Data/korisnici.txt");

            //FileStream stream = new FileStream(path, FileMode.Append);

            //using (StreamWriter writetext = new StreamWriter(path))
            //{
            //    writetext.WriteLine(k);
            //}

            File.AppendAllText(path, k);
        }

        public static void UpdateUser(Dictionary<string, Korisnik> korisnik)
        {

            string path = HostingEnvironment.MapPath("~/App_Data/korisnici.txt");


            string k = "";
            foreach (var item in korisnik)
            {
                k+= item.Value.Username + "|" + item.Value.Password + "|" + item.Value.Name + "|" +
                  item.Value.LastName + "|" + item.Value.Gender + "|" + item.Value.DateOfBirth + "|" + item.Value.Uloga +
                  "|" + item.Value.Br_Bodova + "|" + item.Value.TipKorisnika.ImeTipaKorisnika + Environment.NewLine;


              
            }


            File.WriteAllText(path, k);


        }

        public static Dictionary<string, Manifestacija> Search(Dictionary<string, Manifestacija> manifestacije, string tipManifestacije)
        {
            Dictionary<string, Manifestacija> podudara = new Dictionary<string, Manifestacija>();

            eTipManifestacije tip = (eTipManifestacije)Enum.Parse(typeof(eTipManifestacije), tipManifestacije);

            foreach (var item in manifestacije)
           {
                if (item.Value.TipManifestacije.Equals(tip))
                {
                    podudara.Add(item.Key, item.Value);
                }
            }

            return podudara;
        }

        internal static Dictionary<string, Manifestacija> RangeCena(Dictionary<string, Manifestacija> manifestacije, string from1, string to1)
        {
            Dictionary<string, Manifestacija> podudara = new Dictionary<string, Manifestacija>();


            foreach (var item in manifestacije)
            {
                if (item.Value.Cena_REGULAR>= Int32.Parse(from1) && item.Value.Cena_REGULAR<= Int32.Parse(to1))
                {
                    podudara.Add(item.Key, item.Value);
                }
            }

            return podudara;
        }
        internal static Dictionary<string, Karta> RangePriceTickets(Dictionary<string, Karta> rezervacije, string from1, string to1)
        {
            Dictionary<string, Karta> podudara = new Dictionary<string, Karta>();


            foreach (var item in rezervacije)
            {
                if (item.Value.Naplacena_cena >= Int32.Parse(from1) && item.Value.Naplacena_cena <= Int32.Parse(to1))
                {
                    podudara.Add(item.Key, item.Value);
                }
            }

            return podudara;
        }

        internal static Dictionary<string, Manifestacija> SortDateAsc(Dictionary<string, Manifestacija> manifestacije)
        {
            return manifestacije.OrderBy(x => x.Value.Date).ToDictionary(x => x.Key, x => x.Value);
        }

        internal static Dictionary<string, Manifestacija> Unsold(Dictionary<string, Manifestacija> manifestacije)
        {
            Dictionary<string, Manifestacija> podudara = new Dictionary<string, Manifestacija>();


            foreach (var item in manifestacije)
            {
                if (item.Value.Br_mesta>0)
                {
                    podudara.Add(item.Key, item.Value);
                }
            }

            return podudara;
        }
        internal static Dictionary<string, Karta> RangeDateTicket(Dictionary<string, Karta> rezevacije, DateTime from1, DateTime to1)
        {
            Dictionary<string, Karta> podudara = new Dictionary<string, Karta>();

            //-1 --ranije
            //1--kasnije
            //0--jednako

            foreach (var item in rezevacije)
            {
                int rez1 = DateTime.Compare(item.Value.Manifestacija.Date, from1);
                int rez2 = DateTime.Compare(item.Value.Manifestacija.Date, to1);

                if (rez1 > 0 && rez2 < 0)
                {
                    podudara.Add(item.Key, item.Value);
                }
            }

            return podudara;

        }

        internal static Dictionary<string, Korisnik> FilterUloga(Dictionary<string, Korisnik> korisnici, string uloga)
        {
            Dictionary<string, Korisnik> podudara = new Dictionary<string, Korisnik>();

            eUloga tip = (eUloga)Enum.Parse(typeof(eUloga), uloga);

            foreach (var item in korisnici)
            {
                if (item.Value.Uloga.Equals(uloga))
                {
                    podudara.Add(item.Key, item.Value);
                }
            }

            return podudara;
        }

        internal static Dictionary<string, Korisnik> FilterTipKorisnika(Dictionary<string, Korisnik> korisnici, string tipKorisnika)
        {
            Dictionary<string, Korisnik> podudara = new Dictionary<string, Korisnik>();

            eTipKorisnika tip = (eTipKorisnika)Enum.Parse(typeof(eTipKorisnika), tipKorisnika);

            foreach (var item in korisnici)
            {
                if (item.Value.TipKorisnika.ImeTipaKorisnika.Equals(tipKorisnika))
                {
                    podudara.Add(item.Key, item.Value);
                }
            }

            return podudara;
        }

        internal static Dictionary<string, Manifestacija> RangeDate(Dictionary<string, Manifestacija> manifestacije, DateTime from1, DateTime to1)
        {
            Dictionary<string, Manifestacija> podudara = new Dictionary<string, Manifestacija>();

            //-1 --ranije
            //1--kasnije
            //0--jednako

            foreach (var item in manifestacije)
            {
                int rez1 = DateTime.Compare(item.Value.Date,from1);
                int rez2 = DateTime.Compare(item.Value.Date,to1);

                if (rez1>0 && rez2<0  )
                {
                    podudara.Add(item.Key, item.Value);
                }
            }

            return podudara;

        }

        internal static Dictionary<string, Manifestacija> SortDateDesc(Dictionary<string, Manifestacija> manifestacije)
        {
           return manifestacije.OrderByDescending(x => x.Value.Date).ToDictionary(x => x.Key, x => x.Value);
        }


        #endregion

        #region Manifestation
        public static Dictionary<string, Manifestacija> ReadManifestation(string path)
        {
            Dictionary<string, Manifestacija> manifestacija = new Dictionary<string, Manifestacija>();

            path = HostingEnvironment.MapPath(path);

            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);

            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] tokens = line.Split('|');
                

                Manifestacija m1 = new Manifestacija(
                    tokens[0],//naziv
                   (eTipManifestacije)Enum.Parse(typeof(eTipManifestacije), tokens[1]),//tip
                   Int32.Parse (tokens[2]),//br_mesta
                   DateTime.Parse(tokens[3]),//Date
                   Int32.Parse(tokens[4]),//Cena_REG
                   bool.Parse(tokens[5]),//status
                   tokens[6],//ulica
                   Int32.Parse(tokens[7]),//broj
                   tokens[8],//grad
                   Int32.Parse(tokens[9]),//postanski broj
                   tokens[10] ,//img path
                   tokens[11] ,//organisator_name
                   bool.Parse(tokens[12]), //Odobreno
                   double.Parse(tokens[13]) //prosecna ocena
                    );
                manifestacija.Add(m1.Naziv, m1);
            }
            sr.Close();
            stream.Close();

            return manifestacija;
        }

        internal static Dictionary<string, Karta> SortDateDescTickets(Dictionary<string, Karta> mojeRez)
        {
            return mojeRez.OrderByDescending(x => x.Value.Manifestacija.Date).ToDictionary(x => x.Key, x => x.Value);
        }

        internal static Dictionary<string, Karta> SearchTipKarte(Dictionary<string, Karta> rezervacije, string tipKarte)
        {
            Dictionary<string, Karta> podudara = new Dictionary<string, Karta>();

            eTipKarte tip = (eTipKarte)Enum.Parse(typeof(eTipKarte), tipKarte);

            foreach (var item in rezervacije)
            {
                if (item.Value.TipaKarte.Equals(tip))
                {
                    podudara.Add(item.Key, item.Value);
                }
            }

            return podudara;
        }

        internal static Dictionary<string, Karta> SortDateAscTickets(Dictionary<string, Karta> mojeRez)
        {
            return mojeRez.OrderBy(x => x.Value.Manifestacija.Date).ToDictionary(x => x.Key, x => x.Value);
        }

        public static void SaveManifestation(Manifestacija manifestacija)
        {
            // save user in file users.txt

            string k = manifestacija.Naziv + "|" + manifestacija.TipManifestacije + "|" + manifestacija.Br_mesta + "|" +
               manifestacija.Date + "|" + manifestacija.Cena_REGULAR + "|" + manifestacija.Status+ "|" + manifestacija.MestoOdrzavanja.Ulica+
                "|" + manifestacija.MestoOdrzavanja.Broj + "|" + manifestacija.MestoOdrzavanja.Grad + "|" + manifestacija.MestoOdrzavanja.Postanski_Broj +
               "|"  +manifestacija.Image_path+"|"
               +manifestacija.Organizator.Username+"|"+ manifestacija.Odobreno 
              +"|"+ manifestacija.ProsecnaOcena + Environment.NewLine;

            string path = HostingEnvironment.MapPath("~/App_Data/manifestacije.txt");

           

            File.AppendAllText(path, k);
        }
        public static void UpdateManifestation(Dictionary<string, Manifestacija> manifestacija)
        {
            string k = "";
            foreach (var item in manifestacija)
            {
                 k+= item.Value.Naziv + "|" + item.Value.TipManifestacije + "|" + item.Value.Br_mesta + "|" +
               item.Value.Date + "|" + item.Value.Cena_REGULAR + "|" + item.Value.Status + "|" + item.Value.MestoOdrzavanja.Ulica +
                  "|" + item.Value.MestoOdrzavanja.Broj + "|" + item.Value.MestoOdrzavanja.Grad + "|" + item.Value.MestoOdrzavanja.Postanski_Broj +
                 "|" + item.Value.Image_path + "|" + item.Value.Organizator.Username+"|"+item.Value.Odobreno

                   + "|" + item.Value.ProsecnaOcena  + Environment.NewLine;



            }
            File.WriteAllText(HostingEnvironment.MapPath("~/App_Data/manifestacije.txt"), k);

        }
        #endregion


        #region Rezervacija


        public static Dictionary<string, Karta> ReadTicket(string path)
        {
            Dictionary<string, Karta> karte = new Dictionary<string, Karta>();

            path = HostingEnvironment.MapPath(path);

            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);

            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] tokens = line.Split('|');

                Karta k1 = new Karta(
                    tokens[0],//id
                    tokens[1],//naziv manifest
                    DateTime.Parse( tokens[2]),//date
                    Int32.Parse(tokens[3]),//cena
                    tokens[4],//username
                   bool.Parse( tokens[5]),//status
                    (eTipKarte)Enum.Parse(typeof(eTipKarte), tokens[6])//tip karte
                    );

                karte.Add(tokens[0], k1);
            }
            sr.Close();
            stream.Close();

            return karte;
        }
        public static void SaveTicket(Karta karta,string tipKarte)
        {
            // save user in file users.txt

            string k = karta.ID+"|"+karta.Manifestacija.Naziv+"|"+karta.Manifestacija.Date+"|"+
                karta.Naplacena_cena + "|" + karta.Kupac
                .Username + "|" + karta.Status + "|" +
                tipKarte + Environment.NewLine;

            string path = HostingEnvironment.MapPath("~/App_Data/rezervacija.txt");

           

            File.AppendAllText(path, k);
        }

        public static void UpdateTicket(Dictionary<string, Karta> rez, string tipKarte)
        {
            string path = HostingEnvironment.MapPath("~/App_Data/rezervacije.txt");
            string k = "";
            foreach (var item in rez)
            {
                k += item.Value.ID + "|" + item.Value.Manifestacija.Naziv + "|" + item.Value.Manifestacija.Date + "|" +
               item.Value.Naplacena_cena + "|" + item.Value.Kupac.Username + "|" + item.Value.Status + "|" +
               tipKarte + Environment.NewLine;



            }
            File.WriteAllText(path, k);
        }
        #endregion

         #region Logger
        public static void Logger(Karta karta, string tipKarte, DateTime date)
        {
         

            string k = karta.ID + "|" + karta.Manifestacija.Naziv + "|" + karta.Manifestacija.Date + "|" +
                karta.Naplacena_cena + "|" + karta.Kupac
                .Username + "|" + karta.Status + "|" +
                tipKarte+"|"+date+ Environment.NewLine;

            string path = HostingEnvironment.MapPath("~/App_Data/logger.txt");



            File.AppendAllText(path, k);
        }

        public static Dictionary<string, Karta> ReadLogger(string path)
        {

            int brojac = 0;

            Dictionary<string, Karta> karte = new Dictionary<string, Karta>();

            path = HostingEnvironment.MapPath(path);

            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);

            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] tokens = line.Split('|');

                Karta k1 = new Karta(
                    tokens[0],//id
                    tokens[1],//naziv manifest
                    DateTime.Parse(tokens[2]),//date
                    Int32.Parse(tokens[3]),//cena
                    tokens[4],//username
                   bool.Parse(tokens[5]),//status
                    (eTipKarte)Enum.Parse(typeof(eTipKarte), tokens[6]),//tip karte
                     DateTime.Parse(tokens[7])//date
                    );
                brojac++;

                karte.Add(brojac.ToString(), k1);
            }
            sr.Close();
            stream.Close();

            return karte;
        }


        #endregion

        #region Komentar


        public static Dictionary<string, Komentar> ReadKomentar(string path)
        {
            Dictionary<string, Komentar> komentar = new Dictionary<string, Komentar>();

            path = HostingEnvironment.MapPath(path);

            FileStream stream = new FileStream(path, FileMode.Open);
            StreamReader sr = new StreamReader(stream);

            string line = "";
            while ((line = sr.ReadLine()) != null)
            {
                string[] tokens = line.Split('|');

                Komentar k1 = new Komentar(
                    tokens[0],//id
                    tokens[1],//manifestacija
                    tokens[2],//username
                    tokens[3],//text
                    Int32.Parse(tokens[4]),//ocena
                     (eStanjeKomentara)Enum.Parse(typeof(eStanjeKomentara),tokens[5]) //status
                    );

                komentar.Add(tokens[0], k1);
            }
            sr.Close();
            stream.Close();

            return komentar;
        }
        public static void SaveKomentar(Komentar komentar)
        {
            // save user in file users.txt

            string k = komentar.ID+"|"+komentar.Manifestacija.Naziv+"|"+
                komentar.Korisnik.Username+"|"+komentar.TextComment+"|"+komentar.Ocena 
                +"|"+ komentar.Status + Environment.NewLine;

            string path = HostingEnvironment.MapPath("~/App_Data/komentari.txt");



            File.AppendAllText(path, k);
        }

        public static void UpdateComments(Dictionary<string, Komentar> komentar)
        {
            string k = "";
            foreach (var item in komentar)
            {
                k += item.Value.ID + "|" + item.Value.Manifestacija.Naziv + "|" +
                item.Value.Korisnik.Username + "|" + item.Value.TextComment + "|" + item.Value.Ocena
                + "|" + item.Value.Status + Environment.NewLine;



            }
            File.WriteAllText(HostingEnvironment.MapPath("~/App_Data/komentari.txt"), k);

        }


        #endregion
    }
}