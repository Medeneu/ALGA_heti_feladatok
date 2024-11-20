using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OE.ALGA.Adatszerkezetek
{
    public class TombVerem<T> : Verem<T>
    {
        private T[] E;
        private int n = 0;

        public TombVerem(int meret)
        {
            this.E = new T[meret];
        }

        public bool Ures
        {
            get { return n == 0; }
        }

        public T Felso()
        {
            if (!Ures) return E[n - 1];
            else throw new NincsElemKivetel();

        }

        public void Verembe(T ertek)
        {
            if (n < E.Length)
            {
                E[n++] = ertek;
            }
            else throw new NincsHelyKivetel();
        }

        public T Verembol()
        {
            if (!Ures) return E[--n];

            else throw new NincsElemKivetel();

        }
    }
    public class TombSor<T> : Sor<T>
    {
        private T[] E;
        private int e = 0;
        private int u = -1;
        private int n = 0;

        public TombSor(int meret)
        {
            E = new T[meret];

        }
        public bool Ures { get { return n == 0; } }

        public T Elso()
        {
            if (!Ures)
            {
                return E[e];

            }
            else throw new NincsElemKivetel();

        }

        public void Sorba(T ertek)
        {
            if (n < E.Length)
            {
                u = (u + 1) % E.Length;
                E[u] = ertek;
                n++;
            }
            else throw new NincsHelyKivetel();

        }

        public T Sorbol()
        {
            if (!Ures)
            {
                T item = E[e];
                e = (e + 1) % E.Length;
                n--;
                return item;

            }
            else throw new NincsElemKivetel();

        }
    }





    public class TombLista<T> : Lista<T>, IEnumerable<T>
    {
        T[] E;
        int n;

        public TombLista(int meret)
        {
            E = new T[meret];
            n = 0;

        }
        public TombLista()
        {
            E = new T[1];
            n = 0;

        }
        public int Elemszam { get { return n; } }

        public void Bejar(Action<T> muvelet)
        {
            for (int i = 0; i < n; i++)
            {
                muvelet(E[i]);
            }

        }

        private void Meretnovel()
        {
            T[] k = new T[E.Length * 2];

            for (int i = 0; i < E.Length; i++)
            {
                k[i] = E[i];
            }

            E = k;
        }

        public void Beszur(int index, T ertek)
        {
            if (index <= n)
            {
                if (n + 1 == E.Length)
                {
                    Meretnovel();
                }
                n++;
                for (int i = n; i > index; i--)
                {
                    E[i] = E[i - 1];
                }

                E[index] = ertek;
            }
            else throw new HibasIndexKivetel();
        }


        public void Hozzafuz(T ertek)
        {
            Beszur(n, ertek);

        }

        public T Kiolvas(int index)
        {
            if (index <= n - 1)
            {
                return E[index];
            }
            else throw new HibasIndexKivetel();

        }

        public void Modosit(int index, T ertek)
        {
            if (index <= n - 1)
            {
                E[index] = ertek;
            }
            else throw new HibasIndexKivetel();

        }

        public void Torol(T ertek)
        {
            int db = 0;

            for (int i = 0; i < n; i++)
            {
                if (E[i].Equals(ertek)) db++;
                else
                {
                    E[i - db] = E[i];
                }

            }
            n -= db;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new TombListaBejaro<T>(E, n);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

    public class TombListaBejaro<T> : IEnumerator<T>
    {
        T[] E;
        int n;
        int aktualisIndex = -1;

        public TombListaBejaro(T[] E, int n)
        {
            this.E = E;
            this.n = n;
        }


        public T Current
        {
            get
            {
                try
                {
                    return E[aktualisIndex];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }

        object IEnumerator.Current => throw new NotImplementedException();

        public void Dispose()
        {
        }

        public bool MoveNext()
        {
            if (n == aktualisIndex) return false;
            else
            {
                aktualisIndex++;
                return true;
            }

        }

        public void Reset()
        {
            aktualisIndex = -1;
        }
    }
}
