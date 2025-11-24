using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TP_GRAFOS
{
    /// <summary>
    /// Implementa um grafo direcionado utilizando uma lista de adjacência.
    /// Cada vértice possui uma lista de arestas que partem dele.
    /// </summary>
    /// <typeparam name="T">Tipo dos valores armazenados nos vértices.</typeparam>
    public class GrafoListaAdjacencia<T> : IGrafo<T>
    {
        /// <summary>
        /// Estrutura principal de armazenamento:
        /// um dicionário que associa cada vértice à sua lista de arestas.
        /// </summary>
        private readonly Dictionary<Vertice<T>, List<Aresta<T>>> _listaAdjacencia;

        /// <summary>
        /// Inicializa um grafo vazio utilizando lista de adjacência.
        /// </summary>
        public GrafoListaAdjacencia()
        {
            _listaAdjacencia = new Dictionary<Vertice<T>, List<Aresta<T>>>();
        }

        /// <summary>
        /// Adiciona um novo vértice ao grafo, caso ainda não exista.
        /// </summary>
        /// <param name="dado">Valor armazenado no vértice.</param>
        public void AdicionarVertice(T dado)
        {
            var novoVertice = new Vertice<T>(dado);

            if (!_listaAdjacencia.ContainsKey(novoVertice))
                _listaAdjacencia.Add(novoVertice, new List<Aresta<T>>());
        }

        /// <summary>
        /// Adiciona uma aresta entre dois vértices existentes.
        /// </summary>
        /// <param name="origem">Valor do vértice de origem.</param>
        /// <param name="destino">Valor do vértice de destino.</param>
        /// <param name="peso">Peso da aresta.</param>
        /// <param name="capacidade">Capacidade da aresta.</param>
        public void AdicionarAresta(T origem, T destino, int peso = 1, int capacidade = 0)
        {
            var verticeOrigem = EncontrarVertice(origem);
            var verticeDestino = EncontrarVertice(destino);

            if (verticeOrigem != null && verticeDestino != null)
                _listaAdjacencia[verticeOrigem].Add(new Aresta<T>(verticeOrigem, verticeDestino, peso, capacidade));
        }

        /// <summary>
        /// Localiza e retorna o vértice que contém o dado informado.
        /// </summary>
        /// <param name="dado">Valor procurado.</param>
        /// <returns>O vértice correspondente ou null, se inexistente.</returns>
        private Vertice<T> EncontrarVertice(T dado)
        {
            foreach (var vertice in _listaAdjacencia.Keys)
                if (vertice.Dado.Equals(dado))
                    return vertice;

            return null;
        }

        /// <summary>
        /// Retorna a lista completa de vértices do grafo.
        /// </summary>
        public List<Vertice<T>> ObterVertices()
        {
            return _listaAdjacencia.Keys.ToList();
        }

        /// <summary>
        /// Retorna todas as arestas do grafo.
        /// </summary>
        public List<Aresta<T>> ObterArestas()
        {
            var listaArestas = new List<Aresta<T>>();

            foreach (var arestas in _listaAdjacencia.Values)
                listaArestas.AddRange(arestas);

            return listaArestas;
        }

        public List<(T Vizinho, int Peso, int Capacidade)> ObterVizinhos(T Vertice)
        {
            var vertice = EncontrarVertice(Vertice);
            var listaVizinhos = new List<(T Vizinho, int Peso, int Capacidade)>();

            if (vertice != null && _listaAdjacencia.ContainsKey(vertice))
            {
                foreach (var aresta in _listaAdjacencia[vertice])
                {
                    listaVizinhos.Add((aresta.Destino.Dado, aresta.Peso, aresta.Capacidade));
                }
            }

            return listaVizinhos;
        }
        /// <summary>
        /// Exibe no console a estrutura do grafo: vértices e suas arestas.
        /// </summary>
        public void ExibirGrafo()
        {
            foreach (var vertice in _listaAdjacencia)
            {
                Console.Write($"Vértice {vertice.Key.Dado}: ");

                foreach (var aresta in vertice.Value)
                    Console.Write($"-> {aresta.Destino.Dado} (Peso: {aresta.Peso} | Capacidade: {aresta.Capacidade}) ");

                Console.WriteLine();
            }
        }
    }
}