using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class BeatFlagItem
    {
        public float TimePoint { get; set; }
        public float ArmAround { get; set; }
        public bool IsTaken { get; set; }
        public BeatFlagItem()
        {
            TimePoint = 0;
            ArmAround = 0;
            IsTaken = false;
        }
        public BeatFlagItem(float timePoint, float armAround, bool isTaken = false)
        {
            TimePoint = timePoint;
            ArmAround = armAround;
            IsTaken = isTaken;
        }
    }
}
