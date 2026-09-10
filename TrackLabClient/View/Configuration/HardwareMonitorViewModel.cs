using Core.IO;
using Core.Modules;
using System.Windows;
using UI.Common;

namespace TrackLabClient.View.Configuration
{
    public class HardwareMonitorViewModel : ViewModelBase
    {
        private IReadOnlyList<ModuleBase> _modules = [];
        private ModuleBase? _selectedModule;
        private IReadOnlyList<IOPoint> _dIs = [];
        private IReadOnlyList<IOPoint> _dOs = [];

        public IReadOnlyList<ModuleBase> Modules
        {
            get => _modules;
            private set
            {
                _modules = value;
                NotifyOfPropertyChange();
            }
        }

        public ModuleBase? SelectedModule
        {
            get => _selectedModule;
            set
            {
                if (_selectedModule == value)
                {
                    return;
                }

                _selectedModule = value;
                NotifyOfPropertyChange();

                RefreshIO();
            }
        }

        public IReadOnlyList<IOPoint> DIs
        {
            get => _dIs;
            private set
            {
                _dIs = value;
                NotifyOfPropertyChange();
            }
        }

        public IReadOnlyList<IOPoint> DOs
        {
            get => _dOs;
            private set
            {
                _dOs = value;
                NotifyOfPropertyChange();
            }
        }

        public override void Active()
        {
            base.Active();

            Modules = ModuleManager.Instance.Modules;
            SelectedModule = Modules.FirstOrDefault();
        }

        public override void Deactivate()
        {
            base.Deactivate();
        }

        private void RefreshIO()
        {
            if (SelectedModule == null)
            {
                DIs = [];
                DOs = [];
                return;
            }

            DIs = IOManager.Instance.GetDI(SelectedModule.Name);
            DOs = IOManager.Instance.GetDO(SelectedModule.Name);
        }

        public void ToggleWaferPresent()
        {
            if (SelectedModule == null)
            {
                return;
            }

            IOPoint? ioPoint = DIs.FirstOrDefault(x => x.Name == "WaferPresent");

            if (ioPoint == null)
            {
                return;
            }

            IOManager.Instance.UpdateDI(SelectedModule.Name, ioPoint.Name, !ioPoint.Value);
        }
    }
}
