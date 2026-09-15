using System.IO;
using System.Text.Json;

namespace Core.Config
{
    public sealed class ConfigManager
    {
        public static ConfigManager Instance { get; } = new();

        private readonly string _configPath =
            Path.Combine(AppContext.BaseDirectory, "Config", "UserConfig.json");

        private Dictionary<string, string> _configs = [];

        private ConfigManager()
        {
            if (File.Exists(_configPath))
            {
                string json = File.ReadAllText(_configPath);

                _configs =
                    JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? [];
            }
        }

        public string? GetConfig(string key)
        {
            return _configs.TryGetValue(key, out string? value)
                ? value
                : null;
        }

        public void SetConfig(string key, string value)
        {
            _configs[key] = value;

            Directory.CreateDirectory(
                Path.GetDirectoryName(_configPath)!);

            string json = JsonSerializer.Serialize(
                _configs,
                new JsonSerializerOptions { WriteIndented = true });

            File.WriteAllText(_configPath, json);
        }
    }
}