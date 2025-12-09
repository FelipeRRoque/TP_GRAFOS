using System.Text;

namespace TP_GRAFOS
{
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
