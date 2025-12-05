using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_GRAFOS
{
    public class AnaliseCaminhoMinimoDijkstra : IAnalises
    {

        private Vertice<int> _origem;
        private IGrafo<int> _grafo;
        private List<int> _distancias;
        private List<Vertice<int>> _predecessores;
        List<Vertice<int>> _corteS = new List<Vertice<int>>();

        public AnaliseCaminhoMinimoDijkstra(IGrafo<int> grafo, Vertice<int> origem)
        {
            _grafo = grafo;
            _origem = origem;
        }


        public void Executar()
        {
            Dijkstra(_grafo, _origem);
        }

        private Aresta<int> EncontrarProximoVertice()
        {

        }

        public List<Vertice<int>> Dijkstra(IGrafo<int> grafo, Vertice<int> vertice)
        {
            //Inicializa todos os valores de distancias como maxValue e os pais como null.
            List<Vertice<int>> listaVerticesOriginal = grafo.ObterVertices();
            List<Aresta<int>> listaArestasOriginal = grafo.ObterArestas();
            Aresta<int> menorValor =  ;  

            for (int i = 0; i < listaVerticesOriginal.Count(); i++)
            {
                _distancias[i] = int.MaxValue;
                _predecessores[i] = (null);
            }

            //Adiciona o vértice original no corte de S e define a distância como 0.
            _corteS.Add(vertice);
            _distancias[vertice.Dado] = 0;

            for (int i = 1; i < listaVerticesOriginal.Count() - 1; i++)
            {
                menorValor = EncontrarProximoVertice();
            }


            



        }

    }
}
