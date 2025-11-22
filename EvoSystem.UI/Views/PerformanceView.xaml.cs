using System.Windows;
using System.Windows.Controls;
using System.Threading.Tasks;
using evosystem_backend;
using EvoSystem.UI.Helpers; // Necessário para AppSettings

namespace EvoSystem.UI.Views
{
    public partial class PerformanceView : UserControl
    {
        private CleanupManager _cleaner = new CleanupManager();

        public PerformanceView()
        {
            InitializeComponent();
            
            // Carrega idioma ao abrir
            AplicarIdioma();
            
            // Se inscreve para mudanças futuras
            AppSettings.LanguageChanged += AplicarIdioma;
        }

        private void AplicarIdioma()
        {
            bool isEnglish = AppSettings.Data.LanguageIndex == 1;

            // Título da Página
            if (TitlePage != null) TitlePage.Text = isEnglish ? "Tools Dashboard" : "Painel de Ferramentas";

            // Card 1 (Limpeza)
            if (TitleClean != null) TitleClean.Text = isEnglish ? "Quick Cleanup" : "Limpeza Rápida";
            if (DescClean != null) DescClean.Text = isEnglish ? "Remove temporary files and system cache to free up space." : "Remove arquivos temporários e cache do sistema para liberar espaço.";
            if (BtnClean != null) BtnClean.Content = isEnglish ? "Run Cleanup" : "Executar Limpeza";
            if (StatusClean != null) StatusClean.Text = isEnglish ? "Active" : "Ativado";

            // Card 2 (Inicialização)
            if (TitleStartup != null) TitleStartup.Text = isEnglish ? "Startup" : "Inicialização";
            if (DescStartup != null) DescStartup.Text = isEnglish ? "Manage applications that start with Windows." : "Gerencie aplicativos que iniciam junto com o Windows.";
            if (BtnStartup != null) BtnStartup.Content = isEnglish ? "View Apps" : "Ver Apps";
            if (StatusStartup != null) StatusStartup.Text = isEnglish ? "Active" : "Ativado";

            // Card 3 (Winget)
            if (TitleWinget != null) TitleWinget.Text = isEnglish ? "Update Apps" : "Atualizar Apps";
            if (DescWinget != null) DescWinget.Text = isEnglish ? "Search updates for installed programs (via Winget)." : "Busca atualizações para programas instalados (via Winget).";
            if (BtnWinget != null) BtnWinget.Content = isEnglish ? "Search Updates" : "Procurar Atualizações";
            if (StatusWinget != null) StatusWinget.Text = isEnglish ? "Active" : "Ativado";

            // Card 4 (Arquivos Grandes)
            if (TitleLargeFiles != null) TitleLargeFiles.Text = isEnglish ? "Large Files" : "Arquivos Grandes";
            if (DescLargeFiles != null) DescLargeFiles.Text = isEnglish ? "Find files larger than 100 MB in your folders." : "Encontre arquivos maiores que 100 MB nas suas pastas.";
            if (BtnLargeFiles != null) BtnLargeFiles.Content = isEnglish ? "Search" : "Pesquisar";
            if (StatusLargeFiles != null) StatusLargeFiles.Text = isEnglish ? "Active" : "Ativado";
        }

        private void BtnLimpeza_Click(object sender, RoutedEventArgs e)
        {
            CleanupWindow janela = new CleanupWindow();
            janela.ShowDialog();
        }

        private void BtnStartup_Click(object sender, RoutedEventArgs e)
        {
            StartupWindow janela = new StartupWindow();
            janela.ShowDialog();
        }

        private void BtnWinget_Click(object sender, RoutedEventArgs e)
        {
            WingetWindow janela = new WingetWindow();
            janela.ShowDialog();
        }

        private void BtnLargeFiles_Click(object sender, RoutedEventArgs e)
        {
            LargeFilesWindow janela = new LargeFilesWindow();
            janela.ShowDialog();
        }
    }
}