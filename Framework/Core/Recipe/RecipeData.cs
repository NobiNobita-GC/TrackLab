using System.Collections.ObjectModel;
using System.Text.Json;

namespace Core.Recipe
{
    public class RecipeData
    {
        public Dictionary<string, string> Header { get; set; } = [];
        public ObservableCollection<Dictionary<string, string>> Step { get; set; } = [];
        public Dictionary<string, string> Config { get; set; } = [];

        public string ToJsonString()
        {
            JsonSerializerOptions options = new()
            {
                WriteIndented = true,
            };

            return JsonSerializer.Serialize(this, options);
        }

        public static RecipeData FromJsonString(string recipeContent)
        {
            if (string.IsNullOrEmpty(recipeContent))
            {
                return new RecipeData();
            }

            return JsonSerializer.Deserialize<RecipeData>(recipeContent);
        }

        public void Copy(RecipeData? source)
        {
            if (source == null)
            {
                return;
            }

            Header.Clear();
            foreach (var item in source.Header)
            {
                Header.Add(item.Key, item.Value);
            }

            Step.Clear();
            foreach (var item in source.Step)
            {
                Step.Add(new Dictionary<string, string>(item));
            }

            Config.Clear();
            foreach (var item in source.Config)
            {
                Config.Add(item.Key, item.Value);
            }
        }
    }
}
