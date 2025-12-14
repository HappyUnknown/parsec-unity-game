using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class BeatFlagController
    {
        /// <summary>
        /// Contains whole batch of beat flags with timePoint and armAround
        /// </summary>
        public List<BeatFlagItem> BeatFlags { get; set; }
        public BeatFlagController()
        {
            BeatFlags = new List<BeatFlagItem>{
                        new BeatFlagItem(5, 0.5f),
                        new BeatFlagItem(8, 0.5f)
            };
        }

        /// <summary>
        /// By certain time point (seconds), returns respective flag index
        /// </summary>
        /// <param name="timeFlag"></param>
        /// <returns></returns>
        public int GetFlagIndex(float timeFlag)
        {
            for (int i = 0; i < BeatFlags.Count; i++)
            {
                bool isTimeIntersects = timeFlag >= BeatFlags[i].TimePoint - BeatFlags[i].ArmAround && timeFlag <= BeatFlags[i].TimePoint + BeatFlags[i].ArmAround;
                if (isTimeIntersects)
                    return i;
            }
            return -1;
        }

        /// <summary>
        /// Checks whether timeFlag for time point (seconds) exists or not.
        /// Refers to GetFlagIndex(float).
        /// </summary>
        /// <param name="timeFlag"></param>
        /// <returns></returns>
        public bool IsFlagExists(float timeFlag)
        {
            return GetFlagIndex(timeFlag) != -1;
        }

        /// <summary>
        /// Sets flag, including certain time point (seconds), as taken.
        /// Refers to GetFlagIndex(float).
        /// </summary>
        /// <param name="timeFlag"></param>
        /// <returns></returns>
        public bool SetFlagTaken(float timeFlag)
        {
            int flagIndex = GetFlagIndex(timeFlag);
            if (flagIndex != -1)
            {
                BeatFlags[flagIndex].IsTaken = true;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks whether or not certain time point (seconds) is taken.
        /// Refers to GetFlagIndex(float).
        /// </summary>
        /// <param name="timeFlag"></param>
        /// <returns></returns>
        public bool IsFlagTaken(float timeFlag)
        {
            int flagIndex = GetFlagIndex(timeFlag);
            if (flagIndex == -1)
                return true;
            return BeatFlags[flagIndex].IsTaken;
        }

        /// <summary>
        /// Seconds left to take matching flag
        /// </summary>
        /// <param name="timeFlag"></param>
        /// <returns></returns>
        public float TimeLeftThisSpan(float timeFlag)
        {
            float timeLeft = -1;
            if (IsFlagExists(timeFlag))
            {
                // Маємо індекс флагу
                int flagIndex = GetFlagIndex(timeFlag);

                // Обчислюємо час від поточної точки до кінця флагу
                float finalTime = BeatFlags[flagIndex].TimePoint + BeatFlags[flagIndex].ArmAround;

                timeLeft = finalTime - timeFlag;
                return timeLeft;
            }
            return timeLeft;
        }

        /// <summary>
        /// Checks whether or not all flags have been taken
        /// </summary>
        /// <returns></returns>
        public bool IsWinAchieved()
        {
            foreach (BeatFlagItem flag in BeatFlags)
                if (!flag.IsTaken)
                    return false;
            return true;
        }

        /// <summary>
        /// Checks whether or not some flag has not been taken, and timepoint already passed it
        /// </summary>
        /// <param name="timePoint"></param>
        /// <returns></returns>
        public bool IsLoseAchieved(float timePoint)
        {
            foreach (BeatFlagItem flag in BeatFlags)
                if (!flag.IsTaken && timePoint > flag.TimePoint + flag.ArmAround)
                    return true;
            return false;
        }
    }
}
