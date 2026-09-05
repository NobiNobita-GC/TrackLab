using Caliburn.Micro;
using System.IO;
using System.Windows;
using TrackLabClient.View;
using Core.IO;
using Core.Modules;

namespace TrackLabClient
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

            string moduleConfigPath = Path.Combine(AppContext.BaseDirectory, "Config", "ModuleConfig.json");

            // 反射创建对象并加载模块配置
            moduleManager.Load(moduleConfigPath, typeof(Bootstrapper).Assembly);

            // 初始化所有模块
            moduleManager.InitializeAll();

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
