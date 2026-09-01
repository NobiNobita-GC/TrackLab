using System.Runtime.Versioning;
using System.Windows;
using TrackLab.Core.Menu;
using TrackLab.UI.Common;

namespace TrackLab.Client.View
{
    public class HomeViewModel : ViewModelBase
    {

        private ViewModelBase? _currentView;

        public ViewModelBase? CurrentView
        {
            get
            {
                return _currentView;
            }

            set
            {
                if (_currentView == value)
                {
                    return;
                }
                if (_currentView != null)
                {
                    _currentView?.Deactivate();
                }

                _currentView = value;
                NotifyOfPropertyChange();

                if (_currentView != null)
                {
                    _currentView?.Active();
                }
            }
        }

        public List<MenuItemEntity> MenuItems { get; }

        private List<MenuItemEntity> _subMenuItems = [];
        public List<MenuItemEntity> SubMenuItems
        {
            get => _subMenuItems;
            set
            {
                _subMenuItems = value;
                NotifyOfPropertyChange();
            }
        }

        private MenuItemEntity? _selectedMainMenu;

        public MenuItemEntity? SelectedMainMenu
        {
            get => _selectedMainMenu;
            set
            {
                _selectedMainMenu = value;
                NotifyOfPropertyChange();
            }
        }

        public void SelectMainMenu(MenuItemEntity menuItem, FrameworkElement source)
        {
            SelectedMainMenu = menuItem;

            if (menuItem.SubMenuItems == null || menuItem.SubMenuItems.Count == 0)
            {               
                OpenMenu(menuItem);
                return;
            }

            if (GetView() is not HomeView view)
            {
                return;
            }

            if (view.MenuPopup.IsOpen && ReferenceEquals(view.MenuPopup.PlacementTarget, source))
            {
                view.MenuPopup.IsOpen = false;
                return;
            }

            SubMenuItems = menuItem.SubMenuItems;
            view.MenuPopup.PlacementTarget = source;
            view.MenuPopup.IsOpen = true;
        }

        public void SelectSubMenu(MenuItemEntity menuItem)
        {
            OpenMenu(menuItem);

            if (GetView() is HomeView view)
            {
                view.MenuPopup.IsOpen = false;
            }
        }

        public HomeViewModel()
        {
            MenuManager menuManager = new MenuManager();

            MenuItems = menuManager.LoadMenu();

            MenuItemEntity? homeMenu = FindMenuItem(MenuItems, "Home Page");
            if (homeMenu != null)
            {
                OpenMenu(homeMenu);
            }

        }

        public void OpenMenu(MenuItemEntity menuItem)
        {
            if (menuItem.View is not ViewModelBase viewModel)
            {
                return;
            }

            CurrentView = viewModel;
        }

        private MenuItemEntity? FindMenuItem(IEnumerable<MenuItemEntity> menuItems, string name)
        {
            foreach (MenuItemEntity item in menuItems)
            {
                if (item.Name == name)
                {
                    return item;
                }

                if (item.SubMenuItems == null)
                {
                    continue;
                }

                MenuItemEntity? result =
                    FindMenuItem(item.SubMenuItems, name);

                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }
    }
}
