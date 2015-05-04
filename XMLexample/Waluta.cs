using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XMLexample
{
    public class Waluta
    {
        string nazwa_kraju;
        string nazwa_waluty;
        string przelicznik;
        string kod_waluty;
        string kurs_sredni;

        public string NazwaKraju
        {
            get { return nazwa_kraju; }
            set { nazwa_kraju = value; }
        }
        public string Przelicznik
        {
            get { return przelicznik; }
            set { przelicznik = value; }
        }
        public string KodWaluty
        {
            get { return kod_waluty; }
            set { kod_waluty = value; }
        }
        public string KursSredni
        {
            get { return kurs_sredni; }
            set { kurs_sredni = value; }
        }

        public string NazwaWaluty
        {
            get { return nazwa_waluty; }
            set { nazwa_waluty = value; }
        }
    }
}
