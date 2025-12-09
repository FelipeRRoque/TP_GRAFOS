namespace TP_GRAFOS
{
    /// <summary>
    /// Interface base para todos os módulos de análise do Sistema de Otimização 
    /// de Rotas Logísticas (SORL).  
    /// 
    /// Cada algoritmo (ex.: Dijkstra, Edmonds-Karp, AGM, Welsh-Powell, 
    /// Euler/Hamilton) implementa esta interface para garantir um ponto 
    /// padronizado de execução.  
    /// 
    /// A utilização de uma interface permite flexibilizar a arquitetura, 
    /// facilitar substituições de algoritmos e seguir princípios como DIP e ISP 
    /// do SOLID.
    /// </summary>
    public interface IAnalises
    {
        /// <summary>
        /// Executa a análise referente ao algoritmo representado pelo módulo
        /// concreto que implementa esta interface.  
        /// Retorna uma string contendo o relatório ou resultado processado.
        /// </summary>
        /// <returns>
        /// Texto contendo o resultado final da análise realizada.
        /// </returns>
        string Executar();
    }
}