using System;
using System.Windows;
using System.Windows.Input;
using EvoSystem.UI.Views;
using EvoSystem.UI.Helpers;

namespace EvoSystem.UI
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            // Carrega config inicial
            AppSettings.Load(); 
            
            // Aplica idioma inicial
            AplicarIdioma(); 

            // ** INSCRICÃO NO EVENTO **
            // Quando o idioma mudar lá nas opções, roda o "AplicarIdioma" aqui
            AppSettings.LanguageChanged += AplicarIdioma;

            // Inicia na Home
            MainContent.Content = new HomeView();
            AtualizarTituloPagina();
        }

        // Método que traduz o menu lateral e o título
        private void AplicarIdioma()
        {
            // O segredo: Dispatcher.Invoke garante que a atualização visual 
            // ocorra na thread principal, mesmo vindo do Task.Run das opções
            Dispatcher.Invoke(() => 
            {
                bool isEnglish = AppSettings.Data.LanguageIndex == 1;

                // Botões do Menu
                if (BtnMenuHome != null) BtnMenuHome.Content = isEnglish ? "Scan" : "Verificar";
                if (BtnMenuUpdate != null) BtnMenuUpdate.Content = isEnglish ? "Update" : "Atualizar";
                if (BtnMenuPerformance != null) BtnMenuPerformance.Content = isEnglish ? "Performance" : "Desempenho";
                if (BtnMenuOptions != null) BtnMenuOptions.Content = isEnglish ? "Options" : "Opções";

                // Atualiza o título da página atual também
                AtualizarTituloPagina();
            });
        }

        private void AtualizarTituloPagina()
        {
            Dispatcher.Invoke(() => 
            {
                bool isEnglish = AppSettings.Data.LanguageIndex == 1;
                
                if (MainContent.Content is HomeView)
                    PageTitle.Text = isEnglish ? "Dashboard" : "Painel Principal";
                else if (MainContent.Content is UpdateView)
                    PageTitle.Text = isEnglish ? "Driver Updates" : "Atualização de Drivers";
                else if (MainContent.Content is PerformanceView)
                    PageTitle.Text = isEnglish ? "Performance & Optimization" : "Desempenho e Otimização";
                else if (MainContent.Content is OptionsView)
                    PageTitle.Text = isEnglish ? "Options & Settings" : "Opções e Configurações";
            });
        }

        // Navegação
        private void Nav_Home_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new HomeView();
            AtualizarTituloPagina();
        }

        private void Nav_Atualizar_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new UpdateView();
            AtualizarTituloPagina();
        }

        private void Nav_Desempenho_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new PerformanceView();
            AtualizarTituloPagina();
        }

        private void Nav_Opcoes_Click(object sender, RoutedEventArgs e)
        {
            MainContent.Content = new OptionsView();
            AtualizarTituloPagina();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void BtnFechar_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}