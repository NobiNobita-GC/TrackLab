namespace TrackLab.Core.Modules
{
    public class HotPlateModule : ModuleBase
    {
        public HotPlateModule(int index, string name) : base(index, name)
        {
        }

        public override ModuleType Type => ModuleType.HotPlate;

        public double SetTemperature { get; set; }
        public double ActualTemperature { get; set; }
    }
}
