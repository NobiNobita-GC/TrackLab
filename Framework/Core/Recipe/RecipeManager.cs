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

            for (int i = 0; i < paths.Length; i++)
            {
                string item = paths[i];

                //配方节点文件
                bool isLastItem = i == paths.Length - 1;

                //配方文件节点，而不是碰巧同名的文件夹
                findNode = findNode?.SubNodes.FirstOrDefault(n => n.Name.Equals(item, StringComparison.OrdinalIgnoreCase) && n.IsFolder != isLastItem);

                if (findNode == null)
                {
                    break;
                }
            }
            return findNode;
        }

        public IEnumerable<RecipeNodeItem> FindAllRecipe()
        {
            return FindAllNode(_recipeNodeItem).Where(x => !x.IsFolder);
        }

        private IEnumerable<RecipeNodeItem> FindAllNode(RecipeNodeItem node)
        {
            List<RecipeNodeItem> nodes = [node];

            foreach (RecipeNodeItem child in node.SubNodes)
            {
                nodes.AddRange(FindAllNode(child));
            }

            return nodes;
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

        //生成不重复的配方副本名称
        public string GetCopyName(string sourceName, RecipeNodeItem parentNode)
        {
            for (int i = 1; i < int.MaxValue; i++)
            {
                string newName = $"{sourceName}({i})";
                string newPath = Path.Combine(parentNode.FullPath, $"{newName}.json");

                if (!File.Exists(newPath))
                {
                    return newName;
                }
            }

            return string.Empty;
        }
    }
}