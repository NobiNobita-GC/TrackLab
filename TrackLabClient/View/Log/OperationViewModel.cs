using Core.DataBase;
using Core.Log;
using System.Collections.ObjectModel;
using UI.Common;

namespace TrackLabClient.View.Log
{
    public class OperationViewModel : ViewModelBase
    {
        public ObservableCollection<OperationLogEntity> Logs { get; } = [];

        private DateTime _beginTime = DateTime.Today;

        public DateTime BeginTime
        {
            get => _beginTime;
            set
            {
                if (_beginTime == value)
                    return;

                _beginTime = value;
                NotifyOfPropertyChange();
            }
        }

        private DateTime _endTime = DateTime.Today.AddDays(1).AddTicks(-1);

        public DateTime EndTime
        {
            get => _endTime;
            set
            {
                if (_endTime == value)
                    return;

                _endTime = value;
                NotifyOfPropertyChange();
            }
        }
        public override void Active()
        {
            base.Active();
            Refresh();
        }

        public void Refresh()
        {
            Logs.Clear();
            List<OperationLogEntity> logs = DBManager.Instance.GetOperationLogs();
            foreach (var log in logs)
            {
                Logs.Add(log);
            }
        }

        public void Query()
        {
            DateTime beginTime = BeginTime.Date;
            DateTime endTime = EndTime.Date.AddDays(1).AddTicks(-1);

            Logs.Clear();

            List<OperationLogEntity> logs =
                DBManager.Instance.GetOperationLogs(
                    beginTime,
                    endTime);

            foreach (OperationLogEntity log in logs)
            {
                Logs.Add(log);
            }
        }
    }
}
