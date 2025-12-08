using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TP_GRAFOS
{
    public class Program
    {
        static void Main(string[] args)
        {
            var grafo = Arquivo.LerDados("dataGrafos/grafo07.dimacs");

            grafo.ExibirGrafo();
            
            var analise = new AnaliseArvoreGeradoraMinima(grafo);
            analise.Executar();
        }
    }
}