using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class BeatFlagItem
    {
        /// <summary>
        /// Pivotal point of a beat flag
        /// </summary>
        [SerializeField]
        private float timePoint;

        /// <summary>
        /// Modular shift value. Applied to TimePoint.
        /// </summary>
        [SerializeField]
        private float armAround;

        /// <summary>
        /// Flag disposal state
        /// </summary>
        [SerializeField]
        private bool isTaken;

        public float TimePoint { get { return timePoint; } set { timePoint = value; } }
        public float ArmAround { get { return armAround; } set { armAround = value; } }
        public bool IsTaken { get { return isTaken; } set { isTaken = value; } }

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

        public bool Interferes(BeatFlagItem other, float sensitivity)
        {
            float stillFlagLeft = other.TimePoint - other.ArmAround - sensitivity;
            float stillFlagRight = other.TimePoint + other.ArmAround + sensitivity;
            float iterFlagLeft = this.TimePoint - this.ArmAround;
            float iterFlagRight = this.TimePoint + this.ArmAround;

            bool leftIterInclude = iterFlagLeft > stillFlagLeft && iterFlagLeft < stillFlagRight;
            bool rightIterInclude = iterFlagRight > stillFlagLeft && iterFlagRight < stillFlagRight;
            bool fullStillInclude = iterFlagRight > stillFlagRight && iterFlagLeft < stillFlagLeft;
            bool fullIterInclude = iterFlagRight < stillFlagRight && iterFlagLeft > stillFlagLeft;

            return leftIterInclude || rightIterInclude || fullStillInclude || fullIterInclude;
        }
    }
}
