using System.Text;

namespace TP_GRAFOS
{
    /// <summary>
    ///
    /// Esta classe realiza a análise de Caminho e Ciclo Euleriano em um grafo direcionado,
    /// utilizando o **Método de Fleury** para determinar a sequência de visitação das arestas.
    ///
    /// 1) Fluxo básico da execução:
    /// - O método <see cref="Executar"/> verifica preliminarmente as condições matemáticas:
    ///     • Calcula os graus de entrada e saída de todos os vértices.
    ///     • Determina a viabilidade:
    ///         - **Ciclo Euleriano**: Todos os vértices têm (Grau Entrada == Grau Saída).
    ///         - **Caminho Euleriano**: Apenas um vértice de início (Saída - Entrada = 1) e um de fim (Entrada - Saída = 1).
    ///     • Se inviável, encerra a execução. Caso contrário, inicia o algoritmo de Fleury.
    ///
    /// 2) Implementações internas:
    /// - <see cref="AlgoritmoFleury"/>:
    ///     • Constrói o percurso de forma "gulosa", escolhendo arestas uma a uma e removendo-as do grafo (cópia temporária).
    ///     • Para cada passo, decide qual aresta atravessar baseando-se na validação de pontes.
    ///
    /// - <see cref="EhProximaArestaValida"/> (Validação de Ponte):
    ///     • Simula a remoção da aresta candidata.
    ///     • Compara a quantidade de vértices alcançáveis antes e depois da remoção (usando BFS em <see cref="ContarVerticesAlcancaveis"/>).
    ///     • **Regra de Fleury**: Uma aresta só é escolhida se não for uma "ponte" (isto é, sua remoção não desconecta o grafo restante),
    ///       exceto se ela for a única opção disponível.
    ///
    /// 3) Finalização:
    /// - O resultado é formatado em uma string contendo o diagnóstico (Impossível / Caminho / Ciclo)
    ///   e a sequência ordenada de vértices visitados.
    ///     
    /// </summary>

    public class AnalisarCaminhoEuleriano : IAnalises
    {
        private readonly IGrafo<int> _grafo;

        public AnalisarCaminhoEuleriano(IGrafo<int> grafo)
        {
            _grafo = grafo;
        }

        public string Executar()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("=== Caminho / Ciclo Euleriano (Método de Fleury) ===");
            sb.AppendLine();

            // 1. Validar condições de existência (Graus)
            var vertices = _grafo.ObterVertices();
            if (vertices.Count == 0)
            {
                sb.AppendLine("Grafo vazio.");
                return sb.ToString();
            }

            var inDegree = new Dictionary<int, int>();
            var outDegree = new Dictionary<int, int>();

            // Inicializa contadores
            foreach (var v in vertices)
            {
                inDegree[v.Dado] = 0;
                outDegree[v.Dado] = 0;
            }

            // Calcula graus
            foreach (var u in vertices)
            {
                var vizinhos = _grafo.ObterVizinhos(u);
                outDegree[u.Dado] = vizinhos.Count;
                foreach (var v in vizinhos)
                {
                    inDegree[v.Dado]++;
                }
            }

            // Verifica condições de Euler para grafos direcionados
            Vertice<int> inicio = vertices[0]; // Padrão
            int startNodes = 0;
            int endNodes = 0;
            bool impossivel = false;

            foreach (var v in vertices)
            {
                int dif = outDegree[v.Dado] - inDegree[v.Dado];

                if (dif == 1)
                {
                    startNodes++;
                    inicio = v; // Candidato a início
                }
                else if (dif == -1)
                {
                    endNodes++;
                }
                else if (dif != 0)
                {
                    impossivel = true; // Diferença > 1 ou < -1
                    break;
                }
            }

            // Validação das regras
            bool temCiclo = !impossivel && startNodes == 0 && endNodes == 0;
            bool temCaminho = !impossivel && startNodes == 1 && endNodes == 1;

            if (!temCiclo && !temCaminho)
            {
                sb.AppendLine("Resultado: IMPOSSÍVEL.");
                sb.AppendLine("O grafo não atende às condições de graus para um Caminho ou Ciclo Euleriano.");
                sb.AppendLine("(Para caminho: exatamente um vértice com Grau Saída - Entrada = 1 e um com Entrada - Saída = 1).");
                sb.AppendLine("(Para ciclo: todos os vértices com Grau Entrada = Grau Saída).");
                return sb.ToString();
            }

            sb.AppendLine(temCiclo ? "Condição: CICLO Euleriano detectado." : "Condição: CAMINHO Euleriano detectado.");

