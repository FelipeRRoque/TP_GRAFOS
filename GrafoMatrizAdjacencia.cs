using System.Text;

namespace TP_GRAFOS
{
    /// <summary>
    /// Implementa um grafo direcionado utilizando matriz de adjacência,
    /// armazenando pesos e capacidades para cada ligação entre vértices.
    /// </summary>
    /// <typeparam name="T">Tipo dos valores armazenados nos vértices.</typeparam>
    public class GrafoMatrizAdjacencia<T> : IGrafo<T>
    {
        /// <summary>
        /// Lista dos vértices do grafo, usada para indexação na matriz.
        /// </summary>
        private readonly List<Vertice<T>> _vertices;

        /// <summary>
        /// Matriz que armazena os pesos das arestas.
        /// Uma entrada [i, j] representa o peso da aresta entre os vértices.
        /// </summary>
        private readonly int[,] _matrizPesos;

        /// <summary>
        /// Matriz que armazena as capacidades das arestas.
        /// Uma entrada [i, j] representa a capacidade da rota entre dois vértices.
        /// </summary>
        private readonly int[,] _matrizCapacidades;

        /// <summary>
        /// Cria um grafo baseado em matriz de adjacência com capacidade fixa de vértices.
        /// </summary>
        /// <param name="capacidadeMaxima">Número máximo de vértices permitidos.</param>
        public GrafoMatrizAdjacencia(int capacidadeMaxima)
        {
            _vertices = new List<Vertice<T>>();
            _matrizPesos = new int[capacidadeMaxima, capacidadeMaxima];
            _matrizCapacidades = new int[capacidadeMaxima, capacidadeMaxima];
        }

        /// <summary>
        /// Adiciona um novo vértice ao grafo, caso ainda haja espaço disponível.
        /// </summary>
        /// <param name="dado">Valor armazenado no vértice.</param>
        public void AdicionarVertice(T dado)
        {
            foreach (var v in _vertices)
            {
                if (v.Dado.Equals(dado))
                    return;
            }

            _vertices.Add(new Vertice<T>(dado));
        }

        /// <summary>
        /// Adiciona uma aresta entre dois vértices existentes, preenchendo peso e capacidade.
        /// </summary>
        /// <param name="origem">Vértice de origem.</param>
        /// <param name="destino">Vértice de destino.</param>
        /// <param name="peso">Peso associado à aresta.</param>
        /// <param name="capacidade">Capacidade máxima da rota.</param>
        public void AdicionarAresta(T origem, T destino, int peso = 1, int capacidade = 0)
        {
            int i = _vertices.FindIndex(v => v.Dado.Equals(origem));
            int j = _vertices.FindIndex(v => v.Dado.Equals(destino));

            if (i == -1 || j == -1)
                throw new Exception("Um ou mais vértices não existem no grafo.");

            _matrizPesos[i, j] = peso;
            _matrizCapacidades[i, j] = capacidade;
        }

        /// <summary>
        /// Retorna a lista completa de vértices do grafo.
        /// </summary>
        public List<Vertice<T>> ObterVertices()
        {
            return _vertices;
        }

        /// <summary>
        /// Retorna todas as arestas presentes no grafo.
        /// </summary>
        public List<Aresta<T>> ObterArestas()
        {
            var arestas = new List<Aresta<T>>();

            for (int i = 0; i < _vertices.Count; i++)
            {
                for (int j = 0; j < _vertices.Count; j++)
                {
                    if (_matrizPesos[i, j] != 0)
                    {
                        arestas.Add(
                            new Aresta<T>(
                                _vertices[i],
                                _vertices[j],
                                _matrizPesos[i, j],
                                _matrizCapacidades[i, j]
                                )
                        );
                    }
                }
            }
            return arestas;
        }

        /// <summary>
        /// Retorna uma lista contendo todos os vizinhos de um vertice.
        /// </summary>
        public List<Vertice<T>> ObterVizinhos(Vertice<T> verticeOrigem)
        {
            var vizinhos = new List<Vertice<T>>();

            int indexOrigem = _vertices.IndexOf(verticeOrigem);
            if (indexOrigem == -1)
                return vizinhos;

            for (int j = 0; j < _vertices.Count; j++)
            {
                int peso = _matrizPesos[indexOrigem, j];

                if (peso > 0)
                {
                    vizinhos.Add(_vertices[j]);
                }
            }

            return vizinhos;
        }

        /// <summary>
        /// Retorna o peso de uma arestas.
        /// </summary>
        public int ObterPeso(Vertice<T> origem, Vertice<T> destino)
        {
            int i = _vertices.IndexOf(origem);
            int j = _vertices.IndexOf(destino);
            return _matrizPesos[i, j];
        }
        /// <summary>
        /// Retorna a capacidade de uma aresta.
        /// </summary>
        public int ObterCapacidade(Vertice<T> origem, Vertice<T> destino)
        {
            int i = _vertices.IndexOf(origem);
            int j = _vertices.IndexOf(destino);
            return _matrizCapacidades[i, j];
        }

        /// <summary>
        /// Retorna uma lista de gaus com todos os vértices do grafo.
        /// </summary>
        public List<(Vertice<T> Vertice, int)> ObterGraus()
        {
            var listaVertices = ObterVertices();
            var listaGraus = new List<(Vertice<T> Vertice, int)>();

            foreach (var vertice in listaVertices)
            {
                int grau = ObterVizinhos(vertice).Count;
                listaGraus.Add((vertice, grau));
            }

            return listaGraus;
        }

        /// <summary>
        /// Exibe no console os vértices e suas conexões representadas na matriz de adjacência.
        /// </summary>
        public string ExibirGrafo()
        {
            StringBuilder grafoStringfado = new StringBuilder("");

            for (int i = 0; i < _vertices.Count; i++)
            {
                grafoStringfado.Append($"{_vertices[i].Dado}: ");

                for (int j = 0; j < _vertices.Count; j++)
                {
                    if (_matrizPesos[i, j] != 0)
                    {
                        grafoStringfado.Append(
                            $" -> {_vertices[j].Dado} (Peso: {_matrizPesos[i, j]}, Capacidade: {_matrizCapacidades[i, j]})"
                        );
                    }
                }
                grafoStringfado.AppendLine();
            }
            return grafoStringfado.ToString();
        }
    }
}