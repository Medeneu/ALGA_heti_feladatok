namespace OE.ALGA.Adatszerkezetek
{
    public class Kupac<T>
    {
        protected T[] E;
        protected int n;
        protected Func<T, T, bool> nagyobbPrioritás; // Összehasonlító függvény

        public Kupac(T[] e, int n, Func<T, T, bool> nagyobbPrioritás)
        {
            E = e;
            this.n = n;
            this.nagyobbPrioritás = nagyobbPrioritás;
            KupacEpit();
        }

        public static int BAL(int i) => 2 * i + 1;
        public static int JOBB(int i) => 2 * i + 2;
        public static int Szulo(int i) => (i - 1) / 2;

        protected void Kupacol(int i)
        {
            int b = BAL(i);
            int j = JOBB(i);
            int max = i;

            if (b < n && nagyobbPrioritás(E[b], E[max]))
                max = b;

            if (j < n && nagyobbPrioritás(E[j], E[max]))
                max = j;

            if (max != i)
            {
                (E[i], E[max]) = (E[max], E[i]);
                Kupacol(max);
            }
        }

        protected void KupacEpit()
        {
            for (int i = (n / 2) - 1; i >= 0; i--)
            {
                Kupacol(i);
            }
        }
    }

    public class KupacRendezes<T> : Kupac<T> where T : IComparable<T>
    {
        public KupacRendezes(T[] elemek) : base(elemek, elemek.Length, (a, b) => a.CompareTo(b) > 0) { }

        public void Rendezes()
        {
            KupacEpit();

            for (int i = n - 1; i > 0; i--)
            {
                (E[0], E[i]) = (E[i], E[0]);
                n--;
                Kupacol(0);
            }
        }
    }

    public class KupacPrioritasosSor<T> : Kupac<T>, PrioritasosSor<T>
    {
        public KupacPrioritasosSor(int méret, Func<T, T, bool> nagyobbPrioritás)
            : base(new T[méret], 0, nagyobbPrioritás)
        {
        }

        public void KulcsotFelvisz(int i)
        {
            int sz = Szulo(i);

            if (sz >= 0 && nagyobbPrioritás(E[i], E[sz]))
            {
                (E[i], E[sz]) = (E[sz], E[i]);
                KulcsotFelvisz(sz);
            }
        }

        public bool Ures => n == 0;

        public void Sorba(T érték)
        {
            if (n < E.Length)
            {
                E[n] = érték;
                n++;
                KulcsotFelvisz(n - 1);
            }
            else throw new NincsHelyKivetel();
        }

        public T Sorbol()
        {
            if (!Ures)
            {
                var max = E[0];
                E[0] = E[n - 1];
                n--;
                Kupacol(0);
                return max;
            }
            else throw new NincsElemKivetel();
        }

        public T Elso()
        {
            if (Ures)
                throw new NincsElemKivetel();
            return E[0];
        }

        public void Frissit(T ertek)
        {
            int i = Array.IndexOf(E, ertek, 0, n);

            if (i != -1)
            {
                KulcsotFelvisz(i);
                Kupacol(i);
            }
            else
            {
                throw new NincsElemKivetel();
            }
        }
    }
}
