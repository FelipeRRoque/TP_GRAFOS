using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TP_GRAFOS
{
    public class Program
    {
        static void Main(string[] args)
        {
            var grafo = new Grafo<string>();

            grafo.AdcionarVertice("A");
            grafo.AdcionarVertice("B");
            grafo.AdcionarVertice("C");

            grafo.AdcionarAresta("A", "B", 5);
            grafo.AdcionarAresta("A", "C", 10);
            grafo.AdcionarAresta("B", "C", 3);

            grafo.ExibirGrafo();
        }
    }
}