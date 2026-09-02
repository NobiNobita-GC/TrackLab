namespace TrackLab.Core.Modules
{
    public class CoolPlateModule : ProcessModuleBase
    {
        public override ModuleType Type => ModuleType.CoolPlate;

        public CoolPlateModule(int index, string name) : base(index, name)
        {
        }

        public double ActualTemperature { get; set; }
    }
}
