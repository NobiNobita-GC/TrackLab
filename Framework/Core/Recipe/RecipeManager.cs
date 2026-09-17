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

        public string GetRootPath()
        {
            return _rootPath;
        }
    }
}