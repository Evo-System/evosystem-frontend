using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using evosystem_backend;
using EvoSystem.UI.Helpers;

namespace EvoSystem.UI.Views
{
    public partial class UpdateView : UserControl
    {
        private DriverManager _driverManager = new DriverManager();

        // --- PROPRIEDADES DINÂMICAS PARA TRADUÇÃO DENTRO DA LISTA ---

        // 1. Texto "Fabricante:"
        public static readonly DependencyProperty TxtFabricanteProperty =
            DependencyProperty.Register("TxtFabricante", typeof(string), typeof(UpdateView), new PropertyMetadata("Fabricante: "));

        public string TxtFabricante
        {
            get { return (string)GetValue(TxtFabricanteProperty); }
            set { SetValue(TxtFabricanteProperty, value); }
        }

        // 2. Texto "Versão:"
        public static readonly DependencyProperty TxtVersaoProperty =
            DependencyProperty.Register("TxtVersao", typeof(string), typeof(UpdateView), new PropertyMetadata("Versão: "));

        public string TxtVersao
        {
            get { return (string)GetValue(TxtVersaoProperty); }
            set { SetValue(TxtVersaoProperty, value); }
        }

        // 3. Texto do Botão "Atualizar" (NOVO)
        public static readonly DependencyProperty TxtBtnUpdateProperty =
            DependencyProperty.Register("TxtBtnUpdate", typeof(string), typeof(UpdateView), new PropertyMetadata("Atualizar"));

        public string TxtBtnUpdate
        {
            get { return (string)GetValue(TxtBtnUpdateProperty); }
            set { SetValue(TxtBtnUpdateProperty, value); }
        }

        // --- CONSTRUTOR ---
        public UpdateView()
        {
            InitializeComponent();
            
            AplicarIdioma();
            AppSettings.LanguageChanged += AplicarIdioma;
            
            CarregarDrivers();
        }

        private void AplicarIdioma()
        {
            Dispatcher.Invoke(() => 
            {
                bool isEnglish = AppSettings.Data.LanguageIndex == 1;

                // Tradução dos textos soltos (Cabeçalho)
                if (TxtFoundPrefix != null) TxtFoundPrefix.Text = isEnglish ? "We found " : "Encontramos ";
                if (TxtFoundSuffix != null) TxtFoundSuffix.Text = isEnglish ? " drivers installed." : " drivers instalados.";
                if (BtnUpdateAll != null) BtnUpdateAll.Content = isEnglish ? "Update All" : "Atualizar Todos";

                // Tradução dos textos DENTRO da lista (Atualiza as propriedades dinâmicas)
                TxtFabricante = isEnglish ? "Manufacturer: " : "Fabricante: ";
                TxtVersao = isEnglish ? "Version: " : "Versão: ";
                TxtBtnUpdate = isEnglish ? "Update" : "Atualizar"; // <--- AQUI A CORREÇÃO
            });
        }

        private void CarregarDrivers()
        {
            List<DriverInfo> drivers = _driverManager.GetDrivers();
            TxtCount.Text = drivers.Count.ToString();
            ListaDrivers.ItemsSource = drivers;
        }
    }
}