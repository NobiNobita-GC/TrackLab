using Core.Alarm;
using System.Collections.ObjectModel;
using UI.Common;

namespace TrackLabClient.View.Log
{
    public class AlarmViewModel : ViewModelBase
    {
        public ObservableCollection<AlarmItem> Alarms { get; } =
        [
            new AlarmItem
            {
                Time = DateTime.Now,
                Module = "Robot",
                Level = AlarmLevel.Error,
                Message = "Communication timeout"
            },

            new AlarmItem
            {
                Time = DateTime.Now,
                Module = "LoadPort",
                Level = AlarmLevel.Warning,
                Message = "Door open failed"
            }
        ];

        private AlarmItem? _selectedAlarm;
        public AlarmItem? SelectedAlarm
        {
            get => _selectedAlarm;
            set
            {
                if (_selectedAlarm == value)
                    return;

                _selectedAlarm = value;
                NotifyOfPropertyChange();
            }
        }

        public void AddTestAlarm()
        {
            Alarms.Add(new AlarmItem
            {
                Time = DateTime.Now,
                Module = "Robot",
                Level = AlarmLevel.Info,
                Message = "Test Info alarm"
            });
        }
    }
}
