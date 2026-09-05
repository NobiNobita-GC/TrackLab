using System.ComponentModel;

namespace Core.IO
{
    public class IOPoint : INotifyPropertyChanged
    {
        public IOPoint(int index, string name, IOType type)
        {
            Index = index;
            Name = name;
            Type = type;
        }

        public int Index { get; }
        public string Name { get; }
        public IOType Type { get; }
        public bool Value { get; private set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        public void SetValue(bool value)
        {
            if (Value == value)
            {
                return;
            }

            Value = value;

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Value)));
        }
    }
}
