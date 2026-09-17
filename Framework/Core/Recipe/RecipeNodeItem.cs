using System.Collections.ObjectModel;

namespace Core.Recipe
{
    public class RecipeNodeItem
    {
        public bool IsFolder { get; set; }
        public string Name { get; set; } = string.Empty;
        public string FullPath { get; set; } = string.Empty;
        public string Path => GetRecipePath();
        public string PathWithType => GetPathWithType();
        public string RecipeType => GetRecipeType();

        public RecipeData? RecipeData { get; set; }

        public RecipeNodeItem? Parent { get; set; }

        public ObservableCollection<RecipeNodeItem> SubNodes { get; set; } = [];

        private string GetPathWithType()
        {
            string path = System.IO.Path.GetRelativePath(RecipeManager.Instance.GetRootPath(), FullPath);

            if (!IsFolder)
            {
                path = System.IO.Path.ChangeExtension(path, null) ?? string.Empty;
            }

            return path;
        }

        private string GetRecipeType()
        {
            string path = GetPathWithType();
            string[] paths = path.Split(System.IO.Path.DirectorySeparatorChar);

            return paths.FirstOrDefault() ?? string.Empty;
        }

        private string GetRecipePath()
        {
            string path = GetPathWithType();
            int index = path.IndexOf(System.IO.Path.DirectorySeparatorChar);

            if (index < 0)
            {
                return string.Empty;
            }

            return path.Substring(index + 1);
        }
        public RecipeNodeItem()
        {
        }

        public RecipeNodeItem(DirectoryInfo directory, RecipeNodeItem? parent)
        {
            Name = directory.Name;
            FullPath = directory.FullName;
            Parent = parent;
            IsFolder = true;

            foreach (DirectoryInfo subDirectory in directory.GetDirectories())
            {
                SubNodes.Add(new RecipeNodeItem(subDirectory, this));
            }

            foreach (FileInfo file in directory.GetFiles("*.json"))
            {
                SubNodes.Add(new RecipeNodeItem(file, this));
            }
        }

        public RecipeNodeItem(FileInfo file, RecipeNodeItem parent)
        {
            Name = System.IO.Path.GetFileNameWithoutExtension(file.Name);
            FullPath = file.FullName;
            Parent = parent;
            IsFolder = false;

            string content = File.ReadAllText(FullPath);
            RecipeData = RecipeData.FromJsonString(content);
        }

        public void Load()
        {
            if (!File.Exists(FullPath) || RecipeData == null)
            {
                return;
            }

            RecipeData? recipeData =
                RecipeData.FromJsonString(File.ReadAllText(FullPath));

            RecipeData.Copy(recipeData);
        }

        public void Save()
        {
            if (IsFolder || RecipeData == null)
            {
                return;
            }

            string content = RecipeData.ToJsonString();
            RecipeManager.Instance.SaveRecipeData(FullPath, content);
        }
    }
}
