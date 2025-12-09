using System.Text;

namespace TP_GRAFOS
{
    /// <summary>
    ///
    /// Esta classe implementa o algoritmo de coloração de grafos **Welsh-Powell**,  
    /// usado para minimizar o número de "cores" (neste caso, turnos) atribuídas a um grafo,
    /// garantindo que vértices adjacentes nunca recebam a mesma cor.
    ///
    /// 1) Fluxo geral da análise:
    /// - O método <see cref="Executar"/> inicia o processo, chamando o algoritmo principal
    ///   <see cref="WelshPowell"/> e, em seguida, formatando o resultado por meio de
    ///   <see cref="ExibirResultado"/>.
    /// - O algoritmo realiza uma coloração gulosa baseada na ordem dos graus dos vértices.
    ///
    /// 2) Funcionamento interno do algoritmo:
    /// - <see cref="WelshPowell"/>:
    ///     • Obtém os graus de todos os vértices através de <c>_grafo.ObterGraus()</c>;
    ///     • Ordena os vértices em ordem decrescente de grau (regra principal do método);
    ///     • Percorre os vértices e tenta atribuir cada um ao turno corrente;
    ///     • Verifica conflitos observando se algum vizinho já recebeu a mesma cor;
    ///     • Caso não haja conflito, atribui o turno ao vértice;
    ///     • Caso contrário, ele será tentado novamente no próximo turno.
    ///
    /// 3) Formatação final:
    /// - <see cref="ExibirResultado"/> monta e retorna uma string descrevendo:
    ///     • Cada turno criado,
    ///     • Quais vértices pertencem a cada turno,
    ///     • Em um formato organizado e adequado para exibição ou registro em log.
    ///     
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
