using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TP_GRAFOS
{
    public interface IAnalises
    {
        void Executar(IGrafo<int> grafo);
    }
}