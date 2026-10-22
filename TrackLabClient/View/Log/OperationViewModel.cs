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

        private int _currentPage = 1;

        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                if (_currentPage == value)
                    return;
                _currentPage = value;
                NotifyOfPropertyChange();
            }
        }

        private int PageSize { get; } = 5;

        private bool _isQueryMode;

        public bool CanPreviousPage =>
            _isQueryMode && CurrentPage > 1;

        public bool CanNextPage =>
            _isQueryMode && Logs.Count == PageSize;

        public override void Active()
        {
            base.Active();
            Refresh();
        }


        public void Refresh()
        {
            _isQueryMode = false;
            CurrentPage = 1;

            Logs.Clear();

            foreach (OperationLogEntity log in
                     DBManager.Instance.GetOperationLogs())
            {
                Logs.Add(log);
            }

            UpdatePageButtons();
        }

        public void Query()
        {
            if (TryLoadPage(1))
            {
                _isQueryMode = true;
                UpdatePageButtons();
            }
        }

        public void PreviousPage()
        {
            if (!CanPreviousPage)
                return;

            TryLoadPage(CurrentPage - 1);
        }

        public void NextPage()
        {
            if (!CanNextPage)
                return;

            TryLoadPage(CurrentPage + 1);
        }

        private bool TryLoadPage(int page)
        {
            if (page < 1)
                return false;

            DateTime beginTime = BeginTime.Date;
            DateTime endTime = EndTime.Date.AddDays(1).AddTicks(-1);

            if (beginTime > endTime)
                return false;

            List<OperationLogEntity> logs =
                DBManager.Instance.GetOperationLogs(
                    beginTime,
                    endTime,
                    page,
                    PageSize);

            // 不允许翻到不存在的下一页
            if (page > 1 && logs.Count == 0)
                return false;

            CurrentPage = page;

            Logs.Clear();

            foreach (OperationLogEntity log in logs)
            {
                Logs.Add(log);
            }

            UpdatePageButtons();
            return true;
        }

        private void UpdatePageButtons()
        {
            NotifyOfPropertyChange(nameof(CanPreviousPage));
            NotifyOfPropertyChange(nameof(CanNextPage));
        }

    }
}
