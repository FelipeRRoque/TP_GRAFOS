using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Arquivo;
using ClassificaGrafo;



namespace TP_GRAFOS
{
    public class Program
    {
        static void Main(string[] args)
        {
            var grafo = Arquivo.LerDados("dataGrafos/grafo01.dimacs");
            var grafo = ClassificaGrafo.CriarGrafo<int>(dados.Vertices, dados.Arestas);

            // 3. Preencher os vértices
            for (int i = 1; i <= dados.Vertices; i++)
                grafo.AdicionarVertice(i);

            // 4. Preencher as arestas
            foreach (var a in dados.ListaArestas)
                grafo.AdicionarAresta(a.origem, a.destino, a.peso, a.capacidade);

            grafo.ExibirGrafo();
        }
    }
}