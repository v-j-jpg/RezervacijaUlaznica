using Projekat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Projekat.Controllers
{
    public class RezervacijaController : Controller
    {
        // GET: Rezervacija
        public ActionResult Index()
        {


            return View();
        }
        // GET: Rezervacija/Reserve
        public ActionResult Reserve(string naziv,string tipKarte,int br_karata)
        {

            CommonMethods cm = new CommonMethods();

            Dictionary<string, Manifestacija> manifestacija = (Dictionary<string, Manifestacija>)HttpContext.Application["Manifestacije"];
            Dictionary<string, Korisnik> korisnici = (Dictionary<string, Korisnik>)HttpContext.Application["Korisnici"];

            Dictionary<string, Karta> rez = (Dictionary<string, Karta>)HttpContext.Application["Rezervacije"];

            ViewBag.JednaManifestacija = manifestacija[naziv];

            Manifestacija m = manifestacija[naziv];

            Korisnik k = (Korisnik)Session["Korisnik"];


            if (m.Br_mesta>br_karata)
            {
                //IZMENITI BROJ PREOSTALIH MESTA NA MANIFESTACIJI
                m.Br_mesta -= br_karata;
                manifestacija[naziv] = m;//postavi izmenjenu manifest na svoje mesto u listi

                Baza.UpdateManifestation(manifestacija);//update broj mesta

                //napravi kartu
                int lenght = rez.Count();
                Karta karta1 = new Karta(++lenght, k, m, (eTipKarte)Enum.Parse(typeof(eTipKarte), tipKarte), m.Cena_REGULAR, br_karata);

                karta1.Status = true;// rezervisana


                Session["Rezervacija"] = karta1;


                rez.Add(karta1.ID, karta1);

                ViewBag.Message = "You are close to reserve a ticket, congrats!";

                //Dodajemo bodove za kupovinu

                k.Br_Bodova = cm.GetBodovi(k.Br_Bodova, karta1.Naplacena_cena);
                //Da li korisnik moze da predje u drugi tip?
                //Provera

                 k.TipKorisnika.ImeTipaKorisnika=cm.GetTipKorisnika(k.Br_Bodova);
                 korisnici[k.Username] = k;
                 Baza.UpdateUser(korisnici);

                //Popust
                double popust= cm.GetPopust(k.TipKorisnika.ImeTipaKorisnika,double.Parse(karta1.Naplacena_cena.ToString()));
                karta1.Naplacena_cena -=(int)Math.Floor(popust);
                ViewBag.Popust = popust;

                ///////////
               Baza.Logger(karta1, tipKarte,DateTime.Now);//logujemo promenu
                Baza.SaveTicket(karta1,tipKarte); //sacuvaj kupljenu kartu
                ViewBag.Korpa = rez.Values;//stavi u korisnikovu korpu


                ////RACUN
           
                int cena_reg = manifestacija[naziv].Cena_REGULAR;
                //uzimamo poslednje unetu kartu

               
                ViewBag.Karta = karta1;
                ViewBag.CenaJedneKarte = cm.GetCenaKarte((eTipKarte)Enum.Parse(typeof(eTipKarte), tipKarte), cena_reg);
                
                
            }
            else
            {
                ViewBag.Message("There is no tickets left!");
                return RedirectToAction("GetManifestationDetail", "Manifestacija");
            }


     

            return View();
        }

        public ActionResult Confirmation()
        {
                   

            return RedirectToAction("Index","Home");
        }

        public ActionResult DeleteTicket(string id,string tipKarte,int br_karata)
        {
            Dictionary<string, Manifestacija> manifestacija = (Dictionary<string, Manifestacija>)HttpContext.Application["Manifestacije"];
            Dictionary<string, Korisnik> korisnici = (Dictionary<string, Korisnik>)HttpContext.Application["Korisnici"];

            Dictionary<string, Karta> rez = (Dictionary<string, Karta>)HttpContext.Application["Rezervacije"];
            Korisnik k = (Korisnik)Session["Korisnik"];

            manifestacija[rez[id].Manifestacija.Naziv].Br_mesta+=br_karata;
            Baza.UpdateManifestation(manifestacija);//vracamo br mesta

            rez[id].Status = false;//karta je free opet
            int bodovi_nakon_gubitka = new CommonMethods().RemoveBodovi(rez[id].Kupac.Br_Bodova, rez[id].Naplacena_cena);

            if (manifestacija[rez[id].Manifestacija.Naziv].Date<(DateTime.Now.AddDays(7)) )//ukoliko je manje od 7 dana ranije otkazao 
            {
                if (k.Br_Bodova < bodovi_nakon_gubitka)//ukoliko korisnik ima manje nego sto moze da izgubi
                {
                    k.Br_Bodova = 0;
                    korisnici[k.Username] = k;
                    Baza.UpdateUser(korisnici);
                }
                //oduzmimo bodove
                k.Br_Bodova = bodovi_nakon_gubitka;
                korisnici[k.Username] = k;
                Baza.UpdateUser(korisnici);

            }
         
            
       

            Baza.Logger(rez[id], tipKarte,DateTime.Now);//logujemo promenu

          //  rez.Remove(id);//brisemo kartu
           
            Baza.UpdateTicket(rez,tipKarte);
            

            return RedirectToAction("Index", "Home");
        }
        public ActionResult GetAllTickets()
        {
            Dictionary<string, Karta> rez = (Dictionary<string, Karta>)HttpContext.Application["Rezervacije"];
            List<Karta> korisnikKupovine = new List<Karta>();

            Korisnik k = (Korisnik)Session["Korisnik"];



            foreach (var proizvodi in rez)
            {
                if (k.Username.Equals(proizvodi.Value.Kupac.Username))
                {
                    korisnikKupovine.Add(proizvodi.Value);
                }
            }


            ViewBag.Korpa = korisnikKupovine;
            return View();
        }
        public ActionResult LoggerHistory()
        {

            Dictionary<string, Karta> rez = (Dictionary<string, Karta>)HttpContext.Application["Istorija"];
          
            ViewBag.Istorija = rez.Values;
           
            return View();


          
        }
        public ActionResult Filter(string tipKarte,string from1,string to1,string from,string to)
        {
            Dictionary<string, Karta> rezervacije = (Dictionary<string, Karta>)HttpContext.Application["Rezervacije"];
            Korisnik k = (Korisnik)Session["Korisnik"];

            Dictionary<string, Karta> mojeRez = new Dictionary<string, Karta>();

            foreach (var item in rezervacije)
            {
                if (item.Value.Kupac.Username.Equals(k.Username))
                {
                    mojeRez.Add(item.Key, item.Value);
                }
            }
                mojeRez = Baza.SearchTipKarte(mojeRez, tipKarte);
                mojeRez = Baza.RangePriceTickets(mojeRez, from1, to1);
                mojeRez = Baza.RangeDateTicket(mojeRez, DateTime.Parse(from), DateTime.Parse(to));

            Session["Rezervacije"] = mojeRez;
            

            // Dictionary<string, Korisnik> korisnici = (Dictionary<string, Korisnik>)HttpContext.Application["Korisnici"];


            if (k == null)
            {
                ViewBag.Korpa = mojeRez.Values;
                return View("Index");
                //return View((IEnumerable<string>)vozila);
            }


            ViewBag.korisnici = k;
            ViewBag.Korpa = mojeRez.Values;

            return View("GetAllTickets");

        }
        public ActionResult SortDateDesc()
        {

            Dictionary<string, Karta> rezervacije = (Dictionary<string, Karta>)HttpContext.Application["Rezervacije"];
            Korisnik k = (Korisnik)Session["Korisnik"];

            Dictionary<string, Karta> mojeRez = new Dictionary<string, Karta>();

            foreach (var item in rezervacije)
            {
                if (item.Value.Kupac.Username.Equals(k.Username))
                {
                    mojeRez.Add(item.Key, item.Value);
                }
            }
            mojeRez = Baza.SortDateDescTickets(mojeRez);

            Session["Rezervacije"] = mojeRez;


            // Dictionary<string, Korisnik> korisnici = (Dictionary<string, Korisnik>)HttpContext.Application["Korisnici"];


            if (k == null)
            {
                ViewBag.Korpa = mojeRez.Values;
                return View("Index");
                //return View((IEnumerable<string>)vozila);
            }


            ViewBag.korisnici = k;
            ViewBag.Korpa = mojeRez.Values;

            return View("GetAllTickets");
        }

        public ActionResult SortDateAsc()
        {

            Dictionary<string, Karta> rezervacije = (Dictionary<string, Karta>)HttpContext.Application["Rezervacije"];
            Korisnik k = (Korisnik)Session["Korisnik"];

            Dictionary<string, Karta> mojeRez = new Dictionary<string, Karta>();

            foreach (var item in rezervacije)
            {
                if (item.Value.Kupac.Username.Equals(k.Username))
                {
                    mojeRez.Add(item.Key, item.Value);
                }
            }
            mojeRez = Baza.SortDateAscTickets(mojeRez);

            Session["Rezervacije"] = mojeRez;


            // Dictionary<string, Korisnik> korisnici = (Dictionary<string, Korisnik>)HttpContext.Application["Korisnici"];


            if (k == null)
            {
                ViewBag.Korpa = mojeRez.Values;
                return View("Index");
                //return View((IEnumerable<string>)vozila);
            }


            ViewBag.korisnici = k;
            ViewBag.Korpa = mojeRez.Values;

            return View("GetAllTickets");
        }
    }
  
}