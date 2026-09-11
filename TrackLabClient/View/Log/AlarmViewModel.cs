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
                Message = "Communication timeout",
                Cause = "Robot controller is not responding",
                Solution = "Check the robot controller"
            },

            new AlarmItem
            {
                Time = DateTime.Now,
                Module = "LoadPort",
                Level = AlarmLevel.Warning,
                Message = "Door open failed",
                Cause = "Door sensor is abnormal",
                Solution = "Check the door sensor"
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
                NotifyOfPropertyChange(nameof(CanRemoveSelectedAlarm));
            }
        }

        public bool CanRemoveSelectedAlarm => SelectedAlarm != null;

        public void RemoveSelectedAlarm()
        {
            if (SelectedAlarm == null)
                return;

            Alarms.Remove(SelectedAlarm);
        }

        public void AddTestAlarm()
        {
            Alarms.Add(new AlarmItem
            {
                Time = DateTime.Now,
                Module = "Robot",
                Level = AlarmLevel.Info,
                Message = "Test Info alarm",
                Cause = "Test alarm",
                Solution = "No solution"
            });
        }
    }
}
