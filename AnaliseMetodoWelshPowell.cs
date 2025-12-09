using System.Text;

namespace TP_GRAFOS
{
    /// <summary>
    /// Implementa o algoritmo de coloração de grafos Welsh-Powell, utilizado para 
    /// minimizar o número de cores necessárias para colorir um grafo sem que 
    /// vértices adjacentes compartilhem a mesma cor.
    /// </summary>
    public class AnaliseMetodoWelshPowell : IAnalises
    {
        private readonly IGrafo<string> _grafo;
        private Dictionary<Vertice<string>, string> colorir;

        public AnaliseMetodoWelshPowell(IGrafo<string> grafo)
        {
            _grafo = grafo;
            colorir = new Dictionary<Vertice<string>, string>();
        }

        /// <summary>
        /// Executa o algoritmo Welsh-Powell e retorna o texto da análise,
        /// substituindo a saída no console.
        /// </summary>
        public string Executar()
        {
            WelshPowell();
            return ExibirResultado();
        }

        public void WelshPowell()
        {
            var listaGrausDecrescente = _grafo.ObterGraus()
                .OrderByDescending(v => v.Item2)
                .Select(v => v.Item1)
                .ToList();

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

        /// <summary>
        /// Retorna o resultado da coloração em string, sem imprimir no console.
        /// </summary>
        public string ExibirResultado()
        {
            var sb = new StringBuilder();
            sb.AppendLine("\n--- Método de Welsh-Powell ---");
            sb.AppendLine("\n--- Agendamentos de Manutenções por turnos: ---");

            var agrupadoPorTurno = colorir
                .GroupBy(x => x.Value)
                .OrderBy(x => x.Key);

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
            return sb.ToString();
        }
    }
}
