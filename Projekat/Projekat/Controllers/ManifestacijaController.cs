using Projekat.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static System.Net.WebRequestMethods;

namespace Projekat.Controllers
{
    public class ManifestacijaController : Controller
    {
        // GET: Manifestacija
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AddManifestation()
        {
            Manifestacija manifestacija = new Manifestacija();
            Session["manifestacija"] = manifestacija;
            return View(manifestacija);
        }


    
        [HttpPost]
        public ActionResult AddManifestation(Manifestacija m, HttpPostedFileBase file)
        {
            if (m.Date > DateTime.Now)
            {
                m.Status = true;
            }
            else
            {
                m.Status = false;
            }

         

            Dictionary<string, Manifestacija> manifestacija = (Dictionary<string, Manifestacija>)HttpContext.Application["Manifestacije"];
            List<FileUpload> files = (List<FileUpload>)HttpContext.Application["files"];
            Korisnik k = (Korisnik)Session["Korisnik"];

            foreach (var item in manifestacija)
            {
                if (m.MestoOdrzavanja.Ulica.Equals(item.Value.MestoOdrzavanja.Ulica) //ulica
                    && m.MestoOdrzavanja.Broj.Equals(item.Value.MestoOdrzavanja.Broj) && //broj 
                    m.MestoOdrzavanja.Grad.Equals(item.Value.MestoOdrzavanja.Grad) //grad
                    && m.Date.Equals(item.Value.Date)) //vreme
                {
                    ViewBag.Message = "Already registered at that location or time!";
                    return View();
                }
               
            }
        
            m.Odobreno = false;
            m.Organizator = new Korisnik(k.Username);
            m.ProsecnaOcena = 0;


            if (manifestacija.ContainsKey(m.Naziv))
                {
                    ViewBag.Message = "Already registered!";
                    return View();
                }



                if (manifestacija == null)
                {
                    ViewBag.Message = "Not added, must enter all information!";
                    return View();
                }

                Session["manifestacija"] = manifestacija;



                try
                {
                    if (file.ContentLength > 0)
                    {

                        string fileName = Path.GetFileName(file.FileName);
                        string path = Path.Combine(Server.MapPath("~/Files/"), fileName);
                        file.SaveAs(path);
                        files.Add(new FileUpload(fileName, path));


                        m.Image_path = fileName;


                    }
                    ViewBag.Message = "File Uploaded Successfully!!";

                }
                catch
                {
                    m.Image_path = "";
                    ViewBag.Message = "File upload failed!!";

                }





                manifestacija.Add(m.Naziv, m);
                Baza.SaveManifestation(m);
            

            return RedirectToAction("Index", "Home");



        }

