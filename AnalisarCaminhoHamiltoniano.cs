using System.Text;

namespace TP_GRAFOS
{
    /// <summary>
    ///
    /// Esta classe realiza a análise de Caminho e Ciclo Hamiltoniano em um grafo direcionado,
    /// escolhendo automaticamente entre uma **Busca Exata** ou uma **Heurística**,
    /// dependendo da quantidade de vértices do grafo (limite de 20).
    ///
    /// 1) Fluxo básico da execução:
    /// - O método <see cref="Executar"/> avalia o tamanho do grafo (N):
    ///     • Se N &lt;= 20: utiliza o algoritmo de **Backtracking** (busca exaustiva).
    ///     • Se N > 20: utiliza a **Heurística de Warnsdorff** (abordagem gulosa).
    /// - O resultado é processado para distinguir entre apenas um caminho ou um ciclo fechado.
    ///
    /// 2) Implementações internas:
    /// - <see cref="Backtrack"/> (Busca Exata):
    ///     • Executa uma DFS (Busca em Profundidade) recursiva para testar permutações.
    ///     • Ordena os vizinhos pelo grau de saída (crescente) para priorizar caminhos
    ///       mais restritivos, podando a árvore de recursão mais cedo.
    ///     • Garante a resposta correta (se existe ou não) para grafos pequenos.
    ///
    /// - <see cref="TentarHeuristica"/> (Aproximação):
    ///     • Aplica a **Regra de Warnsdorff**: escolhe sempre o próximo vértice que possui
    ///       a menor quantidade de saídas disponíveis.
    ///     • Minimiza o risco de entrar em "becos sem saída" prematuramente.
    ///     • Realiza múltiplas tentativas variando o ponto de partida para aumentar a taxa de sucesso.
    ///
    /// 3) Finalização:
    /// - O método <see cref="ImprimirResultado"/> verifica a conexão entre o último
    ///   e o primeiro vértice da sequência encontrada:
    ///     • Se houver aresta de retorno: Identifica como **Ciclo Hamiltoniano**.
    ///     • Caso contrário: Identifica como **Caminho Hamiltoniano**.
    ///     • Retorna a string formatada com a sequência de visita.
    ///     
    /// </summary>

    public class AnalisarCaminhoHamiltoniano : IAnalises
    {
        private readonly IGrafo<int> _grafo;

        public AnalisarCaminhoHamiltoniano(IGrafo<int> grafo)
        {
            _grafo = grafo;
        }

        public string Executar()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("=== Caminho / Ciclo Hamiltoniano ===");
            sb.AppendLine();

            VerificarHamiltoniano(sb);

            return sb.ToString();
        }

        private void VerificarHamiltoniano(StringBuilder sb)
        {
            var vertices = _grafo.ObterVertices();
            int n = vertices.Count;

            if (n == 0)
            {
                sb.AppendLine("Grafo vazio.");
                return;
            }

            // Pré-processamento: Converter para Dicionário de Adjacência (Int -> List<Int>)
            // Isso acelera muito o acesso aos vizinhos durante a recursão
            var adj = new Dictionary<int, List<int>>();
            foreach (var v in vertices)
            {
                // Em grafo direcionado, ObterVizinhos retorna apenas para onde a aresta aponta
                var vizinhos = _grafo.ObterVizinhos(v).Select(u => u.Dado).ToList();
                adj[v.Dado] = vizinhos;
            }

            // --- ESTRATÉGIA 1: HEURÍSTICA (Para grafos grandes) ---
            if (n > 20)
            {
                sb.AppendLine($"Grafo com {n} vértices. A busca exata (O(n!)) é inviável.");
                sb.AppendLine("Utilizando Heurística de Warnsdorff (Guloso)...");

                // Tenta encontrar usando heurística
                var resultadoHeuristico = TentarHeuristica(vertices.Select(v => v.Dado).ToList(), adj);

                if (resultadoHeuristico != null)
                {
                    ImprimirResultado(resultadoHeuristico, adj, sb);
                }
                else
                {
                    sb.AppendLine("Resultado: NENHUM percurso encontrado pela heurística.");
                    sb.AppendLine("(Nota: Em problemas NP-Completos grandes, a heurística pode falhar mesmo que o caminho exista).");
                }
                return;
            }

            // --- ESTRATÉGIA 2: BACKTRACKING EXATO (Para grafos pequenos) ---
            // Tenta começar de cada vértice até achar uma solução
            foreach (var verticeInicio in vertices)
            {
                var caminho = new List<int>();
                var visitados = new HashSet<int>();
                int inicio = verticeInicio.Dado;

                caminho.Add(inicio);
                visitados.Add(inicio);

                // Chama a recursão
                if (Backtrack(inicio, inicio, visitados, caminho, adj, n))
                {
                    ImprimirResultado(caminho, adj, sb);
                    return; // Encontrou, pode parar
                }
            }

            sb.AppendLine("Resultado: IMPOSSÍVEL.");
            sb.AppendLine("Verificado exaustivamente: Não existe caminho ou ciclo Hamiltoniano neste grafo.");
        }

