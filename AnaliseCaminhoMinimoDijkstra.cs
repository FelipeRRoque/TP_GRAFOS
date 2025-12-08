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
        private Vertice<int> _destino;
        private IGrafo<int> _grafo;
        private Dictionary<Vertice<int>, int> _distancia = new Dictionary<Vertice<int>, int>();
        private Dictionary<Vertice<int>, Vertice<int>> _predecessor = new Dictionary<Vertice<int>, Vertice<int>>();


        public AnaliseCaminhoMinimoDijkstra(IGrafo<int> grafo, Vertice<int> origem, Vertice<int> destino)
        {
            _grafo = grafo;
            _origem = origem;
            _destino = destino;
        }


        public void Executar()
        {
            Dijkstra(_grafo, _origem);
            ExibirResultado();
        }

        private Vertice<int> EncontrarVerticeMenorDistancia(List<Vertice<int>> naoVisitados)
        {
            Vertice<int> verticeMinimo = null;
            int menorDistancia = int.MaxValue;

            foreach (Vertice<int> v in naoVisitados)
            {
                if (_distancia.ContainsKey(v) && _distancia[v] < menorDistancia)
                {
                    menorDistancia = _distancia[v];
                    verticeMinimo = v;
                }
            }

            return verticeMinimo;
        }

        private void Dijkstra(IGrafo<int> grafo, Vertice<int> verticeOriginal)
        {
            List<Vertice<int>> listaVerticesOriginal = grafo.ObterVertices();

            //Inicializa todos os valores de distancias como maxValue e os pais como null.
            foreach (Vertice<int> vertice in listaVerticesOriginal)
            {
                _distancia[vertice] = int.MaxValue;
                _predecessor[vertice] = null;
            }
            _distancia[verticeOriginal] = 0;

            //(V - S)
            List<Vertice<int>> naoVisitado = new List<Vertice<int>>(listaVerticesOriginal);


            while (naoVisitado.Count > 0)
            {
                Vertice<int> v = EncontrarVerticeMenorDistancia(naoVisitado);

                //Se o vértice de menor distância é nulo ou a distância é infinita, 
                //significa que o resto do grafo é inacessível.
                if (v == null || _distancia[v] == int.MaxValue)
                {
                    return;
                }

                naoVisitado.Remove(v);


                foreach (Vertice<int> w in grafo.ObterVizinhos(v))
                {
                    int peso = grafo.ObterPeso(v, w);

                    if (naoVisitado.Contains(w))
                    {
                        int novaDistancia = _distancia[v] + peso;

                        if (novaDistancia < _distancia[w])
                        {
                            _distancia[w] = novaDistancia;
                            _predecessor[w] = v;
                        }
                    }
                }
            }
        }

        private void ExibirResultado()
        {
            // Verifica se o destino foi alcançado
            if (!_distancia.ContainsKey(_destino) || _distancia[_destino] == int.MaxValue)
            {
                Console.WriteLine($"Não foi encontrado um caminho de {_origem.Dado} para {_destino.Dado}.");
                return;
            }

            // Constrói o caminho retrocedendo a partir do predecessor
            var caminho = new Stack<Vertice<int>>();
            Vertice<int> atual = _destino;

            while (atual != null)
            {
                caminho.Push(atual);
                if (_predecessor.ContainsKey(atual))
                {
                    atual = _predecessor[atual];
                }

            }

            Console.WriteLine($"Caminho Mínimo de {_origem.Dado} para {_destino.Dado}:");
            Console.WriteLine($"Distância Total: {_distancia[_destino]}");
            Console.Write("Caminho: ");

            while (caminho.Count > 0)
            {
                Console.Write(caminho.Pop().Dado);
                if (caminho.Count > 0)
                {
                    Console.Write(" - ");
                }
                else
                {
                    Console.Write(".");
                }
            }
            Console.WriteLine();
        }

    }
}
