using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        /// Exibe no console os vértices e suas conexões representadas na matriz de adjacência.
        /// </summary>
        public void ExibirGrafo()
        {
            for (int i = 0; i < _vertices.Count; i++)
            {
                Console.Write($"{_vertices[i].Dado}: ");

                for (int j = 0; j < _vertices.Count; j++)
                {
                    if (_matrizPesos[i, j] != 0)
                    {
                        Console.Write(
                            $" -> {_vertices[j].Dado} (Peso: {_matrizPesos[i, j]}, Capacidade: {_matrizCapacidades[i, j]})"
                        );
                    }
                }

                Console.WriteLine();
            }
        }
    }
}