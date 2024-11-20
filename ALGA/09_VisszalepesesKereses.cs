using OE.ALGA.Optimalizalas;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OE.ALGA.Optimalizalas
{

    public class VisszalepesesOptimalizacio<T>
    {
        protected int n; // Number of levels/subproblems
        protected int[] M; // Number of options at each level
        protected T[,] R; // Possible partial solutions
        protected Func<int, T, bool> ft; // First constraint function
        protected Func<int, T, T[], bool> fk; // Second constraint function
        protected Func<T[], float> josag; // Quality function to evaluate solutions
        public int LepesSzam { get; protected set; } = 0; // Step count

        public VisszalepesesOptimalizacio(int n, int[] m, T[,] r, Func<int, T, bool> ft, Func<int, T, T[], bool> fk, Func<T[], float> josag)
        {
            this.n = n;
            this.M = m;
            this.R = r;
            this.ft = ft;
            this.fk = fk;
            this.josag = josag;
        }

        virtual protected void Backtrack(int szint, ref T[] E, ref bool van, ref T[] O)
        {
            LepesSzam++;
            int i = -1;
            while (i < M[szint] - 1)
            {
                i++;
                if (ft(szint, R[szint, i]))
                {
                    if (fk(szint, R[szint, i], E))
                    {
                        E[szint] = R[szint, i];
                        if (szint == n - 1)
                        {
                            if (!van || josag(E) > josag(O))
                            {
                                Array.Copy(E, O, E.Length);
                                van = true;
                            }


                        }
                        else
                        {
                            Backtrack(szint + 1, ref E, ref van, ref O);
                        }
                    }
                }
            }
        }

        public T[] OptimalisMegoldas()
        {
            T[] E = new T[n];
            T[] O = new T[n];
            bool van = false;
            Backtrack(0, ref E, ref van, ref O);
            return van ? O : null;
        }
    }

    public class VisszalepesesHatizsakPakolas
    {
        protected HatizsakProblema problema;

        public VisszalepesesHatizsakPakolas(HatizsakProblema problema)
        {
            this.problema = problema;
        }

        public int LepesSzam
        {
            get; private set;
        }

        public bool[] OptimalisMegoldas()
        {
            // 1. Készítjük az M és R tömböket
            int[] M = new int[problema.n];
            bool[,] R = new bool[problema.n, 2];

            for (int i = 0; i < problema.n; i++)
            {
                M[i] = 2; // M értéke mindig 2
                R[i, 0] = true;  // R[i, 0] = igaz
                R[i, 1] = false; // R[i, 1] = hamis
            }
            Func<int, bool, bool> ft = (x, y) => { return !y || problema.w[x] <= problema.Wmax; };
            Func<int, bool, bool[], bool> fk = (x, y, z) =>
            {
                int weigth = 0;
                for (int i = 0; i < x; i++)
                {
                    if (z[i])
                    {
                        weigth += problema.w[i];
                    }
                }

                return (weigth <= problema.Wmax) && (!y || weigth + problema.w[x] <= problema.Wmax);
            };


            var opti = new VisszalepesesOptimalizacio<bool>(
                problema.n,
                M,
                R,
                ft,
                fk,
                problema.OsszErtek
                );


            bool[] optimalisEredmeny = opti.OptimalisMegoldas();

            LepesSzam = opti.LepesSzam;

            return optimalisEredmeny;

        }

        public int OptimalisErtek()
        {
            bool[] optimalErdemeny = OptimalisMegoldas();

            return (int)problema.OsszErtek(optimalErdemeny);
        }


    }


    public class SzetvalasztasEsKorlatozasOptimalizacio<T> : VisszalepesesOptimalizacio<T>
    {
        Func<int, T[], int> fb;
        public SzetvalasztasEsKorlatozasOptimalizacio(int n, int[] m, T[,] r, Func<int, T, bool> ft, Func<int, T, T[], bool> fk, Func<T[], float> josag, Func<int, T[], int> fb) :
            base(n, m, r, ft, fk, josag)
        {
            this.fb = fb;

        }

        protected override void Backtrack(int szint, ref T[] E, ref bool van, ref T[] O)
        {
            for (int i = 0; i < M[szint]; i++)
            {


                if (ft(szint, R[szint, i]))
                {
                    if (fk(szint, R[szint, i], E))
                    {
                        E[szint] = R[szint, i];
                        if (szint == n - 1)
                        {
                            if (!van || josag(E) > josag(O))
                            {
                                Array.Copy(E, O, E.Length);
                                van = true;
                            }


                        }
                        else
                        {
                            if (josag(E) + fb(szint, E) > josag(O))
                            {
                                Backtrack(szint + 1, ref E, ref van, ref O);
                            }
                        }
                    }
                }
            }
        }
    }

    public class SzetvalasztasEsKorlatozasHatizsakPakolas : VisszalepesesHatizsakPakolas
    {
        public SzetvalasztasEsKorlatozasHatizsakPakolas(HatizsakProblema problema) : base(problema)
        {

        }

        public bool[] OptimalisMegoldas()
        {
            // 1. Készítjük az M és R tömböket
            int[] M = new int[problema.n];
            bool[,] R = new bool[problema.n, 2];

            for (int i = 0; i < problema.n; i++)
            {
                M[i] = 2; // M értéke mindig 2
                R[i, 0] = true;  // R[i, 0] = igaz
                R[i, 1] = false; // R[i, 1] = hamis
            }
            Func<int, bool, bool> ft = (x, y) => { return !y || problema.w[x] <= problema.Wmax; };
            Func<int, bool, bool[], bool> fk = (x, y, z) =>
            {
                int weigth = 0;
                for (int i = 0; i < x; i++)
                {
                    if (z[i])
                    {
                        weigth += problema.w[i];
                    }
                }

                return (weigth <= problema.Wmax) && (!y || weigth + problema.w[x] <= problema.Wmax);
            };

            Func<int, bool[], int> fb = (szint, E) =>
            {
                int b = 0;
                for (int i = szint + 1; i < problema.n; i++)
                {
                    if (problema.OsszSuly(E) + problema.w[i] <= problema.Wmax)
                    {
                        b += (int)problema.p[i];
                    }
                }
                return b;
            };


            var opti = new SzetvalasztasEsKorlatozasOptimalizacio<bool>(
                problema.n,
                M,
                R,
                ft,
                fk,
                problema.OsszErtek,
                fb
                );


            bool[] optimalisEredmeny = opti.OptimalisMegoldas();


            return optimalisEredmeny;

        }

        public int OptimalisErtek()
        {
            bool[] optimalErdemeny = OptimalisMegoldas();

            return (int)problema.OsszErtek(optimalErdemeny);
        }


    }




}




