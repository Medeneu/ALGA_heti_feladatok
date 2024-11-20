using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OE.ALGA.Adatszerkezetek
{
    public class FaElem<T> where T : IComparable<T>
    {
        public T tart { get; set; }
        public FaElem<T> bal { get; set; }
        public FaElem<T> jobb { get; set; }

        public FaElem(T tart, FaElem<T>? bal = null, FaElem<T>? jobb = null) //lehet hiba
        {
            this.tart = tart;
            this.bal = bal;
            this.jobb = jobb;

        }
    }

    public class FaHalmaz<T> : Halmaz<T> where T : IComparable<T>
    {
        FaElem<T>? gyoker;
        public void Bejar(Action<T> muvelet)
        {
            ReszfaBejarasPreOrder(gyoker, muvelet);
        }

        public void Beszur(T ertek)
        {
            gyoker = ReszfabaBeszur(gyoker, ertek);
        }

        public bool Eleme(T ertek)
        {
            return ReszfaEleme(gyoker, ertek);
        }

        public void Torol(T ertek)
        {
            gyoker = ReszfabolTorol(gyoker, ertek);
        }

        static FaElem<T> ReszfabolTorol(FaElem<T> p, T ertek)
        {
            if (p != null)
            {
                if (p.tart.CompareTo(ertek) > 0)
                {
                    p.bal = ReszfabolTorol(p.bal, ertek);
                }
                else if (p.tart.CompareTo(ertek) < 0)
                {
                    p.jobb = ReszfabolTorol(p.jobb, ertek);
                }
                else if (p.bal == null)
                {

                    p = p.jobb;
                }
                else if (p.jobb == null)
                {

                    p = p.bal;

                }
                else
                {
                    p.bal = KetGyerekesTorles(p, p.bal);
                }
                return p;
            }
            else throw new NincsElemKivetel();
        }
        static FaElem<T> KetGyerekesTorles(FaElem<T> e, FaElem<T> r)
        {
            if (r.jobb != null)
            {
                r.jobb = KetGyerekesTorles(e, r.jobb);
                return r;
            }
            else
            {
                e.tart = r.tart;

                return r.bal;
            }

        }
        static FaElem<T> ReszfabaBeszur(FaElem<T> p, T ertek)
        {
            if (p == null)
            {
                FaElem<T> uj = new FaElem<T>(ertek);
                return uj;
            }
            else
            {
                if (p.tart.CompareTo(ertek) > 0)
                {
                    p.bal = ReszfabaBeszur(p.bal, ertek);
                }
                else if (p.tart.CompareTo(ertek) < 0)
                {
                    p.jobb = ReszfabaBeszur(p.jobb, ertek);
                }

            }
            return p;
        }
        static bool ReszfaEleme(FaElem<T> p, T ertek)
        {
            if (p != null)
            {
                if (p.tart.CompareTo(ertek) > 0)
                {
                    return ReszfaEleme(p.bal, ertek);
                }
                else if (p.tart.CompareTo(ertek) < 0)
                {
                    return ReszfaEleme(p.jobb, ertek);
                }
                else { return true; }
            }
            else
            {
                return false;
            }
        }
        protected static void ReszfaBejarasPreOrder(FaElem<T> p, Action<T> muvelet)
        {
            if (p != null)
            {
                muvelet(p.tart);
                ReszfaBejarasPreOrder(p.bal, muvelet);
                ReszfaBejarasPreOrder(p.jobb, muvelet);
            }
        }
        protected static void ReszfaBejarasInOrder(FaElem<T> p, Action<T> muvelet)
        {
            if (p != null)
            {
                ReszfaBejarasInOrder(p.bal, muvelet);
                muvelet(p.tart);
                ReszfaBejarasInOrder(p.jobb, muvelet);
            }
        }
        protected static void ReszfaBejarasPostOrder(FaElem<T> p, Action<T> muvelet)
        {
            if (p != null)
            {

                ReszfaBejarasPostOrder(p.bal, muvelet);
                ReszfaBejarasPostOrder(p.jobb, muvelet);
                muvelet(p.tart);
            }
        }
    }
}
