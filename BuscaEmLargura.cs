using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TP_GRAFOS
{
    public static class BuscaEmLargura
    {
        /// <summary>
        /// Realiza uma BFS para encontrar o caminho mais curto (em número de arestas) entre origem e destino.
        /// Permite injetar uma função de validação para determinar se uma aresta pode ser atravessada.
        /// </summary>
        /// <typeparam name="T">Tipo do dado do vértice.</typeparam>
        /// <param name="grafo">Instância do grafo (via interface).</param>
        /// <param name="origem">Vértice de partida.</param>
        /// <param name="destino">Vértice de chegada.</param>
        /// <param name="criterioTravessia">
        /// (Opcional) Função que recebe (origem, destino) e retorna true se a aresta pode ser usada.
        /// Se null, considera todas as arestas existentes.
        /// Útil para Grafos Residuais no Edmonds-Karp.
        /// </param>
        /// <returns>Lista de arestas representando o caminho, ou null se não houver caminho.</returns>
        public static List<Aresta<T>>? EncontrarCaminho<T>(IGrafo<T> grafo,Vertice<T> origem, Vertice<T> destino, 
            Func<Vertice<T>, Vertice<T>, bool>? criterioTravessia = null)
        {
            Dictionary<Vertice<T>, Vertice<T>>? predecessores = new Dictionary<Vertice<T>, Vertice<T>>();
            HashSet<Vertice<T>> visitados = new HashSet<Vertice<T>>();
            Queue<Vertice<T>> fila = new Queue<Vertice<T>>();

            fila.Enqueue(origem);
            visitados.Add(origem);
            predecessores[origem] = null;

            bool encontrouDestino = false;

            while (fila.Count > 0)
            {
                Vertice<T> atual = fila.Dequeue();

                if (atual.Equals(destino))
                {
                    encontrouDestino = true;
                    return ReconstruirCaminho(grafo, predecessores, destino);
                }

                foreach (Vertice<T> vizinho in grafo.ObterVizinhos(atual))
                {
                    if (!visitados.Contains(vizinho))
                    {
                        bool podeAtravessar = criterioTravessia == null || criterioTravessia(atual, vizinho);

                        if (podeAtravessar)
                        {
                            visitados.Add(vizinho);
                            predecessores[vizinho] = atual;
                            fila.Enqueue(vizinho);
                        }
                    }
                }
            }

            if (!encontrouDestino) return null;

            return ReconstruirCaminho(grafo, predecessores, destino);
        }

        /// <summary>
        /// Método auxiliar para reconstruir o caminho de trás para frente.
        /// </summary>
        private static List<Aresta<T>> ReconstruirCaminho<T>(IGrafo<T> grafo, Dictionary<Vertice<T>, Vertice<T>> predecessores, Vertice<T> destino)
        {
            List<Aresta<T>> caminho = new List<Aresta<T>>();
            Vertice<T> atual = destino;

            while (predecessores.ContainsKey(atual) && predecessores[atual] != null)
            {
                Vertice<T> pai = predecessores[atual];

                int peso = grafo.ObterPeso(pai, atual);
                int capacidade = grafo.ObterCapacidade(pai, atual);

                Aresta<T> aresta = new Aresta<T>(pai, atual, peso, capacidade);
                caminho.Add(aresta);

                atual = pai;
            }

            caminho.Reverse(); 
            return caminho;
        }
    }
}

