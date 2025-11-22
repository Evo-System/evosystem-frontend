using System;
using System.IO;
using System.Xml.Serialization;

namespace EvoSystem.UI.Helpers
{
    public class AppData
    {
        public DateTime? LastScanDate { get; set; } = null;
        public bool StartWithWindows { get; set; } = false;
        public bool MinimizeToTray { get; set; } = true;
        public int LanguageIndex { get; set; } = 0; // 0 = PT-BR, 1 = EN-US
        public bool AutoCheckDrivers { get; set; } = true;
        public int CheckFrequencyIndex { get; set; } = 0;
    }

    public static class AppSettings
    {
        private static string FilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "EvoSystem_Config.xml");

        public static AppData Data { get; private set; } = new AppData();

        // *** NOVIDADE: Evento para avisar que o idioma mudou ***
        public static event Action LanguageChanged;

        public static void Load()
        {
            try
            {
                if (File.Exists(FilePath))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(AppData));
                    using (StreamReader reader = new StreamReader(FilePath))
                    {
                        Data = (AppData)serializer.Deserialize(reader);
                    }
                }
            }
            catch 
            {
                Data = new AppData();
            }
        }

        public static void Save()
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(AppData));
                using (StreamWriter writer = new StreamWriter(FilePath))
                {
                    serializer.Serialize(writer, Data);
                }
                
                // Avisa todas as telas que os dados mudaram!
                LanguageChanged?.Invoke(); 
            }
            catch { }
        }
    }
}