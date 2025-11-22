using System;
using System.IO;

namespace EvoSystem.UI.Helpers
{
    public static class ScanState
    {
        // Caminho do arquivo onde vamos salvar a data (na pasta de dados do aplicativo)
        private static string FilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
            "EvoSystem_LastScan.txt");

        public static void SalvarDataAtual()
        {
            try
            {
                // Salva a data e hora atual
                File.WriteAllText(FilePath, DateTime.Now.ToString());
            }
            catch { /* Ignora erros de permissão */ }
        }

        public static string CarregarUltimaData()
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    // Lê o arquivo e tenta converter para data
                    string dataSalva = File.ReadAllText(FilePath);
                    if (DateTime.TryParse(dataSalva, out DateTime data))
                    {
                        // Se foi hoje, mostra "Hoje, HH:mm"
                        if (data.Date == DateTime.Today)
                            return "Hoje, " + data.ToString("HH:mm");
                        
                        // Se foi outro dia, mostra a data completa
                        return data.ToString("dd/MM/yyyy HH:mm");
                    }
                }
            }
            catch { }

            return null; // Nunca foi verificado
        }
    }
}