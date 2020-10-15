using Projekat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Projekat.Controllers
{
    public class KomentarController : Controller
    {
        // GET: Komentar
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Komentar(string textComment, string username,int ocena,string naziv)
        {
            Dictionary<string, Manifestacija> manifestacija = (Dictionary<string, Manifestacija>)HttpContext.Application["Manifestacije"];            Dictionary<string, Karta> rez = (Dictionary<string, Karta>)HttpContext.Application["Rezervacije"];
            Dictionary<string, Karta> rezervacije = (Dictionary<string, Karta>)HttpContext.Application["Rezervacije"];
            Dictionary<string, Komentar> komentari = (Dictionary<string, Komentar>)HttpContext.Application["Komentari"];

            int lenght=komentari.Count;
            Komentar k = new Komentar((lenght++).ToString(),naziv,username, textComment,ocena,eStanjeKomentara.PENDING);

            Baza.SaveKomentar(k);

            

            return RedirectToAction("Index","Home");
        }
        public ActionResult RequestComments()
        {
            Dictionary<string, Manifestacija> manifestacija = (Dictionary<string, Manifestacija>)HttpContext.Application["Manifestacije"]; Dictionary<string, Karta> rez = (Dictionary<string, Karta>)HttpContext.Application["Rezervacije"];

            Dictionary<string, Komentar> komentari = new Dictionary<string, Komentar>();
            Korisnik k = (Korisnik)Session["Korisnik"];

            komentari = Baza.ReadKomentar("~/App_Data/komentari.txt");

            //foreach (var item in manifestacija)
            //{
            //    if (k.Username.Equals(item.Value.Organizator.Username))
            //    {
            //        ViewBag.Organizator = item.Value.Organizator.Username;
            //        ViewBag.ManifestacijeOrganizatora = item.Value;
            //    }
            //}

            ViewBag.Korisnik = k;
            ViewBag.Komentari = komentari.Values;

            return View();
        }
        public ActionResult PostComment(string id)
        {
            //Dictionary<string, Komentar> komentari = (Dictionary<string, Komentar>)HttpContext.Application["Komentari"];

            Dictionary<string, Komentar> komentari = Baza.ReadKomentar("~/App_Data/komentari.txt");
            
          
            komentari[id].Status = eStanjeKomentara.ACCEPTED;

            Baza.UpdateComments(komentari);

            ViewBag.Komentari = komentari.Values;
            komentari= (Dictionary<string, Komentar>)HttpContext.Application["Komentari"];

            return View("RequestComments");
        }
        public ActionResult DeniedComment(string id)
        {
            Dictionary<string, Komentar> komentari = (Dictionary<string, Komentar>)HttpContext.Application["Komentari"];

            komentari[id].Status = eStanjeKomentara.DENIED;

            Baza.UpdateComments(komentari);

            ViewBag.Komentari = komentari.Values;

            return View("RequestComments");
        }
        public ActionResult DeleteComment(string ID,string naziv)
        {
            Dictionary<string, Komentar> komentari = (Dictionary<string, Komentar>)HttpContext.Application["Komentari"];

            komentari.Remove(ID);

            return RedirectToAction("GetManifestationDetail","Manifestacija",new { Naziv=naziv });
        }
        
    }
}