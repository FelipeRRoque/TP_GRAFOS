using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TP_GRAFOS
{
    public class AnaliseArvoreGeradoraMinima : IAnalises
    {
        public void Executar(IGrafo<int> grafo)
        {
            ListaArestasSort = grafo.ObterArestas().Sort;
            ListaVertices = grafo.ObterVertices();
        }
    }
}