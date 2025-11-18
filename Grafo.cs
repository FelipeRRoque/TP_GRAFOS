using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TP_GRAFOS
{
    public class Grafo<T>
    {
        private readonly Dictionary<Vertice<T>, List<Aresta<T>>> _listaAdjacencia;

        public Grafo()
        {
            _listaAdjacencia = new Dictionary<Vertice<T>, List<Aresta<T>>>();
        }

        public void AdcionarVertice(T dado)
        {
            var novoVertice = new Vertice<T>(dado);
            if (!_listaAdjacencia.ContainsKey(novoVertice))
            {
                _listaAdjacencia.Add(novoVertice, new List<Aresta<T>>());
            }
        }

        public void AdcionarAresta(T dadoOrigem, T dadoDestino, int peso = 1)
        {
            var verticeOrigem = EncontrarVertice(dadoOrigem);
            var verticeDestino = EncontrarVertice(dadoDestino);

            if (verticeOrigem != null && verticeDestino != null)
            {
                _listaAdjacencia[verticeOrigem].Add(new Aresta<T>(verticeDestino, peso));
                _listaAdjacencia[verticeDestino].Add(new Aresta<T>(verticeOrigem, peso));
            }
        }

        private Vertice<T> EncontrarVertice(T dado)
        {
            foreach (var vertice in _listaAdjacencia.Keys)
            {
                if (vertice.Dado.Equals(dado))
                {
                    return vertice;
                }
            }
            return null;
        }

        public void ExibirGrafo()
        {
            foreach (var vertice in _listaAdjacencia.Keys)
            {
                Console.Write($"Vértice {vertice.Dado}: ");
                foreach (var aresta in _listaAdjacencia[vertice])
                {
                    Console.Write($"-> {aresta.Destino.Dado} (Peso: {aresta.Peso}) ");
                }
                Console.WriteLine();
            }
        }
    }
}