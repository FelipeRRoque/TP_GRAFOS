using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TP_GRAFOS
{
    public static class GrafoUtilitario
    {
        public static IGrafo<T> CriarSubgrafoSomenteVertices<T>(IGrafo<T> grafoOriginal)
        {
            IGrafo<T> subgrafo;

            if (grafoOriginal is GrafoListaAdjacencia<T>)
                subgrafo = new GrafoListaAdjacencia<T>();
            else if (grafoOriginal is GrafoMatrizAdjacencia<T>)
                subgrafo = new GrafoMatrizAdjacencia<T>(grafoOriginal.ObterVertices().Count);
            else
                throw new Exception("Tipo de representacao de grafo desconhecido.");

            foreach (var v in grafoOriginal.ObterVertices())
                subgrafo.AdicionarVertice(v.Dado);

            return subgrafo;
        }
        public static IGrafo<string> GerarGrafoDeConflitos<T>(IGrafo<T> grafoOriginal)
        {
            var arestasOriginais = grafoOriginal.ObterArestas();
            var grafoConflitos = new GrafoMatrizAdjacencia<string>(arestasOriginais.Count);

            foreach (var arestas in arestasOriginais)
            {
                string rota = $"{arestas.Origem.Dado}-{arestas.Destino.Dado}";
                grafoConflitos.AdicionarVertice(rota);
            }
            var verticesConflitos = grafoConflitos.ObterVertices();

            for (int i = 0; i < arestasOriginais.Count; i++)
            {
                for (int j = i + 1; j < arestasOriginais.Count; j++)
                {
                    var arestaA = arestasOriginais[i];
                    var arestaB = arestasOriginais[j];

                    bool haConflito =
                        arestaA.Origem.Equals(arestaB.Origem) ||
                        arestaA.Origem.Equals(arestaB.Destino) ||
                        arestaA.Destino.Equals(arestaB.Origem) ||
                        arestaA.Destino.Equals(arestaB.Destino);

                    if (haConflito)
                    {
                        string rotaA = $"{arestaA.Origem.Dado}-{arestaA.Destino.Dado}";
                        string rotaB = $"{arestaB.Origem.Dado}-{arestaB.Destino.Dado}";

                        grafoConflitos.AdicionarAresta(rotaA, rotaB);
                        grafoConflitos.AdicionarAresta(rotaB, rotaA);
                    }
                }
            }
            return grafoConflitos;
        }
    }
}