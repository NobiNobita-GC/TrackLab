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

        private RecipeNodeItem? _selectedRecipe;

        public RecipeNodeItem? SelectedRecipe
        {
            get => _selectedRecipe;
            set
            {
                if (_selectedRecipe == value)
                {
                    return;
                }

                _selectedRecipe = value;
                NotifyOfPropertyChange();
            }
        }

        public void SelectedItemChanged(RecipeNodeItem? recipeNode)
        {
            if (recipeNode == null || recipeNode.IsFolder)
            {
                SelectedRecipe = null;
                return;
            }

            SelectedRecipe = recipeNode;
        }
    }
}
