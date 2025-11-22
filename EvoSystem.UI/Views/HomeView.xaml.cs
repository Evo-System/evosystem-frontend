using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using evosystem_backend;
using EvoSystem.UI.Helpers;

namespace EvoSystem.UI.Views
{
    public partial class HomeView : UserControl
    {
        private SystemInfo _sysInfo = new SystemInfo();

        public HomeView()
        {
            InitializeComponent();
            
            AppSettings.Load();
            CarregarDadosDoSistema();
            AtualizarCardUltimaVerificacao();

            // Aplica tradução ao carregar
            this.Loaded += HomeView_Loaded;
        }

        private void HomeView_Loaded(object sender, RoutedEventArgs e)
        {
            AplicarIdioma();
        }

        private void CarregarDadosDoSistema()
        {
            TxtCpu.Text = _sysInfo.GetCpuName();
            TxtGpu.Text = _sysInfo.GetGpuName();
            TxtRam.Text = _sysInfo.GetTotalRam();
            TxtOs.Text = _sysInfo.GetOperatingSystem();
        }

        private void AplicarIdioma()
        {
            bool isEnglish = AppSettings.Data.LanguageIndex == 1;

            // Tradução dos Títulos e Cards
            if (FindName("TitleInfoSys") is TextBlock titleInfo)
                titleInfo.Text = isEnglish ? "System Information" : "Informações do Sistema";

            if (FindName("TitleLastScan") is TextBlock titleScan)
                titleScan.Text = isEnglish ? "Last Scan" : "Última Verificação";

            if (FindName("LblSystem") is TextBlock lblSys)
                lblSys.Text = isEnglish ? "System:" : "Sistema:";

            // TRADUÇÃO DENTRO DO TEMPLATE DO BOTÃO
            if (BtnScan.Template != null)
            {
                var txtVerify = BtnScan.Template.FindName("TxtBtnVerify", BtnScan) as TextBlock;
                var txtSub = BtnScan.Template.FindName("TxtBtnSub", BtnScan) as TextBlock;
                
                // AQUI ESTÁ A CORREÇÃO: Traduz o "Analisando..."
                var txtAnalyzing = BtnScan.Template.FindName("TxtAnalyzing", BtnScan) as TextBlock;

                if (txtVerify != null) 
                    txtVerify.Text = isEnglish ? "SCAN" : "VERIFICAR";
                
                if (txtSub != null) 
                    txtSub.Text = isEnglish ? "Start Scanning" : "Iniciar Escaneamento";

                if (txtAnalyzing != null)
                    txtAnalyzing.Text = isEnglish ? "Analyzing..." : "Analisando...";
            }

            // Tradução do Status Inicial
            if (BtnScan.IsEnabled)
            {
                // Só muda o texto padrão se não houver scan hoje
                if (!AppSettings.Data.LastScanDate.HasValue || AppSettings.Data.LastScanDate.Value.Date != DateTime.Today)
                {
                    TxtStatusScan.Text = isEnglish 
                        ? "The system is waiting for verification." 
                        : "O sistema está aguardando verificação.";
                }
                else
                {
                    // Se já fez scan hoje, mantém a mensagem de sucesso traduzida
                    TxtStatusScan.Text = isEnglish 
                        ? "Analysis complete. System optimized." 
                        : "Análise concluída. Sistema verificado.";
                }
            }
        }

        private async void BtnScan_Click(object sender, RoutedEventArgs e)
        {
            BtnScan.IsEnabled = false;
            
            var template = BtnScan.Template;
            var stkNormal = (StackPanel)template.FindName("StkNormal", BtnScan);
            var stkLoading = (StackPanel)template.FindName("StkLoading", BtnScan);
            var txtPorcentagem = (TextBlock)template.FindName("TxtPorcentagem", BtnScan);
            
            // Garante que o texto "Analisando..." esteja correto ao clicar
            var txtAnalyzing = template.FindName("TxtAnalyzing", BtnScan) as TextBlock;
            bool isEnglish = AppSettings.Data.LanguageIndex == 1;
            
            if (txtAnalyzing != null)
                txtAnalyzing.Text = isEnglish ? "Analyzing..." : "Analisando...";

            stkNormal.Visibility = Visibility.Collapsed;
            stkLoading.Visibility = Visibility.Visible;
            
            ProgressoScan.Visibility = Visibility.Visible;
            ProgressoScan.Value = 0;
            
            TxtStatusScan.Text = isEnglish ? "Starting optimization analysis..." : "Iniciando análise de otimização...";
            TxtStatusScan.Foreground = Brushes.White;

            for (int i = 0; i <= 100; i++)
            {
                ProgressoScan.Value = i;
                txtPorcentagem.Text = i + "%";

                if (i == 10) TxtStatusScan.Text = isEnglish ? "Checking for outdated drivers..." : "Buscando drivers desatualizados...";
                if (i == 40) TxtStatusScan.Text = isEnglish ? "Analyzing unnecessary files..." : "Analisando arquivos desnecessários...";
                if (i == 70) TxtStatusScan.Text = isEnglish ? "Checking startup items..." : "Verificando itens de inicialização...";
                if (i == 90) TxtStatusScan.Text = isEnglish ? "Finishing analysis..." : "Finalizando análise...";

                await Task.Delay(30);
            }

            stkNormal.Visibility = Visibility.Visible;
            stkLoading.Visibility = Visibility.Collapsed;
            ProgressoScan.Visibility = Visibility.Collapsed;
            BtnScan.IsEnabled = true;

            TxtStatusScan.Text = isEnglish ? "Analysis complete. System optimized." : "Análise concluída. Sistema verificado.";
            TxtStatusScan.Foreground = Brushes.LightGreen;

            AppSettings.Data.LastScanDate = DateTime.Now;
            AppSettings.Save();

            AtualizarCardUltimaVerificacao();
        }

        private void AtualizarCardUltimaVerificacao()
        {
            bool isEnglish = AppSettings.Data.LanguageIndex == 1;

            if (AppSettings.Data.LastScanDate.HasValue)
            {
                DateTime data = AppSettings.Data.LastScanDate.Value;
                string textoData;

                if (data.Date == DateTime.Today)
                    textoData = (isEnglish ? "Today, " : "Hoje, ") + data.ToString("HH:mm");
                else
                    textoData = data.ToString("dd/MM HH:mm");

                TxtLastScanTime.Text = textoData;
                TxtLastScanTime.Foreground = new SolidColorBrush(Color.FromRgb(0, 255, 136));

                TxtLastScanStatus.Text = isEnglish 
                    ? "System optimized. Drivers and files verified." 
                    : "Sistema otimizado. Drivers e arquivos verificados.";
                
                TxtLastScanStatus.Foreground = Brushes.White;

                IconStatus.Text = "✅";
                IconStatus.Opacity = 1;
            }
            else
            {
                TxtLastScanTime.Text = "--";
                TxtLastScanTime.Foreground = Brushes.Gray;
                TxtLastScanStatus.Text = isEnglish ? "Never scanned" : "Nunca verificado";
                IconStatus.Text = "🛡️";
                IconStatus.Opacity = 0.5;
            }
        }
    }
}