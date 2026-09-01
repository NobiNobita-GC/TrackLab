namespace TrackLab.Core.Modules;

public class LoadPortModule : ModuleBase
{
    public LoadPortModule(int index, string name) : base(index, name)
    {
    }

    public override ModuleType Type => ModuleType.LoadPort;

    public bool HasCarrier { get; set; }
}