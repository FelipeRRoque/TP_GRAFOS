using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TP_GRAFOS
{
    /// <summary>
    /// Representa uma aresta em um grafo genérico.
    /// Armazena o vértice de destino e o peso associado à ligação.
    /// </summary>
    /// <typeparam name="T">Tipo do dado armazenado no vértice.</typeparam>
    public class Aresta<T>
    {
        /// <summary>
        /// Vértice para o qual a aresta aponta.
        /// </summary>
        public Vertice<T> Destino { get; set; }

        /// <summary>
        /// Peso associado à aresta. 
        /// Pode representar custo, distância, ou qualquer outra métrica.
        /// </summary>
        public int Peso { get; set; }

        /// <summary>
        /// Inicializa uma nova instância de uma aresta com vértice de destino e peso.
        /// </summary>
        /// <param name="destino">Vértice conectado por esta aresta.</param>
        /// <param name="peso">Peso da aresta. Por padrão, 1.</param>
        public Aresta(Vertice<T> destino, int peso = 1)
        {
            Destino = destino;
            Peso = peso;
        }
    }
}
