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
            var grafo = Arquivo.LerDados("dataGrafos/grafo01.dimacs");
            grafo.ExibirGrafo();

            var analise = new AnaliseArvoreGeradoraMinima(grafo);
            analise.Executar();

            IGrafo<string> grafoConflitos = GrafoUtilitario.GerarGrafoDeConflitos(grafo);
            Console.WriteLine($"   -> Grafo de conflitos gerado com {grafoConflitos.ObterVertices().Count} tarefas de manutenção.\n");
            var analiseWP = new AnaliseMetodoWelshPowell(grafoConflitos);
            analiseWP.Executar();
        }
    }
}