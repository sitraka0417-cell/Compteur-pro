using System;
using System.IO;
using System.Text.Json;
using CompteurCarnet.Models;

namespace CompteurCarnet.Services
{
    /// <summary>
    /// Loads and saves the application's persisted state as JSON in the
    /// current user's AppData folder. This replaces the SharedPreferences
    /// storage used by the original Android app.
    /// </summary>
    public static class DataStore
    {
        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            WriteIndented = true
        };

        private static string FolderPath =>
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "CompteurCarnet");

        private static string FilePath => Path.Combine(FolderPath, "data.json");

        public static AppData Load()
        {
            try
            {
                if (!File.Exists(FilePath))
                {
                    return new AppData();
                }

                var json = File.ReadAllText(FilePath);
                var data = JsonSerializer.Deserialize<AppData>(json, JsonOptions);
                return data ?? new AppData();
            }
            catch
            {
                return new AppData();
            }
        }

        public static void Save(AppData data)
        {
            try
            {
                Directory.CreateDirectory(FolderPath);
                var json = JsonSerializer.Serialize(data, JsonOptions);
                File.WriteAllText(FilePath, json);
            }
            catch
            {
                // Silently ignore, consistent with the original app's behaviour
                // (it also swallowed most I/O exceptions).
            }
        }
    }
}
