﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OE.ALGA.Adatszerkezetek
{
    public class LancElem<T>
    {
        public T tart;

        public LancElem<T> kov;

        public LancElem(T tart, LancElem<T> kov)
        {
            this.tart = tart;
            this.kov = kov;
        }
    }

    public class LancoltVerem<T> : Verem<T>
    {
        LancElem<T> fej;


        public LancoltVerem()
        {
            fej = null;
        }

        public bool Ures
        {
            get
            {
                return fej == null;
            }
        }

        public T Felso()
        {
            if (fej != null)
            {

                return fej.tart;

            }
            else throw new NincsElemKivetel();

        }

        public void Verembe(T ertek)
        {
            LancElem<T> uj = new LancElem<T>(ertek, fej);

            fej = uj;

        }

        public T Verembol()
        {
            if (fej != null)
            {
                T ertek = fej.tart;
                fej = fej.kov;
                return ertek;
            }
            else throw new NincsElemKivetel();

        }
    }
    public class LancoltSor<T> : Sor<T>
    {

        LancElem<T> fej;
        LancElem<T> vege;

        public LancoltSor()
        {
            fej = null;
        }
        public bool Ures
        {
            get
            {
                return (fej == null);
            }
        }
        public T Elso()
        {
            if (fej != null)
            {
                return fej.tart;
            }

            else throw new NincsElemKivetel();

        }

        public void Sorba(T ertek)
        {
            LancElem<T> uj = new LancElem<T>(ertek, null);

            if (vege != null)
            {
                vege.kov = uj;
            }
            else
            {
                fej = uj;
            }
            vege = uj;

        }

        public T Sorbol()
        {
            if (fej != null)
            {
                T ertek = fej.tart;
                fej = fej.kov;
                if (fej == null)
                {
                    vege = null;
                }
                return ertek;

            }
            else throw new NincsElemKivetel();

        }
    }
    public class LancoltLista<T> : Lista<T>, IEnumerable<T>
    {
        LancElem<T> fej;

        public LancoltLista()
        {
            fej = null;
        }
        public int Elemszam { get { return szamolo(); ; } }

        public void Bejar(Action<T> muvelet)
        {
            LancElem<T> p = fej;
            while (p != null)
            {
                muvelet(p.tart);
                p = p.kov;
            }
        }

        private int szamolo()
        {
            int elemszam = 0;
            LancElem<T> p = fej;
            while (p != null)
            {
                elemszam++;
                p = p.kov;
            }
            return elemszam;
        }

        public void Beszur(int index, T ertek)
        {
            if ((fej == null) || (index == 0))
            {
                LancElem<T> uj = new LancElem<T>(ertek, fej);
                fej = uj;
            }
            else
            {
                LancElem<T> p = fej;
                int i = 1;
                while ((p.kov != null) && (i < index))
                {
                    p = p.kov;
                    i++;
                }
                if (i <= index)
                {
                    LancElem<T> uj = new LancElem<T>(ertek, p.kov);
                    p.kov = uj;
                }
                else throw new HibasIndexKivetel();
            }

        }

        public void Hozzafuz(T ertek)
        {
            LancElem<T> uj = new LancElem<T>(ertek, null);

            if (fej == null)
            {
                fej = uj;
            }
            else
            {
                LancElem<T> p = fej;
                while (p.kov != null)
                {
                    p = p.kov;
                }
                p.kov = uj;
            }

        }

        public T Kiolvas(int index)
        {
            LancElem<T> p = fej;
            int i = 0;

            while ((p != null) && (i < index))
            {
                p = p.kov;
                i++;
            }
            if (p != null)
            {
                return p.tart;
            }
            else throw new HibasIndexKivetel();
        }

        public void Modosit(int index, T ertek)
        {
            LancElem<T> p = fej;
            int i = 0;

            while ((p != null) && (i < index))
            {
                p = p.kov;
                i++;
            }
            if (p != null)
            {
                p.tart = ertek;
            }
            else throw new HibasIndexKivetel();
        }

        public void Torol(T ertek)
        {
            LancElem<T> p = fej;
            LancElem<T> e = null;
            do
            {
                while ((p != null) && !(p.tart.Equals(ertek)))
                {
                    e = p;
                    p = p.kov;
                }
                if (p != null)
                {
                    LancElem<T> q = p.kov;
                    if (e == null)
                    {
                        fej = q;
                    }
                    else
                    {
                        e.kov = q;
                    }
                    p = q;
                }

            } while (p != null);

        }

        public IEnumerator<T> GetEnumerator()
        {
            return new LancoltListaBejaro<T>(fej);

        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
    public class LancoltListaBejaro<T> : IEnumerator<T>
    {
        LancElem<T> fej;
        LancElem<T> aktualisElem;

        public LancoltListaBejaro(LancElem<T> fej)
        {
            this.fej = fej;
            this.aktualisElem = null;
        }

        public T Current
        {
            get { return aktualisElem.tart; }
        }

        object IEnumerator.Current => throw new NotImplementedException();

        public bool MoveNext()
        {
            if (aktualisElem == null)
            {
                aktualisElem = fej;
            }
            else if (aktualisElem.kov != null)
            {
                aktualisElem = aktualisElem.kov;
            }
            else
            {
                return false;
            }

            return aktualisElem != null;
        }

        public void Reset()
        {
            aktualisElem = null;
        }

        public void Dispose() { }
    }

}
