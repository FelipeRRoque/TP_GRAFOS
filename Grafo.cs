using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TP_GRAFOS
{
    /// <summary>
    /// Representa um grafo genérico utilizando lista de adjacência.
    /// Permite adicionar vértices, adicionar arestas e exibir a estrutura do grafo.
    /// </summary>
    /// <typeparam name="T">Tipo do dado armazenado em cada vértice.</typeparam>
    public class Grafo<T>
    {
        /// <summary>
        /// Estrutura de dados que armazena cada vértice e sua lista de arestas.
        /// A chave é um vértice, e o valor é uma lista de arestas conectadas a ele.
        /// </summary>
        private readonly Dictionary<Vertice<T>, List<Aresta<T>>> _listaAdjacencia;
        // readonly para garantir que a referência do dicionário não seja alterada após a inicialização.

        /// <summary>
        /// Inicializa um novo grafo com lista de adjacência vazia.
        /// </summary>
        public Grafo()
        {
            _listaAdjacencia = new Dictionary<Vertice<T>, List<Aresta<T>>>();
        }

        /// <summary>
        /// Adiciona um novo vértice ao grafo, se ele ainda não existir.
        /// </summary>
        /// <param name="dado">Dado armazenado no vértice.</param>
        public void AdcionarVertice(T dado)
        {
            var novoVertice = new Vertice<T>(dado);

            if (!_listaAdjacencia.ContainsKey(novoVertice))
            {
                _listaAdjacencia.Add(novoVertice, new List<Aresta<T>>());
            }
        }

        /// <summary>
        /// Adiciona uma aresta entre dois vértices já existentes no grafo.
        /// O grafo é considerado não-direcionado, portanto a ligação é inserida nos dois sentidos.
        /// </summary>
        /// <param name="dadoOrigem">Valor do vértice de origem.</param>
        /// <param name="dadoDestino">Valor do vértice de destino.</param>
        /// <param name="peso">Peso da aresta. Por padrão, 1.</param>
        public void AdcionarAresta(T dadoOrigem, T dadoDestino, int peso = 1)
        {
            var verticeOrigem = EncontrarVertice(dadoOrigem);
            var verticeDestino = EncontrarVertice(dadoDestino);

            if (verticeOrigem != null && verticeDestino != null)
            {
                // Grafo não-direcionado: adiciona ida e volta
                _listaAdjacencia[verticeOrigem].Add(new Aresta<T>(verticeDestino, peso));
                _listaAdjacencia[verticeDestino].Add(new Aresta<T>(verticeOrigem, peso));
            }
        }

        /// <summary>
        /// Busca e retorna o vértice cujo dado seja igual ao valor informado.
        /// Se não encontrado, retorna null.
        /// </summary>
        /// <param name="dado">Valor do vértice buscado.</param>
        /// <returns>Objeto Vertice<T> correspondente ou null se não existir.</returns>
        private Vertice<T> EncontrarVertice(T dado)
        {
            foreach (var vertice in _listaAdjacencia.Keys)
            {
                if (vertice.Dado.Equals(dado))
                {
                    return vertice;
                }
            }
            return null;
        }

        /// <summary>
        /// Exibe no console todos os vértices do grafo e suas arestas adjacentes.
        /// Formato: Vértice X -> valor1 (peso) -> valor2 (peso)...
        /// </summary>
        public void ExibirGrafo()
        {
            foreach (var vertice in _listaAdjacencia.Keys)
            {
                Console.Write($"Vértice {vertice.Dado}: ");

                foreach (var aresta in _listaAdjacencia[vertice])
                {
                    Console.Write($"-> {aresta.Destino.Dado} (Peso: {aresta.Peso}) ");
                }

                Console.WriteLine();
            }
        }
    }
}