        /// <summary>
        /// Algoritmo de Busca Exata (DFS).
        /// Retorna true assim que encontra um caminho que visita todos os nós.
        /// </summary>
        private bool Backtrack(
            int atual,
            int inicioGeral,
            HashSet<int> visitados,
            List<int> caminho,
            Dictionary<int, List<int>> adj,
            int totalVertices)
        {
            // Caso Base: Visitou todos os vértices
            if (caminho.Count == totalVertices)
            {
                // O caminho já é válido.
                // A verificação se fecha ciclo (volta ao início) é feita na hora de imprimir.
                return true;
            }

            // Ordenação Otimizada (Heurística dentro do Backtrack):
            // Tenta ir primeiro nos vizinhos que têm MENOS saídas (Warnsdorff).
            // Isso ajuda a podar a árvore de recursão mais cedo.
            var vizinhosOrdenados = adj[atual]
                .Where(v => !visitados.Contains(v)) // Apenas não visitados
                .OrderBy(v => adj[v].Count)         // Prioriza os "mais difíceis"
                .ToList();

            foreach (var vizinho in vizinhosOrdenados)
            {
                visitados.Add(vizinho);
                caminho.Add(vizinho);

                if (Backtrack(vizinho, inicioGeral, visitados, caminho, adj, totalVertices))
                    return true;

                // Backtrack (desfaz a escolha)
                visitados.Remove(vizinho);
                caminho.RemoveAt(caminho.Count - 1);
            }

            return false;
        }

        /// <summary>
        /// Tenta encontrar um caminho usando a regra de Warnsdorff sem backtracking profundo.
        /// Tenta começar de VÁRIOS pontos diferentes para aumentar a chance de sucesso.
        /// </summary>
        private List<int> TentarHeuristica(List<int> todosVertices, Dictionary<int, List<int>> adj)
        {
            // Limita as tentativas para não demorar demais se o grafo for gigante
            int tentativasMaximas = Math.Min(todosVertices.Count, 100);

            for (int i = 0; i < tentativasMaximas; i++)
            {
                int inicio = todosVertices[i];
                var caminho = new List<int> { inicio };
                var visitados = new HashSet<int> { inicio };
                int atual = inicio;
                bool semSaida = false;

                while (caminho.Count < todosVertices.Count)
                {
                    // Regra de Warnsdorff: Escolhe o vizinho não visitado que tem o MENOR grau de saída
                    var proximo = adj[atual]
                        .Where(v => !visitados.Contains(v))
                        .OrderBy(v => adj[v].Count)
                        .FirstOrDefault();

                    // Se proximo for 0 (valor default de int) e não estiver na lista (assumindo vértices > 0), ou se a lista for vazia
                    // Melhor verificação:
                    bool encontrou = false;
                    foreach (var v in adj[atual].Where(v => !visitados.Contains(v)).OrderBy(v => adj[v].Count))
                    {
                        proximo = v;
                        encontrou = true;
                        break;
                    }

                    if (!encontrou)
                    {
                        semSaida = true;
                        break;
                    }

                    visitados.Add(proximo);
                    caminho.Add(proximo);
                    atual = proximo;
                }

                if (!semSaida && caminho.Count == todosVertices.Count)
                {
                    return caminho;
                }
            }
            return null;
        }

        /// <summary>
        /// Formata a saída, verificando se o caminho encontrado permite voltar ao início (Ciclo).
        /// </summary>
        private void ImprimirResultado(List<int> caminho, Dictionary<int, List<int>> adj, StringBuilder sb)
        {
            int ultimo = caminho.Last();
            int primeiro = caminho.First();

            // Verifica se existe aresta do último para o primeiro
            bool fechaCiclo = adj[ultimo].Contains(primeiro);

            if (fechaCiclo)
            {
                sb.AppendLine("Resultado: VIÁVEL (Ciclo Hamiltoniano).");
                sb.AppendLine("O inspetor pode visitar todos os hubs e retornar ao ponto de partida.");
            }
            else
            {
                sb.AppendLine("Resultado: PARCIAL (Caminho Hamiltoniano).");
                sb.AppendLine("É possível visitar todos os hubs uma única vez, MAS NÃO é possível retornar diretamente ao início (não há aresta de volta).");
            }

            sb.AppendLine();
            sb.Append("Sequência de Visita: ");

            for (int i = 0; i < caminho.Count; i++)
            {
                sb.Append(caminho[i]);
                if (i < caminho.Count - 1) sb.Append(" -> ");
            }

            if (fechaCiclo)
            {
                sb.Append($" -> {primeiro} (Retorno)");
            }

            sb.AppendLine();
        }
    }
}