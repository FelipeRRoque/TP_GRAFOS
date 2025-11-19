using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TP_GRAFOS
{
    /// <summary>
    /// Define as operações básicas para manipulação de um grafo genérico,
    /// independentemente da estrutura interna de armazenamento.
    /// </summary>
    /// <typeparam name="T">Tipo de dado armazenado nos vértices.</typeparam>
    public interface IGrafo<T>
    {
        /// <summary>
        /// Adiciona um novo vértice ao grafo.
        /// </summary>
        /// <param name="dado">Valor associado ao vértice.</param>
        void AdicionarVertice(T dado);

        /// <summary>
        /// Cria uma nova aresta entre dois vértices existentes.
        /// </summary>
        /// <param name="origem">Valor do vértice de origem.</param>
        /// <param name="destino">Valor do vértice de destino.</param>
        /// <param name="peso">Peso associado à aresta.</param>
        /// <param name="capacidade">Capacidade associada à aresta.</param>
        void AdicionarAresta(T origem, T destino, int peso = 1, int capacidade = 0);

        /// <summary>
        /// Retorna a lista completa de vértices do grafo.
        /// </summary>
        List<Vertice<T>> ObterVertices();

        /// <summary>
        /// Retorna a lista completa de arestas presentes no grafo.
        /// </summary>
        List<Aresta<T>> ObterArestas();

        /// <summary>
        /// Exibe no console a estrutura completa do grafo.
        /// </summary>
        void ExibirGrafo();
    }
}