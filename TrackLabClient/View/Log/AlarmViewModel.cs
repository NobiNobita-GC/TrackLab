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
                Level = "Error",
                Message = "Communication timeout"
            },

            new AlarmItem
            {
                Time = DateTime.Now,
                Module = "LoadPort",
                Level = "Warning",
                Message = "Door open failed"
            }
        ];

        public void AddTestAlarm()
        {
            Alarms.Add(new AlarmItem
            {
                Time = DateTime.Now,
                Module = "Robot",
                Level = "Warning",
                Message = "Test alarm"
            });
        }
    }
}
