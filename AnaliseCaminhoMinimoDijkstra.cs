using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_GRAFOS
{
    public class AnaliseCaminhoMinimoDijkstra : IAnalises
    {

        private int _origem;
        private IGrafo<int> _grafo;

        public AnaliseCaminhoMinimoDijkstra(IGrafo<int> grafo, int origem)
        {
            _grafo = grafo;
            _origem = origem;
        }


        public void Executar()
        {
            Dijkstra(_grafo, _origem);
        }


        public List<Vertice<int>> Dijkstra(IGrafo<int> grafo, int vertice)
        {

            return ;
        }

    }
}
