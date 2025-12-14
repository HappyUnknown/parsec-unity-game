using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class BeatFlagItem
    {
        /// <summary>
        /// Pivotal point of a beat flag
        /// </summary>
        public float TimePoint { get; set; }

        /// <summary>
        /// Modular shift value. Applied to TimePoint.
        /// </summary>
        public float ArmAround { get; set; }

        /// <summary>
        /// Flag disposal state
        /// </summary>
        public bool IsTaken { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public BeatFlagItem()
        {
            TimePoint = 0;
            ArmAround = 0;
            IsTaken = false;
        }

        /// <summary>
        /// Default initialization constructor
        /// </summary>
        /// <param name="timePoint"></param>
        /// <param name="armAround"></param>
        public BeatFlagItem(float timePoint, float armAround)
        {
            TimePoint = timePoint;
            ArmAround = armAround;
            IsTaken = false;
        }
    }
}
