using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TP_GRAFOS
{
    public interface IGrafo<T>
    {
        void AdicionarVertice(T dado);
        void AdicionarAresta(T origem, T destino, int peso = 1, int capacidade = 0);
        void ExibirGrafo();
    }
}