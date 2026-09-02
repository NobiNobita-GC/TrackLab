namespace TrackLab.Core.Modules
{
    public class HotPlateModule : ProcessModuleBase
    {
        public override ModuleType Type => ModuleType.HotPlate;

        public HotPlateModule(int index, string name) : base(index, name)
        {
        }

        public double SetTemperature { get; set; }
        public double ActualTemperature { get; set; }
    }
}
