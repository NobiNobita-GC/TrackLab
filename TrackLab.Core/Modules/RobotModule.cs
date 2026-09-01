namespace TrackLab.Core.Modules
{
    public class RobotModule : ModuleBase
    {
        public override ModuleType Type => ModuleType.Robot;

        public RobotModule(int index, string name) : base(index, name)
        {
        }
    }
}
