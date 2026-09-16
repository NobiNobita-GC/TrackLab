using System.Collections.ObjectModel;

namespace Core.Recipe
{
    public class RecipeNodeItem
    {
        public bool IsFolder { get; set; }

        public string Name { get; set; } = string.Empty;

        public string FullPath { get; set; } = string.Empty;

        public RecipeData? RecipeData { get; set; }

        public RecipeNodeItem? Parent { get; set; }

        public ObservableCollection<RecipeNodeItem> SubNodes { get; set; } = [];

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
            Name = Path.GetFileNameWithoutExtension(file.Name);
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
            File.WriteAllText(FullPath, content);
        }
    }
}