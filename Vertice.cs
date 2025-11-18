using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TP_GRAFOS
{
    /// <summary>
    /// Representa um vértice genérico em um grafo.
    /// Armazena um dado do tipo T e pode ser utilizado em estruturas de grafos.
    /// </summary>
    /// <typeparam name="T">Tipo do dado armazenado no vértice.</typeparam>
    public class Vertice<T>
    {
        /// <summary>
        /// Valor armazenado no vértice.
        /// </summary>
        public T Dado { get; set; }

        /// <summary>
        /// Inicializa uma nova instância de um vértice contendo o dado informado.
        /// </summary>
        /// <param name="dado">Valor a ser armazenado no vértice.</param>
        public Vertice(T dado)
        {
            Dado = dado;
        }

        /// <summary>
        /// Retorna uma representação textual do vértice, baseada no valor armazenado.
        /// </summary>
        /// <returns>String representando o dado do vértice.</returns>
        public override string ToString()
        {
            return Dado.ToString();
        }
    }
}
