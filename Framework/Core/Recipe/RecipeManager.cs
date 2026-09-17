using System.Collections.ObjectModel;

namespace Core.Recipe
{
    public sealed class RecipeManager
    {
        public static RecipeManager Instance { get; } = new();

        private readonly string _rootPath;
        private readonly RecipeNodeItem _recipeNodeItem;

        private RecipeManager()
        {
            _rootPath = Path.Combine(
                AppContext.BaseDirectory,
                "Recipes");

            DirectoryInfo rootDirectory = Directory.CreateDirectory(_rootPath);

            _recipeNodeItem = new RecipeNodeItem(rootDirectory, null);
        }

        public ObservableCollection<RecipeNodeItem> GetRecipes()
        {
            return _recipeNodeItem.SubNodes;
        }

        public RecipeNodeItem? FindNodeByPath(string path)
        {
            RecipeNodeItem? findNode = _recipeNodeItem;
            string[] paths = path.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries);

            for(int i = 0; i < paths.Length; i++)
            {
                string item = paths[i];
                bool isLastItem = i == paths.Length - 1;

                findNode = findNode?.SubNodes.FirstOrDefault(n => n.Name.Equals(item, StringComparison.OrdinalIgnoreCase));

                if (findNode == null)
                {
                    break;
                }
            }
            return findNode;
        }

        public string GetRootPath()
        {
            return _rootPath;
        }
        public void SaveRecipeData(string fullFilePath, string content)
        {
            if (File.Exists(fullFilePath))
            {
                File.WriteAllText(fullFilePath, content);
            }
        }
    }
}