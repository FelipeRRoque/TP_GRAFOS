namespace TP_GRAFOS
{

    /// <summary>
    /// Classe responsável por analisar a existência de caminho ou ciclo Euleriano em um grafo direcionado.
    /// 
    /// O processo segue quatro etapas principais:
    /// 
    /// 1) Verificação das condições matemáticas de existência  
    ///    - Realizada em <see cref="VerificarEuleriano"/>  
    ///    - Calcula o grau de entrada e saída de cada vértice  
    ///    - Determina se o grafo atende às regras para ciclo (inDegree = outDegree)  
    ///      ou para caminho Euleriano (apenas 1 vértice com out−in = 1 e outro com in−out = 1)
    ///
    /// 2) Verificação da conectividade ignorando direção  
    ///    - Realizada em <see cref="ConectadoIgnorandoDirecao"/>  
    ///    - O algoritmo monta um grafo não-direcionado auxiliar e usa BFS  
    ///      para garantir que todos os vértices com grau > 0 pertencem à mesma componente.
    ///
    /// 3) Construção do percurso Euleriano (Hierholzer)  
    ///    - Realizada em <see cref="ConstruirEuleriano"/>  
    ///    - O algoritmo percorre cada aresta exatamente uma vez, utilizando pilha e recuo controlado.
    /// 
    /// 4) Impressão do resultado  
    ///    - Executada dentro de <see cref="VerificarEuleriano"/>  
    ///    - Mostra se o grafo possui CAMINHO ou CICLO Euleriano e imprime o percurso final.
    /// </summary>

    public class AnalisarCaminhoEuleriano : IAnalises
    {
        private readonly IGrafo<int> _grafo;

        public AnalisarCaminhoEuleriano(IGrafo<int> grafo)
        {
            _grafo = grafo;
        }

        public void Executar()
        {
            Console.WriteLine("=== Caminho / Ciclo Euleriano ===\n");
            VerificarEuleriano();
        }

        private void VerificarEuleriano()
        {
            var vertices = _grafo.ObterVertices();
            if (vertices == null || vertices.Count == 0)
            {
                Console.WriteLine("Grafo vazio. Não há caminho nem ciclo Euleriano.");
                return;
            }

            var outDegree = new Dictionary<Vertice<int>, int>();
            var inDegree = new Dictionary<Vertice<int>, int>();

            foreach (var v in vertices)
            {
                outDegree[v] = 0;
                inDegree[v] = 0;
            }

            foreach (var u in vertices)
            {
                var vizinhos = _grafo.ObterVizinhos(u) ?? new List<Vertice<int>>();
                outDegree[u] = vizinhos.Count;

                foreach (var v in vizinhos)
                {
                    if (!inDegree.ContainsKey(v)) inDegree[v] = 0;
                    inDegree[v]++;
                }
            }

            foreach (var v in vertices)
            {
                if (!outDegree.ContainsKey(v)) outDegree[v] = 0;
                if (!inDegree.ContainsKey(v)) inDegree[v] = 0;
            }

            int startCandidates = 0, endCandidates = 0;
            Vertice<int> startVertex = null;

            foreach (var v in vertices)
            {
                int indeg = inDegree[v];
                int outdeg = outDegree[v];

                if (outdeg - indeg == 1)
                {
                    startCandidates++;
                    startVertex = v;
                }
                else if (indeg - outdeg == 1)
                {
                    endCandidates++;
                }
                else if (indeg != outdeg)
                {
                    startCandidates = 9999;
                    break;
                }
            }

            bool hasEulerCycle = (startCandidates == 0 && endCandidates == 0);
            bool hasEulerPath = (startCandidates == 1 && endCandidates == 1);

            var anyNonIsolated = vertices
                .FirstOrDefault(v => inDegree[v] + outDegree[v] > 0);

            if (anyNonIsolated == null)
            {
                Console.WriteLine("Grafo sem arestas. Há ciclo Euleriano trivial.");
                return;
            }

            if (!ConectadoIgnorandoDirecao(anyNonIsolated, vertices, inDegree, outDegree))
            {
                Console.WriteLine("Grafo não é conexo quanto às arestas. Não existe caminho/ciclo Euleriano.");
                return;
            }

            if (!hasEulerCycle && !hasEulerPath)
            {
                Console.WriteLine("Não existe caminho nem ciclo Euleriano.");
                return;
            }

            Vertice<int> inicio = startVertex ?? anyNonIsolated;
            var caminho = ConstruirEuleriano(inicio);

            Console.WriteLine(hasEulerCycle ? "Existe CICLO Euleriano." : "Existe CAMINHO Euleriano.");
            Console.WriteLine("Percurso Euleriano:");
            Console.Write("  ");
            for (int i = 0; i < caminho.Count; i++)
            {
                Console.Write(caminho[i].Dado);
                if (i < caminho.Count - 1) Console.Write(" -> ");
            }
            Console.WriteLine("\n");
        }

        private bool ConectadoIgnorandoDirecao(
            Vertice<int> start,
            List<Vertice<int>> vertices,
            Dictionary<Vertice<int>, int> inD,
            Dictionary<Vertice<int>, int> outD)
        {
            var adj = new Dictionary<Vertice<int>, List<Vertice<int>>>();
            foreach (var v in vertices) adj[v] = new List<Vertice<int>>();

            foreach (var a in _grafo.ObterArestas())
            {
                adj[a.Origem].Add(a.Destino);
                adj[a.Destino].Add(a.Origem);
            }

            var visitado = new HashSet<Vertice<int>>();
            var fila = new Queue<Vertice<int>>();
            fila.Enqueue(start);
            visitado.Add(start);

            while (fila.Count > 0)
            {
                var u = fila.Dequeue();
                foreach (var v in adj[u])
                {
                    if (!visitado.Contains(v))
                    {
                        visitado.Add(v);
                        fila.Enqueue(v);
                    }
                }
            }

            foreach (var v in vertices)
            {
                if (inD[v] + outD[v] > 0 && !visitado.Contains(v))
                    return false;
            }
            return true;
        }

        private List<Vertice<int>> ConstruirEuleriano(Vertice<int> inicio)
        {
            var adj = new Dictionary<Vertice<int>, Stack<Vertice<int>>>();
            var vertices = _grafo.ObterVertices();

            foreach (var v in vertices)
                adj[v] = new Stack<Vertice<int>>();

            var mapTemp = new Dictionary<Vertice<int>, List<Vertice<int>>>();
            foreach (var v in vertices) mapTemp[v] = new List<Vertice<int>>();

            foreach (var a in _grafo.ObterArestas())
                mapTemp[a.Origem].Add(a.Destino);

            foreach (var kv in mapTemp)
            {
                var destinos = kv.Value;
                for (int i = destinos.Count - 1; i >= 0; i--)
                    adj[kv.Key].Push(destinos[i]);
            }

            var stack = new Stack<Vertice<int>>();
            var caminho = new List<Vertice<int>>();
            var atual = inicio;

            while (true)
            {
                if (adj[atual].Count > 0)
                {
                    stack.Push(atual);
                    atual = adj[atual].Pop();
                }
                else
                {
                    caminho.Add(atual);
                    if (stack.Count == 0) break;
                    atual = stack.Pop();
                }
            }

            caminho.Reverse();
            return caminho;
        }
    }
}
