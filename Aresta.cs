using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TP_GRAFOS
{
    /// <summary>
    /// Representa uma rota logística dentro da malha de transporte da Entrega Máxima Logística S.A.
    /// Cada aresta corresponde a uma ligação viária entre dois hubs ou pontos de entrega.
    /// </summary>
    /// <typeparam name="T">Tipo do dado armazenado no vértice que representa um hub logístico.</typeparam>
    public class Aresta<T>
    {
        /// <summary>
        /// Hub ou ponto de entrega para o qual esta rota está direcionada.
        /// Em um grafo direcionado, indica o sentido da movimentação de carga.
        /// </summary>
        public Vertice<T> Destino { get; set; }

        /// <summary>
        /// Custo financeiro da rota (em R$) por unidade de carga.
        /// Esse custo considera fatores como distância, tempo, combustível, pedágios e desgaste operacional.
        /// </summary>
        public int Peso { get; set; }

        /// <summary>
        /// Capacidade máxima diária de transporte desta rota (em toneladas).
        /// Representa o limite operacional baseado em infraestrutura, tráfego e restrições viárias.
        /// </summary>
        public int Capacidade { get; set; }

        /// <summary>
        /// Inicializa uma nova rota logística indicando seu hub de destino, custo operacional e capacidade máxima.
        /// </summary>
        /// <param name="destino">Hub de destino ao qual a carga será transportada.</param>
        /// <param name="peso">Custo financeiro da rota por unidade de carga.</param>
        /// <param name="capacidade">Capacidade máxima de transporte diário da rota (em toneladas).</param>
        public Aresta(Vertice<T> destino, int peso = 1, int capacidade = 0)
        {
            Destino = destino;
            Peso = peso;
            Capacidade = capacidade;
        }
    }
}
