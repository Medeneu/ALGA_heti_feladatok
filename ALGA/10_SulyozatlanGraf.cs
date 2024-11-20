using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OE.ALGA.Adatszerkezetek
{
    public class EgeszGrafEl : GrafEl<int>, IComparable<EgeszGrafEl>
    {
        public EgeszGrafEl(int honnan, int hova)
        {
            Honnan = honnan;
            Hova = hova;
        }

        public int Honnan { get; private set; }

        public int Hova { get; private set; }


        public int CompareTo(EgeszGrafEl? other)
        {
            if (other != null && other is EgeszGrafEl b)
            {
                if (Honnan != b.Honnan)
                {
                    return Honnan.CompareTo(b.Honnan);

                }
                else
                {
                    return Hova.CompareTo(b.Hova);
                }

            }
            else
                throw new InvalidOperationException();
        }
    }


    public class CsucsmatrixSulyozatlanEgeszGraf : SulyozatlanGraf<int, EgeszGrafEl>
    {
        int n;
        bool[,] M;

        public CsucsmatrixSulyozatlanEgeszGraf(int n)
        {
            this.n = n;
            M = new bool[n, n];
        }

        public int CsucsokSzama { get { return n; } private set { } }

        public int ElekSzama
        {
            get
            {
                int count = 0;
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (M[i, j])
                        {
                            count++;
                        }
                    }

                }
                return count;
            }
            private set { }
        }

        public Halmaz<int> Csucsok
        {
            get
            {
                FaHalmaz<int> k = new FaHalmaz<int>();

                for (int i = 0; i < n; i++)
                {
                    k.Beszur(i);

                }

                return k;
            }

        }

        public Halmaz<EgeszGrafEl> Elek
        {
            get
            {
                FaHalmaz<EgeszGrafEl> k = new FaHalmaz<EgeszGrafEl>();
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        if (M[i, j])
                        {
                            EgeszGrafEl temp = new EgeszGrafEl(i, j);
                            k.Beszur(temp);
                        }
                    }
                }
                return k;

            }
            private set { }
        }

        public Halmaz<int> Szomszedai(int csucs)
        {
            FaHalmaz<int> szomszed = new FaHalmaz<int>();

            for (int i = 0; i < n; i++)
            {
                if (M[csucs, i])
                {
                    szomszed.Beszur(i);
                }
            }
            return szomszed;
        }

        public void UjEl(int honnan, int hova)
        {
            M[honnan, hova] = true;
        }

        public bool VezetEl(int honnan, int hova)
        {
            return M[honnan, hova];
        }


    }

    public static class GrafBejarasok
    {
        public static Halmaz<V> SzelessegiBejaras<V, E>(Graf<V, E> g, V start, Action<V> muvelet) where V : IComparable<V>
        {
            TombSor<V> Sor = new TombSor<V>(g.CsucsokSzama);
            var F = new FaHalmaz<V>();

            Sor.Sorba(start);
            F.Beszur(start);

            while (!Sor.Ures)
            {
                V k = Sor.Sorbol();
                muvelet(k);

                g.Szomszedai(k).Bejar((x) =>
                {
                    if (!F.Eleme(x))
                    {
                        Sor.Sorba(x);
                        F.Beszur(x);

                    }

                }
             );
            }
            return F;


        }

        private static void MelysegiBejarasRekurzio<V, E>(Graf<V, E> g, V k, Halmaz<V> F, Action<V> muvelet) where V : IComparable<V>
        {
            F.Beszur(k);
            muvelet(k);

            g.Szomszedai(k).Bejar((x) =>
            {
                if (!F.Eleme(x))
                {
                    MelysegiBejarasRekurzio(g, x, F, muvelet);
                }
            }
            );
        }

        public static Halmaz<V> MelysegiBejaras<V, E>(Graf<V, E> g, V start, Action<V> muvelet) where V : IComparable<V>
        {
            var F = new FaHalmaz<V>();

            
            MelysegiBejarasRekurzio(g, start, F, muvelet);

            return F;
        }








    }


}


