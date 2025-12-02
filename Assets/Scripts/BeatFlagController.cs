using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts
{
    public class BeatFlagController
    {
        public List<BeatFlagItem> BeatFlags { get; set; }
        public BeatFlagController()
        {
            BeatFlags = new List<BeatFlagItem>{
                        new BeatFlagItem(5, 0.5f),
                        new BeatFlagItem(8, 0.5f)
            };
        }

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

        public bool IsFlagExists(float timeFlag)
        {
            return GetFlagIndex(timeFlag) != -1;
        }

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

        public bool IsFlagTaken(float timeFlag)
        {
            int flagIndex = GetFlagIndex(timeFlag);
            if (flagIndex == -1)
                return true;
            return BeatFlags[flagIndex].IsTaken;
        }

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

        public bool IsWinAchieved()
        {
            foreach (BeatFlagItem flag in BeatFlags)
                if (!flag.IsTaken)
                    return false;
            return true;
        }

        public bool IsLoseAchieved(float timePoint)
        {
            foreach (BeatFlagItem flag in BeatFlags)
                if (!flag.IsTaken && timePoint > flag.TimePoint + flag.ArmAround)
                    return true;
            return false;
        }
    }
}
