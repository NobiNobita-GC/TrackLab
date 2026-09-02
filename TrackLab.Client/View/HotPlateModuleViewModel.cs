using TrackLab.Core.Modules;
using TrackLab.UI.Common;

namespace TrackLab.Client.View
{
    public class HotPlateModuleViewModel : ViewModelBase
    {
        public ModuleBase Module { get; }

        public HotPlateModuleViewModel(ModuleBase module)
        {
            Module = module;
            DisplayName = module.Name;
        }
    }
}
