using System.Text;

namespace TP_GRAFOS
{

    /// <summary>
    /// Classe responsável por analisar a existência de caminho ou ciclo Hamiltoniano em um grafo.
    /// 
    /// O processo segue duas abordagens principais, escolhidas de acordo com o tamanho do grafo:
    /// 
    /// 1) Busca exata por backtracking  
    ///    - Realizada em <see cref="BacktrackHamilton"/>  
    ///    - Testa recursivamente todas as sequências possíveis de vértices sem repetições  
    ///    - Pode identificar CAMINHO e também CICLO Hamiltoniano (se existir aresta para retornar ao início)
    ///    - Usada somente quando o grafo possui até 20 vértices (complexidade O(n!))
    /// 
    /// 2) Heurística gulosa para grafos maiores  
    ///    - Realizada em <see cref="HeuristicaHamiltoniana"/>  
    ///    - Sempre escolhe o próximo vértice com menor grau (princípio de escolha mínima)  
    ///    - Pode encontrar uma solução válida, mas não garante exatidão.
    /// 
    /// O fluxo geral executado por <see cref="Executar"/> é:  
    /// - Carregar todos os vizinhos do grafo  
    /// - Testar método exato (<see cref="BacktrackHamilton"/>) para grafos pequenos  
    /// - Caso contrário, aplicar heurística (<see cref="HeuristicaHamiltoniana"/>)  
    /// - Retornar string com o caminho encontrado.
    /// </summary>

    public class AnalisarCaminhoHamiltoniano : IAnalises
    {
        private readonly IGrafo<int> _grafo;

        public AnalisarCaminhoHamiltoniano(IGrafo<int> grafo)
        {
            _grafo = grafo;
        }

        // Alterado de void para string para retornar o conteúdo acumulado
        public string Executar()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("=== Caminho / Ciclo Hamiltoniano ===\n");

            // Passa o StringBuilder para os métodos internos preencherem
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

            if (n > 20)
            {
                sb.AppendLine($"Grafo com {n} vértices — busca exata é inviável. Usando heurística.");
                var heuristica = HeuristicaHamiltoniana(vertices);

                if (heuristica != null)
                {
                    sb.AppendLine("Caminho Hamiltoniano encontrado (heurística):");
                    Imprimir(heuristica, sb);
                }
                else
                {
                    sb.AppendLine("Nenhum caminho Hamiltoniano encontrado (heurística).");
                }

                return;
            }

            var adj = new Dictionary<Vertice<int>, List<Vertice<int>>>();
            foreach (var v in vertices)
                adj[v] = _grafo.ObterVizinhos(v)?.ToList() ?? new List<Vertice<int>>();

            foreach (var inicio in vertices)
            {
                var visited = new HashSet<Vertice<int>>();
                var path = new List<Vertice<int>>();

                if (BacktrackHamilton(inicio, inicio, visited, path, adj, n, false))
                {
                    sb.AppendLine("Caminho Hamiltoniano encontrado:");
                    Imprimir(path, sb);

                    if (adj[path.Last()].Contains(path.First()))
                        sb.AppendLine("Este caminho forma um ciclo Hamiltoniano.");

                    return;
                }
            }

            sb.AppendLine("Nenhum caminho Hamiltoniano encontrado.");
        }

        private bool BacktrackHamilton(
            Vertice<int> atual,
            Vertice<int> inicio,
            HashSet<Vertice<int>> visitado,
            List<Vertice<int>> caminho,
            Dictionary<Vertice<int>, List<Vertice<int>>> adj,
            int n,
            bool requireCycle)
        {
            visitado.Add(atual);
            caminho.Add(atual);

            if (caminho.Count == n)
            {
                if (!requireCycle) return true;
                return adj[atual].Contains(inicio);
            }

            var vizinhos = adj[atual]
                .Where(v => !visitado.Contains(v))
                .OrderByDescending(v => adj[v].Count)
                .ToList();

            foreach (var v in vizinhos)
            {
                if (BacktrackHamilton(v, inicio, visitado, caminho, adj, n, requireCycle))
                    return true;
            }

            visitado.Remove(atual);
            caminho.RemoveAt(caminho.Count - 1);
            return false;
        }

        private List<Vertice<int>> HeuristicaHamiltoniana(List<Vertice<int>> vertices)
        {
            var adj = new Dictionary<Vertice<int>, List<Vertice<int>>>();
            foreach (var v in vertices)
                adj[v] = _grafo.ObterVizinhos(v)?.ToList() ?? new List<Vertice<int>>();

            foreach (var inicio in vertices)
            {
                var visited = new HashSet<Vertice<int>>();
                var path = new List<Vertice<int>>();
                var atual = inicio;

                visited.Add(atual);
                path.Add(atual);

                while (path.Count < vertices.Count)
                {
                    var next = adj[atual]
                        .Where(v => !visited.Contains(v))
                        .OrderBy(v => adj[v].Count)
                        .FirstOrDefault();

                    if (next == null)
                        break;

                    visited.Add(next);
                    path.Add(next);
                    atual = next;
                }

                if (path.Count == vertices.Count)
                    return path;
            }

            return null;
        }

        // Método auxiliar agora recebe o StringBuilder
        private void Imprimir(List<Vertice<int>> caminho, StringBuilder sb)
        {
            for (int i = 0; i < caminho.Count; i++)
            {
                sb.Append(caminho[i].Dado);
                if (i < caminho.Count - 1) sb.Append(" -> ");
            }
            sb.AppendLine("\n");
        }
    }
}