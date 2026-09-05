using Core.Modules;
using UI.Common;

namespace TrackLabClient.View.Status
{
    public class HotPlateModuleViewModel : ViewModelBase
    {
        public ModuleBase Module { get; }

        public HotPlateModuleViewModel(ModuleBase module)
        {
            Module = module;
            DisplayName = module.Name;
        }

        public void StartProcess()
        {
            if (Module is ProcessModuleBase processModule)
            {
                processModule.StartProcess();
            }
        }

        public void CompleteProcess()
        {
            if (Module is ProcessModuleBase processModule)
            {
                processModule.CompleteProcess();
            }
        }
    }
}
