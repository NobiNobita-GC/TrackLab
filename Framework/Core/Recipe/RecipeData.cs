using System.Collections.ObjectModel;

namespace Core.Recipe
{
    public class RecipeData
    {
        public Dictionary<string, string> Header { get; set; } = [];
        public ObservableCollection<Dictionary<string, string>> Step { get; set; } = [];
        public Dictionary<string, string> Config { get; set; } = [];
    }
}
