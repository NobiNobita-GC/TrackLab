using Core.Alarm;
using System.Collections.ObjectModel;
using UI.Common;

namespace TrackLabClient.View.Log
{
    public class AlarmViewModel : ViewModelBase
    {
        /// <summary>全量告警数据源。</summary>
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

        /// <summary>当前表格实际展示的告警，为 Alarms 按查询条件过滤后的结果。</summary>
        public ObservableCollection<AlarmItem> DisplayedAlarms { get; } = [];

        private string _searchMessage = string.Empty;

        public string SearchMessage
        {
            get => _searchMessage;
            set
            {
                if (_searchMessage == value)
                    return;

                _searchMessage = value;
                NotifyOfPropertyChange();
            }
        }

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

        public AlarmViewModel()
        {
            RefreshDisplayedAlarms();
        }

        /// <summary>按当前 SearchMessage 过滤告警，供 Query 按钮调用。</summary>
        public void Query()
        {
            RefreshDisplayedAlarms();
        }

        public void RemoveSelectedAlarm()
        {
            if (SelectedAlarm == null)
                return;

            Alarms.Remove(SelectedAlarm);
            RefreshDisplayedAlarms();
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

            RefreshDisplayedAlarms();
        }

        private void RefreshDisplayedAlarms()
        {
            DisplayedAlarms.Clear();

            foreach (AlarmItem alarm in Alarms)
            {
                if (IsMatchSearchCondition(alarm))
                {
                    DisplayedAlarms.Add(alarm);
                }
            }
        }

        private bool IsMatchSearchCondition(AlarmItem alarm)
        {
            string keyword = SearchMessage?.Trim() ?? string.Empty;

            // 未输入关键字时展示全部
            if (keyword.Length == 0)
                return true;

            return alarm.Message.Contains(keyword, StringComparison.OrdinalIgnoreCase);
        }
    }
}
