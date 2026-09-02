using System.ComponentModel;
using System.Runtime.CompilerServices;
using TrackLab.Core.Modules;

namespace TrackLab.Core.StateMachine
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

        protected void ChangeState(ModuleState state)
        {
            State = state;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}