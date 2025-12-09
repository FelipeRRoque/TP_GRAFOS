using System.Text;

namespace TP_GRAFOS
{

    /// <summary>
    ///
    /// Esta classe implementa o algoritmo de **Edmonds–Karp**, uma versão do método
    /// de Ford–Fulkerson que utiliza **busca em largura (BFS)** para encontrar caminhos
    /// aumentantes na rede residual, permitindo o cálculo do **fluxo máximo** entre um
    /// vértice de origem e um vértice destino.
    ///
    /// 1) Fluxo geral da análise:
    /// - O método <see cref="Executar"/> inicia o processo chamando
    ///   <see cref="EdmondsKarp"/>, que controla toda a lógica de expansão do fluxo.
    /// - Ao finalizar, <see cref="ExibirResultado"/> monta e retorna uma string com:
    ///     • fluxo máximo total encontrado;  
    ///     • fluxos efetivamente utilizados em cada aresta;  
    ///     • detalhes da configuração da rede.
    ///
    /// 2) Estrutura interna do algoritmo:
    /// - <see cref="EdmondsKarp"/>:
    ///     • Inicializa o dicionário de fluxos e constrói a rede residual por meio
    ///       de <see cref="ConstruirRedeResidual"/>;  
    ///     • Repetidamente encontra caminhos aumentantes usando
    ///       <see cref="CaminhoAumentanteMenosArestas"/>, que emprega BFS para localizar
    ///       caminhos válidos com capacidade residual positiva;  
    ///     • Calcula o aumento possível no fluxo (delta) com
    ///       <see cref="CalcularDelta"/>;  
    ///     • Atualiza a rede residual e os fluxos reais via
    ///       <see cref="AtualizarRedeResidual"/>;  
    ///     • Repete até não existirem mais caminhos aumentantes.
    ///
    /// 3) Rede residual e regras de fluxo:
    /// - A rede residual é construída e mantida através de
    ///   <see cref="ConstruirRedeResidual"/> e atualizada por
    ///   <see cref="AtualizarRedeResidual"/>.  
    /// - Para cada aresta (u → v):  
    ///     - capacidade residual forward diminui conforme o fluxo cresce;  
    ///     - capacidade residual reverse aumenta, permitindo desfazer fluxos.
    ///
    /// 3) BFS para encontrar caminhos aumentantes:
    /// - <see cref="CaminhoAumentanteMenosArestas"/> usa BFS (via
    ///   <c>BuscaEmLargura.EncontrarCaminho</c>) para garantir sempre o menor número de arestas,
    ///   resultando em maior eficiência e garantindo complexidade O(V·E²).
    ///
    /// 4) Finalização:
    /// - O método <see cref="ExibirResultado"/> retorna o relatório completo,
    ///   detalhando o fluxo máximo e listando todas as arestas que efetivamente
    ///   transportaram fluxo.
    ///   
    /// </summary>

    internal class AnaliseFluxoMaximoEdmondsKarp : IAnalises
    {
        private IGrafo<int> _grafo;
        private List<Aresta<int>> _arestas;
        private Dictionary<Aresta<int>, int> _fluxos = new Dictionary<Aresta<int>, int>();
        private Dictionary<(Vertice<int>, Vertice<int>), int> _capacidadeResidual = new Dictionary<(Vertice<int>, Vertice<int>), int>();
        private int _fluxoMaximo = 0;
        private Vertice<int> _origem;
        private Vertice<int> _destino;

        public AnaliseFluxoMaximoEdmondsKarp(IGrafo<int> grafo, Vertice<int> origem, Vertice<int> destino)
        {
            _grafo = grafo;
            _arestas = grafo.ObterArestas();
            _origem = origem;
            _destino = destino;
        }

        /// <summary>
        /// Agora retorna uma string com todo o conteúdo textual gerado pela análise.
        /// </summary>
        public string Executar()
        {
            EdmondsKarp();
            return ExibirResultado();
        }

