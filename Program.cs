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
                grafo.ExibirGrafo();

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
                            // new AnaliseCaminhoMinimoDijkstra(grafo).Executar();
                            break;

                        case "2":
                            // new AnaliseFluxoMaximoEdmondsKarp(grafo).Executar();
                            break;

                        case "3":
                            new AnaliseArvoreGeradoraMinima(grafo).Executar();
                            break;

                        case "4":
                            IGrafo<string> grafoConflitos = GrafoUtilitario.GerarGrafoDeConflitos(grafo);
                            Console.WriteLine($" -> Conflitos mapeados: {grafoConflitos.ObterVertices().Count} nós de tarefa.");

                            var analiseWP = new AnaliseMetodoWelshPowell(grafoConflitos);
                            analiseWP.Executar();
                            break;

                        case "5":
                            Console.WriteLine("\nPercurso de Rotas");
                            new AnalisarCaminhoEuleriano(grafo).Executar();

                            Console.WriteLine("\nPercurso de Hubs");
                            new AnalisarCaminhoHamiltoniano(grafo).Executar();
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
    }
}
