using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
    public class TipKorisnika
    {
        private eTipKorisnika imeTipaKorisnika;

        public eTipKorisnika ImeTipaKorisnika
        {
            get { return imeTipaKorisnika; }
            set { imeTipaKorisnika = value; }
        }


        private double popust;

        public double Popust
        {
            get { return popust; }
            set { popust = value; }
        }

        private int granica_Bodova;

        public int Granica_Bodova
        {
            get { return granica_Bodova; }
            set { granica_Bodova = value; }
        }

        public TipKorisnika(eTipKorisnika imeTipaKorisnika)
        {
            this.imeTipaKorisnika = imeTipaKorisnika;
            
        }

       
    }
}