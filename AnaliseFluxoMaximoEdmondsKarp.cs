using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public void Executar()
        {
            EdmondsKarp();
            ExibirResultado();
        }

        private void EdmondsKarp()
        {
            foreach (Aresta<int> e in _arestas)
            {
                if (!_fluxos.ContainsKey(e))
                {
                    _fluxos.Add(e, 0);
                }
            }

            ConstruirRedeResidual();

            Dictionary<Vertice<int>, Vertice<int>> predecessores;

            List<Aresta<int>> p = CaminhoAumentanteMenosArestas(out predecessores);

            while (p != null && p.Count > 0)
            {
                int delta = CalcularDelta(p);

                if (delta > 0)
                {
                    _fluxoMaximo += delta; 

                    AtualizarRedeResidual(p, delta);
                }

                p = CaminhoAumentanteMenosArestas(out predecessores);
            }
        }

        private int CalcularDelta(List<Aresta<int>> caminhoAumentante)
        {
            int delta = int.MaxValue;

            foreach (Aresta<int> aresta in caminhoAumentante)
            {
                int capacidadeResidual = ObterCapacidadeResidualDaAresta(aresta);

                if (capacidadeResidual < delta)
                {
                    delta = capacidadeResidual;
                }
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

                Aresta<int>? arestaOriginalDireta = _arestas
                    .FirstOrDefault(e => e.Origem.Equals(v) && e.Destino.Equals(w));

                if (arestaOriginalDireta != null)
                {
                    _fluxos[arestaOriginalDireta] += delta;
                }
                else
                {
                    Aresta<int>? arestaOriginalReversa = _arestas
                        .FirstOrDefault(e => e.Origem.Equals(w) && e.Destino.Equals(v));

                    if (arestaOriginalReversa != null)
                    {
                        _fluxos[arestaOriginalReversa] -= delta;
                    }
                }
            }
        }

        private List<Aresta<int>>? CaminhoAumentanteMenosArestas(out Dictionary<Vertice<int>, Vertice<int>> predecessores)
        {
            predecessores = new Dictionary<Vertice<int>, Vertice<int>>();
            Queue<Vertice<int>> fila = new Queue<Vertice<int>>();
            HashSet<Vertice<int>> visitados = new HashSet<Vertice<int>>();
            return null;
        }

        private void ExibirResultado()
        {
            Console.WriteLine("--- Análise de Fluxo Máximo (Edmonds-Karp) ---");
            Console.WriteLine($"Fonte: {_origem.Dado}, Sumidouro: {_destino.Dado}");
            Console.WriteLine($"Fluxo Máximo Total Encontrado: {_fluxoMaximo}");

            Console.WriteLine("\nFluxo por Aresta Original:");
            foreach (var par in _fluxos)
            {
                if (par.Value > 0)
                {
                    Console.WriteLine($"Aresta ({par.Key.Origem.Dado} -> {par.Key.Destino.Dado}): Fluxo = {par.Value}");
                }
            }
        }
    }
}