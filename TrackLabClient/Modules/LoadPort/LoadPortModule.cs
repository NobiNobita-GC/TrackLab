using Core.Modules;

namespace TrackLabClient.Modules.LoadPort;
public class LoadPortModule : CarrierModuleBase
{
    public override ModuleType Type => ModuleType.LoadPort;

    public LoadPortModule(int index, string name) : base(index, name)
    {
    }

    public bool HasCarrier { get; set; }
}
