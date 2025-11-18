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
            var grafo = Arquivo.LerDados("dataGrafos/grafo05.dimacs");

            grafo.ExibirGrafo();
        }
    }
}