using System.Text;

namespace TP_GRAFOS
{
    /// <summary>
    /// Esta classe realiza a análise de existência e construção de um
    /// Caminho ou Ciclo Euleriano em um grafo direcionado usando o Método de Fleury.
    ///
    /// Principais etapas:
    /// 1) Validação de graus:
    ///    - Calcula grau de entrada (inDegree) e saída (outDegree) para cada vértice.
    ///    - Determina se o grafo admite:
    ///        * Ciclo Euleriano: para todo vértice indeg == outdeg; ou
    ///        * Caminho Euleriano: existe exatamente um vértice com outdeg - indeg = 1
    ///          (início) e um com indeg - outdeg = 1 (fim).
    /// 2) Preparação de uma cópia mutável do grafo:
    ///    - Constrói um dicionário adjTemporaria: int -> List<Aresta<int>> que será
    ///      modificado durante a execução (remoção de arestas).
    /// 3) Algoritmo de Fleury:
    ///    - A cada passo escolhe uma aresta saindo do vértice atual que não seja ponte
    ///      (ou a única disponível).
    ///    - Para determinar se uma aresta é ponte, remove-se temporariamente a aresta,
    ///      conta-se os vértices alcançáveis antes e depois (BFS) e compara-se as contagens.
    ///    - Evita-se a exceção "Collection was modified" iterando sobre cópias (.ToList())
    ///      das listas de adjacência quando for necessário testar/remover/reinserir arestas.
    /// </summary>
    public class AnalisarCaminhoEuleriano : IAnalises
    {
        private readonly IGrafo<int> _grafo;

        public AnalisarCaminhoEuleriano(IGrafo<int> grafo)
        {
            _grafo = grafo ?? throw new ArgumentNullException(nameof(grafo));
        }

        /// <summary>
        /// Executa a análise completa: valida graus, decide o tipo (círculo/caminho),
        /// e, se aplicável, constrói o percurso Euleriano usando Fleury.
        /// Retorna um relatório em texto com o diagnóstico e a sequência de visita.
        /// </summary>
        public string Executar()
        {
            var sb = new StringBuilder();
            sb.AppendLine("=== Caminho / Ciclo Euleriano (Método de Fleury) ===");
            sb.AppendLine();

            // 1) Obter lista de vértices e checar grafo vazio
            var vertices = _grafo.ObterVertices();
            if (vertices == null || vertices.Count == 0)
            {
                sb.AppendLine("Grafo vazio.");
                return sb.ToString();
            }

            // 2) Inicializa contadores de grau de entrada e saída
            var inDegree = new Dictionary<int, int>();
            var outDegree = new Dictionary<int, int>();
            foreach (var v in vertices)
            {
                inDegree[v.Dado] = 0;
                outDegree[v.Dado] = 0;
            }

            // 3) Calcula graus usando a lista de vizinhos do grafo original
            foreach (var u in vertices)
            {
                var vizinhos = _grafo.ObterVizinhos(u) ?? new List<Vertice<int>>();
                outDegree[u.Dado] = vizinhos.Count;
                foreach (var v in vizinhos)
                {
                    // incrementa grau de entrada do destino
                    if (!inDegree.ContainsKey(v.Dado))
                        inDegree[v.Dado] = 0;
                    inDegree[v.Dado]++;
                }
            }

            // 4) Determina se existe ciclo ou caminho Euleriano (baseado nos graus)
            Vertice<int> inicio = vertices[0]; // padrão: primeiro vértice
            int startNodes = 0;
            int endNodes = 0;
            bool impossivel = false;

            foreach (var v in vertices)
            {
                int outDeg = outDegree.ContainsKey(v.Dado) ? outDegree[v.Dado] : 0;
                int inDeg = inDegree.ContainsKey(v.Dado) ? inDegree[v.Dado] : 0;
                int dif = outDeg - inDeg;

                if (dif == 1)
                {
                    startNodes++;
                    inicio = v; // candidato a início do caminho
                }
                else if (dif == -1)
                {
                    endNodes++;
                }
                else if (dif != 0)
                {
                    // diferença maior que 1 ou menor que -1 -> impossível
                    impossivel = true;
                    break;
                }
            }

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

            // 5) Prepara uma cópia mutável do grafo (adjTemporaria) onde removeremos arestas
            var adjTemporaria = new Dictionary<int, List<Aresta<int>>>();
            foreach (var v in vertices)
            {
                adjTemporaria[v.Dado] = new List<Aresta<int>>();
            }

            // Copia arestas do grafo original para a estrutura temporária (novas instâncias)
            foreach (var aresta in _grafo.ObterArestas())
            {
                // cria nova instância para manter original imutável
                var copia = new Aresta<int>(aresta.Origem, aresta.Destino, aresta.Peso, aresta.Capacidade);
                if (!adjTemporaria.ContainsKey(aresta.Origem.Dado))
                {
                    adjTemporaria[aresta.Origem.Dado] = new List<Aresta<int>>();
                }
                adjTemporaria[aresta.Origem.Dado].Add(copia);
            }

            // 6) Executa Fleury sobre a cópia
            try
            {
                var caminho = AlgoritmoFleury(inicio.Dado, adjTemporaria);

                // Formata a sequência encontrada
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
                // captura qualquer exceção e retorna no relatório
                sb.AppendLine($"Erro durante execução do algoritmo: {ex.Message}");
            }

            return sb.ToString();
        }

