using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TP_GRAFOS
{
    /// <summary>
    /// Representa um hub logístico ou ponto de entrega pertencente à malha de transporte
    /// da Entrega Máxima Logística S.A. dentro do Sistema de Otimização de Rotas Logísticas (SORL).
    /// Cada vértice modela uma unidade operacional da rede.
    /// </summary>
    /// <typeparam name="T">Tipo do dado associado ao hub, como um identificador ou nome.</typeparam>
    public class Vertice<T>
    {
        /// <summary>
        /// Identificador ou informação principal do hub logístico.
        /// Pode representar um código numérico, sigla ou descrição do ponto de distribuição.
        /// </summary>
        public T Dado { get; set; }

        /// <summary>
        /// Cria um novo hub logístico (vértice) contendo os dados fornecidos.
        /// </summary>
        /// <param name="dado">Valor que identifica o hub ou ponto de entrega.</param>
        public Vertice(T dado)
        {
            Dado = dado;
        }

        /// <summary>
        /// Retorna uma representação textual do hub, útil para exibição e depuração.
        /// </summary>
        /// <returns>Texto contendo o identificador do hub logístico.</returns>
        public override string ToString()
        {
            return Dado.ToString();
        }
    }
}
