using System;
using System.Windows;
using System.Collections.Generic;
using Microsoft.Win32;
using evosystem_backend;
using EvoSystem.UI.Helpers;

namespace EvoSystem.UI.Views
{
    public class ItemStartup
    {
        public string Nome { get; set; }
        public string Caminho { get; set; }
        public string Origem { get; set; }
        public string Status { get; set; }
        public string CorStatus { get; set; }
    }

    public partial class StartupWindow : Window
    {
        private StartupManager _manager = new StartupManager();

        public StartupWindow()
        {
            InitializeComponent();
            AplicarIdioma();
            CarregarStartup();
        }

        private void AplicarIdioma()
        {
            bool isEnglish = AppSettings.Data.LanguageIndex == 1;

            if (TitleText != null) TitleText.Text = isEnglish ? "Startup Applications" : "Aplicativos de Inicialização";
            if (DescText != null) DescText.Text = isEnglish ? "Manage system startup impact." : "Gerencie o impacto na inicialização do sistema.";
            if (BtnClose != null) BtnClose.Content = isEnglish ? "Close" : "Fechar";

            if (GridStartup.Columns.Count >= 4)
            {
                GridStartup.Columns[0].Header = isEnglish ? "Name" : "Nome";
                GridStartup.Columns[1].Header = isEnglish ? "Path" : "Caminho";
                GridStartup.Columns[2].Header = isEnglish ? "Source" : "Origem";
                GridStartup.Columns[3].Header = isEnglish ? "Status" : "Status";
            }
        }

        private void CarregarStartup()
        {
            try
            {
                var apps = _manager.GetStartupApps();
                var lista = new List<ItemStartup>();
                bool isEnglish = AppSettings.Data.LanguageIndex == 1;

                foreach (var app in apps)
                {
                    bool habilitado = VerificarSeEstaHabilitado(app.Name);
                    
                    string statusTxt = habilitado 
                        ? (isEnglish ? "Enabled" : "Habilitado") 
                        : (isEnglish ? "Disabled" : "Desabilitado");
                    
                    string origemTxt = app.IsFromCurrentUser 
                        ? (isEnglish ? "User" : "Usuário") 
                        : (isEnglish ? "System" : "Sistema");

                    lista.Add(new ItemStartup
                    {
                        Nome = app.Name,
                        Caminho = app.FilePath,
                        Origem = origemTxt,
                        Status = statusTxt,
                        CorStatus = habilitado ? "#00ff88" : "#ff4d4d"
                    });
                }
                GridStartup.ItemsSource = lista;
            }
            catch { }
        }

        private bool VerificarSeEstaHabilitado(string nome)
        {
            try {
                using (var key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\Run")) {
                    var val = key?.GetValue(nome) as byte[];
                    return val == null || val[0] % 2 == 0;
                }
            } catch { return true; }
        }

        private void BtnFechar_Click(object sender, RoutedEventArgs e) => Close();
    }
}