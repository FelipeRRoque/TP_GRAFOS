using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TP_GRAFOS
{
    /// <summary>
    /// Representa um grafo genérico usando lista de adjacência.
    /// Permite adicionar vértices, inserir arestas e visualizar a estrutura resultante.
    /// </summary>
    /// <typeparam name="T">Tipo dos valores armazenados nos vértices.</typeparam>
    public class GrafoListaAdjacencia<T> : IGrafo<T>
    {
        /// <summary>
        /// Lista de adjacência do grafo.
        /// Cada vértice é associado a uma lista de arestas que saem dele.
        /// </summary>
        private readonly Dictionary<Vertice<T>, List<Aresta<T>>> _listaAdjacencia;

        /// <summary>
        /// Cria um grafo vazio.
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
                _listaAdjacencia[verticeOrigem].Add(new Aresta<T>(verticeDestino, peso, capacidade));
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