        /// <summary>
        /// Algoritmo de Fleury: constrói o percurso Euleriano escolhendo arestas que não
        /// sejam pontes sempre que possível. Trabalha sobre 'adj' (cópia mutável do grafo).
        /// </summary>
        private List<int> AlgoritmoFleury(int u, Dictionary<int, List<Aresta<int>>> adj)
        {
            var caminho = new List<int>();
            caminho.Add(u); // adiciona vértice inicial

            // Loop principal: enquanto existirem arestas saindo do vértice atual
            while (adj.ContainsKey(u) && adj[u].Count > 0)
            {
                Aresta<int> arestaEscolhida = null;

                // Se só há uma aresta, não há escolha: usá-la
                if (adj[u].Count == 1)
                {
                    arestaEscolhida = adj[u][0];
                }
                else
                {
                    // IMPORTANTE: iterar sobre uma cópia (ToList) para evitar "Collection was modified"
                    // pois EhProximaArestaValida faz remoção temporária/reinserção na mesma lista.
                    foreach (var aresta in adj[u].ToList())
                    {
                        // testa se a aresta não é ponte (ou seja, safe to cross)
                        if (EhProximaArestaValida(u, aresta, adj))
                        {
                            arestaEscolhida = aresta;
                            break;
                        }
                    }

                    // Se nenhuma aresta foi considerada "não-ponte", escolhe-se a primeira (fallback)
                    if (arestaEscolhida == null)
                    {
                        arestaEscolhida = adj[u][0];
                    }
                }

                // Efetiva a travessia: remove a aresta e avança para o destino
                // Remover a instância da aresta que foi escolhida
                bool removed = adj[u].Remove(arestaEscolhida);
                if (!removed)
                {
                    // Caso improvável de falha na remoção (por segurança)
                    throw new InvalidOperationException("Falha ao remover aresta escolhida da lista de adjacência.");
                }

                // Avança para o próximo vértice
                u = arestaEscolhida.Destino.Dado;
                caminho.Add(u);
            }

            return caminho;
        }

        /// <summary>
        /// Verifica se a aresta (u -> aresta.Destino) pode ser escolhida agora.
        /// Remove-se temporariamente a aresta, conta-se os vértices alcançáveis antes e depois,
        /// e compara-se. Reinsere-se a aresta posteriormente (backtrack).
        /// </summary>
        private bool EhProximaArestaValida(int u, Aresta<int> aresta, Dictionary<int, List<Aresta<int>>> adj)
        {
            // Contagem antes da remoção
            int contagemAntes = ContarVerticesAlcancaveis(u, adj);

            // Remove temporariamente a aresta (backtracking posterior)
            bool removed = adj[u].Remove(aresta);

            // Se por algum motivo a remoção falhar, consideramos a aresta inválida
            if (!removed)
            {
                // Esse caso é improvável porque chamamos EhProximaArestaValida apenas com arestas existentes,
                // mas aqui garantimos comportamento consistente.
                return false;
            }

            // Contagem depois da remoção
            int contagemDepois = ContarVerticesAlcancaveis(u, adj);

            // Reinsere a aresta para restaurar o estado anterior. Inserimos no final;
            // a ordem das arestas pode ser relevante em algumas políticas, mas aqui não afeta corretude.
            adj[u].Add(aresta);

            // Se a contagem permanece a mesma, a remoção não desconectou o componente -> aresta não é ponte
            return contagemAntes == contagemDepois;
        }

        /// <summary>
        /// BFS para contar quantos vértices são alcançáveis a partir de 'inicio' na estrutura 'adj'.
        /// Itera sobre cópias (.ToList()) das listas para proteger contra modificações em chamadas aninhadas.
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

                if (!adj.ContainsKey(atual))
                    continue;

                // ITERAR SOBRE UMA CÓPIA para evitar problemas se a lista for modificada enquanto fazemos buscas
                foreach (var aresta in adj[atual].ToList())
                {
                    int destino = aresta.Destino.Dado;
                    if (!visitados.Contains(destino))
                    {
                        visitados.Add(destino);
                        fila.Enqueue(destino);
                    }
                }
            }

            return visitados.Count;
        }
    }
}
