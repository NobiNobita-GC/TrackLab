namespace Core.Recipe
{
    public sealed class RecipeManager
    {
        public static RecipeManager Instance { get; } = new();

        private readonly string _rootPath;

        private RecipeManager()
        {
            _rootPath = Path.Combine(
                AppContext.BaseDirectory,
                "Recipes");

            Directory.CreateDirectory(_rootPath);
        }

        public string GetRootPath()
        {
            return _rootPath;
        }
    }
}