using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TP_GRAFOS
{
    public class Aresta<T>
    {
        public Vertice<T> Destino { get; set; }
        public int Peso { get; set; }

        public Aresta(Vertice<T> destino, int peso = 1)
        {
            Destino = destino;
            Peso = peso;
        }
    }
}