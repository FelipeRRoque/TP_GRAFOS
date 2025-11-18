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
            var grafoNaoDirecionado = new Grafo<string>();

            grafoNaoDirecionado.AdcionarVertice("A");
            grafoNaoDirecionado.AdcionarVertice("B");
            grafoNaoDirecionado.AdcionarVertice("C");

            grafoNaoDirecionado.AdcionarAresta("A", "B", 5);
            grafoNaoDirecionado.AdcionarAresta("A", "C", 10);
            grafoNaoDirecionado.AdcionarAresta("B", "C", 3);

            grafoNaoDirecionado.ExibirGrafo();
        }
    }
}