using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;

namespace TP_GRAFOS
{
    /// <summary>
    /// Responsável pela leitura de arquivos no formato DIMACS e construção de um grafo.
    /// Esta classe contém métodos utilitários para carregar vértices e arestas a partir
    /// de um arquivo de texto estruturado.
    /// </summary>
    public static class Arquivo
    {
        /// <summary>
        /// Lê um arquivo DIMACS contendo a definição de um grafo e retorna um objeto <see cref="Grafo{T}"/>.
        /// O arquivo deve seguir o formato:
        /// 
        ///     N M
        ///     u v peso capacidade
        ///     ...
        /// 
        /// Onde:
        /// N = número total de vértices
        /// M = número total de arestas
        /// u, v = vértices de origem e destino
        /// peso = custo da aresta
        /// capacidade = capacidade máxima da rota
        /// 
        /// </summary>
        /// <param name="caminho">Caminho do arquivo a ser lido.</param>
        /// <returns>Um grafo populado com todos os vértices e arestas especificados no arquivo.</returns>
        public static IGrafo<int> LerDados(string caminho)
        {
            var grafo = new Grafo<int>();
            var linhas = File.ReadAllLines(caminho);

            var cabecalho = linhas[0].Split(' ');
            int numeroVertices = int.Parse(cabecalho[0]);
            int numeroArestas = int.Parse(cabecalho[1]);

            // Adiciona todos os vértices primeiro
            for (int v = 1; v <= numeroVertices; v++)
                grafo.AdcionarVertice(v);

            // Lê todas as arestas
            for (int i = 1; i <= numeroArestas; i++)
            {
                var dados = linhas[i].Split(' ');

                int origem = int.Parse(dados[0]);
                int destino = int.Parse(dados[1]);
                int peso = int.Parse(dados[2]);
                int capacidade = int.Parse(dados[3]);

                grafo.AdcionarAresta(origem, destino, peso, capacidade);
            }

            return grafo;
        }
    }
}