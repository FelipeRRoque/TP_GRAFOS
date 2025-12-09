using System.Text;

namespace TP_GRAFOS
{
    /// <summary>
    /// Classe utilitária responsável por registrar os resultados das análises
    /// executadas no sistema SORL. O objetivo é persistir logs de cada grafo
    /// analisado em arquivos separados, garantindo organização e rastreabilidade.
    /// Cada execução gera um cabeçalho temporal para facilitar auditoria e revisão.
    /// </summary>
    public static class RegistradorGrafo
    {
        private const string PastaLogs = "logs";
        private const string NomeArquivo = "log_grafo{0}.txtr";
        private static readonly object travaArquivo = new object();

        /// <summary>
        /// Garante que a pasta de logs exista antes de iniciar qualquer gravação.
        /// Caso ainda não exista, o diretório é criado no disco.
        /// </summary>
        private static void GarantirPasta()
        {
            if (!Directory.Exists(PastaLogs)) Directory.CreateDirectory(PastaLogs);
        }

        /// <summary>
        /// Monta e retorna o caminho completo do arquivo de log associado ao grafo
        /// informado, formatando o índice para possuir sempre dois dígitos.
        /// </summary>
        /// <param name="indiceGrafo">Número identificador do grafo analisado.</param>
        /// <returns>Caminho completo para o arquivo correspondente.</returns>
        private static string ObterCaminhoLog(int indiceGrafo)
        {
            string idx = indiceGrafo.ToString("D2");
            return Path.Combine(PastaLogs, string.Format(NomeArquivo, idx));
        }

        /// <summary>
        /// Registra um texto no arquivo de log do grafo correspondente e também
        /// imprime no console. O método adiciona automaticamente um cabeçalho
        /// contendo data e hora, criando o arquivo caso necessário.
        /// A escrita é protegida com lock para evitar inconsistências.
        /// </summary>
        /// <param name="indiceGrafo">Identificador do grafo associado ao log.</param>
        /// <param name="texto">Conteúdo registrável, normalmente saída das análises.</param>
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
