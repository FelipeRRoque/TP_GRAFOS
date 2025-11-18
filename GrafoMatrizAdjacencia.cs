using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TP_GRAFOS
{
    /// <summary>
    /// Representa um grafo utilizando matriz de adjacência.
    /// Armazena pesos e capacidades para cada aresta entre os vértices.
    /// </summary>
    /// <typeparam name="T">Tipo dos dados armazenados em cada vértice.</typeparam>
    public class GrafoMatrizAdjacencia<T> : IGrafo<T>
    {
        /// <summary>
        /// Lista dos vértices do grafo, usada para indexação na matriz.
        /// </summary>
        private readonly List<T> _vertices;

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
            _vertices = new List<T>();
            _matrizPesos = new int[capacidadeMaxima, capacidadeMaxima];
            _matrizCapacidades = new int[capacidadeMaxima, capacidadeMaxima];
        }

        /// <summary>
        /// Adiciona um novo vértice ao grafo, caso ainda haja espaço disponível.
        /// </summary>
        /// <param name="dado">Valor armazenado no vértice.</param>
        public void AdicionarVertice(T dado)
        {
            if (_vertices.Contains(dado))
                return;

            _vertices.Add(dado);
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
            int i = _vertices.IndexOf(origem);
            int j = _vertices.IndexOf(destino);

            if (i == -1 || j == -1)
                throw new Exception("Um ou mais vértices não existem no grafo.");

            _matrizPesos[i, j] = peso;
            _matrizCapacidades[i, j] = capacidade;
        }

        /// <summary>
        /// Exibe no console os vértices e suas conexões representadas na matriz de adjacência.
        /// </summary>
        public void ExibirGrafo()
        {
            for (int i = 0; i < _vertices.Count; i++)
            {
                Console.Write($"{_vertices[i]}: ");

                for (int j = 0; j < _vertices.Count; j++)
                {
                    if (_matrizPesos[i, j] != 0)
                    {
                        Console.Write(
                            $" -> {_vertices[j]} (Peso: {_matrizPesos[i, j]}, Capacidade: {_matrizCapacidades[i, j]})"
                        );
                    }
                }

                Console.WriteLine();
            }
        }
    }
}