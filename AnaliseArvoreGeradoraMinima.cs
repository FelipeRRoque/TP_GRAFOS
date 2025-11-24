using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TP_GRAFOS
{
    public class AnaliseArvoreGeradoraMinima : IAnalises
    {
        public void Executar(IGrafo<int> grafo)
        {
            if (grafo is GrafoListaAdjacencia<int> listaAD)
                Prim(listaAD);
            else
                Kruskal(grafo);
        }

        public void Prim(GrafoListaAdjacencia<int> grafo)
        {
            var vertices = grafo.ObterVertices();
            var subgrafo = GrafoUtilitario.CriarSubgrafoSomenteVertices(grafo);

            int r = vertices[0].Dado;

            var conjuntoVerticesAdicionados = new HashSet<int> { r };
            var conjuntoArestasAdicionadas = new List<Aresta<int>>();

            while (conjuntoVerticesAdicionados.Count < vertices.Count)
            {
                Aresta<int> menorAresta = null;

                foreach (var verticeAtual in conjuntoVerticesAdicionados)
                {
                    var vizinhos = grafo.ObterVizinhos(verticeAtual);

                    foreach (var (verticeDestino, peso) in vizinhos)
                    {
                        if(!conjuntoVerticesAdicionados.Contains(verticeDestino))
                        {
                            if (menorAresta == null || peso < menorAresta.Peso)
                            {
                                menorAresta = new Aresta<int>(new Vertice<int>(verticeAtual), new Vertice<int>(verticeDestino), peso);
                            }
                        }
                    }
                }
                conjuntoVerticesAdicionados.Add(menorAresta.Destino.Dado);

                conjuntoArestasAdicionadas.Add(menorAresta);
                subgrafo.AdicionarAresta(menorAresta.Origem.Dado, menorAresta.Destino.Dado, menorAresta.Peso, menorAresta.Capacidade);
            }
        }

        public void Kruskal(IGrafo<int> grafo)
        {
            // Implementação futura
        }
    }
}