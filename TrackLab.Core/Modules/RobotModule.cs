namespace TrackLab.Core.Modules;

public class RobotModule : RobotModuleBase
{
    public override ModuleType Type => ModuleType.Robot;

    public RobotModule(int index, string name) : base(index, name)
    {
    }
}
