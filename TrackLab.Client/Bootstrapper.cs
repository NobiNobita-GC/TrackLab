using Caliburn.Micro;
using System.Windows;
using TrackLab.Client.View;
using TrackLab.Core.IO;
using TrackLab.Core.Modules;

namespace TrackLab.Client
{
    public class Bootstrapper : BootstrapperBase
    { 
        public Bootstrapper()
        {
            Initialize();
        }

        protected override async void OnStartup(object sender, StartupEventArgs e)
        {
            InitializeRuntimeData();

            await DisplayRootViewForAsync<HomeViewModel>();
        }

        private void InitializeRuntimeData()
        {
            ModuleManager moduleManager = ModuleManager.Instance;
            IOManager ioManager = IOManager.Instance;

            for (int i = 1; i <= 4; i++)
            {
                moduleManager.Add(new LoadPortModule(100 + i, $"LP{i:00}"));
            }

            moduleManager.Add(new RobotModule(150, "ROBOT01"));

            for (int i = 1; i <= 10; i++)
            {
                moduleManager.Add(new HotPlateModule(200 + i, $"HP{i:00}"));
            }

            for (int i = 1; i <= 6; i++)
            {
                moduleManager.Add(new CoolPlateModule(300 + i, $"CP{i:00}"));
            }

            IOPoint waferPresent = new IOPoint(1, "WaferPresent", IOType.DI);
            waferPresent.SetValue(true);

            IOPoint vacuumOK = new IOPoint(2, "VacuumOK", IOType.DI);
            vacuumOK.SetValue(true);

            IOPoint vacuumValve = new IOPoint(1, "VacuumValve", IOType.DO);
            vacuumValve.SetValue(false);

            IOPoint heaterOn = new IOPoint(2, "HeaterOn", IOType.DO);
            heaterOn.SetValue(true);

            ioManager.AddDI("HP01", waferPresent);
            ioManager.AddDI("HP01", vacuumOK);
            ioManager.AddDO("HP01", vacuumValve);
            ioManager.AddDO("HP01", heaterOn);

            ioManager.AddDI("HP02", new IOPoint(1, "WaferPresent", IOType.DI));
            ioManager.AddDO("HP02", new IOPoint(1, "VacuumValve", IOType.DO));

            ioManager.AddDI("CP01", new IOPoint(1, "WaferPresent", IOType.DI));
            ioManager.AddDO("CP01", new IOPoint(1, "CoolingValve", IOType.DO));

            ioManager.AddDI("LP01", new IOPoint(1, "CarrierPresent", IOType.DI));
            ioManager.AddDO("LP01", new IOPoint(1, "DoorOpen", IOType.DO));
        }
    }
}
