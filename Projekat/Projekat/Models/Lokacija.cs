using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
    public class Lokacija
    {

        public int Width { get; set; }
        public int Height { get; set; }

        private MestoOdrzavanja mesto;

        public MestoOdrzavanja Mesto
        {
            get { return mesto; }
            set { mesto = value; }
        }

        public Lokacija()
        {
        }

    }
}