using Newtonsoft.Json;
using System.Reflection;

namespace TrackLab.Core.Menu
{
    public class MenuManager
    {
        public List<MenuItemEntity> LoadMenu()
        {
            string filePath = Path.Combine(
                AppContext.BaseDirectory,
                "Config",
                "MenuConfig.json");

            string json = File.ReadAllText(filePath);

            List<MenuItemEntity>? menuItems =
                JsonConvert.DeserializeObject<List<MenuItemEntity>>(json);

            if (menuItems == null)
            {
                return [];
            }

            CreateMenu(menuItems);

            return menuItems;
        }

        private void CreateMenu(List<MenuItemEntity> menuItems)
        {
            Assembly? assembly = Assembly.GetEntryAssembly();

            if (assembly == null)
            {
                throw new InvalidOperationException(
                    "无法获取当前应用程序集。");
            }

            foreach (MenuItemEntity item in menuItems)
            {
                if (!string.IsNullOrWhiteSpace(item.ViewModelType))
                {
                    Type? viewModelType =
                        assembly.GetType(item.ViewModelType);

                    if (viewModelType == null)
                    {
                        throw new InvalidOperationException(
                            $"找不到类型：{item.ViewModelType}");
                    }

                    object? viewModel =
                        Activator.CreateInstance(viewModelType);

                    if (viewModel == null)
                    {
                        throw new InvalidOperationException(
                            $"无法创建类型：{item.ViewModelType}");
                    }

                    item.View = viewModel;
                }

                if (item.SubMenuItems != null)
                {
                    CreateMenu(item.SubMenuItems);
                }
            }
        }
    }
}
