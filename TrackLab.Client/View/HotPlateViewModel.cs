using TrackLab.Core.Modules;
using TrackLab.UI.Common;

namespace TrackLab.Client.View
{
    public class HotPlateViewModel : ViewModelBase
    {
        public ModuleBase Module { get; }

        public HotPlateViewModel(ModuleBase module)
        {
            Module = module;
            DisplayName = module.Name;
        }
    }
}
