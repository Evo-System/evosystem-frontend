using System.Windows;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using evosystem_backend;
using EvoSystem.UI.Helpers;

namespace EvoSystem.UI.Views
{
    public class AppUpdateInfo
    {
        public string Name { get; set; }
        public string CurrentVersion { get; set; }
        public string NewVersion { get; set; }
    }

    public partial class WingetWindow : Window
    {
        private WingetManager _winget = new WingetManager();

        public WingetWindow()
        {
            InitializeComponent();
            AplicarIdioma();
            BuscarAtualizacoes();
        }

        private void AplicarIdioma()
        {
            bool isEnglish = AppSettings.Data.LanguageIndex == 1;

            if (TitleText != null) TitleText.Text = isEnglish ? "Available Updates" : "Atualizações Disponíveis";
            if (SubtitleText != null) SubtitleText.Text = isEnglish ? "Managed by Winget" : "Gerenciado por Winget";
            if (TxtStatus != null) TxtStatus.Text = isEnglish ? "Checking..." : "Verificando...";
            
            if (HdrName != null) HdrName.Text = isEnglish ? "APP NAME" : "NOME DO APLICATIVO";
            if (HdrCurrent != null) HdrCurrent.Text = isEnglish ? "CURRENT" : "VERSÃO ATUAL";
            if (HdrNew != null) HdrNew.Text = isEnglish ? "NEW" : "NOVA VERSÃO";
            if (HdrAction != null) HdrAction.Text = isEnglish ? "ACTION" : "AÇÃO";
            
            if (BtnClose != null) BtnClose.Content = isEnglish ? "Close" : "Fechar";
        }

        private async void BuscarAtualizacoes()
        {
            bool isEnglish = AppSettings.Data.LanguageIndex == 1;
            
            var listaFormatada = await Task.Run(() => 
            {
                var outputBruto = _winget.ListUpgradableApps();
                var listaApps = new List<AppUpdateInfo>();

                foreach (var linha in outputBruto)
                {
                    if (string.IsNullOrWhiteSpace(linha)) continue;
                    if (linha.StartsWith("-") || linha.StartsWith("Name") || linha.StartsWith("Nome")) continue;
                    if (linha.Contains("\\") || linha.Contains("|") || linha.Contains("/")) continue;
                    if (char.IsDigit(linha[0]) && (linha.Contains("atualizações") || linha.Contains("updates"))) continue;

                    var partes = Regex.Split(linha.Trim(), @"\s{2,}");
                    if (partes.Length >= 3)
                    {
                        var app = new AppUpdateInfo { Name = partes[0] };
                        if (partes.Length >= 4) {
                            app.CurrentVersion = partes[2];
                            app.NewVersion = partes[3];
                        } else {
                            app.CurrentVersion = "?";
                            app.NewVersion = partes[partes.Length - 1];
                        }
                        listaApps.Add(app);
                    }
                }
                return listaApps;
            });

            if (listaFormatada.Count == 0)
            {
                TxtStatus.Text = isEnglish ? "System is up to date!" : "Sistema atualizado!";
                TxtStatus.Foreground = System.Windows.Media.Brushes.LightGreen;
            }
            else
            {
                ListApps.ItemsSource = listaFormatada;
                TxtStatus.Text = isEnglish 
                    ? $"{listaFormatada.Count} updates found." 
                    : $"{listaFormatada.Count} atualizações encontradas.";
                TxtStatus.Foreground = System.Windows.Media.Brushes.DeepSkyBlue;
            }
        }

        private void BtnFechar_Click(object sender, RoutedEventArgs e) => Close();
    }
}