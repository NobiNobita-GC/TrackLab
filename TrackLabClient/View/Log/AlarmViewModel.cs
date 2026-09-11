using System.Collections.ObjectModel;
using UI.Common;

namespace TrackLabClient.View.Log 
{ 
    public class AlarmViewModel : ViewModelBase 
    { 
        public ObservableCollection<string> Alarms { get; } = 
            [
            "Robot communication timeout", 
            "LoadPort door open failed"
            ]; 
    } 
}
