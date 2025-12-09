using System.Text;

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

        /// <summary>
        /// Executa o Dijkstra e retorna o texto que antes era exibido no console.
        /// </summary>
        public string Executar()
        {
            Dijkstra(_grafo, _origem);
            return ExibirResultado();
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

            foreach (Vertice<int> vertice in listaVerticesOriginal)
            {
                _distancia[vertice] = int.MaxValue;
                _predecessor[vertice] = null;
            }

            _distancia[verticeOriginal] = 0;

            List<Vertice<int>> naoVisitado = new List<Vertice<int>>(listaVerticesOriginal);

            while (naoVisitado.Count > 0)
            {
                Vertice<int> v = EncontrarVerticeMenorDistancia(naoVisitado);

                if (v == null || _distancia[v] == int.MaxValue)
                    return;

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

        /// <summary>
        /// Agora retorna o texto ao invés de imprimir.
        /// </summary>
        private string ExibirResultado()
        {
            var sb = new StringBuilder();

            if (!_distancia.ContainsKey(_destino) || _distancia[_destino] == int.MaxValue)
            {
                sb.AppendLine($"Não foi encontrado um caminho de {_origem.Dado} para {_destino.Dado}.");
                return sb.ToString();
            }

            var caminho = new Stack<Vertice<int>>();
            Vertice<int> atual = _destino;

            while (atual != null)
            {
                caminho.Push(atual);
                if (_predecessor.ContainsKey(atual))
                {
                    atual = _predecessor[atual];
                }
                else break;
            }

            sb.AppendLine($"Caminho Mínimo de {_origem.Dado} para {_destino.Dado}:");
            sb.AppendLine($"Distância Total: {_distancia[_destino]}");

            sb.Append("Caminho: ");

            while (caminho.Count > 0)
            {
                sb.Append(caminho.Pop().Dado);
                if (caminho.Count > 0)
                    sb.Append(" - ");
                else
                    sb.Append(".");
            }

            sb.AppendLine();

            return sb.ToString();
        }
    }
}
