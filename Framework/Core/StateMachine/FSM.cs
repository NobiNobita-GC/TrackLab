using System.ComponentModel;
using System.Runtime.CompilerServices;
using Core.Modules;

namespace Core.StateMachine
{
    public abstract class FSM : INotifyPropertyChanged
    {
        private ModuleState _state = ModuleState.Unknown;

        public ModuleState State
        {
            get => _state;
            protected set
            {
                if (_state == value)
                {
                    return;
                }

                _state = value;
                OnPropertyChanged();
            }
        }

        protected bool ChangeState(ModuleState newState)
        {
            if (!CanChangeState(State, newState))
            {
                return false;
            }

            State = newState;
            return true;
        }

        private bool CanChangeState(ModuleState currentState, ModuleState newState)
        {
            return currentState switch
            {
                ModuleState.Unknown => newState == ModuleState.Idle,

                ModuleState.Idle =>
                    newState == ModuleState.Running ||
                    newState == ModuleState.Alarm ||
                    newState == ModuleState.Disabled,

                ModuleState.Running =>
                    newState == ModuleState.Idle ||
                    newState == ModuleState.Alarm,

                ModuleState.Alarm =>
                    newState == ModuleState.Idle,

                ModuleState.Disabled =>
                    newState == ModuleState.Idle,

                _ => false
            };
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