        private void EdmondsKarp()
        {
            foreach (Aresta<int> e in _arestas)
            {
                if (!_fluxos.ContainsKey(e))
                    _fluxos.Add(e, 0);
            }

            ConstruirRedeResidual();

            List<Aresta<int>>? p = CaminhoAumentanteMenosArestas();

            int delta = (p != null && p.Count > 0) ? CalcularDelta(p) : 0;

            while (p != null && p.Count > 0 && delta > 0)
            {
                _fluxoMaximo += delta;

                AtualizarRedeResidual(p, delta);

                p = CaminhoAumentanteMenosArestas();

                delta = (p != null && p.Count > 0) ? CalcularDelta(p) : 0;
            }
        }

        private List<Aresta<int>>? CaminhoAumentanteMenosArestas()
        {
            Func<Vertice<int>, Vertice<int>, bool> regraDeFluxo = (u, v) =>
            {
                if (_capacidadeResidual.TryGetValue((u, v), out int capacidade))
                {
                    return capacidade > 0;
                }
                return false;
            };

            return BuscaEmLargura.EncontrarCaminho(_grafo, _origem, _destino, regraDeFluxo);
        }

        private int CalcularDelta(List<Aresta<int>> caminhoAumentante)
        {
            int delta = int.MaxValue;
            foreach (Aresta<int> aresta in caminhoAumentante)
            {
                int capacidade = ObterCapacidadeResidualDaAresta(aresta);
                if (capacidade < delta) delta = capacidade;
            }
            return delta;
        }

        private int ObterCapacidadeResidualDaAresta(Aresta<int> aresta)
        {
            if (_capacidadeResidual.TryGetValue((aresta.Origem, aresta.Destino), out int capacidade))
            {
                return capacidade;
            }
            return 0;
        }

        private void ConstruirRedeResidual()
        {
            _capacidadeResidual.Clear();
            foreach (Aresta<int> e in _arestas)
            {
                _capacidadeResidual[(e.Origem, e.Destino)] = e.Capacidade;

                if (!_capacidadeResidual.ContainsKey((e.Destino, e.Origem)))
                {
                    _capacidadeResidual.Add((e.Destino, e.Origem), 0);
                }
            }
        }

        private void AtualizarRedeResidual(List<Aresta<int>> caminho, int delta)
        {
            foreach (Aresta<int> arestaResidual in caminho)
            {
                Vertice<int> v = arestaResidual.Origem;
                Vertice<int> w = arestaResidual.Destino;

                _capacidadeResidual[(v, w)] -= delta;
                _capacidadeResidual[(w, v)] += delta;

                Aresta<int>? originalDireta = _arestas.FirstOrDefault(e => e.Origem.Equals(v) && e.Destino.Equals(w));
                if (originalDireta != null)
                {
                    _fluxos[originalDireta] += delta;
                }
                else
                {
                    Aresta<int>? originalReversa = _arestas.FirstOrDefault(e => e.Origem.Equals(w) && e.Destino.Equals(v));
                    if (originalReversa != null)
                    {
                        _fluxos[originalReversa] -= delta;
                    }
                }
            }
        }

        /// <summary>
        /// Retorna a string contendo tudo que antes era exibido no console.
        /// </summary>
        private string ExibirResultado()
        {
            var sb = new StringBuilder();

            sb.AppendLine("--- Análise de Fluxo Máximo (Edmonds-Karp) ---");
            sb.AppendLine($"Fonte: {_origem.Dado}, Sumidouro: {_destino.Dado}");
            sb.AppendLine($"Fluxo Máximo Total Encontrado: {_fluxoMaximo}");
            sb.AppendLine();
            sb.AppendLine("Fluxo por Aresta Original:");

            foreach (var par in _fluxos)
            {
                if (par.Value > 0)
                {
                    sb.AppendLine($"Aresta ({par.Key.Origem.Dado} -> {par.Key.Destino.Dado}): Fluxo = {par.Value}");
                }
            }

            sb.AppendLine();

            return sb.ToString();
        }
    }
}
