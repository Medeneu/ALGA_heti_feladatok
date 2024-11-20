using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OE.ALGA.Optimalizalas
{
    public class DinamikusHatizsakPakolas
    {
        readonly HatizsakProblema problema;

        public DinamikusHatizsakPakolas(HatizsakProblema problema)
        {
            this.problema = problema;

        }

        public int LepesSzam { get; private set; }

        private int[,] TablazatFeltoltes()
        {
            int n = problema.n;
            int wmax = problema.Wmax;

            int[,] F = new int[n + 1, wmax + 1];

            for (int t = 0; t <= n; t++)
            {
                F[t, 0] = 0;
            }

            for (int h = 1; h <= wmax; h++)
            {
                F[0, h] = 0;
            }

            for (int t = 1; t <= n; t++)
            {
                for (int h = 1; h <= wmax; h++)
                {
                    if (h >= problema.w[t - 1])
                    {
                        F[t, h] = Math.Max(F[t - 1, h], F[t - 1, h - problema.w[t - 1]] + (int)problema.p[t - 1]);
                    }
                    else
                    {
                        F[t, h] = F[t - 1, h];
                    }
                    LepesSzam++;

                }

            }
            return F;
        }
        public int OptimalisErtek()
        {
            var F = TablazatFeltoltes();
            return F[problema.n, problema.Wmax];
        }

        public bool[] OptimalisMegoldas()
        {
            var F = TablazatFeltoltes();
            bool[] kivalasztott = new bool[problema.n];

            int h = problema.Wmax;

            for (int t = problema.n; t > 0; t--)
            {
                if (h >= problema.w[t - 1] && F[t, h] == F[t - 1, h - problema.w[t - 1]] + problema.p[t - 1])
                {
                    kivalasztott[t - 1] = true;
                    h -= problema.w[t - 1];
                }
                else
                {
                    kivalasztott[t - 1] = false;
                }



            }
            return kivalasztott;
        }


    }
}
