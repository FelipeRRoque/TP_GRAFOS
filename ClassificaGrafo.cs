using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TP_GRAFOS
{
    /// <summary>
    /// Classe responsável por analisar a densidade de um grafo e instanciar
    /// automaticamente a estrutura de dados mais adequada: lista ou matriz de adjacência.
    /// </summary>
    public static class ClassificaGrafo
    {
        /// <summary>
        /// Cria uma instância de grafo (lista ou matriz de adjacência) com base na densidade.
        /// </summary>
        /// <typeparam name="T">Tipo dos valores armazenados nos vértices do grafo.</typeparam>
        /// <param name="vertices">Quantidade total de vértices do grafo.</param>
        /// <param name="arestas">Quantidade total de arestas do grafo.</param>
        /// <returns>
        /// Uma implementação de <see cref="IGrafo{T}"/> adequada à densidade da malha logística.
        /// </returns>
        /// <remarks>
        /// A densidade é calculada como:
        /// <c>densidade = arestas / (vertices * (vertices - 1))</c>.
        /// <para>
        /// Se a densidade for menor que 0.30, o grafo é considerado esparso
        /// e uma lista de adjacência é utilizada.
        /// Caso contrário, o grafo é considerado denso e uma matriz de adjacência é retornada.
        /// </para>
        /// </remarks>
        public static IGrafo<T> CriarGrafo<T>(int vertices, int arestas)
        {
            double densidade = (double)arestas / (vertices * (vertices - 1));

            if (densidade < 0.30)
                return new GrafoListaAdjacencia<T>();

            return new GrafoMatrizAdjacencia<T>(vertices);
        }
    }
}