using System.Windows;
using System.Threading.Tasks;
using evosystem_backend;
using EvoSystem.UI.Helpers;

namespace EvoSystem.UI.Views
{
    public partial class LargeFilesWindow : Window
    {
        private FileManager _fileManager = new FileManager();

        public LargeFilesWindow()
        {
            InitializeComponent();
            AplicarIdioma();
            IniciarVarredura();
        }

        private void AplicarIdioma()
        {
            bool isEnglish = AppSettings.Data.LanguageIndex == 1;

            if (TitleText != null) TitleText.Text = isEnglish ? "Large Files (> 100MB)" : "Arquivos Grandes (> 100MB)";
            if (BtnClose != null) BtnClose.Content = isEnglish ? "Close" : "Fechar";
            TxtStatus.Text = isEnglish ? "Scanning user folders..." : "Varrendo pastas do usuário...";

            if (GridFiles.Columns.Count >= 3)
            {
                GridFiles.Columns[0].Header = isEnglish ? "Name" : "Nome";
                GridFiles.Columns[1].Header = isEnglish ? "Full Path" : "Caminho Completo";
                GridFiles.Columns[2].Header = isEnglish ? "Size (Bytes)" : "Tamanho (Bytes)";
            }
        }

        private async void IniciarVarredura()
        {
            bool isEnglish = AppSettings.Data.LanguageIndex == 1;
            var arquivos = await Task.Run(() => _fileManager.FindLargeFilesQuickScan(104857600));

            GridFiles.ItemsSource = arquivos;
            TxtStatus.Text = isEnglish 
                ? $"Found {arquivos.Count} large files." 
                : $"Encontrados {arquivos.Count} arquivos grandes nas pastas pessoais.";
            
            TxtStatus.Foreground = System.Windows.Media.Brushes.LightGreen;
        }

        private void BtnFechar_Click(object sender, RoutedEventArgs e) => Close();
    }
}