            // 2. Preparar estrutura temporária para Fleury (Cópia Mutável)
            // Usamos um Dicionário de Listas para simular o grafo e permitir remoção de arestas
            var adjTemporaria = new Dictionary<int, List<Aresta<int>>>();

            // Popula com cópias das arestas
            foreach (var v in vertices)
            {
                adjTemporaria[v.Dado] = new List<Aresta<int>>();
            }

            foreach (var aresta in _grafo.ObterArestas())
            {
                // Criamos nova instância para não afetar o grafo original
                adjTemporaria[aresta.Origem.Dado].Add(new Aresta<int>(aresta.Origem, aresta.Destino, aresta.Peso, aresta.Capacidade));
            }

            // 3. Executar Fleury
            try
            {
                var caminho = AlgoritmoFleury(inicio.Dado, adjTemporaria);

                // Formatação da Saída
                sb.AppendLine("Sequência de Visita:");
                for (int i = 0; i < caminho.Count; i++)
                {
                    sb.Append(caminho[i]);
                    if (i < caminho.Count - 1) sb.Append(" -> ");
                }
                sb.AppendLine();
            }
            catch (Exception ex)
            {
                sb.AppendLine($"Erro durante execução do algoritmo: {ex.Message}");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Implementação do Algoritmo de Fleury.
        /// Escolhe arestas uma a uma, evitando pontes.
        /// </summary>
        private List<int> AlgoritmoFleury(int u, Dictionary<int, List<Aresta<int>>> adj)
        {
            var caminho = new List<int>();
            caminho.Add(u); // Adiciona vértice inicial

            // Enquanto houver arestas saindo do vértice atual
            while (adj.ContainsKey(u) && adj[u].Count > 0)
            {
                Aresta<int> arestaEscolhida = null;

                // Caso 1: Se só tem uma aresta, é ela mesma (não há escolha)
                if (adj[u].Count == 1)
                {
                    arestaEscolhida = adj[u][0];
                }
                else
                {
                    // Caso 2: Tem mais de uma. Tenta achar uma que NÃO seja ponte.
                    foreach (var aresta in adj[u])
                    {
                        if (EhProximaArestaValida(u, aresta, adj))
                        {
                            arestaEscolhida = aresta;
                            break;
                        }
                    }

                    // Fallback: Se todas forem pontes (raro em euleriano válido, mas possível no fim), pega a primeira
                    if (arestaEscolhida == null)
                    {
                        arestaEscolhida = adj[u][0];
                    }
                }

                // Efetiva a travessia
                // 1. Remove a aresta do grafo temporário
                adj[u].Remove(arestaEscolhida);

                // 2. Move para o destino
                u = arestaEscolhida.Destino.Dado;

                // 3. Adiciona ao caminho
                caminho.Add(u);
            }

            return caminho;
        }

        /// <summary>
        /// Verifica se a aresta (u -> v) é válida para ser atravessada agora.
        /// No Fleury, uma aresta é válida se:
        /// 1. É a única aresta saindo de u.
        /// 2. OU se sua remoção não torna o grafo "menos alcançável" (não é ponte).
        /// </summary>
        private bool EhProximaArestaValida(int u, Aresta<int> aresta, Dictionary<int, List<Aresta<int>>> adj)
        {
            // 1. Contar vértices alcançáveis ANTES de remover a aresta
            int contagemAntes = ContarVerticesAlcancaveis(u, adj);

            // 2. Remover temporariamente a aresta
            adj[u].Remove(aresta);

            // 3. Contar vértices alcançáveis DEPOIS de remover
            int contagemDepois = ContarVerticesAlcancaveis(u, adj);

            // 4. Adicionar a aresta de volta (backtrack)
            adj[u].Add(aresta);

            // Se a contagem for a mesma, a aresta não é uma ponte (safe to cross)
            // Se a contagem diminuiu, ela é uma ponte (evitar se possível)
            return contagemAntes == contagemDepois;
        }

        /// <summary>
        /// BFS simples para contar quantos vértices são alcançáveis a partir de u.
        /// </summary>
        private int ContarVerticesAlcancaveis(int inicio, Dictionary<int, List<Aresta<int>>> adj)
        {
            var visitados = new HashSet<int>();
            var fila = new Queue<int>();

            fila.Enqueue(inicio);
            visitados.Add(inicio);

            while (fila.Count > 0)
            {
                int atual = fila.Dequeue();

                if (adj.ContainsKey(atual))
                {
                    foreach (var aresta in adj[atual])
                    {
                        if (!visitados.Contains(aresta.Destino.Dado))
                        {
                            visitados.Add(aresta.Destino.Dado);
                            fila.Enqueue(aresta.Destino.Dado);
                        }
                    }
                }
            }

            return visitados.Count;
        }
    }
}