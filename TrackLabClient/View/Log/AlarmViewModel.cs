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
                Time = "09:20:15",
                Module = "Robot",
                Level = "Error",
                Message = "Communication timeout"
            },

            new AlarmItem
            {
                Time = "09:21:32",
                Module = "LoadPort",
                Level = "Warning",
                Message = "Door open failed"
            }
        ];
    }
}
