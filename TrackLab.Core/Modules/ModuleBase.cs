using TrackLab.Core.StateMachine;

namespace TrackLab.Core.Modules
{
    public abstract class ModuleBase : FSM
    {
        public int Index { get; }
        public string Name { get; }
        public bool IsEnabled { get; set; }

        public abstract ModuleType Type { get; }

        protected ModuleBase(int index, string name)
        {
            Index = index;
            Name = name;
        }

        public bool Initialize()
        {
            return ChangeState(ModuleState.Idle);
        }
       
        public bool RaiseAlarm()
        {
            return ChangeState(ModuleState.Alarm);
        }

        public bool Reset()
        {
            return ChangeState(ModuleState.Idle);
        }

        public bool Disable()
        {
            return ChangeState(ModuleState.Disabled);
        }
    }
}