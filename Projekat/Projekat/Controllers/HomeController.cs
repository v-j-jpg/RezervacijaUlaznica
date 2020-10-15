using Projekat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Projekat.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Dictionary<string, Manifestacija> manifestacija = (Dictionary<string, Manifestacija>)HttpContext.Application["Manifestacije"];

            Dictionary<string, Komentar> komentar = (Dictionary<string, Komentar>)HttpContext.Application["Komentari"];

            List<FileUpload> files = (List<FileUpload>)HttpContext.Application["files"];


            Korisnik k = (Korisnik)Session["Korisnik"];
            Session["Manifestacije"] = manifestacija;


            int brojac = 0;
            bool promena=false;

           foreach (var item in manifestacija )
                {

                //izracunaj prosek ocena
                manifestacija[item.Value.Naziv].ProsecnaOcena = new CommonMethods().ProsecnaOcena(komentar, item.Value.Naziv);

                
              
                if (item.Value.Date < DateTime.Now && item.Value.Status.Equals(true))//Da li je manifestacija prosla
                {
                  
                    item.Value.Status = false;
                    manifestacija[item.Value.Naziv] = item.Value;
                    promena = true;
                }
                brojac++;

                    if (manifestacija.Count==brojac)
                    {
                        break; //Dodato jer enumeracija prestne da radi 
                    }
                
            

            }
            if (promena)
            {
                
                Baza.UpdateManifestation(manifestacija);
                manifestacija = (Dictionary<string, Manifestacija>)HttpContext.Application["Manifestacije"];
            }

            
         


            if (k == null)
            {
                ViewBag.SveManifestacije = manifestacija.Values;
                return View("Index");
                //return View((IEnumerable<string>)vozila);
            }


            ViewBag.korisnici = k;
            ViewBag.Images = files;

            manifestacija = Baza.SortDateDesc(manifestacija);
            ViewBag.SveManifestacije = manifestacija.Values;

            return View();
        }
        

        public ActionResult Filter(string tipManifestacije, string from1,string to1,string from,string to)
        {

            Dictionary<string, Manifestacija> manifestacije = (Dictionary<string, Manifestacija>)HttpContext.Application["Manifestacije"];

            if (to1 != null && from1!=null)
            {
                manifestacije = Baza.RangeCena(manifestacije, from1,to1);
                manifestacije = Baza.RangeDate(manifestacije, DateTime.Parse(from),DateTime.Parse(to));
                manifestacije = Baza.Search(manifestacije, tipManifestacije);


                Session["Manifestacije"] = manifestacije;
            }



            Korisnik k = (Korisnik)Session["Korisnik"];


            if (k == null)
            {
                ViewBag.SveManifestacije = manifestacije.Values;
                return View("Index");
                //return View((IEnumerable<string>)vozila);
            }


            ViewBag.korisnici = k;
            ViewBag.SveManifestacije = manifestacije.Values;

            return View("Index");
        }
        public ActionResult SortDateDesc()
        {

            Dictionary<string, Manifestacija> manifestacije = (Dictionary<string, Manifestacija>)HttpContext.Application["Manifestacije"];

         
                manifestacije = Baza.SortDateDesc(manifestacije);
              


                Session["Manifestacije"] = manifestacije;
            



            Korisnik k = (Korisnik)Session["Korisnik"];


            if (k == null)
            {
                ViewBag.SveManifestacije = manifestacije.Values;
                return View("Index");
                //return View((IEnumerable<string>)vozila);
            }


            ViewBag.korisnici = k;
            ViewBag.SveManifestacije = manifestacije.Values;

            return View("Index");
        }
        public ActionResult SortDateAsc()
        {

            Dictionary<string, Manifestacija> manifestacije = (Dictionary<string, Manifestacija>)HttpContext.Application["Manifestacije"];


            manifestacije = Baza.SortDateAsc(manifestacije);



            Session["Manifestacije"] = manifestacije;




            Korisnik k = (Korisnik)Session["Korisnik"];


            if (k == null)
            {
                ViewBag.SveManifestacije = manifestacije.Values;
                return View("Index");
                //return View((IEnumerable<string>)vozila);
            }


            ViewBag.korisnici = k;
            ViewBag.SveManifestacije = manifestacije.Values;

            return View("Index");
        }


    }
}