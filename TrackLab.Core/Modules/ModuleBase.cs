using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TrackLab.Core.Modules
{
    public abstract class ModuleBase : INotifyPropertyChanged
    {
        private ModuleState _state = ModuleState.Unknown;

        public int Index { get; }
        public string Name { get; }
        public bool IsEnabled { get; set; }

        public ModuleState State
        {
            get => _state;
            private set
            {
                if (_state == value)
                {
                    return;
                }

                _state = value;
                OnPropertyChanged();
            }
        }

        public abstract ModuleType Type { get; }

        protected ModuleBase(int index, string name)
        {
            Index = index;
            Name = name;
        }

        public void SetState(ModuleState state)
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