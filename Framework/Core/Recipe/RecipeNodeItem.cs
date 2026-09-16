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
        }
    }
}