using Core.DataBase;
using Core.Log;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UI.Common;

namespace TrackLabClient.View.Log
{
    public class OperationViewModel : ViewModelBase
    {
        public ObservableCollection<OperationLogEntity> Logs { get; } = [];

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
    }
}
