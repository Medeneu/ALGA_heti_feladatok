using OE.ALGA.Paradigmak;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OE.ALGA.Paradigmak
{
    public class FeltetelesFeladatTarolo<T> : FeladatTarolo<T> where T : IVegrehajthato
    {
        public FeltetelesFeladatTarolo(int meret, Func<T, bool> feltetel) : base(meret)
        {
            BejaroFeltetel = feltetel;
        }

        public FeltetelesFeladatTarolo(int meret) : base(meret)
        {
            BejaroFeltetel = A;
        }

        private bool A(T item)
        {
            return true;
        }

        public Func<T, bool> BejaroFeltetel { get; set; }

        public override IEnumerator<T> GetEnumerator()
        {
            return new FeltetelesFeladatTaroloBejaro<T>(tarolo, n, BejaroFeltetel);
        }

        public void FeltetelesVegrehajtas(Predicate<T> feltetel)
        {
            foreach (var item in this)
            {
                if (feltetel(item))
                { item.Vegrehajtas(); }
            }
        }
    }

    public class FeltetelesFeladatTaroloBejaro<T> : IEnumerator<T>
    {
        public T[] tomb;
        public int n;
        public int aktualisIndex;
        public Func<T, bool> BejaroFeltetel { get; set; }
        public FeltetelesFeladatTaroloBejaro(T[] tarolo, int n, Func<T, bool> feltetel)
        {
            this.tomb = tarolo;
            this.n = n;
            aktualisIndex = 0;
            BejaroFeltetel = feltetel;
        }

        public T Current
        {
            get
            {
                try { return tomb[aktualisIndex - 1]; }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException();
                }
            }
        }

        object IEnumerator.Current => throw new NotImplementedException();

        public void Dispose() { }

        public bool MoveNext()
        {
            if (aktualisIndex == n)
            { return false; }
            else
            {
                aktualisIndex++;
                if (BejaroFeltetel(tomb[aktualisIndex - 1]))
                {
                    return true;
                }
                else { return MoveNext(); }
            }
        }

        public void Reset()
        {
            aktualisIndex = -1;
        }

    }
}
