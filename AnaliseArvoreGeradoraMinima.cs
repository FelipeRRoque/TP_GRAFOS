using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace TP_GRAFOS
{
    public class AnaliseArvoreGeradoraMinima : IAnalises
    {
        private readonly IGrafo<int> _grafo;
        private List<Aresta<int>>? _resultadoAGM;

        public AnaliseArvoreGeradoraMinima(IGrafo<int> grafo)
        {
            _grafo = grafo;
        }

        public void Executar()
        {
            if (_grafo is GrafoListaAdjacencia<int> listaAD)
            {
                Console.WriteLine("\nExecutando Algoritmo de Prim...");
                _resultadoAGM = Prim(listaAD);
            }
            else
            {
                Console.WriteLine("\nExecutando Algoritmo de Kruskal...");
                _resultadoAGM = Kruskal(_grafo);
            }
        }


        public List<Aresta<int>> Prim(GrafoListaAdjacencia<int> grafo)
        {
            var vertices = grafo.ObterVertices();
            var subgrafo = GrafoUtilitario.CriarSubgrafoSomenteVertices(grafo);

            Vertice<int> r = vertices[0];

            var conjuntoVerticesAdicionados = new HashSet<Vertice<int>> { r }; //não deixa repetir vertices
            var conjuntoArestasAdicionadas = new List<Aresta<int>>();

            //**** Mudei as condicionais dos loops para estarem de acordo com a nova regra do EncontrarVizinhos da interface (Júlio) ****
            while (conjuntoVerticesAdicionados.Count < vertices.Count)
            {
                Aresta<int>? menorAresta = null;

                foreach (Vertice<int> vAtual in conjuntoVerticesAdicionados)
                {
                    List<Vertice<int>> vizinhos = grafo.ObterVizinhos(vAtual);

                    foreach (Vertice<int> vDestino in vizinhos)
                    {

                        bool destinoJaFoiAdicionado = conjuntoVerticesAdicionados.Contains(vDestino);
                        if (destinoJaFoiAdicionado == false)
                        {
                            int peso = grafo.ObterPeso(vAtual, vDestino);
                            int capacidade = grafo.ObterCapacidade(vAtual, vDestino);

                            if (peso == int.MaxValue) // caso não exista aresta
                                continue;

                            if (menorAresta == null || peso < menorAresta.Peso)
                            {
                                menorAresta = new Aresta<int>(vAtual, vDestino, peso, capacidade);
                            }
                        }
                    }

                    if (menorAresta == null)
                        throw new InvalidOperationException("Grafo não é conexo. Prim não pode continuar.");

                    conjuntoVerticesAdicionados.Add(menorAresta.Destino);

                    conjuntoArestasAdicionadas.Add(menorAresta);

                    subgrafo.AdicionarAresta(menorAresta.Origem.Dado, menorAresta.Destino.Dado, menorAresta.Peso, menorAresta.Capacidade);
                }
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
            {
                juncoesDosVertices[v.Dado] = v.Dado;
            }

            var conjuntoArestasAGM = new List<Aresta<int>>();

            foreach (var aresta in arestas)
            {
                int origemJuncao = juncoesDosVertices[aresta.Origem.Dado];
                int destinoJuncao = juncoesDosVertices[aresta.Destino.Dado];

                if (origemJuncao != destinoJuncao)
                {
                    conjuntoArestasAGM.Add(aresta);
                    subgrafo.AdicionarAresta(aresta.Origem.Dado, aresta.Destino.Dado, aresta.Peso, aresta.Capacidade);

                    foreach (var v in juncoesDosVertices.Keys.ToList())
                    {
                        if (juncoesDosVertices[v] == destinoJuncao)
                        {
                            juncoesDosVertices[v] = origemJuncao;
                        }
                    }
                }
                if (conjuntoArestasAGM.Count == vertices.Count - 1)
                    break;
            }
            return conjuntoArestasAGM;
        }

        public void ExibirAGM<T>(List<Aresta<T>> arestas)
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

            Console.WriteLine(sb.ToString());
        }
    }
}