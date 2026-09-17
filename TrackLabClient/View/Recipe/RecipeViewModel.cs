using Core.Recipe;
using System.Collections.ObjectModel;
using UI.Common;

namespace TrackLabClient.View.Recipe
{
    public class RecipeViewModel : ViewModelBase
    {
        public ObservableCollection<RecipeNodeItem> RecipeNodes { get; }

        public string RootPath => RecipeManager.Instance.GetRootPath();

        public int RecipeCount => RecipeManager.Instance.FindAllRecipe().Count();

        public RecipeViewModel()
        {
            RecipeNodes = RecipeManager.Instance.GetRecipes();
        }
    }
}
