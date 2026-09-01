namespace TrackLab.Core.Modules;

public class CoolPlateModule : ModuleBase
{
    public CoolPlateModule(int index, string name) : base(index, name)
    {
    }

    public override ModuleType Type => ModuleType.CoolPlate;

    public double ActualTemperature { get; set; }
}
