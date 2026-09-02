using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using TrackLab.Core.Modules;
using TrackLab.UI.Common;
using TrackLab.UI.Control;
using Caliburn.Micro;
using System.Threading.Tasks;
using System.Windows.Input;

namespace TrackLab.Client.View
{
    public class StatusViewModel : ViewModelBase
    {
        private readonly IWindowManager _windowManager = new WindowManager();

        private Grid? _layout;

        public Grid? Layout
        {
            get => _layout;
            private set
            {
                if (_layout == value)
                {
                    return;
                }

                _layout = value;
                NotifyOfPropertyChange(nameof(Layout));
            }
        }
        public override void Active()
        {
            base.Active();
            LoadLayout();
        }

        private void LoadLayout()
        {
            string path = Path.Combine(AppContext.BaseDirectory, "Config", "LayoutConfig.xaml");

            if (!File.Exists(path))
            {
                throw new FileNotFoundException("找不到 LayoutConfig.xaml", path);
            }

            using FileStream stream = File.OpenRead(path);

            Grid layout = (Grid)XamlReader.Load(stream);

            ReplaceModuleTextBlocks(layout);

            Layout = layout;
        }
        private void ReplaceModuleTextBlocks(Grid grid)
        {
            for (int i = 0; i < grid.Children.Count; i++)
            {
                if (grid.Children[i] is TextBlock textBlock)
                {
                    string moduleName = textBlock.Text?.Trim() ?? "";

                    ModuleBase? module = ModuleManager.Instance.GetByName(moduleName);

                    if (module == null)
                    {
                        continue;
                    }

                    ModuleControl moduleControl = new ModuleControl
                    {
                        ModuleEntity = module,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch,
                        Cursor = Cursors.Hand
                    };

                    moduleControl.MouseLeftButtonUp += async (_, _) =>
                    {
                        await OpenModuleDetail(module);
                    };

                    Grid.SetRow(moduleControl, Grid.GetRow(textBlock));
                    Grid.SetColumn(moduleControl, Grid.GetColumn(textBlock));
                    Grid.SetRowSpan(moduleControl, Grid.GetRowSpan(textBlock));
                    Grid.SetColumnSpan(moduleControl, Grid.GetColumnSpan(textBlock));

                    grid.Children.RemoveAt(i);
                    grid.Children.Insert(i, moduleControl);
                }
                else if (grid.Children[i] is Grid childGrid)
                {
                    ReplaceModuleTextBlocks(childGrid);
                }
            }
        }
        private async Task OpenModuleDetail(ModuleBase module)
        {
            string viewModelName = $"{module.Type}ModuleViewModel";

            string fullName = $"TrackLab.Client.View.{viewModelName}";

            Type? viewModelType = typeof(StatusViewModel).Assembly.GetType(fullName);

            if (viewModelType == null)
            {
                ModuleDetailViewModel detailViewModel = new ModuleDetailViewModel(module);
                await _windowManager.ShowDialogAsync(detailViewModel);
                return;
            }

            object? viewModel = Activator.CreateInstance(viewModelType, module);

            if (viewModel == null)
            {
                return;
            }

            await _windowManager.ShowDialogAsync(viewModel);
        }

        public IReadOnlyList<ModuleBase> Modules { get; }

        public StatusViewModel()
        {
            Modules = ModuleManager.Instance.Modules;
        }

        public void ChangeHp01State()
        {
            ModuleBase? module = ModuleManager.Instance.GetByName("HP01");

            if (module == null)
            {
                return;
            }

            ModuleState nextState = module.State switch
            {
                ModuleState.Unknown => ModuleState.Idle,
                ModuleState.Idle => ModuleState.Running,
                ModuleState.Running => ModuleState.Alarm,
                ModuleState.Alarm => ModuleState.Disabled,
                _ => ModuleState.Unknown
            };

            module.SetState(nextState);
        }

        private ModuleBase? _selectedModule;

        public ModuleBase? SelectedModule
        {
            get => _selectedModule;
            set
            {
                if (_selectedModule == value)
                {
                    return;
                }

                _selectedModule = value;
                NotifyOfPropertyChange(nameof(SelectedModule));
            }
        }

        public void ModuleControlClick(ModuleBase module)
        {
            SelectedModule = module;
        }
    }
}
