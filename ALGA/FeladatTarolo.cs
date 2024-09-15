using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OE.ALGA.Adatszerkezetek
{
    internal class FeladatTarolo<T>
    {
        protected T[] tarolo;
        protected int n;

        public FeladatTarolo(int meret)
        {
            tarolo = new T[meret];
            n = 0;
        }

        public void Felvesz(T elem)
        {
            if (n <= tarolo.Length)
            {
                tarolo[n++] = elem;
            }
            else
            {
                
            }
        }

       

    }
}
