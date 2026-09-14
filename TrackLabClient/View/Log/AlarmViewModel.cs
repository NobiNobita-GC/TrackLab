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
                Name = "CommunicationTimeout",
                Level = AlarmLevel.Error,
                Message = "Communication timeout",
                Cause = "Robot controller is not responding",
                Solution = "Check the robot controller"
            },

            new AlarmItem
            {
                Time = DateTime.Now,
                Module = "LoadPort",
                Name = "DoorOpenFailed",
                Level = AlarmLevel.Warning,
                Message = "Door open failed",
                Cause = "Door sensor is abnormal",
                Solution = "Check the door sensor"
            }
        ];

        /// <summary>当前表格实际展示的告警，为 Alarms 按查询条件过滤后的结果。</summary>
        public ObservableCollection<AlarmItem> DisplayedAlarms { get; } = [];

        private string _searchName = string.Empty;

        public string SearchName
        {
            get => _searchName;
            set
            {
                if (_searchName == value)
                    return;

                _searchName = value;
                NotifyOfPropertyChange();
            }
        }

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

        private bool _canCancelQuery;

        public bool CanCancelQuery
        {
            get => _canCancelQuery;
            set
            {
                if (_canCancelQuery == value)
                    return;
                _canCancelQuery = value;
                NotifyOfPropertyChange();
            }
        }

        public AlarmViewModel()
        {
            RefreshDisplayedAlarms();
        }

        /// <summary>按当前 SearchMessage 过滤告警，供 Query 按钮调用。</summary>
        public void Query()
        {
            RefreshDisplayedAlarms();
            CanCancelQuery = true;
        }

        public void CancelQuery()
        {
            SearchName = string.Empty;
            SearchMessage = string.Empty;
            RefreshDisplayedAlarms();
            CanCancelQuery = false;
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
                Name = "TestInfoAlarm",
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
            string nameKeyword = SearchName?.Trim() ?? string.Empty;
            string messageKeyword = SearchMessage?.Trim() ?? string.Empty;

            bool isNameMatch = nameKeyword.Length == 0
                               || alarm.Name.Contains(nameKeyword, StringComparison.OrdinalIgnoreCase);

            bool isMessageMatch = messageKeyword.Length == 0
                                  || alarm.Message.Contains(messageKeyword, StringComparison.OrdinalIgnoreCase);

            return isNameMatch && isMessageMatch;
        }
    }
}
