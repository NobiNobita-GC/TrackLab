using Core.Modules;

namespace TrackLabClient.Modules.Robot;

public class RobotModule : RobotModuleBase
{
    public override ModuleType Type => ModuleType.Robot;

    public RobotModule(int index, string name) : base(index, name)
    {
    }
}
