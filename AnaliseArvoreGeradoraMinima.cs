using System.Text;

namespace TP_GRAFOS
{
    /// <summary>
    ///
    /// Esta classe realiza a análise de Árvore Geradora Mínima (AGM) sobre um grafo,
    /// escolhendo automaticamente entre os algoritmos de **Prim** ou **Kruskal**,
    /// dependendo da estrutura de armazenamento do grafo fornecido.
    ///
    /// 1) Fluxo básico da execução:
    /// - O método <see cref="Executar"/> inicia o processo e identifica qual algoritmo usar:
    ///     • Se o grafo for baseado em lista de adjacência, utiliza o algoritmo **Prim**.
    ///     • Caso contrário, utiliza o algoritmo **Kruskal**.
    /// - Após o cálculo, o resultado da AGM é formatado pelo método <see cref="ExibirAGM"/>.
    ///
    /// 2) Implementações internas:
    /// - <see cref="Prim(GrafoListaAdjacencia{int})"/>:
    ///     • Seleciona um vértice inicial e cresce a árvore escolhendo sempre a aresta
    ///       de menor peso que conecta a árvore parcial a um novo vértice.
    ///     • Garante que nenhum ciclo é criado adicionando somente vértices ainda não visitados.
    ///     • Continua até incluir todos os vértices do grafo.
    ///
    /// - <see cref="Kruskal(IGrafo{int})"/>:
    ///     • Ordena todas as arestas por peso crescente.
    ///     • Utiliza união de componentes (union-find simplificado) para garantir que
    ///       nenhuma aresta forme ciclo.
    ///     • Adiciona arestas até obter uma árvore com (n − 1) arestas.
    ///
    /// 3) Finalização:
    /// - O método <see cref="ExibirAGM{T}(List{Aresta{T}})"/> monta e retorna uma string
    ///   contendo todas as arestas escolhidas e o peso total da AGM.
    ///   
    /// </summary>

    public class AnaliseArvoreGeradoraMinima : IAnalises
    {
        private readonly IGrafo<int> _grafo;
        private List<Aresta<int>>? _resultadoAGM;

        public AnaliseArvoreGeradoraMinima(IGrafo<int> grafo)
        {
            _grafo = grafo;
        }

        /// <summary>
        /// Agora retorna uma string contendo tudo que seria impresso no console.
        /// </summary>
        public string Executar()
        {
            var sb = new StringBuilder();

            if (_grafo is GrafoListaAdjacencia<int> listaAD)
            {
                sb.AppendLine("\nExecutando Algoritmo de Prim...");
                _resultadoAGM = Prim(listaAD);
            }
            else
            {
                sb.AppendLine("\nExecutando Algoritmo de Kruskal...");
                _resultadoAGM = Kruskal(_grafo);
            }

            sb.Append(ExibirAGM(_resultadoAGM));
            return sb.ToString();
        }


        public List<Aresta<int>> Prim(GrafoListaAdjacencia<int> grafo)
        {
            var vertices = grafo.ObterVertices();
            var subgrafo = GrafoUtilitario.CriarSubgrafoSomenteVertices(grafo);

            Vertice<int> r = vertices[0];

            var conjuntoVerticesAdicionados = new HashSet<Vertice<int>> { r };
            var conjuntoArestasAdicionadas = new List<Aresta<int>>();

            while (conjuntoVerticesAdicionados.Count < vertices.Count)
            {
                Aresta<int>? menorAresta = null;

                foreach (Vertice<int> vAtual in conjuntoVerticesAdicionados)
                {
                    List<Vertice<int>> vizinhos = grafo.ObterVizinhos(vAtual);

                    foreach (Vertice<int> vDestino in vizinhos)
                    {
                        if (!conjuntoVerticesAdicionados.Contains(vDestino))
                        {
                            int peso = grafo.ObterPeso(vAtual, vDestino);
                            int capacidade = grafo.ObterCapacidade(vAtual, vDestino);

                            if (peso == int.MaxValue)
                                continue;

                            if (menorAresta == null || peso < menorAresta.Peso)
                            {
                                menorAresta = new Aresta<int>(vAtual, vDestino, peso, capacidade);
                            }
                        }
                    }
                }

                if (menorAresta == null)
                    throw new InvalidOperationException("Grafo não é conexo. Prim não pode continuar.");

                conjuntoVerticesAdicionados.Add(menorAresta.Destino);

                conjuntoArestasAdicionadas.Add(menorAresta);

                subgrafo.AdicionarAresta(
                    menorAresta.Origem.Dado,
                    menorAresta.Destino.Dado,
                    menorAresta.Peso,
                    menorAresta.Capacidade
                );
            }

            return conjuntoArestasAdicionadas;
        }

        public List<Aresta<int>> Kruskal(IGrafo<int> grafo)
        {
            var arestas = grafo.ObterArestas();
            arestas.Sort((a, b) => a.Peso.CompareTo(b.Peso));

            var vertices = grafo.ObterVertices();
            var subgrafo = GrafoUtilitario.CriarSubgrafoSomenteVertices(grafo);
            var juncoesDosVertices = new Dictionary<int, int>();

            foreach (var v in vertices)
                juncoesDosVertices[v.Dado] = v.Dado;

            var conjuntoArestasAGM = new List<Aresta<int>>();

            foreach (var aresta in arestas)
            {
                int origemJuncao = juncoesDosVertices[aresta.Origem.Dado];
                int destinoJuncao = juncoesDosVertices[aresta.Destino.Dado];

                if (origemJuncao != destinoJuncao)
                {
                    conjuntoArestasAGM.Add(aresta);
                    subgrafo.AdicionarAresta(
                        aresta.Origem.Dado,
                        aresta.Destino.Dado,
                        aresta.Peso,
                        aresta.Capacidade
                    );

                    foreach (var v in juncoesDosVertices.Keys.ToList())
                        if (juncoesDosVertices[v] == destinoJuncao)
                            juncoesDosVertices[v] = origemJuncao;
                }

                if (conjuntoArestasAGM.Count == vertices.Count - 1)
                    break;
            }

            return conjuntoArestasAGM;
        }

        /// <summary>
        /// Agora retorna a string com o conteúdo, ao invés de imprimir no console.
        /// </summary>
        public string ExibirAGM<T>(List<Aresta<T>> arestas)
        {
            var sb = new StringBuilder();
            sb.AppendLine("\n--- Árvore Geradora Mínima (AGM) ---");
            int pesoTotal = 0;

            foreach (var aresta in arestas)
            {
                sb.AppendLine($"Origem: {aresta.Origem.Dado} -> Destino: {aresta.Destino.Dado} | Peso: {aresta.Peso} | Capacidade: {aresta.Capacidade}");
                pesoTotal += aresta.Peso;
            }

            sb.AppendLine($"Peso Total da AGM: {pesoTotal}");
            sb.AppendLine("-------------------------------------\n");

            return sb.ToString();
        }
    }
}
