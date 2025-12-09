using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TP_GRAFOS
{
    /// <summary>
    /// Classe utilitária que oferece operações auxiliares para manipulação e 
    /// transformação de grafos. Reúne métodos voltados para criação de subgrafos
    /// e para geração de grafos derivados usados em análises específicas.
    /// 
    /// Sua função é centralizar rotinas reaproveitáveis, promovendo maior
    /// coesão, redução de duplicação de código e facilitando a manutenção
    /// da aplicação. Essas utilidades apoiam diferentes etapas do SORL,
    /// como detecção de conflitos, preparação de grafos de apoio e adaptação
    /// de dados para outros algoritmos.
    /// </summary>
    public static class GrafoUtilitario
    {
        /// <summary>
        /// Cria um subgrafo contendo exclusivamente os vértices do grafo original,
        /// sem copiar arestas. Esse método é útil quando algoritmos subsequentes
        /// precisam trabalhar com uma estrutura inicial limpa, preservando apenas
        /// os hubs da malha logística.
        /// 
        /// Escolhe automaticamente a mesma representação do grafo original
        /// (lista de adjacência ou matriz), garantindo consistência interna
        /// e compatibilidade com as demais análises.
        /// </summary>
        /// <typeparam name="T">Tipo dos dados armazenados nos vértices.</typeparam>
        /// <param name="grafoOriginal">Grafo base que será reduzido.</param>
        /// <returns>Um novo grafo contendo somente os vértices.</returns>
        public static IGrafo<T> CriarSubgrafoSomenteVertices<T>(IGrafo<T> grafoOriginal)
        {
            IGrafo<T> subgrafo;

            if (grafoOriginal is GrafoListaAdjacencia<T>)
                subgrafo = new GrafoListaAdjacencia<T>();
            else if (grafoOriginal is GrafoMatrizAdjacencia<T>)
                subgrafo = new GrafoMatrizAdjacencia<T>(grafoOriginal.ObterVertices().Count);
            else
                throw new Exception("Tipo de representacao de grafo desconhecido.");

            foreach (var v in grafoOriginal.ObterVertices())
                subgrafo.AdicionarVertice(v.Dado);

            return subgrafo;
        }
        
        /// <summary>
        /// Gera um grafo de conflitos a partir das arestas do grafo original.
        /// Cada aresta se torna um vértice no novo grafo, e duas arestas são
        /// conectadas caso compartilhem pelo menos um hub. 
        /// 
        /// Esse grafo é indispensável para o algoritmo de coloração aplicado ao
        /// agendamento de manutenções, permitindo detectar grupos de rotas que 
        /// não podem ser interditadas simultaneamente. A partir dele, o SORL 
        /// minimiza o número de turnos necessários para manutenção preventiva.
        /// </summary>
        /// <typeparam name="T">Tipo dos dados do grafo original.</typeparam>
        /// <param name="grafoOriginal">Grafo representando a malha logística atual.</param>
        /// <returns>Um grafo onde cada vértice representa uma rota com possíveis conflitos.</returns>
        public static IGrafo<string> GerarGrafoDeConflitos<T>(IGrafo<T> grafoOriginal)
        {
            var arestasOriginais = grafoOriginal.ObterArestas();
            var grafoConflitos = new GrafoMatrizAdjacencia<string>(arestasOriginais.Count);

            foreach (var arestas in arestasOriginais)
            {
                string rota = $"{arestas.Origem.Dado}-{arestas.Destino.Dado}";
                grafoConflitos.AdicionarVertice(rota);
            }
            var verticesConflitos = grafoConflitos.ObterVertices();

            for (int i = 0; i < arestasOriginais.Count; i++)
            {
                for (int j = i + 1; j < arestasOriginais.Count; j++)
                {
                    var arestaA = arestasOriginais[i];
                    var arestaB = arestasOriginais[j];

                    bool haConflito =
                        arestaA.Origem.Equals(arestaB.Origem) ||
                        arestaA.Origem.Equals(arestaB.Destino) ||
                        arestaA.Destino.Equals(arestaB.Origem) ||
                        arestaA.Destino.Equals(arestaB.Destino);

                    if (haConflito)
                    {
                        string rotaA = $"{arestaA.Origem.Dado}-{arestaA.Destino.Dado}";
                        string rotaB = $"{arestaB.Origem.Dado}-{arestaB.Destino.Dado}";

                        grafoConflitos.AdicionarAresta(rotaA, rotaB);
                        grafoConflitos.AdicionarAresta(rotaB, rotaA);
                    }
                }
            }
            return grafoConflitos;
        }
    }
}