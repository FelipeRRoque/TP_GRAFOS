using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TP_GRAFOS
{
    public class Vertice<T>
    {
        public T Dado { get; set;}

        public Vertice(T dado)
        {
            Dado = dado;
        }

        public override string ToString()
        {
            return Dado.ToString();
        }
    }
}