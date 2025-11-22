using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using EvoSystem.UI.Helpers;

namespace EvoSystem.UI.Views
{
    public partial class OptionsView : UserControl
    {
        public OptionsView()
        {
            InitializeComponent();
            CarregarConfiguracoes();
        }

        private void CarregarConfiguracoes()
        {
            AppSettings.Load();

            if (ChkStartup != null)
                ChkStartup.IsChecked = AppSettings.Data.StartWithWindows;

            if (ChkMinimize != null)
                ChkMinimize.IsChecked = AppSettings.Data.MinimizeToTray;

            if (CmbLanguage != null)
                CmbLanguage.SelectedIndex = AppSettings.Data.LanguageIndex;

            if (ChkAutoCheck != null)
                ChkAutoCheck.IsChecked = AppSettings.Data.AutoCheckDrivers;

            if (CmbFrequency != null)
                CmbFrequency.SelectedIndex = AppSettings.Data.CheckFrequencyIndex;

            AplicarIdioma();
        }

        private void AplicarIdioma()
        {
            bool isEnglish = AppSettings.Data.LanguageIndex == 1;

            if (TxtGeral != null) TxtGeral.Text = isEnglish ? "General" : "Geral";
            if (ChkStartup != null) ChkStartup.Content = isEnglish ? "Start Evo-System with Windows" : "Iniciar Evo-System com o Windows";
            if (ChkMinimize != null) ChkMinimize.Content = isEnglish ? "Minimize to tray on close" : "Minimizar para a bandeja ao fechar";
            if (TxtIdioma != null) TxtIdioma.Text = isEnglish ? "Language" : "Idioma";
            
            if (TxtVerificacao != null) TxtVerificacao.Text = isEnglish ? "Scanning" : "Verificação";
            if (ChkAutoCheck != null) ChkAutoCheck.Content = isEnglish ? "Check drivers automatically" : "Verificar drivers automaticamente";
            if (TxtFrequencia != null) TxtFrequencia.Text = isEnglish ? "Frequency" : "Frequência";
            
            if (CmbFrequency != null && CmbFrequency.Items.Count >= 3)
            {
                ((ComboBoxItem)CmbFrequency.Items[0]).Content = isEnglish ? "Daily" : "Diariamente";
                ((ComboBoxItem)CmbFrequency.Items[1]).Content = isEnglish ? "Weekly" : "Semanalmente";
                ((ComboBoxItem)CmbFrequency.Items[2]).Content = isEnglish ? "Monthly" : "Mensalmente";
            }

            if (BtnSalvar != null) BtnSalvar.Content = isEnglish ? "Save Changes" : "Salvar Alterações";
            
            if (TxtToastMsg != null) TxtToastMsg.Text = isEnglish ? "Settings saved successfully!" : "Configurações salvas com sucesso!";
        }

        private async void BtnSalvar_Click(object sender, RoutedEventArgs e)
        {
            BtnSalvar.IsEnabled = false;
            BtnSalvar.Content = AppSettings.Data.LanguageIndex == 1 ? "Saving..." : "Salvando...";

            if (ChkStartup != null) AppSettings.Data.StartWithWindows = ChkStartup.IsChecked == true;
            if (ChkMinimize != null) AppSettings.Data.MinimizeToTray = ChkMinimize.IsChecked == true;
            if (CmbLanguage != null) AppSettings.Data.LanguageIndex = CmbLanguage.SelectedIndex;
            if (ChkAutoCheck != null) AppSettings.Data.AutoCheckDrivers = ChkAutoCheck.IsChecked == true;
            if (CmbFrequency != null) AppSettings.Data.CheckFrequencyIndex = CmbFrequency.SelectedIndex;

            await Task.Run(() => AppSettings.Save());

            AplicarIdioma();

            await Task.Delay(300);

            BtnSalvar.Content = AppSettings.Data.LanguageIndex == 1 ? "Save Changes" : "Salvar Alterações";
            BtnSalvar.IsEnabled = true;

            MostrarToast();
        }

        private void MostrarToast()
        {
            DoubleAnimation fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromMilliseconds(300));
            DoubleAnimation slideUp = new DoubleAnimation(20, 0, TimeSpan.FromMilliseconds(300));
            slideUp.EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut };

            ToastNotification.BeginAnimation(UIElement.OpacityProperty, fadeIn);
            ToastTransform.BeginAnimation(TranslateTransform.YProperty, slideUp);

            Task.Delay(3000).ContinueWith(_ => 
            {
                Dispatcher.Invoke(() => 
                {
                    DoubleAnimation fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromMilliseconds(500));
                    DoubleAnimation slideDown = new DoubleAnimation(0, 20, TimeSpan.FromMilliseconds(500));
                    
                    ToastNotification.BeginAnimation(UIElement.OpacityProperty, fadeOut);
                    ToastTransform.BeginAnimation(TranslateTransform.YProperty, slideDown);
                });
            });
        }
    }
}