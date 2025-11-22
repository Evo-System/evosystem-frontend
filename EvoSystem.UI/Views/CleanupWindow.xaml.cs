using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using evosystem_backend;
using EvoSystem.UI.Helpers;

namespace EvoSystem.UI.Views
{
    public class CleanupItem
    {
        public string Name { get; set; }
        public string Size { get; set; }
    }

    public partial class CleanupWindow : Window
    {
        private CleanupManager _cleaner = new CleanupManager();

        public CleanupWindow()
        {
            InitializeComponent();
            AplicarIdioma();
            AnalisarSistema();
        }

        private void AplicarIdioma()
        {
            bool isEnglish = AppSettings.Data.LanguageIndex == 1;

            if (TitleText != null) TitleText.Text = isEnglish ? "System Cleanup" : "Limpeza de Disco";
            if (DescText != null) DescText.Text = isEnglish ? "Check files to be removed." : "Verifique os arquivos que serão removidos.";
            if (BtnCancel != null) BtnCancel.Content = isEnglish ? "Cancel" : "Cancelar";
            if (BtnClean != null) BtnClean.Content = isEnglish ? "Clean Now" : "Limpar Agora";
            
            if (ResultTitle != null) ResultTitle.Text = isEnglish ? "Cleanup Complete!" : "Limpeza Concluída!";
            if (BtnCloseResult != null) BtnCloseResult.Content = isEnglish ? "Awesome" : "Incrível";
        }

        private async void AnalisarSistema()
        {
            bool isEnglish = AppSettings.Data.LanguageIndex == 1;
            BtnClean.IsEnabled = false;
            BtnClean.Content = isEnglish ? "Analyzing..." : "Analisando...";

            var items = await Task.Run(() => CalcularPreview(isEnglish));
            GridItems.ItemsSource = items;
            
            BtnClean.IsEnabled = true;
            BtnClean.Content = isEnglish ? "Clean Now" : "Limpar Agora";
        }

        private List<CleanupItem> CalcularPreview(bool isEnglish)
        {
            var lista = new List<CleanupItem>();
            
            long tempSize = GetFolderSize(Path.GetTempPath());
            lista.Add(new CleanupItem { Name = isEnglish ? "Temporary Files" : "Arquivos Temporários", Size = FormatSize(tempSize) });

            long winTempSize = GetFolderSize(@"C:\Windows\Temp");
            lista.Add(new CleanupItem { Name = isEnglish ? "System Cache" : "Cache do Sistema", Size = FormatSize(winTempSize) });

            long prefetchSize = GetFolderSize(@"C:\Windows\Prefetch");
            lista.Add(new CleanupItem { Name = isEnglish ? "Optimization (Prefetch)" : "Otimização (Prefetch)", Size = FormatSize(prefetchSize) });

            return lista;
        }

        private long GetFolderSize(string path)
        {
            long size = 0;
            try {
                if (Directory.Exists(path)) {
                    foreach (var file in new DirectoryInfo(path).GetFiles("*", SearchOption.AllDirectories))
                        size += file.Length;
                }
            } catch { }
            return size;
        }

        private string FormatSize(long bytes) => (bytes / (1024.0 * 1024.0)).ToString("F2") + " MB";

        private async void BtnClean_Click(object sender, RoutedEventArgs e)
        {
            bool isEnglish = AppSettings.Data.LanguageIndex == 1;
            BtnClean.Content = isEnglish ? "Cleaning..." : "Limpando...";
            BtnClean.IsEnabled = false;

            var resultado = await Task.Run(() => _cleaner.RunFullCleanup());

            GridPreview.Visibility = Visibility.Collapsed;
            GridResult.Visibility = Visibility.Visible;

            TxtResultSpace.Text = resultado.FreedMB + (isEnglish ? " Freed" : " Liberados");
            TxtResultFiles.Text = isEnglish 
                ? $"{resultado.FilesDeleted} files removed from your PC." 
                : $"{resultado.FilesDeleted} arquivos removidos do seu PC.";
        }

        private void BtnCancelar_Click(object sender, RoutedEventArgs e) => Close();
    }
}