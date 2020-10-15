using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
    public class CommonMethods
    {
        public double GetPopust(eTipKorisnika imeTipaKorisnika,double cena)
        {

          
            // Potrebno je da student samostalno definiše skalu bodova za Tipove korisnika(npr.da bi
            //korisnik postao Srebrni potrebno je da sakupi 3000 bodova(i tom prilikom ima npr.
            //popust od 3 % prilikom svake kupovine), a da bi postao Zlatan potrebno je da sakupi
            //4000 bodova(i tom prilikom ima npr.popust od 5 % prilikom svake kupovine)).
   

            switch (imeTipaKorisnika)
            {
                case eTipKorisnika.BRONZANI: { return 0; }//nema popust
                case eTipKorisnika.SREBRNI: { return cena* 0.3;  }
                case eTipKorisnika.ZLATNI: {  return cena* 0.4;  }
                default: return 0;
            }

            
        }


        public eTipKorisnika GetTipKorisnika(int bodovi)
        {
            if (bodovi < 3000)
            {
                return eTipKorisnika.BRONZANI;
            }
            else if (bodovi >= 3000 && bodovi < 4000)
            {
                return eTipKorisnika.SREBRNI;
            }
            else
            {
                return eTipKorisnika.ZLATNI;
            }

        }
        public int GetCenaKarte(eTipKarte tipKarte,int Cena_REGULAR)
        {
        
            // ● Cena REGULAR karte je definisana prilikom kreiranja manifestacije.Cena FAN PIT
            //karte je 2 puta veća od cene REGULAR karte, a cena VIP karte je 4 puta veća od cene
            //REGULAR karte .


            switch (tipKarte)
            {
                case eTipKarte.REGULAR: {return  Cena_REGULAR; }
                case eTipKarte.FAN_PIT: { return Cena_REGULAR * 2; }
                case eTipKarte.VIP:     { return Cena_REGULAR * 4;  }

                default: return 0;
            }

            
        }
        public int GetBodovi(int trenutni_bodovi,int cena_karte) 
        {
            //broj_bodova = cena_jedne_karte / 1000 * 133

            return trenutni_bodovi += (cena_karte / 1000) * 133;

        }

        public int RemoveBodovi(int trenutni_bodovi, int cena_karte)
        {
            //broj_bodova = cena_jedne_karte / 1000 * 133*4

            return trenutni_bodovi -= ((cena_karte / 1000) * 133);

        }

        public double ProsecnaOcena( Dictionary<string,Komentar> komentari,string naziv)
        {
            double prosek=0;
            int broj_ocena = 0;

            foreach (var item in komentari)
            {
                if (komentari[item.Value.ID].Manifestacija.Naziv.Equals(naziv) && komentari[item.Value.ID].Status.Equals(eStanjeKomentara.ACCEPTED))
                {
                    prosek += komentari[item.Value.ID].Ocena;
                    broj_ocena++;
                }
            }
           
            if (broj_ocena==0)//ukoliko se desi NaN
            {
                return 0;
            }

            return Math.Round(prosek / broj_ocena,2);//zaokruzi na dve decimale
        }

    }
}