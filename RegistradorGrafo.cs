using System.Text;

namespace TP_GRAFOS
{
    public static class RegistradorGrafo
    {
        private const string PastaLogs = "logs";
        private const string NomeArquivo = "log_grafo{0}.txtr";
        private static readonly object travaArquivo = new object();

        private static void GarantirPasta()
        {
            if (!Directory.Exists(PastaLogs)) Directory.CreateDirectory(PastaLogs);
        }

        private static string ObterCaminhoLog(int indiceGrafo)
        {
            string idx = indiceGrafo.ToString("D2");
            return Path.Combine(PastaLogs, string.Format(NomeArquivo, idx));
        }

        public static void Registrar(int indiceGrafo, string texto)
        {
            if (texto == null) texto = "(null)";

            Console.WriteLine(texto);

            GarantirPasta();
            string caminho = ObterCaminhoLog(indiceGrafo);

            lock (travaArquivo)
            {
                string cabecalho = $"=== REGISTRO GERADO EM {DateTime.Now:dd/MM/yyyy HH:mm:ss} ==={Environment.NewLine}\n";
                File.AppendAllText(caminho, cabecalho + texto + Environment.NewLine, Encoding.UTF8);
            }
        }
    }
}
