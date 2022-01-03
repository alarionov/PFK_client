using PFK.Shared;

namespace PFK.Acts.Act001.SidequestInfo
{
    public class LevelDependency : LevelDependencies
    {
        public bool Locked { get; private set; }

        protected override void WhenSatisfied()
        {
            Locked = false;
        }

        protected override void WhenNotSatisfied()
        {
            Locked = true;
        }
    }
}
