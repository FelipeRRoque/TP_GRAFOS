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
    }
}