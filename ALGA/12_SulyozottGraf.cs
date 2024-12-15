using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OE.ALGA.Adatszerkezetek
{
    // Class representing an edge in a weighted graph with integer vertices
    public class SulyozottEgeszGrafEl : EgeszGrafEl, SulyozottGrafEl<int>
    {
        public float Suly { get; } // Weight of the edge

        // Constructor initializes the edge with source, destination, and weight
        public SulyozottEgeszGrafEl(int honnan, int hova, float suly) : base(honnan, hova)
        {
            Suly = suly;
        }
    }

    // Class for representing a weighted graph using an adjacency matrix
    public class CsucsmatrixSulyozottEgeszGraf : SulyozottGraf<int, SulyozottEgeszGrafEl>
    {
        int n; // Number of vertices
        float[,] M; // Adjacency matrix storing edge weights

        public int CsucsokSzama { get { return n; } }

        // Property to get the number of edges in the graph
        public int ElekSzama
        {
            get
            {
                int counter = 0;
                for (int i = 0; i < n; i++)
                    for (int j = n; j > 0; j--)
                        if (!float.IsNaN(M[i, j])) counter++; // Count valid edges

                return counter;
            }
        }

        // Property to get all vertices in the graph
        public Halmaz<int> Csucsok
        {
            get
            {
                FaHalmaz<int> faHalmaz = new FaHalmaz<int>();
                for (int i = 0; i < n; i++)
                    faHalmaz.Beszur(i); // Insert each vertex into the set

                return faHalmaz;
            }
        }

        // Property to get all edges in the graph
        public Halmaz<SulyozottEgeszGrafEl> Elek
        {
            get
            {
                FaHalmaz<SulyozottEgeszGrafEl> h = new FaHalmaz<SulyozottEgeszGrafEl>();
                for (int i = 0; i < n; i++)
                    for (int j = 0; j < n; j++)
                        if (!float.IsNaN(M[i, j])) h.Beszur(new SulyozottEgeszGrafEl(i, j, M[i, j])); // Insert edge if it exists

                return h;
            }
        }

        // Constructor to initialize the graph with a given number of vertices
        public CsucsmatrixSulyozottEgeszGraf(int n)
        {
            this.n = n;
            this.M = new float[n, n]; // Initialize the adjacency matrix

            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    M[i, j] = float.NaN; // Set initial value to NaN (no edge)
        }

        // Method to get the weight of an edge between two vertices
        public float Suly(int honnan, int hova)
        {
            if (VezetEl(honnan, hova))
            {
                return M[honnan, hova]; // Return the weight if edge exists
            }
            throw new NincsElKivetel(); // Exception if edge does not exist
        }

        // Method to get the neighbors of a vertex
        public Halmaz<int> Szomszedai(int csucs)
        {
            FaHalmaz<int> h = new FaHalmaz<int>();
            for (int i = 0; i < n; i++)
                if (!float.IsNaN(M[csucs, i]))
                {
                    h.Beszur(i); // Add neighbors (vertices connected by an edge)
                }

            return h;
        }

        // Method to add a new edge with a given weight
        public void UjEl(int honnan, int hova, float suly)
        {
            M[honnan, hova] = suly; // Set the weight of the edge
        }

        // Method to check if an edge exists between two vertices
        public bool VezetEl(int honnan, int hova) => !float.IsNaN(M[honnan, hova]);
    }

    // Class for pathfinding using Dijkstra's algorithm
    public class Utkereses
    {
        // Method to implement Dijkstra's algorithm for finding the shortest path
        public static Szotar<V, float> Dijkstra<V, E>(SulyozottGraf<V, E> g, V start)
        {
            // Initialize data structures for storing distances, priority queue, and parent vertices
            Szotar<V, float> Longitude = new HasitoSzotarTulcsordulasiTerulettel<V, float>(g.CsucsokSzama);
            KupacPrioritasosSor<V> Priority = new KupacPrioritasosSor<V>(g.CsucsokSzama, (x, y) => Longitude.Kiolvas(x).CompareTo(Longitude.Kiolvas(y)) < 0);
            Szotar<V, V> Dictionary = new HasitoSzotarTulcsordulasiTerulettel<V, V>(g.CsucsokSzama);

            // Set initial distances to infinity for all vertices
            g.Csucsok.Bejar(x =>
            {
                Longitude.Beir(x, float.MaxValue);
                Priority.Sorba(x);
            });

            // Set the distance of the starting vertex to 0 and update the priority queue
            Longitude.Beir(start, 0);
            Priority.Frissit(start);

            // Main loop for Dijkstra's algorithm
            while (!Priority.Ures)
            {
                V united = Priority.Sorbol();

                // Relax the edges for the current vertex
                g.Szomszedai(united).Bejar(x =>
                {
                    if (Longitude.Kiolvas(united) + g.Suly(united, x) < Longitude.Kiolvas(x))
                    {
                        Longitude.Beir(x, Longitude.Kiolvas(united) + g.Suly(united, x));
                        Priority.Frissit(x);
                        Dictionary.Beir(x, united);
                    }
                });
            }

            return Longitude; // Return the shortest distances from the start vertex
        }
    }

    // Class for finding minimum spanning tree using Prim's and Kruskal's algorithms
    public class FeszitofaKereses
    {
        // Method to implement Prim's algorithm for finding minimum spanning tree
        public static Szotar<V, V> Prim<V, E>(SulyozottGraf<V, E> g, V start) where V : IComparable<V>
        {
            // Initialize data structures for tracking visited vertices, edge weights, and priority queue
            FaHalmaz<V> Seged = new FaHalmaz<V>();
            HasitoSzotarTulcsordulasiTerulettel<V, float> Kevin = new HasitoSzotarTulcsordulasiTerulettel<V, float>(g.CsucsokSzama);
            HasitoSzotarTulcsordulasiTerulettel<V, V> Dict = new HasitoSzotarTulcsordulasiTerulettel<V, V>(g.CsucsokSzama);
            KupacPrioritasosSor<V> Priority = new KupacPrioritasosSor<V>(g.CsucsokSzama, (x, y) => Kevin.Kiolvas(x) < Kevin.Kiolvas(y));

            // Initialize all vertices with infinite distance and add them to the priority queue
            g.Csucsok.Bejar(x =>
            {
                Kevin.Beir(x, float.MaxValue);
                Dict.Beir(x, default);
                Priority.Sorba(x);
                Seged.Beszur(x);
            });

            // Set the distance of the starting vertex to 0 and update the priority queue
            Priority.Frissit(start);
            Kevin.Beir(start, 0);

            // Main loop for Prim's algorithm
            while (!Priority.Ures)
            {
                V united = Priority.Sorbol();
                Seged.Torol(united);

                // Relax the edges for the current vertex
                g.Szomszedai(united).Bejar(n =>
                {
                    if (Seged.Eleme(n) && g.Suly(united, n) < Kevin.Kiolvas(n))
                    {
                        Kevin.Beir(n, g.Suly(united, n));
                        Dict.Beir(n, united);
                        Priority.Frissit(n);
                    }
                });
            }
            return Dict; // Return the minimum spanning tree as a dictionary of parent vertices
        }

        // Method to implement Kruskal's algorithm for finding minimum spanning tree
        public static Halmaz<E> Kruskal<V, E>(SulyozottGraf<V, E> g)
            where V : IComparable<V>
            where E : SulyozottGrafEl<V>, IComparable<E>
        {
            // Initialize data structures for tracking connected components and edges
            Szotar<V, FaHalmaz<V>> Hasito = new HasitoSzotarTulcsordulasiTerulettel<V, FaHalmaz<V>>(g.CsucsokSzama);
            FaHalmaz<E> Answer = new FaHalmaz<E>();
            List<E> Lines = new List<E>();

            // Initialize each vertex with its own set (disjoint set)
            g.Csucsok.Bejar(x =>
            {
                FaHalmaz<V> fa = new FaHalmaz<V>();
                fa.Beszur(x);
                Hasito.Beir(x, fa);
            });

            // Collect all edges in the graph
            g.Elek.Bejar(x => Lines.Add(x));

            // Sort edges by their weight in ascending order
            Lines.Sort((a, b) => a.Suly.CompareTo(b.Suly));

            // Main loop for Kruskal's algorithm
            foreach (E el_1 in Lines)
            {
                V Start = el_1.Honnan;
                V End = el_1.Hova;

                if (Hasito.Kiolvas(Start) != Hasito.Kiolvas(End))
                {
                    Answer.Beszur(el_1);
                    FaHalmaz<V> Valami = Hasito.Kiolvas(Start);
                    Hasito.Kiolvas(End).Bejar(x =>
                    {
                        Valami.Beszur(x);
                        Hasito.Beir(x, Valami);
                    });
                }
            }

            return Answer;
        }
    }
}
