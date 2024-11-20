using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OE.ALGA.Optimalizalas
{
    public class HatizsakProblema
    {
        public int n { get; }
        public int Wmax { get; }
        public int[] w { get; }
        public float[] p { get; }

        public HatizsakProblema(int n, int wmax, int[] w, float[] p)
        {
            this.n = n;
            Wmax = wmax;
            this.w = w;
            this.p = p;
        }

        public int OsszSuly(bool[] k)
        {
            int osszsuly = 0;

            for (int i = 0; i < k.Length; i++)
            {
                if (k[i])
                {
                    osszsuly += w[i];
                }
            }
            return osszsuly;
        }

        public float OsszErtek(bool[] k)
        {
            float osszertek = 0;

            for (int i = 0; i < k.Length; i++)
            {
                if (k[i])
                {
                    osszertek += p[i];
                }
            }

            return osszertek;
        }
        public bool Ervenyes(bool[] k)
        {
            return OsszSuly(k) <= Wmax;
        }
    }


    public class NyersEro<T>
    {
        int m;
        Func<int, T> generator;
        Func<T, float> josag;

        public int LepesSzam { get; private set; }

        public NyersEro(int m, Func<int, T> generator, Func<T, float> josag)
        {
            this.m = m;
            this.generator = generator;
            this.josag = josag;
        }
        public T OptimalisMegoldas()
        {
            T o = generator(1);

            for (int i = 2; i <= m; i++)
            {
                T x = generator(i);
                LepesSzam++;
                if (josag(x) > josag(o))
                    o = x;

            }
            return o;

        }



    }
    public class NyersEroHatizsakPakolas
    {
        public int LepesSzam { get; private set; }

        HatizsakProblema problema;

        public NyersEroHatizsakPakolas(HatizsakProblema problema)
        {
            this.problema = problema;
        }

        public bool[] Generator(int i)
        {
            int szam = i - 1;
            bool[] k = new bool[problema.n];

            for (int j = 0; j < problema.n; j++)
            {
                k[j] = (szam / (int)Math.Pow(2, j)) % 2 == 1;

            }
            return k;
        }



        public float Josag(bool[] pakolas)
        {
            if (!problema.Ervenyes(pakolas))
            {
                return -1;
            }
            return problema.OsszErtek(pakolas);
        }

        public bool[] OptimalisMegoldas()
        {
            NyersEro<bool[]> k = new NyersEro<bool[]>
                (
                (int)Math.Pow(2, problema.n),
                Generator,
                Josag
                );
            LepesSzam = k.LepesSzam;
            return k.OptimalisMegoldas();

        }

        public float OptimalisErtek()
        {
            bool[] OptimalisErtek = OptimalisMegoldas();
            return problema.OsszErtek(OptimalisErtek);
        }


    }




}
