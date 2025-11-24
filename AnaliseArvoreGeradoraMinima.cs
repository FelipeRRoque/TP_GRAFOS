using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace TP_GRAFOS
{
    public class AnaliseArvoreGeradoraMinima : IAnalises
    {
        private List<Aresta<int>>? _resultadoAGM;
        public List<Aresta<int>>? ResultadoAGM => _resultadoAGM;

        public void Executar(IGrafo<int> grafo)
        {
            if (grafo is GrafoListaAdjacencia<int> listaAD)
                _resultadoAGM = Prim(listaAD);
            else
                _resultadoAGM = Kruskal(grafo);
        }

        public List<Aresta<int>> Prim(GrafoListaAdjacencia<int> grafo)
        {
            var vertices = grafo.ObterVertices();
            var subgrafo = GrafoUtilitario.CriarSubgrafoSomenteVertices(grafo);

            int r = vertices[0].Dado;

            var conjuntoVerticesAdicionados = new HashSet<int> { r }; //não deixa repetir vertices
            var conjuntoArestasAdicionadas = new List<Aresta<int>>();

            while (conjuntoVerticesAdicionados.Count < vertices.Count)
            {
                Aresta<int>? menorAresta = null;

                foreach (var verticeAtual in conjuntoVerticesAdicionados)
                {
                    var vizinhos = grafo.ObterVizinhos(verticeAtual);

                    foreach (var (verticeDestino, peso, capacidade) in vizinhos)
                    {
                        if (!conjuntoVerticesAdicionados.Contains(verticeDestino))
                        {
                            if (menorAresta == null || peso < menorAresta.Peso)
                            {
                                menorAresta = new Aresta<int>(new Vertice<int>(verticeAtual), new Vertice<int>(verticeDestino), peso, capacidade);
                            }
                        }
                    }
                }
                if (menorAresta == null)
                    throw new InvalidOperationException("Grafo não é conexo. Prim não pode continuar.");
                
                conjuntoVerticesAdicionados.Add(menorAresta.Destino.Dado);

                conjuntoArestasAdicionadas.Add(menorAresta);
                subgrafo.AdicionarAresta(menorAresta.Origem.Dado, menorAresta.Destino.Dado, menorAresta.Peso, menorAresta.Capacidade);
            }
            return conjuntoArestasAdicionadas;
        }

        public List<Aresta<int>> Kruskal(IGrafo<int> grafo)
        {
            /*  var arestasOrdenadas = grafo.ObterArestas();
            arestasOrdenadas.Sort((a, b) => a.Peso.CompareTo(b.Peso));

            var vertices = grafo.ObterVertices();

            var subgrafo = GrafoUtilitario.CriarSubgrafoSomenteVertices(grafo);

            foreach (var vertice in ListaVertices)
            {

            }*/
            return new List<Aresta<int>>();
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