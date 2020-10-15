using Projekat.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Projekat
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            // Učitamo korisnike iz datoteke u memoriju
            Dictionary<string, Korisnik> korisnici = Baza.ReadUsers("~/App_Data/korisnici.txt");
            HttpContext.Current.Application["Korisnici"] = korisnici;


            // Učitamo manifestacije iz datoteke u memoriju
            Dictionary<string, Manifestacija> manifestacija = Baza.ReadManifestation("~/App_Data/manifestacije.txt");
            HttpContext.Current.Application["Manifestacije"] = manifestacija;

            // Učitamo logger iz datoteke u memoriju
            Dictionary<string, Karta> logg = Baza.ReadLogger("~/App_Data/logger.txt");
            HttpContext.Current.Application["Istorija"] = logg;

            // Učitamo korpu iz datoteke u memoriju
            Dictionary<string, Karta> karte = Baza.ReadTicket("~/App_Data/rezervacija.txt");
            HttpContext.Current.Application["Rezervacije"] = karte;

            // Učitamo komentare iz datoteke u memoriju
            Dictionary<string, Komentar> komentari = Baza.ReadKomentar("~/App_Data/komentari.txt");
            HttpContext.Current.Application["Komentari"] = komentari;

            string path = Path.Combine(Server.MapPath("~/Files/"));
            List<FileUpload> files = new List<FileUpload>();


            foreach (string file in Directory.GetFiles(path))
            {
                files.Add(new FileUpload(Path.GetFileName(file), file));
            }
            HttpContext.Current.Application["Files"] = files;
        }
    }
}
