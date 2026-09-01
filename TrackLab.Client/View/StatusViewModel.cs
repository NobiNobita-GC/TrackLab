using TrackLab.Core.Modules;
using TrackLab.UI.Common;

namespace TrackLab.Client.View
{
    public class StatusViewModel : ViewModelBase
    {
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
    }
}
