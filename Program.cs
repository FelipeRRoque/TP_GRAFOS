namespace TP_GRAFOS
{
    public static class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== Sistema de Otimização de Rotas Logísticas ===");

            while (true)
            {
                PrintHeader("Seleção de Grafos");

                Console.WriteLine("Escolha um grafo para analisar:");
                Console.WriteLine("1) Grafo 1");
                Console.WriteLine("2) Grafo 2");
                Console.WriteLine("3) Grafo 3");
                Console.WriteLine("4) Grafo 4");
                Console.WriteLine("5) Grafo 5");
                Console.WriteLine("6) Grafo 6");
                Console.WriteLine("7) Grafo 7");
                Console.WriteLine("0) Encerrar programa");

                Console.Write("\nOpção: ");
                string opcGrafo = Console.ReadLine()?.Trim();
                Console.WriteLine();

                if (opcGrafo == "0")
                {
                    Console.WriteLine("Encerrando execução...");
                    return;
                }

                // validar número
                if (!int.TryParse(opcGrafo, out int grafoEscolhido) || grafoEscolhido < 1 || grafoEscolhido > 7)
                {
                    Console.WriteLine("Opção inválida. Escolha um número de 1 a 7 ou 0 para sair.");
                    continue;
                }

                // carregar grafo
                IGrafo<int> grafo = null;

                try
                {
                    grafo = Arquivo.LerDados($"dataGrafos/grafo0{grafoEscolhido}.dimacs");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao carregar grafo: {ex.Message}");
                    continue;
                }

                // exibir grafo
                RegistradorGrafo.Registrar(grafoEscolhido, grafo.ExibirGrafo());

                // --- MENU DE OPERAÇÕES DO GRAFO ---
                while (true)
                {
                    PrintHeader($"Operações - Grafo {grafoEscolhido}");

                    Console.WriteLine("Escolha uma operação:");
                    Console.WriteLine("1) Roteamento de Menor Custo");
                    Console.WriteLine("2) Capacidade Máxima de Escoamento");
                    Console.WriteLine("3) Expansão da Rede de Comunicação");
                    Console.WriteLine("4) Agendamento de Manutenções sem Conflito");
                    Console.WriteLine("5) Rota única de Inspeção");
                    Console.WriteLine("0) Voltar para escolha de grafos");

                    Console.Write("\nOpção: ");
                    string opcAnalise = Console.ReadLine()?.Trim();

                    if (opcAnalise == "0") break; // volta para a escolha de grafos

                    switch (opcAnalise)
                    {
                        case "1":
                            Console.WriteLine("Defina os pontos para análise de Distância:");
                            Vertice<int> origemDijkstra = ObterVerticeUsuario(grafo, "Origem");
                            Vertice<int> destinoDijkstra = ObterVerticeUsuario(grafo, "Destino");

                            if (origemDijkstra != null && destinoDijkstra != null)
                            {
                                RegistradorGrafo.Registrar(
                                    grafoEscolhido,
                                    new AnaliseCaminhoMinimoDijkstra(grafo, origemDijkstra, destinoDijkstra).Executar()
                                );
                            }
                            break;

                        case "2":
                            Console.WriteLine("Defina a orgiem e o destino para cálculo de capacidade de escoamento:");

                            Vertice<int> origemEdmondsKarp = ObterVerticeUsuario(grafo, "Origem");
                            Vertice<int> destinoEdmondsKarp = ObterVerticeUsuario(grafo, "Destino");

                            if (origemEdmondsKarp != null && destinoEdmondsKarp != null)
                            {
                                RegistradorGrafo.Registrar(
                                    grafoEscolhido,
                                    new AnaliseFluxoMaximoEdmondsKarp(grafo, origemEdmondsKarp, destinoEdmondsKarp).Executar()
                                );
                            }

                            break;

                        case "3":
                            RegistradorGrafo.Registrar(
                                grafoEscolhido,
                                new AnaliseArvoreGeradoraMinima(grafo).Executar()
                            );

                            break;

                        case "4":
                            IGrafo<string> grafoConflitos = GrafoUtilitario.GerarGrafoDeConflitos(grafo);

                            RegistradorGrafo.Registrar(
                                grafoEscolhido,
                                $" -> Conflitos mapeados: {grafoConflitos.ObterVertices().Count} nós de tarefa.\n{new AnaliseMetodoWelshPowell(grafoConflitos).Executar()}"
                            );

                            break;

                        case "5":
                            RegistradorGrafo.Registrar(
                                grafoEscolhido,
                                $"\nPercurso de Rotas\n{new AnalisarCaminhoEuleriano(grafo).Executar()}"
                            );

                            RegistradorGrafo.Registrar(
                                grafoEscolhido,
                                $"\nPercurso de Hubs\n{new AnalisarCaminhoHamiltoniano(grafo).Executar()}"
                            );

                            break;

                        default:
                            Console.WriteLine("Opção inválida. Escolha uma das opções listadas.");
                            break;
                    }

                    // permitir nova operação
                    Console.Write("\nDeseja executar outra operção para este grafo? (S/N): ");
                    string repetir = Console.ReadLine()?.Trim().ToUpper();

                    if (repetir != "S") break;
                }
            }
        }

        // ---------------- FUNÇÕES AUXILIARES ----------------

        private static void PrintHeader(string titulo)
        {
            Console.WriteLine($"\n========== {titulo} ==========");
        }
        private static Vertice<int> ObterVerticeUsuario(IGrafo<int> grafo, string nomeTipo)
        {
            while (true)
            {
                Console.Write($"Digite o ID do Vértice {nomeTipo} (ou 'sair' para pular): ");
                string input = Console.ReadLine();

                if (input.ToLower() == "sair") return null;

                if (int.TryParse(input, out int id))
                {
                    var vertice = grafo.ObterVertices().FirstOrDefault(v => v.Dado == id);
                    if (vertice != null) return vertice;

                    Console.WriteLine($"[Erro] O vértice {id} não existe no grafo.");
                }
                else
                {
                    Console.WriteLine("[Erro] Digite um número inteiro válido.");
                }
            }
        }
    }
}