        public ActionResult GetManifestation()
        {

            Dictionary<string, Manifestacija> manifestacija = (Dictionary<string, Manifestacija>)HttpContext.Application["Manifestacije"];
            Dictionary<string, Karta> rez = (Dictionary<string, Karta>)HttpContext.Application["Rezervacije"];
            Korisnik k = (Korisnik)Session["Korisnik"];

            ViewBag.Korisnik = k;
          
          

            foreach (var item in manifestacija)
            {
                if (k.Username.Equals(item.Value.Organizator.Username))
                {
                    ViewBag.Organizator = item.Value.Organizator.Username;
                    ViewBag.ManifestacijeOrganizatora = item.Value;
                }
            }

            //int br = 0;
            //foreach (var item in manifestacija)
            //{
            //    string naziv_manifest = item.Value.Naziv;
            //    Karta trenutna_rez = rez[(br++).ToString()];

            //    if (naziv_manifest.Equals(trenutna_rez.Manifestacija.Naziv))
            //    {
            //        trenutna_rez.Manifestacija.Organizator = new Korisnik(item.Value.Organizator.Username);
            //    }
               
                   
            //}

            ViewBag.Rezervacije = rez.Values;
            ViewBag.SveManifestacije = manifestacija.Values;

            return View();
        }
        [HttpPost]
        public ActionResult EditManifestation(Manifestacija m, HttpPostedFileBase file)
        {
            if (m.Date>DateTime.Now)
            {
                m.Status = true;
            }
            else
            {
                m.Status = false;
            }
           
            m.Odobreno = false;

            Dictionary<string, Manifestacija> manifestacija = (Dictionary<string, Manifestacija>)HttpContext.Application["Manifestacije"];
            List<FileUpload> files = (List<FileUpload>)HttpContext.Application["files"];

            foreach (var item in manifestacija)
            {
                if (m.MestoOdrzavanja.Ulica.Equals(item.Value.MestoOdrzavanja.Ulica) //ulica
                    && m.MestoOdrzavanja.Broj.Equals(item.Value.MestoOdrzavanja.Broj) && //broj 
                    m.MestoOdrzavanja.Grad.Equals(item.Value.MestoOdrzavanja.Grad) //grad
                    && m.Date.Equals(item.Value.Date)) //vreme
                {
                    ViewBag.Message = "Already registered at that location or time!";
                    return View();
                }

            }

            manifestacija[m.Naziv] = m;

            try
            {
                foreach (var item in manifestacija)//ukoliko se nista ne doda
                {//problem: file is readonly cannot be set file.FileName=rnd;

                    if (manifestacija.ContainsKey(m.Naziv))
                    {
                        manifestacija[m.Naziv].Image_path = item.Value.Image_path;
                    }
                }

                if (file.ContentLength > 0)
                {

                    string fileName = Path.GetFileName(file.FileName);
                    string path = Path.Combine(Server.MapPath("~/Files/"), fileName);
                    file.SaveAs(path);
                    files.Add(new FileUpload(fileName, path));


                  manifestacija[m.Naziv].Image_path = fileName;

                  

                }
            }
            catch 
            {
                ViewBag.Message = "Error";
           
            }
            


            Baza.UpdateManifestation(manifestacija);

            return RedirectToAction("Index", "Home");




        }
        public ActionResult EditManifestation(string naziv)
        {
            Dictionary<string, Manifestacija> manifestacija = (Dictionary<string, Manifestacija>)HttpContext.Application["Manifestacije"];


            ViewBag.Manifestacije = manifestacija[naziv];

            return View();
        }

        public ActionResult GetManifestationDetail(string naziv)
        {

            Dictionary<string, Manifestacija> manifestacija = (Dictionary<string, Manifestacija>)HttpContext.Application["Manifestacije"];
            Dictionary<string, Karta> rez = (Dictionary<string, Karta>)HttpContext.Application["Rezervacije"];
            Dictionary<string, Komentar> komentari = (Dictionary<string, Komentar>)HttpContext.Application["Komentari"];

            Korisnik k = (Korisnik)Session["Korisnik"];

            ViewBag.Rezervacije = rez.Values;
            ViewBag.Manifestacija = manifestacija[naziv];
            ViewBag.Korisnik = k;

             ViewBag.Komentari = komentari.Values;

            return View();
        }
        public ActionResult Requests()
        {
            Dictionary<string, Manifestacija> manifestacija = (Dictionary<string, Manifestacija>)HttpContext.Application["Manifestacije"];
          

          
            ViewBag.SveManifestacije = manifestacija.Values;
             

            return View();

        }
        public ActionResult PostManifestation(string naziv)
        {
            Dictionary<string, Manifestacija> manifestacija = (Dictionary<string, Manifestacija>)HttpContext.Application["Manifestacije"];


            manifestacija[naziv].Odobreno = true;
            Baza.UpdateManifestation(manifestacija);

      

            return RedirectToAction("Requests", "Manifestacija");

        }

        public ActionResult GetAllManifestation()
        {

            Dictionary<string, Manifestacija> manifestacija = (Dictionary<string, Manifestacija>)HttpContext.Application["Manifestacije"];
            //Dictionary<string, Karta> rez = (Dictionary<string, Karta>)HttpContext.Application["Rezervacije"];
            Korisnik k = (Korisnik)Session["Korisnik"];

            ViewBag.Korisnik = k;



            foreach (var item in manifestacija)
            {
                if (k.Username.Equals(item.Value.Organizator.Username))
                {
                    ViewBag.Organizator = item.Value.Organizator.Username;
                    ViewBag.ManifestacijeOrganizatora = item.Value;
                }
            }

     

           // ViewBag.Rezervacije = rez.Values;
            ViewBag.SveManifestacije = manifestacija.Values;

            return View();
        }

        public ActionResult Delete(string Naziv)
        {

            Dictionary<string, Manifestacija> manifestacije = (Dictionary<string, Manifestacija>)HttpContext.Application["Manifestacije"];

            manifestacije.Remove(Naziv);

            ViewBag.Manifestacije = manifestacije.Values;

            return RedirectToAction("Index", "Home");

        }

    }
}
