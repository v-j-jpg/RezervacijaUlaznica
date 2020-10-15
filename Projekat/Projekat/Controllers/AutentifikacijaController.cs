using Projekat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Projekat.Controllers
{
    public class AutentifikacijaController : Controller
    {
        // GET: Autentifikacija
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Registracija()
        {
            Korisnik korisnik = new Korisnik();
            Session["user"] = korisnik;
            return View(korisnik);
        }
        public ActionResult AddSalesman()
        {
            Korisnik korisnik = new Korisnik();
            Session["user"] = korisnik;
            return View(korisnik);
        }

        [HttpPost]
        public ActionResult AddSalesman(Korisnik korisnik)
        {


            Dictionary<string, Korisnik> korisnici = (Dictionary<string, Korisnik>)HttpContext.Application["Korisnici"];

            if (korisnici.ContainsKey(korisnik.Username))
            {
                ViewBag.Message = $"User with {korisnik.Username} already exists!";
                return View("AddSalesman");
            }




            Session["korisnik"] = korisnik;
            korisnik.Br_Bodova = 0;
            korisnik.TipKorisnika = new TipKorisnika(eTipKorisnika.BRONZANI);
            korisnik.Uloga = eUloga.PRODAVAC;

            korisnici.Add(korisnik.Username, korisnik);

            Baza.SaveUser(korisnik);

            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        public ActionResult Registracija(Korisnik korisnik)
        {


            Dictionary<string, Korisnik> korisnici = (Dictionary<string, Korisnik>)HttpContext.Application["Korisnici"];

            if (korisnici.ContainsKey(korisnik.Username))
            {
                ViewBag.Message = $"User with {korisnik.Username} already exists!";
                return View("Registracija");
            }




            Session["korisnik"] = korisnik;
            korisnik.Uloga = eUloga.KUPAC;
            korisnik.Br_Bodova = 0;
            korisnik.TipKorisnika = new TipKorisnika(eTipKorisnika.BRONZANI);

            korisnici.Add(korisnik.Username, korisnik);

            Baza.SaveUser(korisnik);

            return RedirectToAction("Index", "Autentifikacija");
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {

            Dictionary<string, Korisnik> korisnici = (Dictionary<string, Korisnik>)HttpContext.Application["Korisnici"];
            // Dictionary<string, Kupovina> korpa = (Dictionary<string, Kupovina>)HttpContext.Application["Korpa"];

            Korisnik k = new Korisnik();
            // Kupovina k1 = new Kupovina();

            foreach (Korisnik korisnik in korisnici.Values)
            {
                if (korisnici.ContainsKey(username) && korisnici[username].Password.Equals(password))
                {

                    k = korisnici[username];
                    Session["korisnik"] = k;



                }
                else
                {
                    //ViewBag.Message = $"User {username} with credentials does not exist!";


                    ViewBag.Message = "Your username/password isn't good!";
                    return View("Index");

                }

            }


            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            Session["korisnik"] = null;



            return RedirectToAction("Index", "Home");
        }
        public ActionResult GetUsers()
        {

            Dictionary<string, Korisnik> korisnici = (Dictionary<string, Korisnik>)HttpContext.Application["Korisnici"];


            ViewBag.Korisnici = korisnici.Values;
            return View();

        }

        public ActionResult GetUsersProfile(string username)
        {

            Dictionary<string, Korisnik> korisnici = (Dictionary<string, Korisnik>)HttpContext.Application["Korisnici"];
            Korisnik k = new Korisnik();


            foreach (Korisnik korisnik in korisnici.Values)
            {
                if (korisnici.ContainsKey(username))
                {

                    k = korisnici[username];
                    Session["Korisnik"] = k;
                    ViewBag.Korisnik = k;
                    return View();

                }
                else
                {
                    //ViewBag.Message = $"User {username} with credentials does not exist!";


                    ViewBag.Message = "Your username/password isn't good!";
                    return View("Index");

                }

            }

            return RedirectToAction("Index", "Home");



        }
        public ActionResult Delete(string username)
        {

            Dictionary<string, Korisnik> korisnici = (Dictionary<string, Korisnik>)HttpContext.Application["Korisnici"];

            korisnici.Remove(username);

            ViewBag.Korisnici = korisnici.Values;

            return RedirectToAction("GetUsers", "Autentifikacija");

        }

        [HttpPost]
        public ActionResult EditUser(Korisnik k)
        {


            Dictionary<string, Korisnik> korisnici = (Dictionary<string, Korisnik>)HttpContext.Application["Korisnici"];

            CommonMethods cm = new CommonMethods();

            k.TipKorisnika = new TipKorisnika(cm.GetTipKorisnika(k.Br_Bodova));

            korisnici[k.Username] = k;
            Baza.UpdateUser(korisnici);

            return RedirectToAction("Index", "Home");



        }
        public ActionResult EditUser(string username)
        {
            Dictionary<string, Korisnik> korisnici = (Dictionary<string, Korisnik>)HttpContext.Application["Korisnici"];


            ViewBag.Korisnik = korisnici[username];

            return View();
        }
        public ActionResult Filter(string tipKorisnika, string uloga)
        {
            Dictionary<string, Korisnik> korisnici = (Dictionary<string, Korisnik>)HttpContext.Application["Korisnici"];

            korisnici = Baza.FilterTipKorisnika(korisnici, tipKorisnika);
            korisnici = Baza.FilterUloga(korisnici, uloga);


            Session["Korisnici"] = korisnici;



            Korisnik k = (Korisnik)Session["Korisnik"];


            if (k == null)
            {
                ViewBag.Korisnici = korisnici.Values;
                return View("Index");
                //return View((IEnumerable<string>)vozila);
            }


            ViewBag.Korisnici = korisnici.Values;


            return View("GetUsers");

        }
    }
}