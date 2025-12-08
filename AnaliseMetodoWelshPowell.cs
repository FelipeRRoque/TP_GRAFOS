using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;

namespace TP_GRAFOS
{
    /// <summary>
    /// Implementa o algoritmo de coloração de grafos Welsh-Powell, utilizado para 
    /// minimizar o número de cores necessárias para colorir um grafo sem que 
    /// vértices adjacentes compartilhem a mesma cor.
    /// Este método é amplamente aplicado em problemas de alocação, escalonamento,
    /// logística e organização de recursos que não podem ocorrer simultaneamente.
    /// </summary>
    public class AnaliseMetodoWelshPowell : IAnalises
    {
        /// <summary>
        /// Grafo fornecido para análise e coloração.
        /// </summary>
        private readonly IGrafo<string> _grafo;

        private Dictionary<Vertice<string>, string> colorir;

        /// <summary>
        /// Inicializa a análise Welsh-Powell com um grafo específico.
        /// </summary>
        /// <param name="grafo">
        /// Estrutura de grafo que será utilizada para execução do algoritmo.
        /// O grafo deve permitir a obtenção dos graus dos vértices e seus vizinhos.
        /// </param>
        public AnaliseMetodoWelshPowell(IGrafo<string> grafo)
        {
            _grafo = grafo;
            colorir = new Dictionary<Vertice<string>, string>();
        }

        /// <summary>
        /// Executa o algoritmo de Welsh-Powell realizando a coloração dos vértices do grafo.
        /// O processo consiste em:
        /// 1. Ordenar os vértices em ordem decrescente de grau.
        /// 2. Atribuir cores de forma gulosa (greedy), garantindo que
        ///    nenhum vértice receba a mesma cor de seus vizinhos.
        /// 3. Exibir no console o resultado final da coloração.
        /// 
        /// Este método também é útil em:
        /// - Planejamento de horários (timetabling);
        /// - Alocação de canais em redes sem fio;
        /// - Organização de tarefas que não podem ocorrer simultaneamente;
        /// - Segmentação e clusterização em análise operacional.
        /// </summary>
        public void Executar()
        {
            WelshPowell();
            ExibirResultado();
        }

        public void WelshPowell()
        {
            var listaGrausDecrescente = _grafo.ObterGraus()
                .OrderByDescending(v => v.Item2).Select(v => v.Item1).ToList();

            colorir = new Dictionary<Vertice<string>, string>();

            int turno = 1;

            while (colorir.Count < listaGrausDecrescente.Count)
            {
                string turnoAtual = $"Turno {turno}";

                foreach (var vertice in listaGrausDecrescente)
                {
                    if (colorir.ContainsKey(vertice))
                        continue;

                    bool temConflitoDeTurno = false;

                    foreach (var vizinho in _grafo.ObterVizinhos(vertice))
                    {
                        if (colorir.ContainsKey(vizinho) && colorir[vizinho] == turnoAtual)
                        {
                            temConflitoDeTurno = true;
                            break;
                        }
                    }

                    if (!temConflitoDeTurno)
                        colorir[vertice] = turnoAtual;
                }
                turno++;
            }
        }
        public void ExibirResultado()
        {
            var sb = new StringBuilder();
            sb.AppendLine("\n--- Método de Welsh-Powell ---");
            sb.AppendLine("\n--- Agendamentos de Manutenções por turnos: ---");

            var agrupadoPorTurno = colorir.GroupBy(x => x.Value).OrderBy(x => x.Key);

            foreach (var grupo in agrupadoPorTurno)
            {
                sb.AppendLine($"[{grupo.Key}]:");
                foreach (var item in grupo)
                {
                    sb.AppendLine($"   - Manutenção na Rota: {item.Key.Dado}");
                }
                sb.AppendLine();
            }
            sb.AppendLine("-------------------------------------\n");

            Console.WriteLine(sb.ToString());
        }
    }
}